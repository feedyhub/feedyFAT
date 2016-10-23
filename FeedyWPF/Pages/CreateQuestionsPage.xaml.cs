using FeedyWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedyWPF.Pages
{
    /// <summary>
    /// Interaction logic for CreateQuestionsPage.xaml
    /// </summary>
    public partial class CreateQuestionsPage : BasePage
    {
        public CreateQuestionsPage(string tabUid, Questionnaire questionnaire) : base(tabUid)
        {
            InitializeComponent();

           

            if(questionnaire.Questions == null || questionnaire.Questions.Count == 0)
            {
                FromDatabase = false;
                Questionnaire = questionnaire;
                ViewModel = new CreateQuestionViewModel();
            }

            else
            {
                FromDatabase = true;
                Questionnaire = db.Questionnaires.Single(q => q.QuestionnaireID == questionnaire.QuestionnaireID);
                ViewModel = new CreateQuestionViewModel(Questionnaire.Questions);
            }
           
            DataContext = ViewModel;


        }

        private FeedyDbContext db = new FeedyDbContext();
        private bool FromDatabase { get; set; }
        private CreateQuestionViewModel ViewModel { get; set; }
        private Questionnaire Questionnaire { get; set; }

        public delegate void EventContentUpdateHandler(object sender, EventsContentChangedEventArgs e);
        public event EventContentUpdateHandler OnEventsContentChange;

        public delegate void ContentUpdateHandler(object sender, QuestionnairesContentChangedEventArgs e);
        public event ContentUpdateHandler OnQuestionnairesContentChange;


        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var Sender = sender as Button;
            var SelectedCreateQuestion = Sender.DataContext as CreateQuestion;

            if (SelectedCreateQuestion != null)
            {
                int Index = ViewModel.CreateQuestions.IndexOf(SelectedCreateQuestion);
                ViewModel.CreateQuestions.Insert(Index+1, new CreateQuestion());
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var Sender = sender as Button;
            var SelectedCreateQuestion = Sender.DataContext as CreateQuestion;

            // get all Progress states into List. Then set selectedcreatequestion to the next progress state.
            var ProgressValues = Enum.GetValues(typeof(CreateQuestionProgress)).Cast<CreateQuestionProgress>().ToList();

            foreach(var element in ProgressValues)
            {
                if(element == SelectedCreateQuestion.Progress)
                {
                    if(SelectedCreateQuestion.Progress != ProgressValues.Last())
                    {
                        SelectedCreateQuestion.Progress = ProgressValues.ElementAt(ProgressValues.IndexOf(element) + 1);
                    }
                   
                    break;
                }
            }

        }

        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseTabEvent(this, new CloseTabEventArgs());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var Sender = sender as Button;
            var SelectedCreateQuestion = Sender.DataContext as CreateQuestion;

            // get all Progress states into List. Then set selectedcreatequestion to the previous progress state.
            var ProgressValues = Enum.GetValues(typeof(CreateQuestionProgress)).Cast<CreateQuestionProgress>().ToList();

            foreach (var element in ProgressValues)
            {
                if (element == SelectedCreateQuestion.Progress)
                {
                    if (SelectedCreateQuestion.Progress != ProgressValues.First())
                    {
                        SelectedCreateQuestion.Progress = ProgressValues.ElementAt(ProgressValues.IndexOf(element) - 1);
                    }

                    break;
                }
            }

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        private void RemoveQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var Sender = sender as Button;
            var SelectedCreateQuestion = Sender.DataContext as CreateQuestion;

            if (SelectedCreateQuestion != null)
            {   
                ViewModel.CreateQuestions.Remove(SelectedCreateQuestion);

                if (FromDatabase)
                {
                    db.Questions.Remove(db.Questions.Single(q => q.QuestionID == SelectedCreateQuestion.QuestionID));
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.CreateQuestions.All(cq => cq.Progress == CreateQuestionProgress.FINISHED))
            {

                /// save Questionnaire To Database
                /// 
                if (!FromDatabase)
                {
                    var Questions = new ObservableCollection<Question>();

                    //some type conversion
                    foreach (var createQuestion in ViewModel.CreateQuestions)
                    {
                        Questions.Add(createQuestion.ToQuestion());
                    }

                    foreach (var question in Questions)
                    {
                        if (question.QuestionType == QuestionType.TEXT)
                        {
                            question.EvalMode = EvaluationMode.TEXT;
                        }
                    }

                    Questionnaire.Questions = Questions;
                    db.Questionnaires.Add(Questionnaire);
                } 
                       
                    
                db.SaveChanges();
                OnQuestionnairesContentChange(this, new QuestionnairesContentChangedEventArgs());
                OnEventsContentChange(this, new EventsContentChangedEventArgs());
                

                // Close Tab
                OnCloseTabEvent(this, new CloseTabEventArgs());
            }

            else
            {
                MessageBox.Show("Bitte bei allen Fragen auf Fertig klicken oder ungewünschte Fragen löschen.");
            }
        }
    }
}
