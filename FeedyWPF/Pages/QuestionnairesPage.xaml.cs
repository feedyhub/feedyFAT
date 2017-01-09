using FeedyWPF.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FeedyWPF.Models;
using System.Data.Entity;
using FeedyWPF.Windows;

namespace FeedyWPF.Pages
{
    /// <summary>
    /// Interaction logic for QuestionnairesPage.xaml
    /// </summary>
    public partial class QuestionnairesPage : Page
    {
        public QuestionnairesPage()
        {
            InitializeComponent();
            QuestionnaireViewSource = ((CollectionViewSource)(FindResource("questionnaireViewSource")));

            db.Questionnaires.Load();
            QuestionnaireViewSource.Source = db.Questionnaires.Local;
           
        }

        private FeedyDbContext db = new FeedyDbContext();
        private CollectionViewSource QuestionnaireViewSource;

        public delegate void SetCreateQuestionsPageHandler(object sender, SetCreateQuestionsPageEventArgs e);
        public event SetCreateQuestionsPageHandler OnNewCreateQuestionsPage;


        public delegate void ContentUpdateHandler(object sender, EventsContentChangedEventArgs e);
        public event ContentUpdateHandler OnEventsContentChange;

        public void RefreshTable(object sender, QuestionnairesContentChangedEventArgs e)
        {


            
            QuestionnaireViewSource.Source = db.Questionnaires.ToList();
            this.questionnaireDataGrid.Items.Refresh();
            this.questionnaireDataGrid.UpdateLayout();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool EventsDeleted = false;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var SelectedQuestionnaire = row.DataContext as Questionnaire;
                    if (SelectedQuestionnaire != null)
                        if(SelectedQuestionnaire.Events !=null && SelectedQuestionnaire.EventsCount > 0)
                        {
                            EventsDeleted = true;
                        }
                        db.Questionnaires.Remove(SelectedQuestionnaire);
                    break;
                }
            db.SaveChanges();
            RefreshTable(this, new QuestionnairesContentChangedEventArgs());

            if (EventsDeleted)
            {
                OnEventsContentChange(this, new EventsContentChangedEventArgs());
            }
        }

        private void NewQuestionnaireButton_Click(object sender, RoutedEventArgs e)
        {
            var Window = new CreateQuestionnaireWindow();

            if(Window.ShowDialog() == true)
            {
                OnEventsContentChange(this, new EventsContentChangedEventArgs());

                var args = new SetCreateQuestionsPageEventArgs();
                args.Questionnaire = Window.Questionnaire;
                OnNewCreateQuestionsPage(this, args);
            }
            
           
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
           

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var SelectedQuestionnaire = row.DataContext as Questionnaire;
                    if (SelectedQuestionnaire != null)
                    {
                        var args = new SetCreateQuestionsPageEventArgs();
                        args.Questionnaire = SelectedQuestionnaire;
                        OnNewCreateQuestionsPage(this, args);
                    }

                    break;
                }

           

          
          
        }
    }
}
