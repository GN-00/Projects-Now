using System;
using Dapper;
using System.Linq;
using System.Windows;
using Core.Enums;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Core.Windows.MessageWindows;
using Core.Data.UserData;
using Core.Data.QuotationsData;
using Core.Data.CustomersData;
using Core.Data;
using Core.Data.InquiriesData;

namespace Core.Windows.QuotationsWindows
{
    public partial class QuotationWindow : Window
    {
        public User UserData { get; set; }
        public bool OpenPanelsWindow { get; set; }
        public Quotation QuotationData { get; set; }

        ObservableCollection<Customer> customers;
        ObservableCollection<Consultant> consultants;
        ObservableCollection<Contact> contacts;
        ObservableCollection<Contact> projectContacts;
        CollectionViewSource viewProjectContacts;

        Customer customerData;
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

                query = "Select * From Customer._Customers Order By Name";
                customers = new ObservableCollection<Customer>(connection.Query<Customer>(query));
                CustomerList.ItemsSource = customers;

                query = "Select * From [User].[_Salesmen] Order By Name";
                SalesList.ItemsSource = connection.Query<Salesman>(query);

                query = "Select * From [User].[_Estimators] Order By EstimatorName";
                EstimationList.ItemsSource = connection.Query<Estimator>(query);

                query = "Select * From [Customer].[_Consultants] Order By Name";
                consultants = new ObservableCollection<Consultant>(connection.Query<Consultant>(query));
                ConsultantList.ItemsSource = consultants;
            }

            if (OpenPanelsWindow) Cancel.Visibility = Visibility.Collapsed;

            viewProjectContacts.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            DataContext = new { newQuotationData, customerData, consultantData };
        }
        private void CustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerList.SelectedItem is Customer customer)
                DataContext = new { newQuotationData, customerData, consultantData };
        }
        private void Consultant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EstimationList.SelectedItem is Estimator estimatorData)
            {
                newQuotationData.Inquiry.EstimatorCode = estimatorData.EstimatorCode;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isNull = false;
            var message = "Please Enter:";
            if (newQuotationData.CustomerID == 0) { message += $"\n  Customer Name."; isNull = true; }
            if (newQuotationData.ProjectName == null || newQuotationData.ProjectName == "") { message += $"\n  Project Name."; isNull = true; }
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
                    string query;
                    query = Database.UpdateRecord<Inquiry>();
                    connection.Execute(query, newQuotationData);

                    query = Database.UpdateRecord<Quotation>();
                    connection.Execute(query, newQuotationData);
                }

                QuotationData.Update(newQuotationData);
                if (OpenPanelsWindow)
                {
                    var quotationPanelsWindow = new QuotationPanelsWindow()
                    {
                        UserData = this.UserData,
                        QuotationData = this.QuotationData
                    };
                    this.Close();
                    quotationPanelsWindow.ShowDialog();
                }
                else
                {
                    this.CloseWindow_Click(sender, e);
                }

            }
            else
            {
                CMessageBox.Show("Error", message, CMessageBoxButton.OK, CMessageBoxImage.Information);
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
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                UserData.QuotationId = UserData.InquiryId = null;
                connection.UserAccessUpdate(UserData, nameof(UserData.QuotationId));
                connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
            }
        }
    }
}
