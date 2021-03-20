using System;
using Dapper;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Core.Enums;
using Core.Data.InquiriesData;
using Core.Data.UserData;
using Core.Data.QuotationsData;
using Core.Data;
using Dapper.Contrib.Extensions;
using Core.Data.CustomersData;

namespace Core.Windows.InquiriesWindows
{
    public partial class InquiryWindow : Window
    {
        public User UserData { get; set; }
        public Actions WindowMode { get; set; }
        public Inquiry InquiryData { get; set; }
        public Quotation QuotationData { get; set; }
        public ObservableCollection<Inquiry> InquiriesDataToUpdate { get; set; }

        bool isSaving = false;
        Customer customerData;
        Consultant consultantData;
        Inquiry newInquiryData = new Inquiry();
        List<int> contactsIDs = new List<int>();
        ObservableCollection<Customer> customers;
        CollectionViewSource viewProjectContacts;
        ObservableCollection<Contact> projectContacts;

        public InquiryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            newInquiryData.Update(InquiryData);

            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                if (WindowMode == Actions.New)
                {
                    newInquiryData.RegisterNumber = connection.QueryFirstOrDefault<Inquiry>($"Select MAX(RegisterNumber) as RegisterNumber From [Inquiry].[_Inquiries] Where RegisterYear = {DateTime.Now.Year}").RegisterNumber + 1;
                    newInquiryData.RegisterMonth = DateTime.Now.Month;
                    newInquiryData.RegisterYear = DateTime.Now.Year;
                    newInquiryData.RegisterCode =
                        $"ER-Inquiry{newInquiryData.RegisterNumber:000}/{newInquiryData.RegisterMonth}/{newInquiryData.RegisterYear.ToString().Substring(2, 2)}";

                    newInquiryData.Id = Convert.ToInt32(connection.Insert<Inquiry>(newInquiryData));
                }

                //projectContacts = ContactController.GetProjectContacts(connection, newInquiryData.InquiryID);
                //viewProjectContacts = new CollectionViewSource { Source = projectContacts };
                //ProjectContactsList.ItemsSource = viewProjectContacts.View;

                //foreach (Contact contact in ProjectContactsList.ItemsSource)
                //{
                //    contactsIDs.Add(contact.ContactID);
                //}

                //customers = CustomerController.GetCustomers(connection);
                //CustomerList.ItemsSource = customers;

                //SalesList.ItemsSource = SalesmanController.GetSalesmen(connection);

                //EstimationList.ItemsSource = EstimationController.GetEstimation(connection);

                //ConsultantList.ItemsSource = ConsultantController.GetConsultants(connection);
            }

            //DataContext = new { newInquiryData, customerData, consultantData };

            //viewProjectContacts.View.CollectionChanged += new NotifyCollectionChangedEventHandler(DataGrid_CollectionChanged);

        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //customerData = (((ComboBox)sender).SelectedItem as Customer);

            //if (customerData != null)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        ContactList.ItemsSource = ContactController.GetCustomerRemainingContacts(connection, newInquiryData.CustomerID, contactsIDs.ToArray());

            //        if (ProjectContactsList.Items.Count != 0)
            //        {
            //            var contactsCustomerID = (projectContacts.First()).CustomerID;
            //            if (contactsCustomerID != customerData.CustomerID)
            //            {
            //                connection.Execute($"Delete From [Inquiry].[ProjectsContacts] Where InquiryID = {newInquiryData.InquiryID}");
            //                projectContacts.Clear();
            //                contactsIDs.Clear();
            //            }
            //        }
            //    }

            //    newInquiryData.CustomerName = customerData.CustomerName;

            //}

