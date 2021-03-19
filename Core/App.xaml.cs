using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;


namespace Core
{
    public partial class App : Application
    {
        public static string ComputerName = Environment.MachineName.ToString();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Process myProcess = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(pcProcess =>
                pcProcess.ProcessName == myProcess.ProcessName).Count();

            if (count > 1)
            {
                MessageBox.Show("Application is running...");
                App.Current.Shutdown();
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }

        private void PasswordBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            PasswordBox tb = (PasswordBox)sender;
            tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }
    }
}
