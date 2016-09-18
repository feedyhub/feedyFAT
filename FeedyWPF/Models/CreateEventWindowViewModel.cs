using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FeedyWPF.Models
{
    class CreateEventWindowViewModel : INotifyPropertyChanged
    {
        public CreateEventWindowViewModel()
        {
            Event = new Event();

            using(var db = new FeedyDbContext())
            {
                IList<Questionnaire> list = db.Questionnaires.ToList();
                _questionnaireEntries = new CollectionView(list);
            }
           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Event Event { get; set; }

        #region Entries for Questionnaire ComboBox
        private readonly CollectionView _questionnaireEntries;
        

        public CollectionView QuestionnaireEntries
        {
            get { return _questionnaireEntries; }
        }

        public int QuestionnaireID
        {
            get { return Event.QuestionnaireID; }
            set
            {
                if (Event.QuestionnaireID == value) return;
                Event.QuestionnaireID = value;
                OnPropertyChanged("QuestionnaireID");
            }
        }

        #endregion

      

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private void CreateEventButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
