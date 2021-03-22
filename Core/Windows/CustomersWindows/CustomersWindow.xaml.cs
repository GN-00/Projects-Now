using Core.Data;
using Core.Data.CustomersData;
using Core.Data.UserData;
using Core.Enums;

using Dapper;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

                //query = "Select * From User._Salesmen Orde By Name";
                //SalesmanList.ItemsSource = connection.Query<Salesman>(query);
            }

            CustomerName.ItemsSource = RecordsData;

            viewRecordsData = new CollectionViewSource() { Source = RecordsData };
            RecordsList.ItemsSource = viewRecordsData.View;
            viewRecordsData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(DataGrid_CollectionChanged);
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
            //User usedBy;
            //if (recordData != null)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        usedBy = UserController.CheckUserCustomerID(connection, recordData.CustomerID);
            //        if (usedBy == null || usedBy.UserID == UserData.UserID)
            //        {
            //            UserData.CustomerID = recordData.CustomerID;
            //            UserController.UpdateCustomerID(connection, UserData);
            //        }
            //    }

            //    if (usedBy == null)
            //    {
            //        LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Visible;
            //        Done.Visibility = ReadOnly.Visibility = Visibility.Collapsed;
            //        action = Actions.Edit;
            //        oldData = new Customer();
            //        oldData.Update(recordData);
            //    }
            //    else
            //    {
            //        CMessageBox.Show($"Access", $"This customer data underwork by {usedBy.UserName}!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //    }
            //}
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //User usedBy;
            //if (recordData != null)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        usedBy = UserController.CheckUserCustomerID(connection, recordData.CustomerID);
            //        if (usedBy == null || usedBy.UserID == UserData.UserID)
            //        {
            //            UserData.CustomerID = recordData.CustomerID;
            //            UserController.UpdateCustomerID(connection, UserData);
            //        }
            //    }

            //    if (usedBy == null || usedBy.UserID == UserData.UserID)
            //    {
            //        LoadingControl.Visibility = Visibility.Visible;

            //        MessageBoxResult result = CMessageBox.Show($"Deleting", $"Are you sure want to delete:\n{recordData.CustomerName} ?", CMessageBoxButton.YesNo, CMessageBoxImage.Warning);
            //        if (result == MessageBoxResult.Yes)
            //        {
            //            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //            {
            //                if (recordData.AbilityToDelete(connection))
            //                {
            //                    connection.Execute($"Delete From [Customer].[Customers] Where CustomerID = {recordData.CustomerID} ");
            //                    connection.Execute($"Delete From [Customer].[Contacts] Where CustomerID = {recordData.CustomerID} ");

            //                    RecordsData.Remove(recordData);

            //                    UserData.CustomerID = null;
            //                    UserController.UpdateCustomerID(connection, UserData);
            //                }
            //                else
            //                {
            //                    CMessageBox.Show("Deleting", $"You can't delete {recordData.CustomerName} data \n Because its used in projects data", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //            {
            //                UserData.CustomerID = null;
            //                UserController.UpdateCustomerID(connection, UserData);
            //            }
            //        }

            //        LoadingControl.Visibility = Visibility.Collapsed;
            //    }
            //    else
            //    {
            //        CMessageBox.Show($"Access", $"This customer data underwork by {usedBy.UserName}!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //    }
            //}
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //var checkCustomer = RecordsData.Where(r => r.CustomerName == CustomerName.Text);
            //if ((checkCustomer.Count() > 1 && action == Actions.Edit) ||
            //    (checkCustomer.Count() >= 1 && action == Actions.New))
            //{
            //    CMessageBox.Show("Name Error", "This customer name is already taken!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //    return;
            //}

            //LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Collapsed;
            //Done.Visibility = ReadOnly.Visibility = Visibility.Visible;

            //if (action == Actions.New)
            //{
            //    List<Customer> customers = RecordsData.ToList();
            //    customers.Add(recordData);
            //    customers.Sort((x, y) => x.CustomerName.CompareTo(y.CustomerName));
            //    int index = customers.IndexOf(recordData);
            //    RecordsData.Insert(index, recordData);
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        var query = DatabaseAI.InsertRecord<Customer>();
            //        recordData.CustomerID = (int)(decimal)connection.ExecuteScalar(query, recordData);
            //    }
            //}
            //else if (action == Actions.Edit)
            //{
            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        var query = DatabaseAI.UpdateRecord<Customer>();
            //        connection.Execute(query, recordData);

            //        UserData.CustomerID = null;
            //        UserController.UpdateCustomerID(connection, UserData);
            //    }
            //}
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //LoadingControl.Visibility = Save.Visibility = Cancel.Visibility = Visibility.Collapsed;
            //Done.Visibility = ReadOnly.Visibility = Visibility.Visible;
            //if (action == Actions.New)
            //{
            //    recordData = RecordsList.SelectedItem as Customer;
            //    DataContext = new { recordData, UserData };
            //}
            //else if (action == Actions.Edit)
            //{
            //    recordData.Update(oldData);

            //    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //    {
            //        UserData.CustomerID = null;
            //        UserController.UpdateCustomerID(connection, UserData);
            //    }
            //}
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
        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //var selectedIndex = RecordsList.SelectedIndex;
            //if (selectedIndex == -1)
            //    NavigationPanel.Text = $"Customers: {viewRecordsData.View.Cast<object>().Count()}";
            //else
            //    NavigationPanel.Text = $"Customer: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";
        }


        private void History_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Contacts_Click(object sender, RoutedEventArgs e)
        {
            //ContactsWindow contactsWindow = new ContactsWindow()
            //{
            //    UserData = this.UserData,
            //    CustomerID = (RecordsList.SelectedItem as Customer).CustomerID,
            //    CustomerName = (RecordsList.SelectedItem as Customer).CustomerName,
            //};
            //contactsWindow.ShowDialog();
        }

        private void DataFiltrer(object sender, FilterEventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(FindRecord.Text))
            //{
            //    try
            //    {
            //        e.Accepted = true;
            //        if (e.Item is Customer customer)
            //        {
            //            string value = customer.CustomerName.ToUpper();
            //            if (!value.Contains(FindRecord.Text.ToUpper()))
            //            {
            //                e.Accepted = false;
            //                return;
            //            }
            //        }
            //    }
            //    catch
            //    {
            //        e.Accepted = true;
            //    }
            //}
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
            //DataInput.Input.IntOnly(e, 4);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            //{
            //    UserData.CustomerID = null;
            //    UserController.UpdateCustomerID(connection, UserData);
            //}
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
