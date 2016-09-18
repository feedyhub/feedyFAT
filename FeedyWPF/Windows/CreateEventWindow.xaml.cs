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
    /// Interaction logic for CreateEventWindow.xaml
    /// </summary>
    public partial class CreateEventWindow : Window
    {
        public CreateEventWindow()
        {
            
            InitializeComponent();
            ViewModel = new CreateEventWindowViewModel();
            DataContext = ViewModel;

        }

        private CreateEventWindowViewModel ViewModel { get; set; }

        public Event Event { get { return ViewModel.Event; } }

        private void CreateEventButton_Click(object sender, RoutedEventArgs e)
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

            if(ViewModel.Event.Place != string.Empty)
            {
                if(ViewModel.QuestionnaireID != 0)
                {
                    IsValid = true;
                }

                else
                {
                    MessageBox.Show("Der Fragebogen muss angegeben werden!");
                }
            }

            else
            {
                MessageBox.Show("Der Ort muss angegeben werden!");
            }

            return IsValid;
        }
    }
}
