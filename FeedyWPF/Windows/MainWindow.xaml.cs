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
            // Every time a new tab is created, counter goes ++1.
           

            tabControl.DataContext = Tabs;

            // Set EventsPage as first Tab
            TabItem tabitem = new TabItem();
            tabitem.Header = "Umfragen";

            tabitem.Uid = TabIdCounter.ToString();
            

            Frame eventsFrame = new Frame();
            EventsPage eventsPage = new EventsPage(db);

            eventsPage.OnEvaluationPageEvent += new EventsPage.EvaluationPageEventHandler(setEvaluationTab);
            
            eventsFrame.Content = eventsPage;
            tabitem.Content = eventsFrame;
            Tabs.Add(tabitem);

            // Set QuestionnairesPage as second Tab
            tabitem = new TabItem();
            tabitem.Header = "Fragebögen";

            tabitem.Uid = TabIdCounter.ToString();


             var frame = new Frame();

            QuestionnairesPage = new QuestionnairesPage();

            QuestionnairesPage.OnSetCreateQuestionnairePageEvent += new QuestionnairesPage.SetCreateQuestionnairePageEventHandler(setCreateQuestionnaireTab);


            frame.Content = QuestionnairesPage;
            tabitem.Content = frame;
            Tabs.Add(tabitem);

            //wire up content change events between eventspage and questionnaires page
            QuestionnairesPage.OnEventsContentChange += new QuestionnairesPage.ContentUpdateHandler(eventsPage.RefreshTable);
            eventsPage.OnQuestionnairesContentChange += new EventsPage.ContentUpdateHandler(QuestionnairesPage.RefreshTable);


            //Add + Tab to create new Tab
            PlusTab = new TabItem();
            PlusTab.Header = "+";

            Tabs.Add(PlusTab);

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

        private int tabsCount { get { return Tabs.Count; } }

        private void NewTab()
        {
          

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("Neue Auswertung {0}", tabsCount - 1);
            tab.Uid = TabIdCounter.ToString();
            


            // add controls to tab item
            Frame frame = new Frame();
            SetEvaluationPage page = new SetEvaluationPage(db,tab.Uid);
            

            //to get from setevaluationpage to evaluationpage
            page.OnEvaluationPageEvent += new SetEvaluationPage.EvaluationPageEventHandler(setEvaluationTab);
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
                    // add new tab
                    NewTab();
                }

               

            }
        }

        private void setEvaluationTab(object sender, SetEvaluationPageEventArgs e)
        {
            TabItem tab;

            //create replace setevaluationpage with evaluationpage
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

                evaluationPage.TabUid = tab.Uid;
                tab.Content = frame;
                Tabs.Insert(tabsCount - 1, tab);
                tabControl.SelectedItem = tab;
            } 
        }

       

        private void setCreateQuestionnaireTab(object sender, SetCreateQuestionnairePageEventArgs e)
        {
            TabItem Tab;

            Tab = new TabItem();
            Tab.Uid = TabIdCounter.ToString();
            Tab.Header = string.Format("Neuer Fragebogen {0}", tabsCount - 1);


            var CreateQuestionnairePage = new CreateQuestionnairePage(Tab.Uid);

            //Add Controls to Page
            CreateQuestionnairePage.CloseTabEvent += CloseTab;
            CreateQuestionnairePage.OnSetCreateQuestionsPageEvent += setCreateQuestionsPage;


            Frame frame = new Frame();
            frame.Content = CreateQuestionnairePage;

            CreateQuestionnairePage.TabUid = Tab.Uid;
            Tab.Content = frame;
            Tabs.Insert(tabsCount - 1, Tab);
            tabControl.SelectedItem = Tab;
        }

        private void setCreateQuestionsPage(object sender, SetCreateQuestionsPageEventArgs args)
        {
            var tab = new TabItem();
            if (sender is CreateQuestionnairePage)
            {

                var createQuestionnairePage = sender as CreateQuestionnairePage;
                tab = Tabs.Single(t => t.Uid == createQuestionnairePage.TabUid);


                var createQuestionsPage = new CreateQuestionsPage(tab.Uid,args.Questionnaire);

                //Add Controls to Page
                createQuestionsPage.CloseTabEvent += CloseTab;
                createQuestionsPage.OnQuestionnairesContentChange += new CreateQuestionsPage.ContentUpdateHandler(QuestionnairesPage.RefreshTable);


                Frame frame = new Frame();
                frame.Content = createQuestionsPage;


                tab.Content = frame;
            }
        }


        private void CloseTab(object sender, CloseTabEventArgs e)
        {

            BasePage page = sender as BasePage;

            TabItem Tab = Tabs.Single(t => t.Uid == page.TabUid);


            tabControl.SelectedItem = Tabs.First();
            Tabs.Remove(Tab);
           
            
        }
       
    }

}
