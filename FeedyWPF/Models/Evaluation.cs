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
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace FeedyWPF.Models
{
 
    public class Evaluation
    {
        //Constructors
        public Evaluation() { }
        public Evaluation(ObservableCollection<Event> eventSelection, ObservableCollection<Question> questionSelection, FeedyDbContext database)
        {
            db = new FeedyDbContext();

           
            // get new copies from database.
            db.Events.Load();
            this.Events = new ObservableCollection<Event> (db.Events.Local.Where(ev => eventSelection.Select(eve => eve.EventID).Contains(ev.EventID)));

            
            db.Questions.Load();
            this.Questions = new ObservableCollection<Question>( db.Questions.Local.Where(q => questionSelection.Select(qu => qu.QuestionID).Contains(q.QuestionID)));
            
            this.QuestionEvaluations = ExecuteQuery();

           
        }
        public Evaluation(Event Event, FeedyDbContext database)
        {
            db = new FeedyDbContext();

            // Evaluate full event
            db.Events.Load();
            this.Events = new ObservableCollection<Event>(db.Events.Local.Where(ev => ev.EventID == Event.EventID));

            
            //Events contains only one event
            this.Questions = Events.FirstOrDefault().Questionnaire.Questions;
            this.QuestionEvaluations = ExecuteQuery();
        }



        //Primary Key
        public int EvaluationID { get; set; }

        //Properties
        [NotMapped]
        private FeedyDbContext db { get; set; }
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

                    if (answer.BoolDataSet != null)
                    {
                        IEnumerable<BoolData> CountDataSet =
                                               from countdata in answer.BoolDataSet
                                               where Events.Select(e => e.EventID).Contains(countdata.EventID)
                                               select countdata;

                        // this deletes all other data, that is not needed right now. Entity is untracked, therefore possible.
                        question.Answers.Single(a => a.AnswerID == answer.AnswerID).BoolDataSet = new ObservableCollection<BoolData>(CountDataSet);
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

                

                Evaluations.Add(new QuestionEvaluation(question, ParticipantsCount, db));
            }
            return Evaluations;
        }

    }


    public class QuestionEvaluation : INotifyPropertyChanged
    {
        private FeedyDbContext db { get; set; }
        public QuestionEvaluation(Question question, int participantsCount, FeedyDbContext database)
        {
            db = database;
            // for each answer in question create corresponding evaluations.
            Text = question.Text;
            _evalMode = question.EvalMode;
            Question = question;
            ParticipantsCount = participantsCount;

            PropertyChanged += Question_PropertyChanged;
            EvaluateQuestion();

 
        }

        public string Text { get; set; }
        public Question Question { get; set; }
        public int ParticipantsCount { get; set; }

        public ObservableCollection<NumericEvaluation> AnswerEvaluations { get; set; }
        public ObservableCollection<MeanValueEvaluation> MeanValueEvaluations { get; set; }
        public ObservableCollection<TextEvaluation> TextEvaluations { get; set; }

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
            if (e.PropertyName == "EvalMode")
            {
                //Update Question and Re-Evaluate Question
                Question.EvalMode = EvalMode;

                EvaluateQuestion();
            }

        }

        public void EvaluateQuestion()
        {

             MeanValueEvaluations = new ObservableCollection<MeanValueEvaluation>();
             AnswerEvaluations = new ObservableCollection<NumericEvaluation>();
            TextEvaluations = new ObservableCollection<TextEvaluation>();

            MeanValueEvaluations.Add(new MeanValueEvaluation(Question, ParticipantsCount));

            foreach (var answer in Question.Answers)
            {
                AnswerEvaluations.Add(new NumericEvaluation(this.Question, answer, ParticipantsCount));

                if(answer.TextDataSet != null)
                {
                    foreach (var textData in answer.TextDataSet)
                    {
                        TextEvaluations.Add(new TextEvaluation(textData.Text));
                    }
                }
                
 
            }
     
        }
    }
       
    


   
    public class NumericEvaluation
    {
        public NumericEvaluation(Question question, Answer answer, int participantsCount)
        {
            Text = answer.Text;
            AbsoluteEvaluation = new AbsoluteEvaluation(answer);
            PercentageEvaluation = new PercentageEvaluation(answer, participantsCount);
        }

        public string Text { get; set; }
        public AbsoluteEvaluation AbsoluteEvaluation { get; set; }
        public PercentageEvaluation PercentageEvaluation { get; set; }
        
    }

    public class TextEvaluation
    {

        public TextEvaluation(string text)
        { 
            TextAnswer = text;
        }

        public string TextAnswer { get; set; }
    }

    public class AbsoluteEvaluation
    {
        public AbsoluteEvaluation(Answer answer)
        {
            Value = answer.BoolDataSet.Select(c => c.Value).Sum();
            AnswerText = answer.Text;
        }

        public int Value { get; set; }
        public string AnswerText { get; set; }
    }

    public class PercentageEvaluation
    {

        public PercentageEvaluation(Answer answer, int participantsCount)
        {
            Value = (double)answer.BoolDataSet.Select(c => c.Value).Sum() / participantsCount;
            DisplayValue = (Math.Round(Value, 3) * 100).ToString() + " %";
            AnswerText = answer.Text;
        }

        public double Value { get; set; }
        public string DisplayValue { get; set; }
        public string AnswerText { get; set; }
    }

    public class MeanValueEvaluation
    {
        public MeanValueEvaluation(Question Question, int participantsCount)
        {
            
            int AnswerCount = Question.Answers.Count;
            List<Answer> Answers = Question.Answers.ToList();

            // calc Value
            Value = 0;
            for (int i = 0; i < AnswerCount; ++i)
            {
                Value += (i + 1) * Answers[i].BoolDataSet.Select(c => c.Value).Sum();
            }

            Value = Math.Round(Value / participantsCount,2);

            FirstAnswerValue = 1;
            LastAnswerValue = AnswerCount;

            FirstAnswerDisplay = FirstAnswerValue +" "+ Answers.First().Text ;
            LastAnswerDisplay = LastAnswerValue + " " + Answers.Last().Text;

            
        }

        public string EvaluationLabel { get; set; }
        public double Value { get; set; }

        public string FirstAnswerDisplay { get; set; }
        public int FirstAnswerValue { get; set; }

        public string LastAnswerDisplay { get; set; }
        public int LastAnswerValue { get; set; }

        }

        public enum EvaluationMode
        {
            MEAN_VALUE, ABSOLUTE, PERCENTAGE, TEXT
        }

       
}




