using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using System.IO;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using FeedyWPF.Models;

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        private FeedyDbContext db = new FeedyDbContext();
        public Stream fileStream;

        public delegate void ContentUpdateHandler(object sender, EventsContentChangedEventArgs e);
        public event ContentUpdateHandler OnEventsContentChange;

        public ImportWindow()
        {
           
            InitializeComponent();
            ImportWindowViewModel vm = new ImportWindowViewModel();
            DataContext = vm;
          
        }

     

        private void directoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                // Open the selected file to read and set label in view.
                fileStream = openFileDialog1.OpenFile();
                filenameLabel.Content = openFileDialog1.FileName;
            }
        }

   

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            bool userInputIsValid = false;

            Event Event = ((ImportWindowViewModel)DataContext).Event;
            Questionnaire Questionnaire;

            //Select Questionnaire
            if(((ImportWindowViewModel)DataContext).NewQuestionnaire.Name != null)
            {
                MessageBox.Show("New Questionnaire Created");      
                Questionnaire = ((ImportWindowViewModel)DataContext).NewQuestionnaire;
                db.Questionnaires.Add(Questionnaire);
                db.SaveChanges();

                // TODO: avoid duplicates
            }

            else if (((ImportWindowViewModel)DataContext).QuestionnaireID != 0)
            {
                Questionnaire = db.Questionnaires
                    .Include(t => t.Questions.Select(a => a.Answers.Select(d => d.CountDataSet)))
                    .Include(t => t.Questions.Select(a => a.Answers.Select(d => d.TextDataSet)))
                    .Single(q => q.QuestionnaireID == ((ImportWindowViewModel)DataContext).QuestionnaireID);
            }

            else
            {
                MessageBox.Show("Es muss entweder ein Fragebogen ausgewählt werden, oder einer erstellt werden.");
                return;
            }

            // Validate Textbox and File Input.
            if (fileStream != null)
            {
                if (Event.Place != string.Empty)
                {
                    userInputIsValid = true;
                }
                else
                {
                    userInputIsValid = false;
                }
                
            }
           
            if (userInputIsValid)
            {
                
                Event.Questionnaire = Questionnaire;

                // If Questionnaire has questions saved yet, take the ones provided.
                if (Event.Questionnaire.Questions != null)
                {        
                    AppendDataToQuestionnaire(Event);
                }

                //Otherwise create model for this new questionnaire
                else
                {
                    Event.Questionnaire.Questions = ConvertFileToModel(Event);
                    db.Entry(Event.Questionnaire).State = EntityState.Modified;
                }

                db.Events.Add(Event);
                db.SaveChanges();

            }

            else
            {
                MessageBox.Show("Invalid user Input");
                return;
            }

            EventsContentChangedEventArgs args = new EventsContentChangedEventArgs();
            OnEventsContentChange(this, args);

            this.Close();

        }

        private void AppendDataToQuestionnaire(Event myEvent)
        {

            List<string[]> Data = ParseFileContent(fileStream);

            //first two rows are question and answer texts.
            myEvent.ParticipantsCount = Data.Count - 2;

            try
            {
                //CheckDataForCompatibility();
            }

            catch (Exception e)
            {
                throw new NotImplementedException("Uploading wrong files is not yet handled.");
            }


            IEnumerable<Answer> RefAnswer;
            TextData TextDataElement;
            CountData CountDataElement;

            int DataCounter = 0;
            //go through columns and create corresponding objects
            //First row of Data contains the QuestionTexts. Second Row AnswerTexts. Remaining rows contain data, either text or int.
            for (int column = 0; column < Data[0].Length; ++column)
            {
                DataCounter = 0;


                //find corresponding questions and answers in model.

                RefAnswer =
                    from question in myEvent.Questionnaire.Questions
                    where question.Text == Data[0][column]
                    let answers = question.Answers
                    from answer in answers
                    where answer.Text == Data[1][column]
                    select answer;


                if (RefAnswer.Any())
                {
                    for (int row = 2; row < Data.Count; ++row)
                    {
                        string Element = Data[row][column];

                        // ignore if empty
                        if (!string.IsNullOrWhiteSpace(Data[row][column]))
                        {
                            //store if textanswer and count not null elements
                            if (!Element.All(c => char.IsDigit(c)))
                            {
                                TextDataElement = new TextData(Data[row][column]);
                                TextDataElement.Event = myEvent;
                                RefAnswer.FirstOrDefault().TextDataSet.Add(TextDataElement);

                            }
                            ++DataCounter;
                        }
                    }
                    CountDataElement = new CountData(DataCounter);
                    CountDataElement.Event = myEvent;
                    RefAnswer.FirstOrDefault().CountDataSet.Add(CountDataElement);
                }

                else
                {
                    // This is executed when no corresponding answer is found. For example names in »Mit welchem Teamer*in...«
                    RefAnswer =
                        from question in myEvent.Questionnaire.Questions
                        where question.Text == Data[0][column]
                        let answers = question.Answers
                        from answer in answers
                        select answer;

                    foreach (var answer in RefAnswer)
                    {
                        CountDataElement = new CountData(0);
                        CountDataElement.Event = myEvent;
                        RefAnswer.FirstOrDefault().CountDataSet.Add(CountDataElement);
                    }
                }
            }
        }

        public List<string[]> ParseFileContent(Stream fileStream)
        {
            BinaryReader b = new BinaryReader(fileStream, Encoding.Default);

            byte[] binData = b.ReadBytes(checked((int)fileStream.Length));

            MemoryStream Strm = new MemoryStream(binData);
            TextFieldParser Parser = new TextFieldParser(Strm);

            string[] Delimiters = { ";" };
            Parser.Delimiters = Delimiters;

            List<string[]> Data = new List<string[]>();

            string[] RowElements;

            while (!Parser.EndOfData)
            {
                RowElements = Parser.ReadFields();

                Data.Add(RowElements);
            }

            return Data;

        }

        private ObservableCollection<Question> ConvertFileToModel(Event myEvent)
        {
            List<string[]> Data = ParseFileContent(fileStream);

            //first two rows are question and answer texts.
            myEvent.ParticipantsCount = Data.Count - 2;

            ObservableCollection<Question> Questions = new ObservableCollection<Question>();
            ObservableCollection<Answer> Answers = new ObservableCollection<Answer>();

            TextData TextDataElement;
            CountData CountDataElement;

            int DataCounter = 0;
            //go through columns and create corresponding objects
            //First row of Data contains the QuestionTexts. Second Row AnswerTexts. Remaining rows contain data, either text or int.
            for (int column = 0; column < Data[0].Length; ++column)
            {
                DataCounter = 0;

                for (int row = 0; row < Data.Count; ++row)
                {
                    string Element = Data[row][column];
                   

                    if (row == 0)
                    {
                       
                        if (!string.IsNullOrEmpty(Element))
                        {
                             var Question = new Question(Element);

                            // Default EvalMode is Absolute Mode
                            Question.EvalMode = EvaluationMode.ABSOLUTE;

                            Questions.Add(Question);
                            Questions.Last<Question>().Answers = new ObservableCollection<Answer>();

                        }

                    }

                    else if (row == 1)
                    {
                        Questions.Last<Question>().Answers.Add(new Answer(Element));
                        Questions.Last<Question>().Answers.Last<Answer>().TextDataSet = new ObservableCollection<TextData>();
                        Questions.Last<Question>().Answers.Last<Answer>().CountDataSet = new ObservableCollection<CountData>();
                    }

                    else
                    {

                        if (string.IsNullOrWhiteSpace(Element))
                        {
                            /*ignore. We don't want to save or count emtpy Elements. */
                        }

                        //check if Element is a number, and if it is roughly small enough to fit into an int. Relevant numbers will be much smaller than 10^5. Count them. Don't save.
                        else if (Element.All(c => char.IsDigit(c)) && Element.Count<char>() <= 5)
                        {
                            ++DataCounter;
                        }

                        //else its a TextAnswer or contains a useless large number, save it.
                        else
                        {
                            // if its one of the useless large numbers or numbers separated by dots, ignore, else save textanswer
                            if (!Element.All(c => char.IsDigit(c) || c.Equals(".") ))
                            {
                                ++DataCounter;
                                TextDataElement = new TextData(Element);
                                TextDataElement.Event = myEvent;
                                Questions.Last().Answers.Last().TextDataSet.Add(TextDataElement);
                                Questions.Last().EvalMode = EvaluationMode.TEXT;
                            }
                        }
                    }
                }

                //pass DataCounter to corresponding Answer.
                CountDataElement = new CountData(DataCounter);
                CountDataElement.Event = myEvent;
                Questions.Last().Answers.Last().CountDataSet.Add(CountDataElement);
            }

            return Questions;

        }

        private void questionnaireComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

   
}
