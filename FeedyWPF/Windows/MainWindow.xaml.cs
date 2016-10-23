using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FeedyWPF.Models;
using System.Data.Entity;
using FeedyWPF.Pages;

using System.Collections.ObjectModel;
using FeedyWPF.Windows;

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            /// !!!!! TURN OFF FOR DEPLOYMENT !!!!!IfModelChanges
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Models.FeedyDbContext>());

            try{
                db = new FeedyDbContext();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.ToString());
            }
           
            InitializeComponent();
            _tabIdCounter = 0;
          

            //Set TabView DataContext
            Tabs = new ObservableCollection<TabItem>();
           
            tabControl.DataContext = Tabs;

            #region Eventspage es first Tab
            // Set EventsPage as first Tab
            TabItem tabitem = new TabItem();
            tabitem.Header = "Umfragen";

            tabitem.Uid = TabIdCounter.ToString();
            

            Frame eventsFrame = new Frame();
            EventsPage = new EventsPage(db);

            EventsPage.OnEvaluationPageEvent += new EventsPage.EvaluationPageEventHandler(SetEvaluationPage);
            EventsPage.OnNewSampleCollectionEvent += new EventsPage.OnSetSampleCollectionPageHandler(SetSampleCollectionPage);

            eventsFrame.Content = EventsPage;
            tabitem.Content = eventsFrame;
            Tabs.Add(tabitem);
            #endregion

            #region QuestionnairesPage as second Tab
            // Set QuestionnairesPage as second Tab
            tabitem = new TabItem();
            tabitem.Header = "Fragebögen";

            tabitem.Uid = TabIdCounter.ToString();


             var frame = new Frame();

            QuestionnairesPage = new QuestionnairesPage();

            QuestionnairesPage.OnNewCreateQuestionsPage += new QuestionnairesPage.SetCreateQuestionsPageHandler(SetCreateQuestionsPage);

         

            frame.Content = QuestionnairesPage;
            tabitem.Content = frame;
            Tabs.Add(tabitem);
            #endregion
            
            //wire up content change events between eventspage and questionnaires page
            QuestionnairesPage.OnEventsContentChange += new QuestionnairesPage.ContentUpdateHandler(EventsPage.RefreshTable);
            EventsPage.OnQuestionnairesContentChange += new EventsPage.ContentUpdateHandler(QuestionnairesPage.RefreshTable);

            #region Add "+" Tab
            //Add + Tab to create new Tab
            PlusTab = new TabItem();
            PlusTab.Header = "+";

            Tabs.Add(PlusTab);
            #endregion
            tabControl.SelectedIndex = 0;
        }

       

        private int _tabIdCounter { get; set; }
        private int TabIdCounter
        {
            get
            {
                ++_tabIdCounter;
                return _tabIdCounter;
            }
        }
        private FeedyDbContext db { get; set; }
        private ObservableCollection<TabItem> Tabs { get; set; }
        private TabItem AddTab { get; set; }
        private TabItem PlusTab { get; set; }

        private QuestionnairesPage QuestionnairesPage { get; set; }
        private EventsPage EventsPage { get; set; }

        private int tabsCount { get { return Tabs.Count; } }

        private void AddSetEvaluationTab()
        {
          
            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("Neue Auswertung {0}", tabsCount - 1);
            tab.Uid = TabIdCounter.ToString();
            


            // add controls to tab item
            Frame frame = new Frame();
            SetEvaluationPage page = new SetEvaluationPage(db,tab.Uid);
            

            //to get from setevaluationpage to evaluationpage
            page.OnEvaluationPageEvent += new SetEvaluationPage.EvaluationPageEventHandler(SetEvaluationPage);
            page.CloseTabEvent += CloseTab;
            
           

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
                    // add new tab setevaluation Tab
                    AddSetEvaluationTab();
                }
            }
        }

        private void SetEvaluationPage(object sender, SetEvaluationPageEventArgs e)
        {
            TabItem tab;

            #region from SetEvaluationPage to EvaluationPage
            //replace setevaluationpage with evaluationpage
            if (sender is SetEvaluationPage)
            {

                var setEvaluationPage = sender as SetEvaluationPage;
                tab = Tabs.Single(t => t.Uid == setEvaluationPage.TabUid);
                
                
                EvaluationPage evaluationPage = new EvaluationPage(e.Evaluation, tab.Uid);

                //Add Controls to Page
                evaluationPage.CloseTabEvent += CloseTab;


                Frame frame = new Frame();
                frame.Content = evaluationPage;


                tab.Content = frame;
            }
            #endregion

            #region from EventsPage to EvaluationPage
            // create and insert new tab, set evaluationpage as tabcontent
            if (sender is EventsPage)
            {
                tab = new TabItem();
                tab.Uid = TabIdCounter.ToString();
                tab.Header = string.Format("Neue Auswertung {0}", tabsCount - 1);

               
                EvaluationPage evaluationPage = new EvaluationPage(e.Evaluation, tab.Uid);

                //Add Controls to Page
                evaluationPage.CloseTabEvent += CloseTab;


                Frame frame = new Frame();
                frame.Content = evaluationPage;


                tab.Content = frame;
                Tabs.Insert(tabsCount - 1, tab);
                tabControl.SelectedItem = tab;
            }

            #endregion
        }



        

        private void SetCreateQuestionsPage(object sender, SetCreateQuestionsPageEventArgs args)
        {
            var Tab = new TabItem();
            if (sender is QuestionnairesPage)
            {

                var createQuestionnaireWindow = sender as CreateQuestionnaireWindow;


               
                Tab.Uid = TabIdCounter.ToString();
                Tab.Header = string.Format("Fragebogen erstellen {0}", tabsCount - 2);


                var createQuestionsPage = new CreateQuestionsPage(Tab.Uid,args.Questionnaire);
             

                //Add Controls to Page
                createQuestionsPage.CloseTabEvent += CloseTab;
                createQuestionsPage.OnQuestionnairesContentChange += new CreateQuestionsPage.ContentUpdateHandler(QuestionnairesPage.RefreshTable);
                createQuestionsPage.OnEventsContentChange += new CreateQuestionsPage.EventContentUpdateHandler(EventsPage.RefreshTable);


                Frame frame = new Frame();
                frame.Content = createQuestionsPage;


                Tab.Content = frame;


                Tabs.Insert(tabsCount - 1, Tab);
                tabControl.SelectedItem = Tab;
            }
        }

        private void SetSampleCollectionPage(object sender, SetSampleCollectionPageEventArgs args)
        {
            int EventID;

            if (args.CreateNewEvent)
            {
                var window = new CreateEventWindow();

                if (window.ShowDialog() == true)
                {

                    using (var db = new FeedyDbContext())
                    {
                        db.Events.Add(window.Event);
                        db.SaveChanges();
                    }
                }

                else
                {
                    return;
                }

                EventID = window.Event.EventID;
            }

            else
            {
                EventID = (int)args.EventID;
            } 
               
            //Get Event from Database in order to get all the navigational properties filled out.
            db.Events.Load();
            Event Event = db.Events.Local.Single(ev => ev.EventID == EventID);

            TabItem Tab;

            Tab = new TabItem();
            Tab.Uid = TabIdCounter.ToString();
            Tab.Header = string.Format("Neue Dateneingabe {0}", tabsCount - 2);

            var EventDataCollectionPage = new SampleCollectionPage(Event, db ,Tab.Uid);

            //Add Controls to Page
            EventDataCollectionPage.CloseTabEvent += CloseTab;
            EventDataCollectionPage.OnEventsContentChange += EventsPage.RefreshTable;



            Frame frame = new Frame();
            frame.Content = EventDataCollectionPage;

            Tab.Content = frame;
            Tabs.Insert(tabsCount - 1, Tab);
            tabControl.SelectedItem = Tab;

        }
        

        private void CloseTab(object sender, CloseTabEventArgs e)
        {
            // closes the tab that has the tabUid specified by the page of the sender
            BasePage page = sender as BasePage;

            TabItem Tab = Tabs.Single(t => t.Uid == page.TabUid);


            tabControl.SelectedItem = Tabs.First();
            Tabs.Remove(Tab);
           
            
        }
       
    }

}
