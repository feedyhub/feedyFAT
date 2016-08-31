using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FeedyWPF.Models;
using System.Data.Entity;
using FeedyWPF.Pages;

using System.Collections.ObjectModel;

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
            db = new FeedyDbContext();
            InitializeComponent();

            //Set TabView DataContext
            Tabs = new ObservableCollection<TabItem>();
            tabControl.DataContext = Tabs;

            // Set EventsPage as first Tab
            TabItem tabitem = new TabItem();
            tabitem.Header = "Umfragen";

            Frame eventsFrame = new Frame();
            EventsPage eventsPage = new EventsPage(db);

            eventsPage.OnEvaluationPageEvent += new EventsPage.EvaluationPageEventHandler(setEvaluationTab);

            eventsFrame.Content = eventsPage;
            tabitem.Content = eventsFrame;
            Tabs.Add(tabitem);

            //Add + Tab to create new Tab
            PlusTab = new TabItem();
            PlusTab.Header = "+";

            Tabs.Add(PlusTab);

            tabControl.SelectedIndex = 0;
        }
        private FeedyDbContext db { get; set; }
        private ObservableCollection<TabItem> Tabs { get; set; }
        private TabItem AddTab { get; set; }
        private TabItem PlusTab { get; set; }
        private int tabsCount { get { return Tabs.Count; } }

        private void NewTab()
        {
          

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("Neue Auswertung {0}", tabsCount - 1);
            tab.Name = string.Format("tab{0}", tabsCount - 1);


            // add controls to tab item
            Frame frame = new Frame();
            SetEvaluationPage page = new SetEvaluationPage(db);
            page.tabName = tab.Name;

            //to get from setevaluationpage to evaluationpage
            page.OnEvaluationPageEvent += new SetEvaluationPage.EvaluationPageEventHandler(setEvaluationTab);
            
           

            frame.Content = page;
            tab.Content = frame;

            // insert tab item right before the last (+) tab item
            Tabs.Insert(tabsCount - 1, tab);

            tabControl.SelectedItem = tab;
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TabItem tab = tabControl.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Equals(PlusTab))
                {
                    // add new tab
                    NewTab();
                }

            }
        }

        private void setEvaluationTab(object sender, EvaluationPageEventArgs e)
        {
            TabItem tab = new TabItem();
            
            //Add Controls to Page
            EvaluationPage evaluationPage = new EvaluationPage(e.Evaluation);
            evaluationPage.OnCloseTabEvent += new EvaluationPage.CloseTabEventHandler(CloseTab);

            Frame frame = new Frame();
            frame.Content = evaluationPage;

            if (sender is SetEvaluationPage)
            {
                var setEvaluationPage = sender as SetEvaluationPage;
                tab = Tabs.Single(t => t.Name == setEvaluationPage.tabName);
                tab.Content = frame;
            }
           
            if (sender is EventsPage)
            {
                tab.Content = frame;
                ((TabItem)tab).Header = string.Format("Neue Auswertung {0}", tabsCount - 1);
                tab.Name = string.Format("tab{0}", tabsCount - 1);

                Tabs.Insert(tabsCount - 1, tab);
                tabControl.SelectedItem = tab;
            } 
        }

        private void CloseTab(object sender, CloseTabEventArgs e)
        {


            throw new NotImplementedException();
            
        }
       
    }

}
