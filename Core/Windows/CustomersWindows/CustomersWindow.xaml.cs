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
    public partial class CustomersWindow : Window
    {
        public User UserData { get; set; }
        public ObservableCollection<Customer> RecordsData { get; set; }

        Actions action;
        Customer oldData;
        Customer recordData;

        CollectionViewSource viewRecordsData;
        public CustomersWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (RecordsData != null) Contacts.Visibility = Visibility.Collapsed;

            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                var query = "Select * From Customer._Customers Order By Name";

                if (RecordsData == null)
                    RecordsData = new ObservableCollection<Customer>(connection.Query<Customer>(query));

                query = "Select * From [User].[_Salesmen] Order By Name";
                SalesmanList.ItemsSource = connection.Query<Salesman>(query);
            }

            CustomerName.ItemsSource = RecordsData;

            viewRecordsData = new CollectionViewSource() { Source = RecordsData };
            RecordsList.ItemsSource = viewRecordsData.View;
            viewRecordsData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            viewRecordsData.Filter += DataFiltrer;

            recordData = RecordsList.SelectedItem as Customer;
            DataContext = new { recordData, UserData };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Visible;
            Done.Visibility = Visibility.Collapsed;
            foreach(object control in Body.Children)
            {
                if (control is TextBlock || control is StackPanel) 
                { }
                else
                    ((Control)control).IsEnabled = true;
            }
            action = Actions.New;
            recordData = new Customer();
            DataContext = new { recordData, UserData };
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            User usedBy;
            if (recordData != null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.CustomerId), recordData.Id);
                    if (usedBy == null || usedBy.Id == UserData.Id)
                    {
                        UserData.CustomerId = recordData.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
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
                            ((Control)control).IsEnabled = true;
                    }
                    action = Actions.Edit;
                    oldData = new Customer();
                    oldData.Update(recordData);
                }
                else
                {
                    MessageWindow.Show($"Access", $"This customer data underwork by {usedBy.Name}!", MessageWindowButton.OK, MessageWindowImage.Warning);
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
                    usedBy = connection.AccessValidation(nameof(UserData.CustomerId), recordData.Id);
                    if (usedBy == null || usedBy.Id == UserData.Id)
                    {
                        UserData.CustomerId = recordData.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
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
                            var query = $"Select * From Inquiry._Inquiries Where CustomerId = {recordData.Id}";
                            Customer CheckCustomer = connection.QueryFirstOrDefault<Customer>(query);
                            if (CheckCustomer == null)
                            {
                                connection.Execute($"Delete From [Customer].[_Customers] Where Id = {recordData.Id} ");
                                connection.Execute($"Delete From [Customer].[_Contacts] Where CustomerId = {recordData.Id} ");

                                RecordsData.Remove(recordData);

                                UserData.CustomerId = null;
                                connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
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
                            UserData.CustomerId = null;
                            connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
                        }
                    }

                    LoadingControl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageWindow.Show($"Access", $"This customer data underwork by {usedBy.Name}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var checkCustomer = RecordsData.Where(r => r.Name == CustomerName.Text);
            if ((checkCustomer.Count() > 1 && action == Actions.Edit) ||
                (checkCustomer.Count() >= 1 && action == Actions.New))
            {
                MessageWindow.Show("Name Error", "This customer name is already taken!", MessageWindowButton.OK, MessageWindowImage.Warning);
                return;
            }

            LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Collapsed;
            Done.Visibility  = Visibility.Visible;

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
                    recordData.Id = Convert.ToInt32(connection.Insert<Customer>(recordData));
                }
            }
            else if (action == Actions.Edit)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Update<Customer>(recordData);

                    UserData.CustomerId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
                }

                viewRecordsData.View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Collapsed;
            Done.Visibility  = Visibility.Visible;

            foreach (object control in Body.Children)
            {
                if (control is TextBlock || control is StackPanel)
                { }
                else
                    ((Control)control).IsEnabled = true;
            }

            if (action == Actions.New)
            {
                recordData = RecordsList.SelectedItem as Customer;
                DataContext = new { recordData, UserData };
            }

            else if (action == Actions.Edit)
            {
                recordData.Update(oldData);

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    UserData.CustomerId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
                }
            }
        }

        private void List_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            recordData = RecordsList.SelectedItem as Customer;

            var selectedIndex = RecordsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Customers: {viewRecordsData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Customer: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";

            DataContext = new { recordData, UserData };
        }
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = RecordsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Customers: {viewRecordsData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Customer: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                viewRecordsData.View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }


        private void History_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Contacts_Click(object sender, RoutedEventArgs e)
        {
            if(RecordsList.SelectedItem is Customer customer)
            {
                ContactsWindow contactsWindow = new ContactsWindow()
                {
                    UserData = this.UserData,
                    CustomerData = customer,
                };
                contactsWindow.ShowDialog();
            }
        }

        private void DataFiltrer(object sender, FilterEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FindRecord.Text))
            {
                try
                {
                    e.Accepted = true;
                    if (e.Item is Customer customer)
                    {
                        string value = customer.Name.ToUpper();
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

        private void POBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Data.Input.IntOnly(e, 4);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                UserData.CustomerId = null;
                connection.UserAccessUpdate(UserData, nameof(UserData.CustomerId));
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
