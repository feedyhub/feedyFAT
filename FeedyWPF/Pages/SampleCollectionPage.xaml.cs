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
    /// Interaction logic for SampleCollectionView.xaml
    /// </summary>
    public partial class SampleCollectionPage : BasePage
    {

        public SampleCollectionPage(Event myEvent,FeedyDbContext db, string tabUid) : base (tabUid)
        {
            InitializeComponent();
            Event = myEvent;
            ViewModel = new SampleCollectionPageViewModel(myEvent);
            Db = db;
            DataContext = ViewModel;
            
        }

        private FeedyDbContext Db { get; set; }
        private Event Event { get; set; }

        private SampleCollectionPageViewModel ViewModel { get; set; }

        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseTabEvent(this, new CloseTabEventArgs());
        }

        private void NextSampleButton_Click(object sender, RoutedEventArgs e)
        {
            SampleToModel();
            Db.SaveChanges();

            ViewModel = new SampleCollectionPageViewModel(Event);
            DataContext = ViewModel;

            QuestionListView.ScrollIntoView(ViewModel.ViewQuestions.First());
        }

        

        private void SaveLeaveButton_Click(object sender, RoutedEventArgs e)
        {
            SampleToModel();
            Db.SaveChanges();

            OnCloseTabEvent(this, new CloseTabEventArgs());
            
        }

        ///Converts the current sample of the filled-out questionnaire to the database compatible model.
        private void SampleToModel()
        {
           

            foreach(var viewQuestion in ViewModel.ViewQuestions)
            {
                Question Question = Event.Questionnaire.Questions.Single(q => q.QuestionID == viewQuestion.QuestionID);

                foreach(var viewAnswer in viewQuestion.ViewAnswers)
                {

                    if (viewAnswer.IsChecked)
                    {
                        Answer Answer = Question.Answers.Single(a => a.AnswerID == viewAnswer.AnswerID);

                        CountData Data = Answer.CountDataSet.SingleOrDefault(d => d.EventID == Event.EventID);

                        if(Data == null)
                        {
                            Data = new CountData();
                            Data.EventID = Event.EventID;
                            Answer.CountDataSet.Add(Data);
                        }

                        Data.Count += 1;  
                    }
                }
            }

            Event.ParticipantsCount += 1;
        }
    }
}
