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

namespace FeedyWPF.Pages
{


    /// <summary>
    /// Interaction logic for EvaluationPage.xaml
    /// </summary>
    public partial class EvaluationPage : Page
    {

        public EvaluationPage(FeedyDbContext database)
        {
            InitializeComponent();
            db = database;
        }



        public EvaluationPage(Evaluation evaluation, FeedyDbContext database)
        {

            InitializeComponent();
            db = database;
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
            OnCloseTabEvent(this, new CloseTabEventArgs());
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            TextExport export = new TextExport(Evaluation);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file(*.txt) | *.txt";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                export.Write(dialog.FileName);
            }

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }
    }
}
