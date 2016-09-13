using FeedyWPF.Models;
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

namespace FeedyWPF.Pages
{
    /// <summary>
    /// Interaction logic for QuestionnairesPage.xaml
    /// </summary>
    public partial class QuestionnairesPage : Page
    {
        public QuestionnairesPage()
        {
            InitializeComponent();
            QuestionnaireViewSource = ((CollectionViewSource)(FindResource("questionnaireViewSource")));

            db.Questionnaires.Load();
            QuestionnaireViewSource.Source = db.Questionnaires.Local;
           

        }

        private FeedyDbContext db = new FeedyDbContext();
        private CollectionViewSource QuestionnaireViewSource;

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
           
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var SelectedQuestionnaire = row.DataContext as Questionnaire;
                    if (SelectedQuestionnaire != null)
                        db.Questionnaires.Remove(SelectedQuestionnaire);
                    break;
                }
            db.SaveChanges();
            questionnaireDataGrid.Items.Refresh();
            
           

           
        }
    }
}
