using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedyWPF.Models
{
    public class EventsContentChangedEventArgs : EventArgs
    {

    }

    public class QuestionnairesContentChangedEventArgs : EventArgs { }

    public class SetCreateQuestionnairePageEventArgs : EventArgs
    {
       

    }

    public class SetSampleCollectionPageEventArgs : EventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="EventID">set to null if new event shall be created. Otherwise, specify EventID of existing Event</param>
        public SetSampleCollectionPageEventArgs(int? eventID)
        {

            if(eventID == null)
            {
                CreateNewEvent = true;
            }

            else
            {
                CreateNewEvent = false;
                EventID = eventID;
            }
        }

       public bool CreateNewEvent { get; set;}
       public int? EventID { get; set; }
    }

    public class SetCreateQuestionsPageEventArgs : EventArgs
    {
        public Questionnaire Questionnaire { get; set; }
    }

    public class SetEvaluationPageEventArgs : EventArgs
    {
        public Evaluation Evaluation { get; set; }
    }

    public class AnswerSelectionToChangeEventArgs : EventArgs { }

    public class CloseTabEventArgs : EventArgs
    {

    }

}
