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

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Models.FeedyDbContext>());
            InitializeComponent();

            TabItem tabitem = new TabItem();
            tabitem.Header = "Umfragen";
            Frame eventsFrame = new Frame();
            EventsPage eventsPage = new EventsPage();

            eventsFrame.Content = eventsPage;
            tabitem.Content = eventsFrame;
            tabControl.Items.Add(tabitem);

           
        }

       
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            
            

            //System.Windows.Data.CollectionViewSource eventViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("eventViewSource")));

            //db.Events.Load();
            //eventViewSource.Source = db.Events.Local;
            // Load data by setting the CollectionViewSource.Source property:
            // eventViewSource.Source = [generic data source]
        }

        //private void importDataButton_Click(object sender, RoutedEventArgs e)
        //{
        //    ImportWindow ImpWindow = new ImportWindow();
        //    ImpWindow.Show();
        //}

        //private void DeleteEvent(object sender, RoutedEventArgs e)
        //{
        //    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        //        if (vis is DataGridRow)
        //        {
        //            var row = (DataGridRow)vis;
        //            Event SelectedEvent = row.DataContext as Event;
        //            if(SelectedEvent != null)
        //                db.Events.Remove(SelectedEvent);
        //            break;
        //        }
        //    eventDataGrid.Items.Refresh();  
        //}

        //private void EvaluateEvent(object sender, RoutedEventArgs e)
        //{
        //    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        //        if (vis is DataGridRow)
        //        {
        //            var row = (DataGridRow)vis;
        //            Event SelectedEvent = row.DataContext as Event;

        //            throw new NotImplementedException();

        //            break;
        //        }

        //    eventDataGrid.Items.Refresh();

        //}

        //private void eventDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TabWindow tabWin = new TabWindow();
            tabWin.Show();
        }
    }
}
