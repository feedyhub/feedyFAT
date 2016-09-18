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
using System.Data.Entity;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FeedyWPF.Windows;

using FeedyWPF.Models;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for EventsView.xaml
    /// </summary>
    public partial class EventsPage : Page
    {
        public EventsPage(FeedyDbContext database)
        {
            InitializeComponent();

            db = database;
            ViewModel = new EventsPageViewModel();
            DataContext = ViewModel;

            EventViewSource = ((CollectionViewSource)(FindResource("eventViewSource")));

            
                db.Events.Load();
                db.Questions.Load();

                EventViewSource.Source = db.Events.Local;
                EventViewSource.Filter += new FilterEventHandler(FilterDatabase);
            
        }

        private FeedyDbContext db { get; set; }
        CollectionViewSource EventViewSource;
        public EventsPageViewModel ViewModel;


        public delegate void EvaluationPageEventHandler(object sender, SetEvaluationPageEventArgs e);
        public event EvaluationPageEventHandler OnEvaluationPageEvent;

        public delegate void ContentUpdateHandler(object sender, QuestionnairesContentChangedEventArgs e);
        public event ContentUpdateHandler OnQuestionnairesContentChange;

        public delegate void OnSetSampleCollectionPageHandler(object sender, SetSampleCollectionPageEventArgs e);
        public event OnSetSampleCollectionPageHandler OnNewSampleCollectionEvent;



        private void FilterDatabase(object sender, FilterEventArgs e)
        {

            Event MyEvent = e.Item as Event;
            string SearchString = FilterTextBox.Text;
           
            if (MyEvent !=null)
            {
                // Filter out Events that don't match the searched string
                if ((StaticMethods.Contains(MyEvent.Place,SearchString,StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(SearchString)))
                {
                    //filter out events that don't match the questionnaire type
                    if (MyEvent.QuestionnaireID == ViewModel.QuestionnaireID || ViewModel.QuestionnaireID == 0)
                        e.Accepted = true;

                    else
                        e.Accepted = false;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        public void RefreshTable(object sender, EventsContentChangedEventArgs e)
        {
           
            EventViewSource.Source = db.Events.ToList();
            this.eventDataGrid.Items.Refresh();
            this.eventDataGrid.UpdateLayout();

        }


        private void importDataButton_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow ImpWindow = new ImportWindow();
           
            bool? result = ImpWindow.ShowDialog();
            if ( result == true)
            {
                OnQuestionnairesContentChange(this, new QuestionnairesContentChangedEventArgs());
                RefreshTable(this, new EventsContentChangedEventArgs());
            }
        }

        private void DeleteEvent(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    Event SelectedEvent = row.DataContext as Event;
                    if (SelectedEvent != null)
                        db.Events.Remove(SelectedEvent);
                    break;
                }
            db.SaveChanges();
            eventDataGrid.Items.Refresh();
        }

        private void EvaluateEvent(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    Event SelectedEvent = row.DataContext as Event;

                    Evaluation Evaluation = new Evaluation(SelectedEvent, db);

                    var args = new SetEvaluationPageEventArgs();
                    args.Evaluation = Evaluation;
                    OnEvaluationPageEvent(this, args);

                    break;
                }

            eventDataGrid.Items.Refresh();

        }

        private void AddDataToEvent(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    Event SelectedEvent = row.DataContext as Event;

                    var args = new SetSampleCollectionPageEventArgs(SelectedEvent.EventID);

                    OnNewSampleCollectionEvent(this, args);

                    break;
                }
        }

        private void FilterComoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventViewSource.View.Refresh();
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventViewSource.View.Refresh();
        }

        private void CollectDataButton_Click(object sender, RoutedEventArgs e)
        {

            OnNewSampleCollectionEvent(this, new SetSampleCollectionPageEventArgs(null));
        }
    }
}
