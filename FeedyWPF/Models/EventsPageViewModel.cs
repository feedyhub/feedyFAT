using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace FeedyWPF.Models
{
    public class EventsPageViewModel : INotifyPropertyChanged
    {
        public EventsPageViewModel()
        {
            IList<Questionnaire> list = db.Questionnaires.ToList();
            Questionnaire NullQuest = new Questionnaire();
            NullQuest.Name = string.Empty;
            list.Add(NullQuest);
            _questionnaireEntries = new CollectionView(list);
        }
        FeedyDbContext db = new FeedyDbContext();

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

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }




}
