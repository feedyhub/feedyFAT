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
            Output = Convert(evaluation);
            Delimiter = delimiter;
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

            //#region Line one of CSV File: Questions
            //StringBuilder LineOneBuilder = new StringBuilder();

            //foreach(var question in evaluation.Questions)
            //{
            //    LineOneBuilder.Append(question.Text);
            //    LineOneBuilder.Append(Delimiter);

            //    if(question.Answers.Count > 0)
            //    {
            //        for (int i = 0; i < question.Answers.Count - 1; ++i)
            //        {
            //            LineOneBuilder.Append(Delimiter);
            //        }
            //    }
            //}
            ////remove last Delimiter. Lines end without Delimiter
            //LineOneBuilder.Remove(LineOneBuilder.Length - 1, 1);

            //Result.Add(LineOneBuilder.ToString());
            //#endregion

            //#region Line Two: Answers
            //StringBuilder LineTwoBuilder = new StringBuilder();

            //foreach(var answer in evaluation.Questions.SelectMany(q => q.Answers))
            //{
            //    LineTwoBuilder.Append(answer.Text);
            //    LineTwoBuilder.Append(Delimiter);
            //}

            ////remove last Delimiter. Lines end without Delimiter
            //LineOneBuilder.Remove(LineTwoBuilder.Length - 1, 1);

            //Result.Add(LineTwoBuilder.ToString());
            //#endregion

            StringBuilder Line;

            foreach(var qEval in evaluation.QuestionEvaluations)
            {
                Result.Add(qEval.Text);

                Line = new StringBuilder();
                foreach(var aEval in qEval.AnswerEvaluations)
                {
                    Line.Append(aEval);

                }

            }


            return Result.ToArray();
        }
    }
}
