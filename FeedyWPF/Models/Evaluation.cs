//using FeedyWPF.Models;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;

using FeedyWPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System;
using System.Windows;

namespace FeedyWPF.Models
{
 
    public class Evaluation
    {
        //Constructors
        public Evaluation() { }
        public Evaluation(ObservableCollection<Event> events, ObservableCollection<Question> questionSelection)
        {
            this.Events = events;
            this.Questions = questionSelection;

            this.QuestionEvaluations = ExecuteQuery();
        }



        //Primary Key
        public int EvaluationID { get; set; }

        //Properties
        [NotMapped]
        private FeedyDbContext db = new FeedyDbContext();
        [NotMapped]
        public ObservableCollection<QuestionEvaluation> QuestionEvaluations { get; set; }


        //Navigational Properties
        public virtual ObservableCollection<Event> Events { get; set; }
        public virtual ObservableCollection<Question> Questions { get; set; }

        //Memberfunctions
        public ObservableCollection<QuestionEvaluation> ExecuteQuery()
        {
            //Executes Query according to Events and Questions

            // Get data of events, fill into EvalQuestions for selected Questions. For each, call a new Questionevaluation.
            var Evaluations = new ObservableCollection<QuestionEvaluation>();

            Question EvaluationQuestion = new Question();

            int ParticipantsCount = Events.Select(e => e.ParticipantsCount).Sum();

            // Select from database each question with data from wished events.
            foreach (var question in Questions)
            {

                foreach (var answer in question.Answers)
                {

                    if (answer.CountDataSet != null)
                    {
                        IEnumerable<CountData> CountDataSet =
                                               from countdata in answer.CountDataSet
                                               where Events.Select(e => e.EventID).Contains(countdata.EventID)
                                               select countdata;

                        question.Answers.Single(a => a.AnswerID == answer.AnswerID).CountDataSet = new ObservableCollection<CountData>(CountDataSet);
                    }


                    if (answer.TextDataSet != null)
                    {
                        IEnumerable<TextData> TextDataSet =
                                               from textdata in answer.TextDataSet
                                               where Events.Select(e => e.EventID).Contains(textdata.EventID)
                                               select textdata;

                        question.Answers.Single(a => a.AnswerID == answer.AnswerID).TextDataSet = new ObservableCollection<TextData>(TextDataSet); 
                    }
                    else
                        answer.Question.EvalMode = EvaluationMode.ABSOLUTE;

                }

                Evaluations.Add(new QuestionEvaluation(question, ParticipantsCount));
            }
            return Evaluations;
        }

    }


    public class QuestionEvaluation : INotifyPropertyChanged
    {
        public QuestionEvaluation(Question question, int participantsCount)
        {
            // for each answer in question create corresponding evaluations.
            QuestionName = question.Text;
            _evalMode = question.EvalMode;
            Question = question;
            ParticipantsCount = participantsCount;

            PropertyChanged += Question_PropertyChanged;
            EvaluateQuestion();
        }

        public string QuestionName { get; set; }
        public Question Question { get; set; }
        public int ParticipantsCount { get; set; }
        public ObservableCollection<AnswerEvaluation> AnswerEvaluations { get; set; }
    
        

        private EvaluationMode _evalMode;
        public EvaluationMode EvalMode
        { get { return _evalMode; } set { _evalMode = value; OnPropertyChanged("EvalMode"); } }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

                   
                
           


        private void Question_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "EvalMode")
            {
                EvaluateQuestion();
            }

        }

        public void EvaluateQuestion()
                {

            AnswerEvaluations = new ObservableCollection<AnswerEvaluation>();

            foreach (var answer in Question.Answers)
            {
                if (Question.EvalMode == EvaluationMode.MEAN_VALUE)
                {
                    AnswerEvaluations.Add(new MeanValueEvaluation(Question, ParticipantsCount));
                }

                else
                {
                    switch (Question.EvalMode)
                    {
                        case EvaluationMode.ABSOLUTE:
                            AnswerEvaluations.Add(new AbsoluteEvaluation(answer));
                            break;
                        case EvaluationMode.PERCENTAGE:
                            AnswerEvaluations.Add(new PercentageEvaluation(answer, ParticipantsCount));
                            break;

                        case EvaluationMode.TEXT:
                            foreach(var textData in answer.TextDataSet)
                            {
                                AnswerEvaluations.Add(new TextEvaluation(textData.Text));
                            }
                            

                            break;
                        default:
                            AnswerEvaluations.Add(new AbsoluteEvaluation(answer));
                            break;
                    }
                }
            }
        }

      
    }

    public class AnswerEvaluation
    {

    }

    public class TextEvaluation : AnswerEvaluation
    {
        public TextEvaluation(string text)
        {

            TextAnswer = text;
        }

        public string TextAnswer { get; set; }
    }

    public class AbsoluteEvaluation : AnswerEvaluation
    {
        public AbsoluteEvaluation(Answer answer)
        {
            Value = answer.CountDataSet.Select(c => c.Count).Sum();
            AnswerText = answer.Text;
        }

        public int Value { get; set; }
        public string AnswerText { get; set; }
    }

    public class PercentageEvaluation : AnswerEvaluation
    {

        public PercentageEvaluation(Answer answer, int participantsCount)
        {
            Value = (double)answer.CountDataSet.Select(c => c.Count).Sum() / participantsCount;
            AnswerText = answer.Text;
        }

        public double Value { get; set; }
        public string AnswerText { get; set; }
    }

    public class MeanValueEvaluation : AnswerEvaluation
        {
            public MeanValueEvaluation(Question Question, int participantsCount)
            {
                EvaluationLabel = "Mittelwert:";
                int AnswerCount = Question.Answers.Count;
                List<Answer> Answers = Question.Answers.ToList();

                // calc Value
                Value = 0;
                for (int i = 0; i < AnswerCount; ++i)
                {
                    Value += (i + 1) * Answers[i].CountDataSet.Select(c => c.Count).Sum();
                }

                Value = Value / participantsCount;

                FirstAnswer = Answers.First().Text;
                LastAnswer = Answers.Last().Text;

                FirstAnswerValue = 1;
                LastAnswerValue = AnswerCount;
        }

            public string EvaluationLabel { get; set; }
            public double Value { get; set; }

            public string FirstAnswer { get; set; }
            public int FirstAnswerValue;

            public string LastAnswer { get; set; }
            public int LastAnswerValue;
        }

        public enum EvaluationMode
        {
            MEAN_VALUE, ABSOLUTE, PERCENTAGE, TEXT
        }

       
}




