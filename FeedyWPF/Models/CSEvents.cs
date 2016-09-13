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

    public class SetCreateQuestionsPageEventArgs : EventArgs
    {
        public Questionnaire Questionnaire { get; set; }
    }

    public class SetEvaluationPageEventArgs : EventArgs
    {
        public Evaluation Evaluation { get; set; }
    }

    public class CloseTabEventArgs : EventArgs
    {

    }

}
