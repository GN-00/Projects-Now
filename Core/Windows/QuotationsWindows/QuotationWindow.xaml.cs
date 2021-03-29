using Dapper;
using Core.Data;
using System.Linq;
using System.Windows;
using Core.Data.UserData;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.CustomersData;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;
using Core.Windows.MessageWindows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Core.Windows.QuotationsWindows
{
    public partial class QuotationWindow : Window
    {
        public User UserData { get; set; }
        public bool OpenPanelsWindow { get; set; }
        public Quotation QuotationData { get; set; }
        public PanelsWindow PanelsWindow { get; set; }

        ObservableCollection<Contact> projectContacts;
        CollectionViewSource viewProjectContacts;

        Customer customerData;
        Salesman salesmanData;
        Estimator estimatorData;
        Consultant consultantData;
        Quotation newQuotationData = new Quotation();
        public QuotationWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            newQuotationData.Update(QuotationData);
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query = $"Select * From [Quotation].[_ProjectsContactsView] Where InquiryId = {newQuotationData.Inquiry.Id}";
                projectContacts = new ObservableCollection<Contact>(connection.Query<Contact>(query));
                viewProjectContacts = new CollectionViewSource { Source = projectContacts };
                ProjectContactsList.ItemsSource = viewProjectContacts.View;

                query = $"Select * From Customer._Customers Where Id = {newQuotationData.Inquiry.CustomerId}";
                customerData = connection.QueryFirstOrDefault<Customer>(query);

                query = $"Select * From [User].[_Salesmen] Where Id = {newQuotationData.Inquiry.SalesmanId}";
                salesmanData = connection.QueryFirstOrDefault<Salesman>(query);

                query = $"Select * From [User].[_Estimators] Where Id = {newQuotationData.Inquiry.EstimatorId}";
                estimatorData = connection.QueryFirstOrDefault<Estimator>(query);

                query = $"Select * From [Customer].[_Consultants] Where Id = {newQuotationData.Inquiry.ConsultantId}";
                consultantData = connection.QueryFirstOrDefault<Consultant>(query);
            }

            if (OpenPanelsWindow) Cancel.Visibility = Visibility.Collapsed;

            viewProjectContacts.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            DataContext = new { newQuotationData, customerData, consultantData, salesmanData, estimatorData };
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isNull = false;
            var message = "Please Enter:";
            if (newQuotationData.PowerVoltage == null || newQuotationData.PowerVoltage == "") { message += $"\n  Power Voltage."; isNull = true; }
            if (newQuotationData.Phase == null || newQuotationData.Phase == "") { message += $"\n  Phase."; isNull = true; }
            if (newQuotationData.Frequency == null || newQuotationData.Frequency == "") { message += $"\n  Frequency."; isNull = true; }
            if (newQuotationData.NetworkSystem == null || newQuotationData.NetworkSystem == "") { message += $"\n  Network System."; isNull = true; }
            if (newQuotationData.ControlVoltage == null || newQuotationData.ControlVoltage == "") { message += $"\n  Control Voltage."; isNull = true; }
            if (newQuotationData.TinPlating == null || newQuotationData.TinPlating == "") { message += $"\n  Tin Plating."; isNull = true; }
            if (newQuotationData.NeutralSize == null || newQuotationData.NeutralSize == "") { message += $"\n  Neutral Size."; isNull = true; }
            if (newQuotationData.EarthSize == null || newQuotationData.EarthSize == "") { message += $"\n  Earth Size."; isNull = true; }
            if (newQuotationData.EarthingSystem == null || newQuotationData.EarthingSystem == "") { message += $"\n  Earthing System."; isNull = true; }

            if (!isNull)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Update<Quotation>(newQuotationData);
                }

                QuotationData.Update(newQuotationData);
                if (OpenPanelsWindow)
                {
                    var panelsWindow = new PanelsWindow()
                    {
                        UserData = this.UserData,
                        QuotationData = this.QuotationData
                    };
                    this.Close();
                    panelsWindow.ShowDialog();
                }
                else
                {
                    this.Cancel_Click(sender, e);
                }

            }
            else
            {
                MessageWindow.Show("Error", message, MessageWindowButton.OK, MessageWindowImage.Information);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = ProjectContactsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Contacts: {viewProjectContacts.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewProjectContacts.View.Cast<object>().Count()}";
        }
        private void ProjectContactsList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedIndex = ProjectContactsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Contacts: {viewProjectContacts.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewProjectContacts.View.Cast<object>().Count()}";
        }
        private void Default_Click(object sender, RoutedEventArgs e)
        {
            PowerVoltage.Text = "220-380V";
            Phase.Text = "3P + N";
            Frequency.Text = "60Hz";
            NetworkSystem.Text = "AC";
            ControlVoltage.Text = "230V AC";
            TinPlating.Text = "Bare Copper";
            NeutralSize.Text = "Full of Phase";
            EarthSize.Text = "Half of Neutral";
            EarthingSystem.Text = "TNS";
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PanelsWindow == null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    UserData.QuotationId = UserData.InquiryId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.QuotationId));
                    connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
                }
            }
        }
    }
}
