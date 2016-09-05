using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FeedyWPF.Windows
{
    /// <summary>
    /// Interaction logic for CopyableMessageBox.xaml
    /// </summary>
    public partial class CopyableMessageBox : Window
    {
        public CopyableMessageBox(string message)
        {
            
            InitializeComponent();
            this.Message = message;
            DataContext = this;

        }

        public string Message { get; set; }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
