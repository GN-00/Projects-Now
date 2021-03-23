using System;
using Dapper;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using Core.Data.UserData;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using Core.Data.CustomersData;
using System.Windows.Controls;
using Core.Data.InquiriesData;
using Dapper.Contrib.Extensions;
using Core.Windows.MessageWindows;
using Core.Windows.CustomersWindows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Core.Windows.InquiriesWindows
{
    public partial class InquiryWindow : Window
    {
        public User UserData { get; set; }
        public Actions WindowMode { get; set; }
        public Inquiry InquiryData { get; set; }
        public ObservableCollection<Inquiry> InquiriesDataToUpdate { get; set; }

        bool isSaving = false;
        Customer customerData;
        Consultant consultantData;
        Inquiry newInquiryData = new Inquiry();

        ObservableCollection<Customer> customers;
        ObservableCollection<Contact> contacts;
        ObservableCollection<Contact> projectContacts;
        ObservableCollection<Consultant> consultants;

        CollectionViewSource viewProjectContacts;

        public InquiryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string query;
            newInquiryData.Update(InquiryData);

            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                if (WindowMode == Actions.New)
                {
                    query = $"Select MAX(RegisterNumber) as RegisterNumber From [Inquiry].[_Inquiries] Where RegisterYear = {DateTime.Now.Year}";
                    newInquiryData.RegisterNumber = connection.QueryFirstOrDefault<Inquiry>(query).RegisterNumber + 1;
                    newInquiryData.RegisterMonth = DateTime.Now.Month;
                    newInquiryData.RegisterYear = DateTime.Now.Year;
                    newInquiryData.RegisterCode =
                        $"ER-Inquiry{newInquiryData.RegisterNumber:000}/{newInquiryData.RegisterMonth}/{newInquiryData.RegisterYear.ToString().Substring(2, 2)}";

                    newInquiryData.Id = Convert.ToInt32(connection.Insert<Inquiry>(newInquiryData));
                }

                query = $"Select * From Inquiry._ProjectsContactsView Where InquiryId = {newInquiryData.Id} Order By Name";
                projectContacts = new ObservableCollection<Contact>(connection.Query<Contact>(query));
                viewProjectContacts = new CollectionViewSource { Source = projectContacts };
                ProjectContactsList.ItemsSource = viewProjectContacts.View;

                query = "Select * From Customer._Customers Order By Name";
                customers = new ObservableCollection<Customer>(connection.Query<Customer>(query));
                CustomerList.ItemsSource = customers;

                query = "Select * From [Customer].[_Consultants] Order By Name";
                consultants = new ObservableCollection<Consultant>(connection.Query<Consultant>(query));
                ConsultantList.ItemsSource = consultants;

                query = "Select * From [User].[_Salesmen] Order By Name";
                SalesList.ItemsSource = connection.Query<Salesman>(query);

                query = "Select * From [User].[_Estimators] Order By EstimatorName";
                EstimationList.ItemsSource = connection.Query<Estimator>(query);
            }

            DataContext = new { newInquiryData, customerData, consultantData };

            viewProjectContacts.View.CollectionChanged += new NotifyCollectionChangedEventHandler(DataGrid_CollectionChanged);

        }
        private void CustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CustomerList.SelectedItem is Customer customer)
            {
                customerData = customer;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    contacts = GetContacts(connection, newInquiryData.Id);
                    ContactsList.ItemsSource = contacts;

                    if (projectContacts.Count != 0)
                    {
                        var contactsCustomerId = (projectContacts.First()).CustomerId;
                        if (contactsCustomerId != customerData.Id)
                        {
                            connection.Execute($"Delete From [Inquiry].[_ProjectsContacts] Where InquiryId = {newInquiryData.Id}");
                            projectContacts.Clear();
                        }
                    }
                }
                newInquiryData.CustomerName = customerData.Name;
                DataContext = new { newInquiryData, customerData, consultantData };
            }
        }
        private void EstimationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EstimationList.SelectedItem is Estimator estimatorData)
            {
                newInquiryData.EstimatorCode = estimatorData.EstimatorCode;
            }
        }
        private void Consultant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ConsultantList.SelectedItem is Consultant consultant)
            {
                consultantData = consultant;
                DataContext = new { newInquiryData, customerData, consultantData };
            }
        }

        private void AddCustomers_Click(object sender, RoutedEventArgs e)
        {
            CustomersWindow customersWindow = new CustomersWindow()
            {
                UserData = this.UserData,
                RecordsData = customers,
            };
            customersWindow.ShowDialog();
        }
        private ObservableCollection<Contact> GetContacts(SqlConnection connection, int inquiryId)
        {
            string query = $"Select * From [Customer].[_Contacts] Where CustomerId = {customerData.Id} And Id Not In " +
                           $"(Select ContactId From [Inquiry].[_ProjectsContacts] Where InquiryId = {inquiryId})";
            
            var records = new ObservableCollection<Contact>(connection.Query<Contact>(query));
            return records;
        }
        private void AddContact_Click(object sender, RoutedEventArgs e)
        {
            if (customerData != null)
            {
                ContactsWindow contactsWindow = new ContactsWindow()
                {
                    UserData = this.UserData,
                    CustomerData = customerData,
                };
                contactsWindow.ShowDialog();

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    contacts = GetContacts(connection, newInquiryData.Id);
                    ContactsList.ItemsSource = contacts;
                }
            }
        }
        private void AddConsultant_Click(object sender, RoutedEventArgs e)
        {
            ConsultantsWindow consultantsWindow = new ConsultantsWindow()
            {
                UserData = this.UserData,
                RecordsData = consultants,
            };
            consultantsWindow.ShowDialog();
        }

        private void AddContactToPrject_Click(object sender, RoutedEventArgs e)
        {
            if(ContactsList.SelectedItem is Contact contact)
            {
                if (projectContacts.Count == 0)
                    contact.Attention = true;

                projectContacts.Add(contact);
                ProjectContactsList.ItemsSource = viewProjectContacts.View;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    var query = $"Insert Into [Inquiry].[_ProjectsContacts] " +
                                $"(InquiryId, ContactId, Attention) " +
                                $"Values " +
                                $"({newInquiryData.Id}, {contact.Id}, '{contact.Attention}')";
                    connection.Execute(query);
                }
                ContactsList.SelectedItem = null;
                contacts.Remove(contact);
            }
        }
        private void DuoDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            if (date == null)
            {
                picker.SelectedDate = DateTime.Now;
            }
            else
            {
                picker.SelectedDate = date.Value;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isReady = false;
            var message = "Please Enter:";
            if (newInquiryData.CustomerId == 0) { message += $"\n  Customer Name."; isReady = true; }
            if (newInquiryData.ProjectName == null || newInquiryData.ProjectName == "") { message += $"\n  Project Name."; isReady = true; }

            if (!isReady)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Update<Inquiry>(newInquiryData);
                }

                if (WindowMode == Actions.New)
                {
                    if (InquiriesDataToUpdate != null)
                        InquiriesDataToUpdate.Insert(0, newInquiryData);
                }
                else if (WindowMode == Actions.Edit)
                {
                    InquiryData.Update(newInquiryData);
                }

                isSaving = true;
                this.Close();
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

        private void ProjectContactsList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Delete_Contact(sender, e);
            }
        }
        private void Delete_Contact(object sender, RoutedEventArgs e)
        {
            if (ProjectContactsList.SelectedItem is Contact contact)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Execute($"Delete From [Inquiry].[_ProjectsContacts] Where " +
                                        $"InquiryId = {newInquiryData.Id} And " +
                                        $"ContactId = {contact.Id}");

                    ContactsList.SelectedItem = null;
                    contacts = GetContacts(connection, newInquiryData.Id);
                    ContactsList.ItemsSource = contacts;
                }
                projectContacts.Remove(ProjectContactsList.SelectedItem as Contact);
            }
        }
        private void ProjectContactsList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedIndex = ProjectContactsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Contacts: {viewProjectContacts.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewProjectContacts.View.Cast<object>().Count()}";
        }
        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = ProjectContactsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Contacts: {viewProjectContacts.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewProjectContacts.View.Cast<object>().Count()}";
        }

        private void Attention_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectContactsList.SelectedItem is Contact contact)
            {
                contact.Attention = true;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {

                    foreach (Contact contactData in projectContacts)
                    {
                        if (contactData.Id != contact.Id)
                            contactData.Attention = false;
                    }

                    var query = $"Update [Inquiry].[_ProjectsContacts] Set " +
                                $"Attention = @Attention " +
                                $"Where ContactId = @Id And InquiryId = {newInquiryData.Id};";
                    connection.Execute(query, projectContacts);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                if (WindowMode == Actions.New && isSaving == false)
                {
                    connection.Delete<Inquiry>(newInquiryData);

                    string query = $"Delete From [Inquiry].[_ProjectsContacts] Where InquiryId = {newInquiryData.Id}";
                    connection.Execute(query);
                }
                else
                {
                    UserData.InquiryId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
                }
            }
        }
    }
}
