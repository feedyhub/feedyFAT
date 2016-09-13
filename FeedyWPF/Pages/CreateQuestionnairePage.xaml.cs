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

namespace FeedyWPF.Pages
{
    /// <summary>
    /// Interaction logic for CreateQuestionnairePage.xaml
    /// </summary>
    public partial class CreateQuestionnairePage : BasePage
    {
        public CreateQuestionnairePage(string tabUid)
        {
            InitializeComponent();
            this.TabUid = tabUid;

            ViewModel = new Questionnaire();

            DataContext = ViewModel;
        }

        private Questionnaire ViewModel { get; set; }

        public delegate void SetCreateQuestionsPageEventHandler(object sender, SetCreateQuestionsPageEventArgs e);
        public event SetCreateQuestionsPageEventHandler OnSetCreateQuestionsPageEvent;


        private void CloseTabButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseTabEvent(this, new CloseTabEventArgs());
        }

        private void CreateQuestionsButton_Click(object sender, RoutedEventArgs e)
        {
            if(NameTextBox.Text == string.Empty)
            {
                MessageBox.Show("Es muss der Name der Umfrage angegeben werden!");
            }

            else
            {
                var arg = new SetCreateQuestionsPageEventArgs();
                arg.Questionnaire = ViewModel;
                OnSetCreateQuestionsPageEvent(this, arg);
            }

        }
    }
}
