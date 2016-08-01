using FeedyWPF.Models;
using System.Collections.Generic;
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
    public partial class SetEvaluationPage : Page
    {
        public SetEvaluationPage()
        {

            InitializeComponent();

            SetEvaluationPageModel vm = new SetEvaluationPageModel();
            DataContext = vm;
        }
        private FeedyDbContext db = new FeedyDbContext();

        private CollectionViewSource questionViewSource { get; set; }
        private CollectionViewSource eventViewSource { get; set; }

        private void evaluateButton_Click(object sender, RoutedEventArgs e)
        {
           //New Evaluation
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fills eventDataGrid and questionDataGrid corresponding to Selected Questionnaire

            SetEvaluationPageModel dataContext = DataContext as SetEvaluationPageModel;

            eventViewSource = ((CollectionViewSource)(FindResource("eventViewSource")));

            db.Events
                .Where(ev => ev.QuestionnaireID==dataContext.QuestionnaireID)
                .Load();

            BindingList<SelectEvent> SelectEvents = new BindingList<SelectEvent>();

            foreach (var element in db.Events.Local)
            {
                SelectEvents.Add(new SelectEvent(element));
            }

            eventViewSource.Source = SelectEvents;

            questionViewSource = ((CollectionViewSource)(FindResource("questionViewSource")));

            db.Questions
                .Where(q => q.QuestionnaireID == dataContext.QuestionnaireID)
                .Load();

            var SelectQuestions = new BindingList<SelectQuestion>();

            foreach (var element in db.Questions.Local)
            {
                SelectQuestions.Add(new SelectQuestion(element));
            }

            questionViewSource.Source = SelectQuestions;
            
        }

        
        private void selectAllEvents_Click(object sender, RoutedEventArgs e)
        {
            // If »check all« is checked, check all, otherwise uncheck all.

            var SelectEvents = eventViewSource.Source as BindingList<SelectEvent>;

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

            var SelectQuestions = questionViewSource.Source as BindingList<SelectQuestion>;

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
    }
}
