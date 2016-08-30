using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedyWPF.Models
{
    class TextExport
    {
        public TextExport(Evaluation evaluation, string path)
        {
            Output = Convert(evaluation);
        }

        public string path { get; set; }
        public string[] Output { get; set; }
        public void Write()
        {
            
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(path, Output)
        }
        public string[] Convert(Evaluation evaluation)
        {
            List<string> Lines = new List<string>();
            
            foreach(var question in evaluation.QuestionEvaluations)
            {
                Lines.Add("<b>" + question.Question.Text + "</b>");

                foreach( var answer in question.AnswerEvaluations)
                {
                    switch (question.EvalMode)
                    {
                        case EvaluationMode.ABSOLUTE:
                            Lines.Add(((AbsoluteEvaluation)answer).Value.ToString() +" "+ ((AbsoluteEvaluation)answer).AnswerText);
                            break;
                        case EvaluationMode.PERCENTAGE:
                            Lines.Add(((PercentageEvaluation)answer).DisplayValue + " " + ((PercentageEvaluation)answer).AnswerText);
                            break;
                        case EvaluationMode.MEAN_VALUE:
                            Lines.Add(((MeanValueEvaluation)answer).EvaluationLabel + " " + ((MeanValueEvaluation)answer).FirstAnswer + "/" + ((MeanValueEvaluation)answer).LastAnswer);
                            Lines.Add(((MeanValueEvaluation)answer).Value.ToString() + " " + ((MeanValueEvaluation)answer).FirstAnswerValue.ToString() + "/" + ((MeanValueEvaluation)answer).LastAnswerValue.ToString());
                            break;
                        case EvaluationMode.TEXT:
                            Lines.Add(((TextEvaluation)answer).TextAnswer);
                            break;
                    }
                }
                Lines.Add("");
                
            }

            string[] result = Lines.ToArray();
            return result;
        }
    }
}
