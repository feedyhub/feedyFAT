using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedyWPF.Models
{
    class TextExport
    {
        public TextExport(Evaluation evaluation)
        {
            Output = Convert(evaluation);
            
        }
        
        private string[] Output { get; set; }
        public void Write(string path)
        {

            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(path, Output);

        }
        public string[] Convert(Evaluation evaluation)
        {
            List<string> Lines = new List<string>();

            //Choose IntroText on top of Textfile according to number of events.
            if(evaluation.Events.Count > 1)
            {
                var HeaderMultiple = string.Format("Auswertung der Umfragen {0}", evaluation.Questions.FirstOrDefault().Questionnaire.Name);
                string EventList = "Orte: ";
                foreach (var Event in evaluation.Events)
                {
                    EventList += Event.Place + ", ";
                }
                EventList.Remove(EventList.Length - 1, 1);

                Lines.Add(HeaderMultiple);
                Lines.Add("");
                Lines.Add(EventList);
                Lines.Add("");
            }
            else
            {
                var HeaderSingle = string.Format("Auswertung der Umfrage {0} in {1}", evaluation.Questions.FirstOrDefault().Questionnaire.Name, evaluation.Events.FirstOrDefault().Place);
                Lines.Add(HeaderSingle);
                Lines.Add("");
            }

            #region find maximum char length of displayvalues
            int MaxLengthAbsolute = 0;
            int MaxLengthPercentage = 0;
            foreach (var question in evaluation.QuestionEvaluations)
            {
                if (question.EvalMode == EvaluationMode.ABSOLUTE)
                {
                    
                    foreach (var answer in question.AnswerEvaluations)
                    {
                        if(answer.AbsoluteEvaluation.Value.ToString().Length > MaxLengthAbsolute)
                        {
                            MaxLengthAbsolute = answer.AbsoluteEvaluation.Value.ToString().Length;
                        }
                    }
                }

                if (question.EvalMode == EvaluationMode.PERCENTAGE)
                {

                    foreach (var answer in question.AnswerEvaluations)
                    {
                        if (answer.PercentageEvaluation.DisplayValue.Length > MaxLengthPercentage)
                        {
                            MaxLengthPercentage = answer.PercentageEvaluation.DisplayValue.Length;
                        }
                    }
                }


            }
            #endregion

            #region adapt gap size and write result lines
            //set gapsize between answertext and value corresponding to length of value in order to write nice columns
            string GapAbsolute ="";
            string GapPercentage="";
            int ExtraSpaces = 2;

            for(int i=0; i<MaxLengthAbsolute+ExtraSpaces; ++i)
            {
                GapAbsolute += " ";
            }

            for (int i = 0; i < MaxLengthPercentage + ExtraSpaces; ++i)
            {
                GapPercentage += " ";
            }

            //Print evaluations
            foreach (var question in evaluation.QuestionEvaluations)
            {
                Lines.Add(question.Question.Text);

                if (question.EvalMode == EvaluationMode.MEAN_VALUE)
                {
                    Lines.Add(question.MeanValueEvaluations.FirstOrDefault().Value.ToString());
                }

                else if(question.EvalMode == EvaluationMode.TEXT)
                {
                    foreach(var textAnswer in question.TextEvaluations)
                    {
                        Lines.Add(string.Format("- {0}", (textAnswer.TextAnswer)));
                    }

                }
                else
                {
                 

                    foreach (var answer in question.AnswerEvaluations)
                    {

                        if( question.EvalMode== EvaluationMode.ABSOLUTE)
                        {
                            string value = answer.AbsoluteEvaluation.Value.ToString();
                            string Gap = GapAbsolute.Remove(0, value.Length);
                            Lines.Add((value + Gap + (answer.AbsoluteEvaluation.AnswerText)));
                        }

                        if(question.EvalMode == EvaluationMode.PERCENTAGE)
                        {
                            string value2 = answer.PercentageEvaluation.DisplayValue;
                            string Gap2 = GapPercentage.Remove(0, value2.Length);
                            Lines.Add((value2 + Gap2 + (answer.PercentageEvaluation.AnswerText)));
                        }
                    }
                }

                #endregion

                Lines.Add("");
                
            }

            string[] result = Lines.ToArray();
            return result;
        }
    }
}
