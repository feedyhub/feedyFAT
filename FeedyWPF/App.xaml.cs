using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FeedyWPF.Windows;

namespace FeedyWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string ErrorMessage;

            ErrorMessage = "An unhandled exception just occurred: " + Environment.NewLine + e.Exception.Message + Environment.NewLine + Environment.NewLine;

            if(e.Exception.InnerException != null)
            {
                Exception ex = e.Exception.InnerException;
                while (ex != null)
                {
                    ErrorMessage += ex.Message + Environment.NewLine + Environment.NewLine;
                    ex = ex.InnerException;
                }
            }
          

            CopyableMessageBox MsgBox = new CopyableMessageBox(ErrorMessage);
            //MsgBox.ShowDialog();

        }

    }
}