            //DataContext = new { newInquiryData, customerData, consultantData };
        }
        private void EstimationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (EstimationList.SelectedItem is Estimation estimationData)
            //{
            //    newInquiryData.EstimationName = estimationData.EstimationName;
            //}
        }
        private void Consultant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //consultantData = (((ComboBox)sender).SelectedItem as Consultant);
            //DataContext = new { newInquiryData, customerData, consultantData };
        }

        private void AddCustomers_Click(object sender, RoutedEventArgs e)
        {
            //CustomersWindow customersWindow = new CustomersWindow()
            //{
            //    UserData = this.UserData,
            //    RecordsData = customers,
            //};
            //customersWindow.ShowDialog();
        }
        private void AddContact_Click(object sender, RoutedEventArgs e)
        {
            //if (customerData != null)
            //{
            //    ContactsWindow customersWindow = new ContactsWindow()
            //    {
            //        UserData = this.UserData,
            //        CustomerID = customerData.CustomerID,
            //        CustomerName = customerData.CustomerName,
            //        InquiryID = newInquiryData.InquiryID,
            //        ComboBoxToUpdate = ContactList,
            //        DataGridToUpadate = ProjectContactsList
            //    };
            //    customersWindow.ShowDialog();
            //}
        }
        private void AddConsultant_Click(object sender, RoutedEventArgs e)
        {
            //ConsultantsWindow consultantsWindow = new ConsultantsWindow()
            //{
            //    UserData = this.UserData,
            //    RecordsData = ConsultantList.ItemsSource as ObservableCollection<Consultant>,
            //};
            //consultantsWindow.ShowDialog();
        }

        private void AddContactToPrject_Click(object sender, RoutedEventArgs e)
        {
            //var contact = ContactList.SelectedItem as Contact;
            //if (contact != null)
            //{
            //    if (projectContacts.Count == 0)
            //        contact.Attention = true;

            //    projectContacts.Add(contact);
            //    ProjectContactsList.ItemsSource = viewProjectContacts.View;
            //    contactsIDs.Add(contact.ContactID);
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        var query = $"Insert Into [Inquiry].[ProjectsContacts] " +
            //                    $"(InquiryID, ContactID, Attention) " +
            //                    $"Values " +
            //                    $"({newInquiryData.InquiryID}, {contact.ContactID}, '{contact.Attention}')";
            //        connection.Execute(query);
            //    }
            //    ContactList.SelectedItem = null;
            //    var comboxlist = ContactList.ItemsSource as ObservableCollection<Contact>;
            //    comboxlist.Remove(contact);
            //}
        }
        private void DuoDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //var picker = sender as DatePicker;
            //DateTime? date = picker.SelectedDate;

            //if (date == null)
            //{
            //    picker.SelectedDate = DateTime.Now;
            //}
            //else
            //{
            //    picker.SelectedDate = date.Value;
            //}
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //bool isNull = false;
            //var message = "Please Enter:";
            //if (newInquiryData.CustomerID == 0) { message += $"\n  Customer Name."; isNull = true; }
            //if (newInquiryData.ProjectName == null || newInquiryData.ProjectName == "") { message += $"\n  Project Name."; isNull = true; }

            //if (!isNull)
            //{
            //    if (WindowMode == Actions.New)
            //    {
            //        using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        {
            //            var query = DatabaseAI.UpdateRecord<Inquiry>();
            //            connection.Execute(query, newInquiryData);

            //            UserData.InquiryID = null;
            //            UserController.UpdateInquiryID(connection, UserData);
            //        }

            //        if (InquiriesDataToUpdate != null)
            //        {
            //            InquiriesDataToUpdate.Insert(0, newInquiryData);
            //        }
            //    }
            //    else if (WindowMode == Actions.Edit)
            //    {
            //        using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        {
            //            var query = DatabaseAI.UpdateRecord<Inquiry>();
            //            connection.Execute(query, newInquiryData);

            //            UserData.InquiryID = null;
            //            UserController.UpdateInquiryID(connection, UserData);
            //        }

            //        InquiryData.Update(newInquiryData);

            //        if (QuotationData != null)
            //            QuotationData.Update(newInquiryData);
            //    }

            //    isSaving = true;
            //    this.Close();
            //}
            //else
            //{
            //    CMessageBox.Show("Error", message, CMessageBoxButton.OK, CMessageBoxImage.Information);
            //}
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProjectContactsList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Delete)
            //{
            //    if (ProjectContactsList.SelectedItem != null)
            //    {
            //        using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //        {
            //            connection.Execute($"Delete From [ProjectsContacts] Where " +
            //                                $"InquiryID = {newInquiryData.InquiryID} And " +
            //                                $"ContactID = {(ProjectContactsList.SelectedItem as Contact).ContactID}");

            //            contactsIDs.Remove((ProjectContactsList.SelectedItem as Contact).ContactID);
            //            ContactList.SelectedItem = null;
            //            ContactList.ItemsSource = ContactController.GetCustomerRemainingContacts(connection, newInquiryData.CustomerID, contactsIDs.ToArray());
            //        }
            //        projectContacts.Remove(ProjectContactsList.SelectedItem as Contact);
            //    }
            //}
        }
        private void Delete_Contact(object sender, RoutedEventArgs e)
        {
            //if (ProjectContactsList.SelectedItem is Contact contactData)
            //{

            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        connection.Execute($"Delete From [Inquiry].[ProjectsContacts] Where " +
            //                            $"InquiryID = {newInquiryData.InquiryID} And " +
            //                            $"ContactID = {contactData.ContactID}");

            //        if (contactData.Attention == true && projectContacts.Count > 1)
            //        {
            //            var newAttention = projectContacts.Where(contact => contact.ContactID != contactData.ContactID).First();
            //            newAttention.Attention = true;

            //            var query = $"Update [Inquiry].[ProjectsContacts] Set " +
            //                        $"Attention = @Attention " +
            //                        $"Where ContactID = @ContactID And InquiryID = {newInquiryData.InquiryID} ";
            //            connection.Execute(query, newAttention);
            //        }


            //        contactsIDs.Remove(contactData.ContactID);
            //        ContactList.SelectedItem = null;
            //        ContactList.ItemsSource = ContactController.GetCustomerRemainingContacts(connection, newInquiryData.CustomerID, contactsIDs.ToArray());
            //    }
            //    projectContacts.Remove(contactData);
            //}
        }
        private void ProjectContactsList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //var selectedIndex = ProjectContactsList.SelectedIndex;
            //NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewProjectContacts.View.Cast<object>().Count()}";
        }
        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (projectContacts.Count != 0)
            //{
            //    var selectedIndex = ProjectContactsList.SelectedIndex;
            //    NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewProjectContacts.View.Cast<object>().Count()}";
            //}
            //else
            //{
            //    NavigationPanel.Text = $"Contact: 0";
            //}
        }

        private void Attention_Click(object sender, RoutedEventArgs e)
        {
            //if (ProjectContactsList.SelectedItem is Contact contact)
            //{
            //    contact.Attention = true;
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {

            //        foreach (Contact contactData in projectContacts)
            //        {
            //            if (contactData.ContactID != contact.ContactID)
            //                contactData.Attention = false;
            //        }

            //        var query = $"Update [Inquiry].[ProjectsContacts] Set " +
            //                    $"Attention = @Attention " +
            //                    $"Where ContactID = @ContactID And InquiryID = {newInquiryData.InquiryID} ";
            //        connection.Execute(query, projectContacts);
            //    }
            //}
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //{
            //    if (WindowMode == Actions.New && isSaving == false)
            //    {
            //        var query = DatabaseAI.DeleteRecord<Inquiry>(newInquiryData.InquiryID);
            //        connection.Execute(query);

            //        query = $"Delete From [Inquiry].[ProjectsContacts] Where InquiryID = {newInquiryData.InquiryID}";
            //        connection.Execute(query);
            //    }
            //    else
            //    {
            //        UserData.InquiryID = null;
            //        UserController.UpdateInquiryID(connection, UserData);
            //    }
            //}
        }
    }
}
