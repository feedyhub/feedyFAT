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

using FeedyWPF.Models;

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for EventsView.xaml
    /// </summary>
    public partial class EventsPage : Page
    {
        public EventsPage()
        {
            InitializeComponent();
        }

        public delegate void EvaluationPageEventHandler(object sender, EvaluationPageEventArgs e);
        public event EvaluationPageEventHandler OnEvaluationPageEvent;


        private void RefreshTable(object sender, EventsContentChangedEventArgs e)
        {
            CollectionViewSource eventViewSource = ((CollectionViewSource)(FindResource("eventViewSource")));

            db.Events.Load();
            eventViewSource.Source = db.Events.Local;
            this.eventDataGrid.Items.Refresh();

            
        }

        

        private void eventDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            CollectionViewSource eventViewSource = ((CollectionViewSource)(FindResource("eventViewSource")));

            db.Events.Load();
            eventViewSource.Source = db.Events.Local;
            
        }

        private FeedyDbContext db = new FeedyDbContext();

        private void importDataButton_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow ImpWindow = new ImportWindow();
            ImpWindow.OnEventsContentChange += new ImportWindow.ContentUpdateHandler(RefreshTable);
            ImpWindow.Show();
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

                    Evaluation Evaluation = new Evaluation(SelectedEvent);

                    var args = new EvaluationPageEventArgs();
                    args.Evaluation = Evaluation;
                    OnEvaluationPageEvent(this, args);

                    break;
                }

            eventDataGrid.Items.Refresh();

        }

        private void eventDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
