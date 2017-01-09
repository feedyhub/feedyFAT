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
        ImportWindowViewModel ViewModel;

        public ImportWindow()
        {

            InitializeComponent();
            ViewModel = new ImportWindowViewModel();
            DataContext = ViewModel;

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
                try
                {
                    // Open the selected file to read and set label in view.
                    fileStream = openFileDialog1.OpenFile();
                    filenameLabel.Content = openFileDialog1.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datei konnte nicht geöffnet werden. Ist die .CSV / Excel bereits Datei geöffnet?");
                    return;
                }
            }
        }



        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            bool userInputIsValid = false;

            Event Event = ViewModel.Event;
            Questionnaire Questionnaire;
            var Data = new List<List<string>>();

            #region Set Questionnaire variable

            //New Questionnaire
            if (!string.IsNullOrEmpty(ViewModel.NewQuestionnaire.Name))
            {
                Questionnaire = ViewModel.NewQuestionnaire;

                // make sure no Questionnaire with same name is existing already
                if (!db.Questionnaires.Any(q => q.Name == Questionnaire.Name))
                {
                    db.Questionnaires.Add(Questionnaire);
                    db.SaveChanges();
                }

                else
                {
                    MessageBox.Show("Ein Fragebogen mit diesem Namen existiert bereits!");
                    return;
                }
            }

            // Existing Questionnaire, only if ViewModel.NewQuestionnaire.Name field is empty.
            else if (ViewModel.QuestionnaireID != 0)
            {
                Questionnaire = db.Questionnaires
                    //.Include(t => t.Questions.Select(a => a.Answers.Select(d => d.CountDataSet)))
                    //.Include(t => t.Questions.Select(a => a.Answers.Select(d => d.TextDataSet)))
                    .Single(q => q.QuestionnaireID == ViewModel.QuestionnaireID);
            }

            else
            {
                MessageBox.Show("Es muss entweder ein Fragebogen ausgewählt werden, oder einer erstellt werden.");
                return;
            }
            #endregion

            #region Validate Textbox and File Input.
            //fields must be filled out
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
            #endregion

            if (userInputIsValid)
            {

                Event.Questionnaire = Questionnaire;

                //read data from .csv file
                Data = ParseFileContent(fileStream);

                #region cut off useless columns

                int ColumnCount = Data[0].Count;
                int RowCount = Data.Count;

                for (int column = ColumnCount - 1; column >= 0; --column)
                {
                    if (Data[1][column] == "\0" || Data[1][column] == string.Empty || Data[1][column] == null)
                    {
                        for (int row = 0; row < RowCount; ++row)
                        {
                            Data[row].Remove(Data[row][column]);
                        }
                    }

                }

                #endregion

                // If Questionnaire has questions saved, take the ones provided.
                if (Event.Questionnaire.Questions != null)
                {

                    //Check if format of content is valid

                    //Count number of answers
                    int counter = 0;

                    foreach (var element in Data[1])
                    {
                        if (element != string.Empty && element != null && element != "\0")
                        {
                            ++counter;
                        }
                    }

                    int CountDB = Event.Questionnaire.Questions.SelectMany(q => q.Answers).Count();
                    if (Event.Questionnaire.Questions.SelectMany(q => q.Answers).Count() != counter)
                    {
                        MessageBox.Show("Falscher Fragebogen ausgewählt. Die Anzahl der Spalten der .CSV Datei stimmt nicht mit der gespeicherten Vorlage überein.");
                        return;
                    }

                    else if (AppendDataToQuestionnaire(Event, Data))
                    {
                        db.Events.Add(Event);
                    }

                }

                //Otherwise create model for this new questionnaire
                else
                {

                    Event.Questionnaire.Questions = ConvertFileToModel(Event, Data);
                    //db.Entry(Event.Questionnaire).State = EntityState.Modified;
                    db.Events.Add(Event);
                }


                db.SaveChanges();

            }

            else
            {
                MessageBox.Show("Bitte Ort angeben und Datei auswählen!");
                return;
            }


            // if we get to this point, import was sucessfull.
            this.DialogResult = true;
            this.Close();

        }

        private bool AppendDataToQuestionnaire(Event myEvent, List<List<string>> data)
        {
            bool Success = false;

            //first two rows are question and answer texts.
            myEvent.ParticipantsCount = data.Count - 2;


            Answer RefAnswer = new Answer();
            TextData TextDataElement;
            CountData CountDataElement;
            Question RefQuestion = new Question();

            int DataCounter = 0;
            //go through columns and create corresponding objects
            //First row of Data contains the QuestionTexts. Second Row AnswerTexts. Remaining rows contain data, either text or int.
            for (int column = 0; column < data[0].Count; ++column)
            {
                DataCounter = 0;

                //find matching question
                if (!string.IsNullOrEmpty(data[0][column]))
                {
                    try
                    {
                        RefQuestion = myEvent.Questionnaire.Questions.Single(q => q.Text == data[0][column]);

                        RefAnswer = RefQuestion.Answers.Single(a => a.Text == data[1][column]);


                        if (RefAnswer != null)
                        {
                            for (int row = 2; row < data.Count; ++row)
                            {
                                string Element = data[row][column];

                                // ignore if empty
                                if (!string.IsNullOrWhiteSpace(data[row][column]))
                                {
                                    //store if textanswer and count not null elements
                                    if (!Element.All(c => char.IsDigit(c)))
                                    {
                                        TextDataElement = new TextData(data[row][column]);
                                        TextDataElement.Event = myEvent;
                                        RefAnswer.TextDataSet.Add(TextDataElement);

                                    }
                                    ++DataCounter;
                                }
                            }
                            CountDataElement = new CountData(DataCounter);
                            CountDataElement.Event = myEvent;
                            RefAnswer.CountDataSet.Add(CountDataElement);
                        }
                        Success = true;
                    }
      
                    catch (InvalidOperationException e)
                    {
                        MessageBox.Show("Problem: Es scheint als wären die Texte der Fragen und der Antwortmöglichkeiten geändert worden. Lösung: Es muss dafür ein neuer Fragebogen erstellt werden. Hinweis: Die unpassende Frage/Antwort ist: " + data[0][column]);
                    }
                }
                    

            }
                //find corresponding questions and answers in model.

                return Success;
        }
        

        public List<List<string>> ParseFileContent(Stream fileStream)
        {
            // fileStream to MemoryStream
            BinaryReader b = new BinaryReader(fileStream, Encoding.Default);

            byte[] binData = b.ReadBytes(checked((int)fileStream.Length));
            MemoryStream Strm = new MemoryStream(binData);

            //Parse
            TextFieldParser Parser = new TextFieldParser(Strm);

            string[] Delimiters = { "," };
            Parser.Delimiters = Delimiters;

            List<string[]> Data = new List<string[]>();

            string[] RowElements;

            while (!Parser.EndOfData)
            {
                RowElements = Parser.ReadFields();

                Data.Add(RowElements);
            }

            List<List<string>> ListData = new List<List<string>>();

            foreach (var array in Data)
            {
                ListData.Add(new List<string>(array));
            }
            return ListData;

        }

        private ObservableCollection<Question> ConvertFileToModel(Event myEvent, List<List<string>> data)
        {
            //first two rows are question and answer texts.
            myEvent.ParticipantsCount = data.Count - 2;

            ObservableCollection<Question> Questions = new ObservableCollection<Question>();

            TextData TextDataElement;
            CountData CountDataElement;

            int DataCounter = 0;
            //go through columns and create corresponding objects
            //First row of Data contains the QuestionTexts. Second Row AnswerTexts. Remaining rows contain data, either text or int.
            for (int column = 0; column < data[0].Count; ++column)
            {
                DataCounter = 0;

                for (int row = 0; row < data.Count; ++row)
                {
                    string Element = data[row][column];

                    //First row is where the Question Texts are.
                    if (row == 0)
                    {

                        if (!string.IsNullOrEmpty(Element))
                        {
                            //detect if last Question is a Textquestion
                            if (Questions.Count != 0 && Questions.Last().Answers.Count == 1 && Questions.Last().Answers.First().TextDataSet.Count >= 0)
                            {
                                Questions.Last().EvalMode = EvaluationMode.TEXT;
                                Questions.Last().QuestionType = QuestionType.TEXT;
                            }

                            //create new one
                            var Question = new Question(Element);

                            // Default EvalMode is Absolute Mode
                            Question.EvalMode = EvaluationMode.ABSOLUTE;

                            Questions.Add(Question);
                            Questions.Last<Question>().Answers = new ObservableCollection<Answer>();
                        }

                    }

                    //Second row is where Answer Texts are
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
                            if (!Element.All(c => char.IsDigit(c) || c.Equals(".")))
                            {
                                ++DataCounter;
                                TextDataElement = new TextData(Element);
                                TextDataElement.Event = myEvent;
                                Questions.Last().Answers.Last().TextDataSet.Add(TextDataElement);

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