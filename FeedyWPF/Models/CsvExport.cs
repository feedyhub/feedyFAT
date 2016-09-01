using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedyWPF.Models
{
    class CsvExport
    {

        public CsvExport(Evaluation evaluation, string delimiter)
        {
            Delimiter = delimiter;
            if (Delimiter != null)
            {
                Output = Convert(evaluation);

            }
            
            
        }

        private string[] Output { get; set; }
        private string Delimiter { get; set; }
        public void Write(string path)
        {

            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(path, Output);

        }
        public string[] Convert(Evaluation evaluation)
        {
            var Result = new List<string>();


            StringBuilder Line;

            foreach(var qEval in evaluation.QuestionEvaluations)
            {
                Result.Add(qEval.Text);



                //AnswerTexts
                Line = new StringBuilder();
                foreach (var aEval in qEval.AnswerEvaluations)
                {
                    Line.Append(aEval.Text + Delimiter);      
                }
                Line.Remove(Line.Length - 1, 1);
                Result.Add(Line.ToString());

                //AnswerData
                Line = new StringBuilder();
                foreach (var aEval in qEval.AnswerEvaluations)
                {
                    Line.Append(aEval.AbsoluteEvaluation.Value + Delimiter);
                }
                Line.Remove(Line.Length - 1, 1);
                Result.Add(Line.ToString());

                Result.Add("");
            }


            return Result.ToArray();
        }
    }
}
