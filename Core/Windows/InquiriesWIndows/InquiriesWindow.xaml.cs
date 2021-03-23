using System;
using Dapper;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using Core.Data.General;
using System.Reflection;
using Core.Data.UserData;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.InquiriesData;
using Core.Data.QuotationsData;
using System.Collections.Generic;
using Core.Windows.MessageWindows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Core.Windows.InquiriesWindows
{
    public partial class InquiriesWindow : Window
    {
        public User UserData { get; set; }

        bool isLoading = true;
        Statuses status = Statuses.New;

        ObservableCollection<Inquiry> inquiriesData;
        ObservableCollection<Year> yearsData;
        public InquiriesWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                inquiriesData = GetInquires(connection, status, DateTime.Now.Year);
                yearsData = GetYears(connection, status);
            }
            DataContext = new { UserData };

            viewData = new CollectionViewSource() { Source = inquiriesData };
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = yearsData;

            YearsList.SelectedItem = yearsData.Where(item => item.Value == DateTime.Now.Year).FirstOrDefault();
            YearValue.Text = DateTime.Now.Year.ToString();

            viewData.Filter += DataFilter;
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            if (viewData.View.Cast<object>().Count() == 0)
                CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            isLoading = false;
        }
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = InquiriesList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Inquiries: {viewData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Inquiry: {selectedIndex + 1} / {viewData.View.Cast<object>().Count()}";

            if (InquiriesList.SelectedItem is Inquiry inquiryData)
            {
                if (inquiryData.Status == "New")
                    EditButton.Visibility = AssignButton.Visibility = DeleteButton.Visibility = Visibility.Visible;
                else
                    EditButton.Visibility = AssignButton.Visibility = DeleteButton.Visibility = Visibility.Collapsed;
            }
        }
        private void InquiriesList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedIndex = InquiriesList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Inquiries: {viewData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Inquiry: {selectedIndex + 1} / {viewData.View.Cast<object>().Count()}";

            if (InquiriesList.SelectedItem is Inquiry inquiryData)
            {
                if (inquiryData.Status == "New")
                    EditButton.Visibility = AssignButton.Visibility = DeleteButton.Visibility = Visibility.Visible;
                else
                    EditButton.Visibility = AssignButton.Visibility = DeleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            InquiryWindow inquiryWindow = new InquiryWindow()
            {
                InquiryData = new Inquiry() { Status = Statuses.New.ToString()},
                UserData = this.UserData,
                WindowMode = Actions.New,
                InquiriesDataToUpdate = inquiriesData,
            };
            inquiryWindow.ShowDialog();
        }
        private void Edit_ClicK(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiry)
            {
                User usedBy;
                Quotation quotation;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.InquiryId) , inquiry.Id);
                    quotation = connection.QueryFirstOrDefault<Quotation>($"Select * From [Quotation].[_Quotations] Where InquiryId = {inquiry.Id}");

                    if (usedBy == null)
                    {
                        UserData.InquiryId = inquiry.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
                    }
                }

                if (quotation != null)
                {
                    if (quotation.Status != Statuses.Running.ToString())
                    {
                        MessageWindow.Show($"Access", $"Can't edit this Inquiry!", MessageWindowButton.OK, MessageWindowImage.Warning);
                        return;
                    }
                }

                if (usedBy == null || usedBy.Id == UserData.Id)
                {
                    var inquiryWindow = new InquiryWindow()
                    {
                        UserData = this.UserData,
                        WindowMode = Actions.Edit,
                        InquiryData = inquiry,
                    };
                    inquiryWindow.ShowDialog();
                }
                else
                {
                    MessageWindow.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }
        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiry)
            {
                User usedBy;
                Quotation quotation;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.InquiryId), inquiry.Id);
                    quotation = connection.QueryFirstOrDefault<Quotation>($"Select * From Quotation._Quotations Where InquiryId = {inquiry.Id}");

                    if (usedBy == null)
                    {
                        UserData.InquiryId = inquiry.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
                    }
                }

                if (quotation != null)
                {
                    if (quotation.Status != Statuses.Running.ToString())
                    {
                        MessageWindow.Show($"Access", $"Can't edit this Inquiry!", MessageWindowButton.OK, MessageWindowImage.Warning);
                        return;
                    }
                }

                if (usedBy == null || usedBy.Id == UserData.Id)
                {
                    var assignWindow = new AssignWindow()
                    {
                        UserData = this.UserData,
                        InquiryData = inquiry,
                    };
                    assignWindow.ShowDialog();
                }
                else
                {
                    MessageWindow.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiry)
            {
                User usedBy;
                Quotation quotation;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(UserData.InquiryId), inquiry.Id);
                    quotation = connection.QueryFirstOrDefault<Quotation>($"Select * From Quotation._Quotations Where InquiryId = {inquiry.Id}");

                    if (usedBy == null)
                    {
                        UserData.InquiryId = inquiry.Id;
                        connection.UserAccessUpdate(UserData, nameof(UserData.InquiryId));
                    }
                }

                if (quotation != null)
                {
                    MessageWindow.Show($"Access", $"Can't delete this Inquiry!", MessageWindowButton.OK, MessageWindowImage.Warning);
                    return;
                }

                if (usedBy == null || usedBy.Id == UserData.Id)
                {
                    MessageBoxResult result = MessageWindow.Show("Deleting", $"Do you want to Delete Inquiy: \n{inquiry.RegisterCode}?", MessageWindowButton.YesNo, MessageWindowImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                        {
                            connection.Execute($"Delete From [Inquiry].[_Inquiries] Where Id = {inquiry.Id}");
                            connection.Execute($"Delete From [Inquiry].[_ProjectsContacts] Where InquiryId = {inquiry.Id}");
                            inquiriesData.Remove(inquiry);
                        }
                    }
                }
                else
                {
                    MessageWindow.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }

        #region Filters

        CollectionViewSource viewData;
        private readonly List<PropertyInfo> filterProperties = new List<PropertyInfo>()
        {
            typeof(Inquiry).GetProperty("RegisterCode"),
            typeof(Inquiry).GetProperty("CustomerName"),
            typeof(Inquiry).GetProperty("ProjectName"),
            typeof(Inquiry).GetProperty("EstimatorCode"),
            typeof(Inquiry).GetProperty("RegisterDate"),
            typeof(Inquiry).GetProperty("DuoDate"),
            typeof(Inquiry).GetProperty("Priority"),
            typeof(Inquiry).GetProperty("Status"),
        };
        private void DataFilter(object sender, FilterEventArgs e)
        {
            try
            {
                e.Accepted = true;
                if (e.Item is Inquiry record)
                {
                    string columnName;
                    foreach (PropertyInfo property in filterProperties)
                    {
                        columnName = property.Name;
                        string value;
                        if (property.PropertyType == typeof(DateTime))
                            value = $"{record.GetType().GetProperty(columnName).GetValue(record):dd/MM/yyyy}";
                        else
                            value = $"{record.GetType().GetProperty(columnName).GetValue(record)}".ToUpper();

                        if (!value.Contains(((TextBox)FindName(property.Name)).Text.ToUpper()))
                        {
                            e.Accepted = false;
                            return;
                        }
                    }
                }
            }
            catch
            {
                e.Accepted = true;
            }
        }
        private void ApplyFilter(object sender, KeyEventArgs e)
        {
            viewData.View.Refresh();
        }
        private void DeleteFilter_Click(object sender, RoutedEventArgs e)
        {
            foreach (PropertyInfo property in filterProperties)
            {
                ((TextBox)FindName(property.Name)).Text = null;
            }
            viewData.View.Refresh();
        }

        #endregion

        private void AllInquiries_Click(object sender, RoutedEventArgs e)
        {
            DeleteFilter_Click(sender, e);
            isLoading = true;
            status = Statuses.All;
            StatusName.Text = $"All Inquiries";
            StatusName.Foreground = YearValue.Foreground = Brushes.Black;
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                viewData.Source = inquiriesData = GetInquires(connection, status, DateTime.Now.Year);
                yearsData = GetYears(connection, status);
            }
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = yearsData;

            YearsList.SelectedItem = yearsData.FirstOrDefault(i => i.Value == DateTime.Now.Year);
            YearValue.Text = DateTime.Now.Year.ToString();

            CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            isLoading = false;
        }
        private void NewInquiries_Click(object sender, RoutedEventArgs e)
        {
            DeleteFilter_Click(sender, e);
            isLoading = true;
            status = Statuses.New;
            StatusName.Text = $"New Inquiries";
            StatusName.Foreground = YearValue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9211E8"));
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                viewData.Source = inquiriesData = GetInquires(connection, status, DateTime.Now.Year);
                yearsData = GetYears(connection, status);
            }
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = yearsData;

            YearsList.SelectedItem = yearsData.FirstOrDefault(i => i.Value == DateTime.Now.Year);
            YearValue.Text = DateTime.Now.Year.ToString();

            CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            isLoading = false;
        }
        private void UnderWork_Click(object sender, RoutedEventArgs e)
        {
            DeleteFilter_Click(sender, e);
            isLoading = true;
            status = Statuses.Quoting;
            StatusName.Text = $"Under Work Inquiries";
            StatusName.Foreground = YearValue.Foreground = Brushes.Blue;
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                viewData.Source = inquiriesData = GetInquires(connection, status, DateTime.Now.Year);
                yearsData = GetYears(connection, status);
            }
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = yearsData;

            YearsList.SelectedItem = yearsData.FirstOrDefault(i => i.Value == DateTime.Now.Year);
            YearValue.Text = DateTime.Now.Year.ToString();

            CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            isLoading = false;
        }
        private void Years_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoading)
            {
                if (YearsList.SelectedItem is Year yearData)
                {
                    YearValue.Text = yearData.Value.ToString();
                    DeleteFilter_Click(sender, e);
                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        viewData.Source = inquiriesData = GetInquires(connection, status, yearData.Value);
                    }
                    InquiriesList.ItemsSource = viewData.View;
                    CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                }
            }
        }

        public static ObservableCollection<Inquiry> GetInquires(SqlConnection connection, Statuses status, int year)
        {
            var query = $"Select * From [Inquiry].[_InquiriesView] ";
                query += $"Where RegisterYear = {year} ";

            if(status != Statuses.All) 
                query += $"And Status = '{status}'";

            query += "Order By RegisterYear Desc, RegisterMonth Desc, RegisterNumber Desc";

            var records = connection.Query<Inquiry>(query);
            return new ObservableCollection<Inquiry>(records);
        }
        private static ObservableCollection<Year> GetYears(SqlConnection connection, Statuses status)
        {
            var query = $"Select * From [Inquiry].[_Years] ";
            if (status != Statuses.All)
                query += $"Where Status = '{status}'";

            var records = connection.Query<Year>(query);
            if (status == Statuses.All)
            {
                return new ObservableCollection<Year>(records.GroupBy(g => g.Value).Select(s => new Year() { Value = s.Key }));
            }
            else
            {
                return new ObservableCollection<Year>(records);
            }
        }
    }
}
