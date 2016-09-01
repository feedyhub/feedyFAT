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
            
            //Print evaluations
            foreach(var question in evaluation.QuestionEvaluations)
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

                            switch (question.EvalMode)
                            {
                                case EvaluationMode.ABSOLUTE:
                                    Lines.Add((answer.AbsoluteEvaluation.Value.ToString() + "   " + (answer.AbsoluteEvaluation.AnswerText)));
                                    break;
                                case EvaluationMode.PERCENTAGE:
                                    Lines.Add((answer.PercentageEvaluation.DisplayValue + "   " + (answer.PercentageEvaluation.AnswerText)));
                                    break;

                        }
                    }
                }
               

                Lines.Add("");
                
            }

            string[] result = Lines.ToArray();
            return result;
        }
    }
}
