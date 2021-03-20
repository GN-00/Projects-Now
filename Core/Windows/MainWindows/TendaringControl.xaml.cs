using Dapper;
using Core.Data;
using System.Windows;
using Core.Data.UserData;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.Generic;
using Core.Windows.InquiriesWindows;
//using ProjectsNow.Windows.InquiryWindows;
//using ProjectsNow.Windows.CustomerWindows;
//using ProjectsNow.Windows.QuotationWindows;
//using ProjectsNow.Windows.QuotationWindows.QuotationsInformationWindows;

namespace Core.Windows.MainWindows
{
    public partial class TendaringControl : UserControl
    {
        public User UserData { get; set; }

        int newProjects;
        public TendaringControl()
        {
            InitializeComponent();
        }

        public TendaringControl(User userData)
        {
            InitializeComponent();
            UserData = userData;
            List<StackPanel> buttons = new List<StackPanel>();
            if (UserData.AccessInquiries == true) buttons.Add(Inquiries);
            if (UserData.AccessQuote == true)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    //newProjects = connection.QueryFirstOrDefault<int>($"Select * From User._UsersNewProjects Where UserId = {UserData.Id}");
                }
                buttons.Add(NewProjects);
                buttons.Add(Quote);
            }
            if (UserData.AccessQuotations == true) buttons.Add(QuotationsInformation);
            if (UserData.AccessCustomers == true) buttons.Add(Customers);
            if (UserData.AccessConsultants == true) buttons.Add(Consultants);

            if (buttons.Count == 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int ii = 0; ii < 2; ii++)
                    {
                        ResourcesArea.Children.Remove(buttons[ii + (i * 2)]);
                        ((StackPanel)FindName($"Area{i}")).Children.Add(buttons[ii + (i * 2)]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int ii = 0; ii < 3; ii++)
                    {
                        if ((ii + (i * 3)) >= buttons.Count) break;
                        ResourcesArea.Children.Remove(buttons[ii + (i * 3)]);
                        ((StackPanel)FindName($"Area{i}")).Children.Add(buttons[ii + (i * 3)]);
                    }
                }
            }

            DataContext = new { newProjects };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Inquiries_Click(object sender, RoutedEventArgs e)
        {
            var inquiriesWindow = new InquiriesWindow() { UserData = this.UserData };
            inquiriesWindow.ShowDialog();
            if (UserData.AccessQuote == true)
            {
                //using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                //    newProjects = InquiryController.CountNewInquiries(connection, UserData.UserID);

                //DataContext = new { newProjects };
            }
        }

        private void Quote_Click(object sender, RoutedEventArgs e)
        {
            //var quoteWindow = new QuoteWindow() { UserData = this.UserData };
            //quoteWindow.ShowDialog();
            //if (UserData.AccessQuote == true)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        newProjects = InquiryController.CountNewInquiries(connection, UserData.UserID);

            //    DataContext = new { newProjects };
            //}
        }

        private void Quotation_Click(object sender, RoutedEventArgs e)
        {
            //var quotationsWindow = new QuotationsWindow() { UserData = this.UserData };
            //quotationsWindow.ShowDialog();
            //if (UserData.AccessQuote == true)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        newProjects = InquiryController.CountNewInquiries(connection, UserData.UserID);

            //    DataContext = new { newProjects };
            //}
        }

        private void Customers_Click(object sender, RoutedEventArgs e)
        {
            //var customersWindow = new CustomersWindow() { UserData = this.UserData };
            //customersWindow.ShowDialog();
            //if (UserData.AccessQuote == true)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        newProjects = InquiryController.CountNewInquiries(connection, UserData.UserID);

            //    DataContext = new { newProjects };
            //}
        }

        private void Consultants_Click(object sender, RoutedEventArgs e)
        {
            //var consultantsWindow = new ConsultantsWindow() { UserData = this.UserData };
            //consultantsWindow.ShowDialog();
            //if (UserData.AccessQuote == true)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        newProjects = InquiryController.CountNewInquiries(connection, UserData.UserID);

            //    DataContext = new { newProjects };
            //}
        }

        private void QuotationInfromation_Click(object sender, RoutedEventArgs e)
        {
            //var quotationsWindow = new QuotationsInformationWindow() { UserData = this.UserData };
            //quotationsWindow.ShowDialog();
            //if (UserData.AccessQuote == true)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        newProjects = InquiryController.CountNewInquiries(connection, UserData.UserID);

            //    DataContext = new { newProjects };
            //}
        }
    }
}
