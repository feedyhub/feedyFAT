using FeedyWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FeedyWPF.Pages
{
    public partial class BasePage : Page
    {
        public string TabUid { get; set; }

        public delegate void CloseTabEventHandler(object sender, CloseTabEventArgs e);
        

        public event EventHandler<CloseTabEventArgs> CloseTabEvent;

      

        //The event-invoking method that derived classes can override.
        public virtual void OnCloseTabEvent(object sender, CloseTabEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            CloseTabEvent?.Invoke(this, e);
        }
    }
}
