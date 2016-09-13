using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FeedyWPF.Models
{
    public class CreateQuestionViewModel
    {
        public CreateQuestionViewModel()
        {
            CreateQuestions = new ObservableCollection<CreateQuestion>();
            CreateQuestions.Add(new CreateQuestion());
   
        }
        public ObservableCollection<CreateQuestion> CreateQuestions { get; set; }
        

    }

    [NotMapped]
    public class CreateQuestion : Question, INotifyPropertyChanged
    {
        public CreateQuestion()
        {
            
            // possible numbers of answers: 1-6. Create numbers to fill combobox.
            NumberOfAnswersList = new ObservableCollection<int>();

            for (int i = 0; i < 6; ++i)
            {
                NumberOfAnswersList.Add(i + 1);
            }

            Progress = CreateQuestionProgress.QUESTION_TYPE;

            BackButtonEnabled = false;
            NextButtonEnabled = false;
        }

        public ObservableCollection<AnswerRow> AnswerRows { get; set; }



        public ObservableCollection<int> NumberOfAnswersList { get; }
        public Visibility VisibilityAnswers { get; set;}

        private Boolean _nextButtonEnabled { get; set; }
        public Boolean NextButtonEnabled
        {
            get { return _nextButtonEnabled; }
            set
            {
                _nextButtonEnabled = value;
                OnPropertyChanged("NextButtonEnabled");
            }
        }

        public Boolean BackButtonEnabled { get; set; }

        private CreateQuestionProgress _progress { get; set; }
        public CreateQuestionProgress Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public override QuestionType QuestionType
        {
            get
            {
                return base.QuestionType;
            }

            set
            {
                base.QuestionType = value;

                if(value == QuestionType.TEXT)
                {
                    VisibilityAnswers = Visibility.Collapsed;
                }
                else
                {
                    VisibilityAnswers = Visibility.Visible;
                }

                OnPropertyChanged("QuestionType");
            }
        }
            
           

        private int _numberOfAnswers { get; set; }
        public int NumberOfAnswers { get { return _numberOfAnswers; } set
            {
                _numberOfAnswers = value;

                if(NextButtonEnabled == false && value != 0)
                {
                    NextButtonEnabled = true;
                }
                


                UpdateAnswerRows();
            } }

        public Question ToQuestion()
        {
            Question Question = new Question();

            Question.Text = this.Text;
            Question.QuestionType = this.QuestionType;

            // Map Answerrows to single Answers List.
           var Answers = new ObservableCollection<Answer>();

            Answer Answer;
            foreach( var row in AnswerRows)
            {
                if(row.ColumnOne != null)
                {
                    Answer = new Answer();
                    Answer.Text = row.ColumnOne.Text;
                    Answers.Add(Answer);
                }
                
                if(row.ColumnTwo != null)
                {
                    Answer = new Answer();
                    Answer.Text = row.ColumnTwo.Text;
                    Answers.Add(Answer);
                }

                if (row.ColumnThree != null)
                {
                    Answer = new Answer();
                    Answer.Text = row.ColumnThree.Text;
                    Answers.Add(Answer);
                }
            }

            Question.Answers = Answers;
            


            return Question;
        }

        private void UpdateAnswerRows()
        {
            //creates the required number of answers that will be edited in the view. 
            //It is necessary to map them to the format of the AnswerRow in order to 
            //be able to lay them out nicely in the UI.
            #region do nasty work
            if(AnswerRows == null)
            {
                AnswerRows = new ObservableCollection<AnswerRow>();
            }

            else
            {
                AnswerRows.Clear();
            }

            for(int i=0; i < NumberOfAnswers; ++i)
            {
                if(i%3 == 0)
                {
                    AnswerRows.Add(new AnswerRow());

                    AnswerRows.Last().ColumnOne = new VisibleAnswer(Visibility.Visible);

                }

                if(i%3 == 1)
                {
                    AnswerRows.Last().ColumnTwo = new VisibleAnswer(Visibility.Visible);
                }

                if (i % 3 == 2)
                {
                    AnswerRows.Last().ColumnThree = new VisibleAnswer(Visibility.Visible);
                }    
            }

            foreach(var row in AnswerRows)
            {
                if(row.ColumnOne == null)
                {
                    row.ColumnOne = new VisibleAnswer(Visibility.Hidden);
                }
                if (row.ColumnTwo == null)
                {
                    row.ColumnTwo = new VisibleAnswer(Visibility.Hidden);
                }
                if (row.ColumnThree == null)
                {
                    row.ColumnThree = new VisibleAnswer(Visibility.Hidden);
                }
            }
            #endregion
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged (string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class AnswerRow
    {
       public VisibleAnswer ColumnOne { get; set; }
       public Answer ColumnTwo { get; set; }
       public Answer ColumnThree { get; set; }
    }

    [NotMapped]
    public class VisibleAnswer : Answer
    {
        public VisibleAnswer(Visibility isVisible)
        {
            IsVisible = isVisible;
        }
        public Visibility IsVisible { get;}
    }

    public enum CreateQuestionProgress
    {
        // Progress steps have to be ordered according to the process and may not be branched. See NextButton_Click in view!
        QUESTION_TYPE, FILL_OUT, FINISHED
    }

   
}
