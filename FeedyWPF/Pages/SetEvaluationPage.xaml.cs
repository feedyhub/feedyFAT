using FeedyWPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FeedyWPF.Pages
{
    /// <summary>
    /// Interaction logic for SetEvaluationPage.xaml
    /// </summary>
    public partial class SetEvaluationPage : BasePage
    {
        public SetEvaluationPage(FeedyDbContext database, string tabName)
        {

            InitializeComponent();
            TabUid = tabName;
            db = database;
            
            ViewModel = new SetEvaluationPageModel();
            DataContext = ViewModel;

            eventViewSource = ((CollectionViewSource)(FindResource("eventViewSource")));
            questionViewSource = ((CollectionViewSource)(FindResource("questionViewSource")));

            
            
        }
        
        private FeedyDbContext db { get; set; }

        private SetEvaluationPageModel ViewModel { get; set; }
        private CollectionViewSource questionViewSource { get; set; }
        private CollectionViewSource eventViewSource { get; set; }


        private BindingList<SelectEvent> SelectEvents { get; set; }
        private BindingList<SelectQuestion> SelectQuestions { get; set; }

        public delegate void EvaluationPageEventHandler(object sender, SetEvaluationPageEventArgs e);
        public event EvaluationPageEventHandler OnEvaluationPageEvent;

        private void evaluateButton_Click(object sender, RoutedEventArgs e)
        {
            var SelectedEvents = new List<Event>();
            
            foreach( var item in SelectEvents)
            {
                if (item.IsSelected)
                    SelectedEvents.Add(item);
            }

            var SelectedQuestions = new List<Question>();

            foreach (var item in SelectQuestions)
            {
                if (item.IsSelected)
                    SelectedQuestions.Add(item);
            }

            
            Evaluation Evaluation = new Evaluation(new ObservableCollection<Event>(SelectedEvents), new ObservableCollection<Question>(SelectedQuestions), db);

            SetEvaluationPageEventArgs args = new SetEvaluationPageEventArgs();
            args.Evaluation = Evaluation;
            OnEvaluationPageEvent(this, args);

            

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fills eventDataGrid and questionDataGrid corresponding to Selected Questionnaire

            SelectEvents = new BindingList<SelectEvent>();

            foreach (var element in db.Events.Local.Where(ev => ev.QuestionnaireID == ViewModel.QuestionnaireID))
            {
                SelectEvents.Add(new SelectEvent(element));
            }

            eventViewSource.Source = SelectEvents;


            SelectQuestions = new BindingList<SelectQuestion>();

            foreach (var element in db.Questions.Local.Where(q => q.QuestionnaireID == ViewModel.QuestionnaireID))
            {
                SelectQuestions.Add(new SelectQuestion(element));
            }

            questionViewSource.Source = SelectQuestions;
            
        }

        
        private void selectAllEvents_Click(object sender, RoutedEventArgs e)
        {
            // If »check all« is checked, check all, otherwise uncheck all.

            //var SelectEvents = eventViewSource.Source as BindingList<SelectEvent>;

            if (selectAllEvents.IsChecked == true)
            {
                foreach (var item in SelectEvents)
                {
                    item.IsSelected = true;
                }
            }


            if (selectAllEvents.IsChecked == false)
            {
                foreach (SelectEvent item in SelectEvents)
                {
                    item.IsSelected = false;
                }
            }

            
        }

        private void selectAllQuestions_Click(object sender, RoutedEventArgs e)
        {
            // If »check all« is checked, check all, otherwise uncheck all.

            //var SelectQuestions = questionViewSource.Source as BindingList<SelectQuestion>;

            if (selectAllQuestions.IsChecked == true)
            {
                foreach (var item in SelectQuestions)
                {
                    item.IsSelected = true;
                }
            }


            if (selectAllQuestions.IsChecked == false)
            {
                foreach (var item in SelectQuestions)
                {
                    item.IsSelected = false;
                }
            }
        }

        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseTabEvent(this, new CloseTabEventArgs());
        }
    }
}
