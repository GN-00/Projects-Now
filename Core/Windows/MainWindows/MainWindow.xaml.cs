using System;
using Dapper;
using System.Collections.Generic;
using Core.Data;
using System.Data.SqlClient;
using Core.Data.UserData;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

namespace Core.Windows.MainWindows
{
    public partial class MainWindow : Window
    {
        public User UserData { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                UserData = connection.QueryFirstOrDefault<User>("Select * From [User].[_Users] Where Id = 0");//Test Only
                Constants.VAT = connection.QueryFirstOrDefault<double>($"Select * From [Finance].[_LastVAT]");
            }

            DataContext = UserData;

            var subWindow = new TendaringControl(this.UserData);
            Controls.Children.Add(subWindow);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //{
            //    UserController.ResetIDs(connection, UserData.UserID);
            //}
            //App.Current.Shutdown();
        }

        private void Trinding_Click(object sender, RoutedEventArgs e)
        {
            //Controls.Children.Clear();
            //Controls.Children.Add(new TendaringControl(this.UserData));
        }
        private void Projects_Click(object sender, RoutedEventArgs e)
        {
            //Controls.Children.Clear();
            //Controls.Children.Add(new ProjectsControl() { UserData = this.UserData, MainWindowData = this });
        }

        private void Store_Click(object sender, RoutedEventArgs e)
        {
            //Controls.Children.Clear();
            //Controls.Children.Add(new ItemsControl() { UserData = this.UserData, MainWindowData = this });
        }
        private void Finance_Click(object sender, RoutedEventArgs e)
        {
            //Controls.Children.Clear();
            //Controls.Children.Add(new FinanceControl() { UserData = this.UserData, MainWindowData = this });
        }


    }
}
