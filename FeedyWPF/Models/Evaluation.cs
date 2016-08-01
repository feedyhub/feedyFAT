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

namespace FeedyWPF.Models
{
    //public class Selector
    //{

    //    public int QuestionnaireID { get; set; }

    //    public List<SelectableEvent> Events { get; set; }
    //    public List<SelectableQuestion> Questions { get; set; }
    //}
    //public class SelectableEvent
    //{
    //    //        public SelectableEvent(Event myEvent)
    //    //        {
    //    //            Place = myEvent.Place;
    //    //            ReferencedEventID = myEvent.EventID;
    //    //            IsSelected = false;

    //    //        }
    //    public string Place { get; set; }
    //    public bool IsSelected { get; set; }
    //    public int ReferencedEventID { get; set; }
    //}

    //public class SelectableQuestion
    //{
    //    //        public SelectableQuestion(Question question)
    //    //        {
    //    //            Text = question.Text;
    //    //            ReferencedQuestionID = question.QuestionID;
    //    //            IsSelected = false;

    //    //        }
    //    public string Text { get; set; }
    //    public bool IsSelected { get; set; }
    //    public int ReferencedQuestionID { get; set; }
    //}

    public class Evaluation
    {
        //        //Constructors
        //        public Evaluation() { }
        //        public Evaluation(List<Event> events, List<Question> questionSelection)
        //        {
        //            this.Events = events;
        //            this.Questions = questionSelection;
        //        }

        //Primary Key
        public int EvaluationID { get; set; }

        //Properties
        public string Description { get; set; }
        private FeedyDbContext db = new FeedyDbContext();


        //Navigational Properties
        public virtual ObservableCollection<Event> Events { get; set; }
        public virtual ObservableCollection<Question> Questions { get; set; }

        //Memberfunctions
        public List<QuestionEvaluation> ExecuteQuery()
        {
            //            //Executes Query according to Events and Questions

            //            // Get data of events, fill into EvalQuestions for selected Questions. For each, call a new Questionevaluation.
            List<QuestionEvaluation> Evaluations = new List<QuestionEvaluation>();

            //            Question EvaluationQuestion = new Question();

            //            int ParticipantsCount = Events.Select(e => e.ParticipantsCount).Sum();

            //            // Select from database each question with data from wished events.
            //            foreach (var question in Questions)
            //            {

            //            foreach ( var answer in question.Answers)
            //            {

            //                    if (answer.CountDataSet != null)
            //                    {
            //                        IEnumerable<CountData> CountDataSet =
            //                                               from countdata in answer.CountDataSet
            //                                               where Events.Select(e => e.EventID).Contains(countdata.EventID)
            //                                               select countdata;

            //                        question.Answers.Single(a => a.AnswerID == answer.AnswerID).CountDataSet = CountDataSet.ToList();
            //                    }


            //                    if (answer.TextDataSet != null)
            //                    {
            //                        IEnumerable<TextData> TextDataSet =
            //                                               from textdata in answer.TextDataSet
            //                                               where Events.Select(e => e.EventID).Contains(textdata.EventID)
            //                                               select textdata;

            //                        question.Answers.Single(a => a.AnswerID == answer.AnswerID).TextDataSet = TextDataSet.ToList();
            //                    }
            //                    else
            //                        answer.Question.EvalMode = EvaluationMode.ABSOLUTE;

            //                }

            //            Evaluations.Add(new QuestionEvaluation(question, ParticipantsCount));
            //            }
            return Evaluations;
        }


    }


    public class QuestionEvaluation
    {
        //        public QuestionEvaluation(Question question, int participantsCount)
        //        {
        //            // for each answer in question create corresponding evaluations.
        //            QuestionName = question.Text;
        //            EvalMode = question.EvalMode;
        //            Question = question;
        //            ParticipantsCount = participantsCount;

        //            EvaluateQuestion();
        //        }

        public void EvaluateQuestion()
        {
            //            AnswerEvaluations = new List<AnswerEvaluation>();

            //            foreach (var answer in Question.Answers)
            //            {
            //                if (Question.EvalMode == EvaluationMode.MEAN_VALUE)
            //                {
            //                    AnswerEvaluations.Add(new MeanValueEvaluation(Question, ParticipantsCount));
            //                }
            //                else
            //                {
            //                    switch (Question.EvalMode)
            //                    {
            //                        case EvaluationMode.ABSOLUTE:
            //                            AnswerEvaluations.Add(new AbsoluteEvaluation(answer));
            //                            break;
            //                        case EvaluationMode.PERCENTAGE:
            //                            AnswerEvaluations.Add(new PercentageEvaluation(answer, ParticipantsCount));
            //                            break;

            //                        case EvaluationMode.TEXT:
            //                                AnswerEvaluations.Add(new TextEvaluation(answer));

            //                            break;
            //                        default:
            //                            AnswerEvaluations.Add(new AbsoluteEvaluation(answer));
            //                            break;
            //                    }
            //                }
        }





        public string QuestionName { get; set; }
        public Question Question { get; set; }
        public int ParticipantsCount { get; set; }
        public EvaluationMode EvalMode { get; set; }
        public List<AnswerEvaluation> AnswerEvaluations { get; set; }
    }

    public class AnswerEvaluation
    {

    }
    public class TextEvaluation : AnswerEvaluation
    {
        //        public TextEvaluation(Answer answer)
        //        {
        //            TextAnswers = answer.TextDataSet.Select(t => t.Text).ToList();
        //}

        public List<string> TextAnswers { get; set; }
    }

    public class AbsoluteEvaluation : AnswerEvaluation
    {
        //        public AbsoluteEvaluation(Answer answer)
        //        {
        //            Value = answer.CountDataSet.Select(c => c.Count).Sum();
        //            AnswerText = answer.Text;
        //        }

        public int Value { get; set; }
        public string AnswerText { get; set; }
    }

    public class PercentageEvaluation : AnswerEvaluation
    {
        //        private Answer answer;


        //        public PercentageEvaluation(Answer answer, int participantsCount)
        //        {
        //            Value = (double)answer.CountDataSet.Select(c => c.Count).Sum() / participantsCount;
        //            AnswerText = answer.Text;
        //        }

        //        public double Value { get; set; }
        //        public string AnswerText { get; set; }
    }

    public class MeanValueEvaluation : AnswerEvaluation
        {
            public MeanValueEvaluation(Question Question, int participantsCount)
            {
                //            EvaluationLabel = "Mittelwert:";
                //            int AnswerCount = Question.Answers.Count;
                //            List<Answer> Answers = Question.Answers.ToList();

                //            // calc Value
                //            Value = 0;
                //            for (int i = 0; i < AnswerCount; ++i)
                //            {
                //                Value += (i + 1) * Answers[i].CountDataSet.Select(c => c.Count).Sum();
                //            }

                //            Value = Value / participantsCount;

                //            FirstAnswer = Answers.First().Text;
                //            LastAnswer = Answers.Last().Text;

                //            FirstAnswerValue = 1;
                //            LastAnswerValue = AnswerCount;
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

        public class SetEvaluationModesViewModel
        {
            public List<Question> Questions { get; set; }
        }
    
}




