using Dapper;
using Core.Data;
using System.Windows;
using Core.Data.UserData;
using System.Data.SqlClient;
using Core.Data.InquiriesData;
using System.Windows.Controls;
using Dapper.Contrib.Extensions;

namespace Core.Windows.InquiriesWindows
{
    public partial class AssignWindow : Window
    {
        public User UserData { get; set; }
        public Inquiry InquiryData { get; set; }

        Inquiry newInquiryData;
        public AssignWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query = "Select * From [User].[_Salesmen] Order By Name";
                SalesmanList.ItemsSource = connection.Query<Salesman>(query);
                
                query = "Select * From [User].[_Estimators] Order By EstimatorName";
                EstimationList.ItemsSource = connection.Query<Estimator>(query);
            }

            newInquiryData = new Inquiry();
            newInquiryData.Update(InquiryData);

            DataContext = newInquiryData;
        }
        
        private void EstimationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(EstimationList.SelectedItem is Estimator estimator)
            {
                newInquiryData.EstimatorCode = estimator.EstimatorCode;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Update<Inquiry>(newInquiryData);

                InquiryData.Update(newInquiryData);
            }

            this.Cancel_Click(sender, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                UserData.InquiryId = null;
                connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
            }
        }
    }
}
