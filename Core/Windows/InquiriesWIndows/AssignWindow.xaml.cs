using Dapper;
using System.Windows;
using System.Windows.Input;
using ProjectsNow.Database;
using System.Data.SqlClient;
using System.Windows.Controls;
using ProjectsNow.Controllers;

namespace Core.Windows.InquiriesWindows
{
    public partial class AssignWindow : Window
    {
        public User UserData { get; set; }
        public Inquiry InquiryData { get; set; }
        public Quotation QuotationData { get; set; }

        Inquiry newInquiryData;
        public AssignWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                SalesmanList.ItemsSource = SalesmanController.GetSalesmen(connection);

                EstimationList.ItemsSource = EstimationController.GetEstimation(connection);
            }

            if (InquiryData != null)
            {
                newInquiryData = new Inquiry();
                newInquiryData.Update(InquiryData);
            }
            else
            {
                newInquiryData = new Inquiry();
                newInquiryData.QuotationToInquiry(QuotationData);
            }

            DataContext = newInquiryData;

        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void EstimationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var EstimationData = (((ComboBox)sender).SelectedItem as Estimation);

            if (EstimationData != null)
            {
                newInquiryData.EstimationName = EstimationData.EstimationName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                var query = DatabaseAI.UpdateRecord<Inquiry>();
                connection.Execute(query, newInquiryData);

                if (InquiryData != null)
                {
                    InquiryData.Update(newInquiryData);
                }
                else
                {
                    QuotationData.SalesmanID = newInquiryData.SalesmanID;
                    QuotationData.EstimationID = newInquiryData.EstimationID;
                    QuotationData.EstimationName = newInquiryData.EstimationName;
                }
            }

            this.CloseWindow_Click(sender, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow_Click(sender, e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                UserData.InquiryID = null;
                UserController.UpdateInquiryID(connection, UserData);
            }
        }
    }
}
