﻿using System;
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
        

        //Foreign Key
        [Required]
        public int QuestionnaireID { get; set; }

        //Navigational Properties
        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ObservableCollection<Participant> Participants { get; set; }


    }

    public class Questionnaire
    {
        //Primary key
        public int QuestionnaireID { get; set; }
            
        public string Name { get; set; }
        public string Comments { get; set; }

        [NotMapped]
        public int EventsCount
        {
            get
            {
                if (Events != null)
                {
                    return Events.Count;
                }

                else return 0;
            }
        }

        //Navigation Properties
        public virtual ObservableCollection<Question> Questions { get; set; }
        public virtual ObservableCollection<Event> Events { get; set; }

    }

    public class Question
    {
            //Constructors
            public Question()
            {
                // Default value
                EvalMode = EvaluationMode.ABSOLUTE;
            }
       
            public Question(string QuestionText) { this.Text = QuestionText; EvalMode = EvaluationMode.ABSOLUTE; }

            //Primary Key
            public int QuestionID { get; set; }

            public string Text { get; set; }
            public virtual QuestionType QuestionType { get; set; }
            public EvaluationMode EvalMode { get; set; }

            //Foreign Key
            public int QuestionnaireID { get; set; }
           

            //Navigation Property
            public virtual Questionnaire Questionnaire { get; set; }
            public virtual ObservableCollection<Answer> Answers { get; set; }

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
            public virtual ObservableCollection<BoolData> BoolDataSet { get; set; }

        }

    public class BoolData
    {
        public BoolData() { }
        public BoolData(bool value) { this.Value = value; }
        //Primary Key
        public int BoolDataID { get; set; }

        public bool Value { get; set; }

        //Foreign Key
        public int AnswerID { get; set; }
        public int ParticipantID { get; set; }

        //Navigation Property
        public virtual Answer Answer { get; set; }
        public virtual Participant Participant { get; set; }
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
        public int ParticipantID { get; set; }

        //Navigation Property
        public virtual Answer Answer { get; set; }
        public virtual Participant Participant { get; set; }

    }

    public class Participant
    {
        public int ParticipantID { get; set; }

        //Foreign Key
        public int EventID { get; set; }

        //Navigational Property
        public virtual Event Event { get; set; }
        public virtual ObservableCollection<BoolData> CountDatas { get; set; }
        public virtual ObservableCollection<TextData> TextDatas { get; set; }
    }

    public enum QuestionType
    {
        MULTIPLE_CHOICE, SINGLE_CHOICE, TEXT
    }

       

    

}