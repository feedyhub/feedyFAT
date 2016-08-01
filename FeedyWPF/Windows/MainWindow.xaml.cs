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
using FeedyWPF.Pages;

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

            //Set TabView DataContext
            Tabs = new List<TabItem>();
            tabControl.DataContext = Tabs;

            // Set EventsPage as first Tab
            TabItem tabitem = new TabItem();
            tabitem.Header = "Umfragen";
            Frame eventsFrame = new Frame();
            EventsPage eventsPage = new EventsPage();

            eventsFrame.Content = eventsPage;
            tabitem.Content = eventsFrame;
            Tabs.Add(tabitem);

            //Add + Tab to create new Tab
            PlusTab = new TabItem();
            PlusTab.Header = "+";

            Tabs.Add(PlusTab);

            tabControl.SelectedIndex = 0;
        }

        private List<TabItem> Tabs { get; set; }
        private TabItem AddTab { get; set; }
        private TabItem PlusTab { get; set; }

        private void NewTab()
        {
            int count = Tabs.Count;

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("Neue Umfrage {0}", count - 1);
            tab.Name = string.Format("tab{0}", count - 1);


            // add controls to tab item, this case I added just a textbox
            Frame frame = new Frame();
            SetEvaluationPage page = new SetEvaluationPage();

            frame.Content = page;
            tab.Content = frame;

            // insert tab item right before the last (+) tab item
            Tabs.Insert(count - 1, tab);

            tabControl.SelectedItem = tab;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TabItem tab = tabControl.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Equals(PlusTab))
                {
                    // clear tab control binding
                    tabControl.DataContext = null;

                    // add new tab
                    NewTab();

                    // bind tab control
                    tabControl.DataContext = Tabs;

                    // select newly added tab item

                }

            }
        }
    }
}
