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
    public partial class ConsultantsWindow : Window
    {
        public User UserData { get; set; }
        public ObservableCollection<Consultant> RecordsData { get; set; }

        Actions action;
        Consultant oldData;
        Consultant recordData;
        CollectionViewSource viewRecordsData;
        public ConsultantsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                var query = $"Select * From Customer._Consultants Order By Name";

                if (RecordsData == null)
                    RecordsData = new ObservableCollection<Consultant>(connection.Query<Consultant>(query));
            }

            viewRecordsData = new CollectionViewSource() { Source = RecordsData };
            RecordsList.ItemsSource = viewRecordsData.View;
            viewRecordsData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            viewRecordsData.Filter += DataFiltrer;

            recordData = RecordsList.SelectedItem as Consultant;
            DataContext = new { recordData, UserData };
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
                    ((Control)control).IsEnabled = true;
            }
            action = Actions.New;
            recordData = new Consultant();
            DataContext = new { recordData, UserData };
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            User usedBy;
            if (recordData != null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.ConsultantId), recordData.Id);
                    if (usedBy == null || usedBy.Id == UserData.Id)
                    {
                        UserData.ConsultantId = recordData.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
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
                    oldData = new Consultant();
                    oldData.Update(recordData);
                }
                else
                {
                    MessageWindow.Show($"Access", $"This consultant data underwork by {usedBy.Name}!", MessageWindowButton.OK, MessageWindowImage.Warning);
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
                    usedBy = connection.AccessValidation(nameof(UserData.ConsultantId), recordData.Id);
                    if (usedBy == null || usedBy.Id == UserData.Id)
                    {
                        UserData.ConsultantId = recordData.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
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
                            var query = $"Select * From Inquiry._Inquiries Where ConsultantId = {recordData.Id}";
                            Consultant CheckConsultant = connection.QueryFirstOrDefault<Consultant>(query);
                            if (CheckConsultant == null)
                            {
                                connection.Execute($"Delete From [Customer].[_Consultants] Where Id = {recordData.Id} ");

                                RecordsData.Remove(recordData);

                                UserData.ConsultantId = null;
                                connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
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
                            UserData.ConsultantId = null;
                            connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
                        }
                    }

                    LoadingControl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageWindow.Show($"Access", $"This consultant data underwork by {usedBy.Name}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var checkConsultant = RecordsData.Where(r => r.Name == recordData.Name);
            if ((checkConsultant.Count() > 1 && action == Actions.Edit) ||
                (checkConsultant.Count() >= 1 && action == Actions.New))
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
                    recordData.Id = Convert.ToInt32(connection.Insert<Consultant>(recordData));
                }
            }
            else if (action == Actions.Edit)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Update<Consultant>(recordData);

                    UserData.ConsultantId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
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
                recordData = RecordsList.SelectedItem as Consultant;
                DataContext = new { recordData, UserData };
            }

            else if (action == Actions.Edit)
            {
                recordData.Update(oldData);

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    UserData.ConsultantId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
                }
            }
        }
        private void List_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            recordData = RecordsList.SelectedItem as Consultant;

            var selectedIndex = RecordsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Consultants: {viewRecordsData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Consultant: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";

            DataContext = new { recordData, UserData };
        }
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = RecordsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Consultants: {viewRecordsData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Consultant: {selectedIndex + 1} / {viewRecordsData.View.Cast<object>().Count()}";

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
                    if (e.Item is Consultant consultant)
                    {
                        string value = consultant.Name.ToUpper();
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
                UserData.ConsultantId = null;
                connection.UserAccessUpdate(UserData, nameof(UserData.ConsultantId));
            }
        }
        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
