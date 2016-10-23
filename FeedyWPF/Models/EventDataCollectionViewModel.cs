using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedyWPF.Models
{
    public class SampleCollectionPageViewModel
    {
        public SampleCollectionPageViewModel(Event Event)
        {
            ViewQuestions = new ObservableCollection<ViewQuestion>();

            foreach(var q in Event.Questionnaire.Questions)
            {
                ViewQuestions.Add(new ViewQuestion(q));
            }

            PageHeaderText = "Dateneingabe: " + Event.Questionnaire.Name + " in " + Event.Place;

        }

        public string PageHeaderText { get; set; }
        public ObservableCollection<ViewQuestion> ViewQuestions { get; set; }
    }



    public class ViewQuestion
    {
        public ViewQuestion(Question question)
        {
            QuestionID = question.QuestionID;
            QuestionType = question.QuestionType;
            Text = question.Text;


            ViewAnswers = new ObservableCollection<ViewAnswer>();
            foreach(var answer in question.Answers)
            {
                var answerSelector = new ViewAnswer(answer);

                
                answerSelector.AnswerSelectionToChange += SelectedAnswerChanged;
                ViewAnswers.Add(answerSelector);
            }


        }
        public int QuestionID { get; }
        public string Text { get; }
        public QuestionType QuestionType {get; }
        public ObservableCollection<ViewAnswer> ViewAnswers { get; }

        private void SelectedAnswerChanged(object sender, AnswerSelectionToChangeEventArgs args)
        {
            if(this.QuestionType == QuestionType.SINGLE_CHOICE)
            {
                var selectedAnswer = ViewAnswers.SingleOrDefault(a => a.IsChecked == true);


                if(selectedAnswer!= null)
                {
                    selectedAnswer.IsCheckedQuiet = false;
                }
                
            }
        }
        private Answer SelectedAnswer { get; set; }
    }

    public class ViewAnswer : INotifyPropertyChanged
    {
        public ViewAnswer(Answer answer)
        {
            AnswerID = answer.AnswerID;
            Text = answer.Text;
            IsChecked = false;
        }

        public int AnswerID { get; set; }
        public string Text { get; set; }
        public string TextAnswer { get; set; }

        public delegate void AnswerSelectionToChageEventHandler(object sender,  AnswerSelectionToChangeEventArgs args);
        public event AnswerSelectionToChageEventHandler AnswerSelectionToChange;

        private bool _isChecked { get; set; }

        // IsCheckedQuiet does not tirgger the AnswerSelectionToChage Event in order to avoid an infinite loop, but still lets the view update.
        public bool IsCheckedQuiet { set { _isChecked = value; OnPropertyChanged("IsChecked"); } }
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if(AnswerSelectionToChange != null)
                {
                    AnswerSelectionToChange(this, new AnswerSelectionToChangeEventArgs());
                }
               

                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectionToChage()
        {

        }
    }
}
