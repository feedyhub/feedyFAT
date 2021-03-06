﻿using System;
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

        public CreateQuestionViewModel(ObservableCollection<Question> Questions)
        {
            CreateQuestions = new ObservableCollection<CreateQuestion>();

            foreach (var question in Questions)
            {
                CreateQuestions.Add(new CreateQuestion(question));
                
            }

        }
        public ObservableCollection<CreateQuestion> CreateQuestions { get; set; }
        

    }

    [NotMapped]
    public class CreateQuestion : Question, INotifyPropertyChanged
    {
        public CreateQuestion()
        {
            int MaxNumberOfAnswers = 9;
            Text = "Wie lautet die Frage?";
            // possible numbers of answers: 1-9. Create numbers to fill combobox.
            NumberOfAnswersList = new ObservableCollection<int>();

            for (int i = 0; i < MaxNumberOfAnswers; ++i)
            {
                NumberOfAnswersList.Add(i + 1);
            }

            Progress = CreateQuestionProgress.QUESTION_TYPE;

            BackButtonEnabled = false;
            NextButtonEnabled = false;
            ComboIsEnabled = true;
            DeleteQuestionButtonVisible = Visibility.Visible;
        }

        public CreateQuestion(Question Question)
        {
          

            int MaxNumberOfAnswers = 9;
            // possible numbers of answers: 1-9. Create numbers to fill combobox.
            NumberOfAnswersList = new ObservableCollection<int>();

            for (int i = 0; i < MaxNumberOfAnswers; ++i)
            {
                NumberOfAnswersList.Add(i + 1);
            }

            Progress = CreateQuestionProgress.FINISHED;

            BackButtonEnabled = true;
            NextButtonEnabled = false;
            ComboIsEnabled = true;
            DeleteQuestionButtonVisible = Visibility.Collapsed;

            Text = Question.Text;
            EvalMode = Question.EvalMode;
            QuestionType = Question.QuestionType;
            Answers = Question.Answers;
            QuestionID = Question.QuestionID;
            Questionnaire = Question.Questionnaire;

        }



        public ObservableCollection<int> NumberOfAnswersList { get; }
        public Visibility VisibilityAnswers { get; set; }
        public Visibility DeleteQuestionButtonVisible { get; set; }

        private string _nextButtonString;
        public string NextButtonString
        {
            get
            {
                return _nextButtonString;
            }

            set
            {
                _nextButtonString = value;
                OnPropertyChanged("NextButtonString");
            }
        }

        private bool _nextButtonEnabled;
        public bool NextButtonEnabled
        {
            get { return _nextButtonEnabled; }
            set
            {
                _nextButtonEnabled = value;
                OnPropertyChanged("NextButtonEnabled");

            }
        }

        private bool _backButtonEnabled {get;set;}
        public bool BackButtonEnabled
        {
            get { return _backButtonEnabled; }
            set
            {
                _backButtonEnabled = value;

                OnPropertyChanged("BackButtonEnabled");
            }
        }

        private CreateQuestionProgress _progress { get; set; }
        public CreateQuestionProgress Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");

                if(value == CreateQuestionProgress.QUESTION_TYPE)
                {
                    NextButtonString = "Weiter";
                    BackButtonEnabled = false;
                }

                else if(value == CreateQuestionProgress.FINISHED)
                {
                    NextButtonEnabled = false;
                }

                else
                {
                    NextButtonString = "Fertig";
                    BackButtonEnabled = true;
                    NextButtonEnabled = true;
                }
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
                    NumberOfAnswers = 1;
                    ComboIsEnabled = false;

                    
                }
                else
                {
                    VisibilityAnswers = Visibility.Visible;
                    ComboIsEnabled = true;
                }

                

                OnPropertyChanged("QuestionType");
            }
        }            
           
        private bool _comboIsEnabled { get; set; }
        public bool ComboIsEnabled { get { return _comboIsEnabled; } set { _comboIsEnabled = value; OnPropertyChanged("ComboIsEnabled"); } }

        private int _numberOfAnswers { get; set; }
        public int NumberOfAnswers { get { return _numberOfAnswers; } set
            {
                _numberOfAnswers = value;

                if(NextButtonEnabled == false && value != 0)
                {
                    NextButtonEnabled = true; 
                }

                Answers = new ObservableCollection<Answer>();
                for(int i=0; i<value; ++i)
                {
                    Answers.Add(new Answer() { Text="Antwort "+ i});
                }

                OnPropertyChanged("NumberOfAnswers");
              
            } }

        

        public Question ToQuestion()
        {
            Question Question = new Question();

            Question.Text = this.Text;
            Question.QuestionType = this.QuestionType;
            Question.Answers = Answers;
            
            return Question;
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

    public enum CreateQuestionProgress
    {
        // Progress steps have to be ordered according to the process and may not be branched. See NextButton_Click in view!
        QUESTION_TYPE, FILL_OUT, FINISHED
    }

   
}
