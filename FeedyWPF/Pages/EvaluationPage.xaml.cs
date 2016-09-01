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
using System.Windows.Forms;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace FeedyWPF.Pages
{


    /// <summary>
    /// Interaction logic for EvaluationPage.xaml
    /// </summary>
    public partial class EvaluationPage : Page
    {

      
        public EvaluationPage(Evaluation evaluation)
        {
            
            InitializeComponent();
           
            Evaluation = evaluation;

            DataContext = Evaluation;
        }

        public delegate void CloseTabEventHandler(object sender, CloseTabEventArgs e);
        public event CloseTabEventHandler OnCloseTabEvent;
        private FeedyDbContext db { get; set; }
        public Evaluation Evaluation { get; set; }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
        }

        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            SaveEvalModeChanges();
        }

        private void TextExportButton_Click(object sender, RoutedEventArgs e)
        {
            TextExport export = new TextExport(Evaluation);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file(*.txt) | *.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                export.Write(dialog.FileName);
            }

        }
        private void CsvExportButton_Click(object sender, RoutedEventArgs e)
        {
            var export = new CsvExport(Evaluation,",");

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV file(*.csv) | *.csv";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                export.Write(dialog.FileName);
            }
        }

        private void SaveEvalModeChanges()
        {
            using (FeedyDbContext context = new FeedyDbContext())
            {
                context.Questions.Load();
                ObservableCollection<Question> ChangedQuestions = new ObservableCollection<Question>(context.Questions.Local.Where(q => Evaluation.Questions.Select(qu => qu.QuestionID).Contains(q.QuestionID)));

                Question RefQuestion;

                foreach (var question in ChangedQuestions)
                {
                    RefQuestion = Evaluation.Questions.Single(q => q.QuestionID == question.QuestionID);
                    question.EvalMode = RefQuestion.EvalMode;
                    context.Entry(question).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
        }

       
    }
}
