using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for TabWindow.xaml
    /// </summary>
    public partial class TabWindow : Window
    {
        public TabWindow()
        {
            InitializeComponent();

            //TabItem tabitem = new TabItem();
            //tabitem.Header = "Tab one";
            //Frame tabFrame = new Frame();
            //TabView page1 = new TabView();
            //tabFrame.Content = page1;
            //tabitem.Content = tabFrame;
            //tabControl.Items.Add(tabitem);


        }
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
