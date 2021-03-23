using System;
using Dapper;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using Core.Data.UserData;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Data.SqlClient;
using Core.Data.CustomersData;
using System.Windows.Controls;
using Dapper.Contrib.Extensions;
using Core.Windows.MessageWindows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Core.Windows.CustomersWindows
{
    public partial class ContactsWindow : Window
    {
        public User UserData { get; set; }
        public Customer CustomerData { get; set; }
        public ObservableCollection<Contact> RecordsData { get; set; }

        Actions action;
        Contact oldData;
        Contact recordData;
        CollectionViewSource viewRecordsData;
        public ContactsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                var query = $"Select * From Customer._Contacts Where CustomerId = {CustomerData.Id} Order By Name";

                if (RecordsData == null)
                    RecordsData = new ObservableCollection<Contact>(connection.Query<Contact>(query));
            }

            viewRecordsData = new CollectionViewSource() { Source = RecordsData };
            RecordsList.ItemsSource = viewRecordsData.View;
            viewRecordsData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            viewRecordsData.Filter += DataFiltrer;

            recordData = RecordsList.SelectedItem as Contact;
            DataContext = new { recordData, UserData, CustomerData };
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Visible;
            Done.Visibility = Visibility.Collapsed;
            foreach (object control in Body.Children)
            {
                if (control is TextBlock || control is StackPanel)
                { }
                else
                {
                    if (((Control)control).Name == "Company")
                    { }
                    else
                        ((Control)control).IsEnabled = true;
                }
            }
            action = Actions.New;
            recordData = new Contact() { CustomerId = CustomerData.Id, CustomerName = CustomerData.Name };
            DataContext = new { recordData, UserData, CustomerData };
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            User usedBy;
            if (recordData != null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.ContactId), recordData.Id);
                    if (usedBy == null || usedBy.Id == UserData.Id)
                    {
                        UserData.ContactId = recordData.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
                    }
                }

                if (usedBy == null)
                {
                    LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Visible;
                    Done.Visibility = Visibility.Collapsed;
                    foreach (object control in Body.Children)
                    {
                        if (control is TextBlock || control is StackPanel)
                        { }
                        else
                        {
                            if (((Control)control).Name == "Company")
                            { }
                            else
                                ((Control)control).IsEnabled = true;
                        }
                    }
                    action = Actions.Edit;
                    oldData = new Contact();
                    oldData.Update(recordData);
                }
                else
                {
                    MessageWindow.Show($"Access", $"This contact data underwork by {usedBy.UserName}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            User usedBy;
            if (recordData != null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.ContactId), recordData.Id);
                    if (usedBy == null || usedBy.Id == UserData.Id)
                    {
                        UserData.ContactId = recordData.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
                    }
                }

                if (usedBy == null || usedBy.Id == UserData.Id)
                {
                    LoadingControl.Visibility = Visibility.Visible;

                    MessageBoxResult result = MessageWindow.Show($"Deleting", $"Are you sure want to delete:\n{recordData.Name} ?", MessageWindowButton.YesNo, MessageWindowImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                        {
                            var query = $"Select * From Inquiry._ProjectsContacts Where ContactId = {recordData.Id}";
                            Contact CheckContact = connection.QueryFirstOrDefault<Contact>(query);
                            if (CheckContact == null)
                            {
                                connection.Execute($"Delete From [Customer].[_Contacts] Where Id = {recordData.Id} ");

                                RecordsData.Remove(recordData);

                                UserData.ContactId = null;
                                connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
                            }
                            else
                            {
                                MessageWindow.Show("Deleting", $"You can't delete {recordData.Name} data \n Because its used in projects data", MessageWindowButton.OK, MessageWindowImage.Warning);
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                        {
                            UserData.ContactId = null;
                            connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
                        }
                    }

                    LoadingControl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageWindow.Show($"Access", $"This contact data underwork by {usedBy.UserName}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var checkContact = RecordsData.Where(r => r.Name == recordData.Name);
            if ((checkContact.Count() > 1 && action == Actions.Edit) ||
                (checkContact.Count() >= 1 && action == Actions.New))
            {
                MessageWindow.Show("Name Error", "This contact name is already taken!", MessageWindowButton.OK, MessageWindowImage.Warning);
                return;
            }

            LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Collapsed;
            Done.Visibility = Visibility.Visible;

            foreach (object control in Body.Children)
            {
                if (control is TextBlock || control is StackPanel)
                { }
                else
                    ((Control)control).IsEnabled = false;
            }

            if (action == Actions.New)
            {
                RecordsData.Add(recordData);
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    recordData.Id = Convert.ToInt32(connection.Insert<Contact>(recordData));
                }
            }
            else if (action == Actions.Edit)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Update<Contact>(recordData);

                    UserData.ContactId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
                }

                viewRecordsData.View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Collapsed;
            Done.Visibility = Visibility.Visible;

            foreach (object control in Body.Children)
            {
                if (control is TextBlock || control is StackPanel)
                { }
                else
                    ((Control)control).IsEnabled = true;
            }

            if (action == Actions.New)
            {
                recordData = RecordsList.SelectedItem as Contact;
                DataContext = new { recordData, UserData };
            }

            else if (action == Actions.Edit)
            {
                recordData.Update(oldData);

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    UserData.ContactId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
                }
            }
        }
        private void List_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            recordData = RecordsList.SelectedItem as Contact;

            var selectedIndex = RecordsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Contacts: {viewRecordsData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";

            DataContext = new { recordData, UserData, CustomerData };
        }
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = RecordsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Contacts: {viewRecordsData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Contact: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                viewRecordsData.View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void DataFiltrer(object sender, FilterEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FindRecord.Text))
            {
                try
                {
                    e.Accepted = true;
                    if (e.Item is Contact contact)
                    {
                        string value = contact.Name.ToUpper();
                        if (!value.Contains(FindRecord.Text.ToUpper()))
                        {
                            e.Accepted = false;
                            return;
                        }
                    }
                }
                catch
                {
                    e.Accepted = true;
                }
            }
        }
        private void FilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            viewRecordsData.View.Refresh();
        }
        private void RemoveFilter(object sender, RoutedEventArgs e)
        {
            FindRecord.Text = null;
            viewRecordsData.View.Refresh();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                UserData.ContactId = null;
                connection.UserAccessUpdate(UserData, nameof(UserData.ContactId));
            }
        }
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
