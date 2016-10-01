using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using FeedyWPF.Models;
using System.Data.Entity;

namespace FeedyWPF.Windows
{
    /// <summary>
    /// Interaction logic for CreateQuestionnaireWindow.xaml
    /// </summary>
    public partial class CreateQuestionnaireWindow : Window
    {
        public CreateQuestionnaireWindow()
        {
            
            InitializeComponent();
            ViewModel = new Questionnaire();
            DataContext = ViewModel;

        }

        private Questionnaire ViewModel { get; set; }

        public Questionnaire Questionnaire { get { return ViewModel; } }

       
        private void CreateQuestionnaireButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsModelValid())
            {
                DialogResult = true;

                Close();
            }
        }

        private bool IsModelValid()
        {
            bool IsValid = false;

            if (ViewModel.Name != string.Empty)
            {
                IsValid = true;
            }

            else
            {
                MessageBox.Show("Der Name des Fragebogens muss angegeben werden!");
            }

            return IsValid;
        }
    }
}
