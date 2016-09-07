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
            
            NumberOfAnswersList = new ObservableCollection<int>();

            for (int i = 0; i < 6; ++i)
            {
                NumberOfAnswersList.Add(i + 1);
            }

            Progress = CreateQuestionProgress.QUESTION_TYPE;
        }

        public ObservableCollection<AnswerRow> AnswerRows { get; set; }

        public ObservableCollection<int> NumberOfAnswersList { get; }

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
                OnPropertyChanged("QuestionType");
            }
        }
            
           

        private int _numberOfAnswers { get; set; }
        public int NumberOfAnswers { get { return _numberOfAnswers; } set
            {
                _numberOfAnswers = value;
                UpdateAnswerRows();
            } }

        private void UpdateAnswerRows()
        {
            if(AnswerRows == null)
            {
                AnswerRows = new ObservableCollection<AnswerRow>();
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
        public Visibility IsVisible { get; set; }
    }

    public enum CreateQuestionProgress
    {
        QUESTION_TYPE, FILL_OUT, FINISHED
    }

   
}
