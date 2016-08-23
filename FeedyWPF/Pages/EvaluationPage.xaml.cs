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

namespace FeedyWPF.Pages
{
    

    /// <summary>
    /// Interaction logic for EvaluationPage.xaml
    /// </summary>
    public partial class EvaluationPage : Page
    {
        public Evaluation Evaluation { get; set; }

        public EvaluationPage()
        {
            InitializeComponent();
           
        }

        public EvaluationPage(Evaluation evaluation)
        {
            InitializeComponent();
            Evaluation = evaluation;

            DataContext = Evaluation;
        }

        

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
        }
    }
}
