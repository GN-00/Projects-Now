using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Core.Windows.UsreWIndows
{
    public partial class LoginWindow : Window
    {
        //User userData;
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //if (userData != null)
            //{
            //    MainWindow mainWindow = new MainWindow()
            //    {
            //        UserData = this.userData,
            //    };
            //    this.Close();
            //    mainWindow.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Login Error", "Incorrect username or password", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }
        private void UserKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtPassword.Focus();
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login_Click(sender, e);
        }
    }
}
