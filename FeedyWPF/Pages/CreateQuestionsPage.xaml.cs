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
    /// Interaction logic for CreateQuestionsPage.xaml
    /// </summary>
    public partial class CreateQuestionsPage : BasePage
    {
        public CreateQuestionsPage(string tabUid)
        {
            InitializeComponent();
            this.TabUid = tabUid;
            ViewModel = new CreateQuestionViewModel();
            DataContext = ViewModel;


        }

        private CreateQuestionViewModel ViewModel { get; set; }

        

        private void AddCreateQuestionButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var Sender = sender as Button;
            var SelectedCreateQuestion = Sender.DataContext as CreateQuestion;

            if (SelectedCreateQuestion != null)
            {
                int Index = ViewModel.CreateQuestions.IndexOf(SelectedCreateQuestion);
                ViewModel.CreateQuestions.Insert(Index+1, new CreateQuestion());
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var Sender = sender as Button;
            var SelectedCreateQuestion = Sender.DataContext as CreateQuestion;

            if (SelectedCreateQuestion != null)
            {
                if(SelectedCreateQuestion.Progress == CreateQuestionProgress.QUESTION_TYPE)
                {
                    SelectedCreateQuestion.Progress = CreateQuestionProgress.FILL_OUT;
                    
                }

                else if(SelectedCreateQuestion.Progress == CreateQuestionProgress.FILL_OUT)
                {
                    SelectedCreateQuestion.Progress = CreateQuestionProgress.FINISHED;
                }
              
            }
        }
    }
}
