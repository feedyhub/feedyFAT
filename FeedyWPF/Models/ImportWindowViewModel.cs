using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FeedyWPF.Models
{
    class ImportWindowViewModel : INotifyPropertyChanged
    {
        
        public ImportWindowViewModel()
        {
            // List to Populate ComboBox
            IList<Questionnaire> list = db.Questionnaires.ToList();
            Event = new Event();
            NewQuestionnaire = new Questionnaire();

            _questionnaireEntries = new CollectionView(list);
        }
        private FeedyDbContext db = new FeedyDbContext();

        private readonly CollectionView _questionnaireEntries;
        private int _questionnaireID;

        public CollectionView QuestionnaireEntries
        {
            get { return _questionnaireEntries; }
        }

        public int QuestionnaireID
        {
            get { return _questionnaireID; }
            set
            {
                if (_questionnaireID == value) return;
                _questionnaireID = value;
                OnPropertyChanged("QuestionnaireID");
            }
        }

        public Event Event { get; set; }
        public Questionnaire NewQuestionnaire { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    
    }
}
