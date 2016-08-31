using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.Windows;

namespace FeedyWPF.Models
    {
       public class Event
    {
        
        // Primary Key
        public int EventID { get; set; }
        
        [Required]
        public string Place { get; set; }

        public int ParticipantsCount { get; set; }
      
        
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }
        
        [NotMapped]
        private string DateString { get { return Date.Value.ToString(); } }
        

        /** TODO: validate File extension **/
       
       
       // public HttpPostedFileBase SourceFile { get; set; }

        //Foreign Key
        [Required]
        public int QuestionnaireID { get; set; }

        //Navigational Properties
        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ObservableCollection<CountData> NumericDatas { get; set; }
        public virtual ObservableCollection<TextData> TextDatas { get; set; }
        public virtual ObservableCollection<Evaluation> Querys { get; set; }


    }

    public class Questionnaire
        {
            //Primary key
            public int QuestionnaireID { get; set; }
            public int dummy { get; set; }
            public string Name { get; set; }

            
            public string Comments { get; set; }

            //Navigation Properties
            public virtual ObservableCollection<Question> Questions { get; set; }
            public virtual ObservableCollection<Event> Events { get; set; }

        }

        public class Question
        {
            //Constructors
            public Question() { }
       
        public Question(string QuestionText) { this.Text = QuestionText; }
            //Primary Key
            public int QuestionID { get; set; }

            public string Text { get; set; }

        
            public EvaluationMode EvalMode { get; set; }

            //Foreign Key
            public int QuestionnaireID { get; set; }
           

            //Navigation Property
            public virtual Questionnaire Questionnaire { get; set; }
            public virtual ObservableCollection<Answer> Answers { get; set; }
            public virtual ObservableCollection<Evaluation> Evaluations { get; set; }


        }

        public class Answer
        {

            public Answer() { }
            public Answer(string AnswerText) { this.Text = AnswerText; }
            //Primary Key
            public int AnswerID { get; set; }

            public string Text { get; set; }
            

            //Foreign Key
            public int QuestionId { get; set; }

            //Navigation Property
            public virtual Question Question { get; set; }
            public virtual ObservableCollection<TextData> TextDataSet { get; set; }
            public virtual ObservableCollection<CountData> CountDataSet { get; set; }

        }

    public class CountData
    {
        public CountData() { }
        public CountData(int Count) { this.Count = Count; }
        //Primary Key
        public int CountDataID { get; set; }

        public int Count { get; set; }

        //Foreign Key
        public int AnswerID { get; set; }
        public int EventID { get; set; }

        //Navigation Property
        public virtual Answer Answer { get; set; }
        public virtual Event Event { get; set; }
    }

        
        public class TextData
    {
        public TextData() { }
        public TextData(string value) { Text = value; }

        //Primary Key
        public int TextDataID { get; set; }

        public string Text { get; set; }

        //Foreign Key
        public int AnswerID { get; set; }
        public int EventID { get; set; }

        //Navigation Property
        public virtual Answer Answer { get; set; }
        public virtual Event Event { get; set; }

    }

        

       

    

}