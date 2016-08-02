using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FeedyWPF.Models
{
    class SetEvaluationPageModel : INotifyPropertyChanged
    {
        FeedyDbContext db = new FeedyDbContext();
        public SetEvaluationPageModel ()
            {
            List<Questionnaire> Quests = db.Questionnaires.ToList();
            _questionnaireEntries = new CollectionView(Quests);
            
    }
        private readonly CollectionView _questionnaireEntries;
        public CollectionView QuestionnaireEntries { get { return _questionnaireEntries; } }

        private int _questionnaireID { get; set; }
        public int QuestionnaireID {
            get { return _questionnaireID; }
            set
            {
                if (_questionnaireID == value) return;
                _questionnaireID = value;
                OnPropertyChanged("QuestionnaireID");
            }
        }
       
        public List<Questionnaire> SelectableQuestionnaires { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    class SelectQuestion : Question, INotifyPropertyChanged
    {
        public SelectQuestion() { }
        public SelectQuestion(Question baseQuestion)
        {
            this.QuestionID = baseQuestion.QuestionID;
            this.EvalMode = baseQuestion.EvalMode;
            this.Text = baseQuestion.Text;
            this.Questionnaire = baseQuestion.Questionnaire;
            this.QuestionnaireID = baseQuestion.QuestionnaireID;
            this.Answers = baseQuestion.Answers;
        }

        private bool _isSelected { get; set; }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    class SelectEvent : Event, INotifyPropertyChanged
    {
        private bool _isSelected;

        public SelectEvent() { }
        public SelectEvent (Event baseEvent)
        {
            this.EventID = baseEvent.EventID;
            this.Place = baseEvent.Place;
            this.ParticipantsCount = baseEvent.ParticipantsCount;
            this.Questionnaire = baseEvent.Questionnaire;
            this.TextDatas = baseEvent.TextDatas;
            this.NumericDatas = baseEvent.NumericDatas;
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
