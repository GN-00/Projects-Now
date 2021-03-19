using System;
using Dapper;
using System.Linq;
using System.Windows;
using ProjectsNow.Enums;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using ProjectsNow.Database;
using System.Windows.Media;
using System.Data.SqlClient;
using ProjectsNow.Controllers;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ProjectsNow.Windows.MessageWindows;

namespace Core.Windows.InquiriesWindows
{
    public partial class InquiriesWindow : Window
    {
        public User UserData { get; set; }

        bool isLoading = true;
        Statuses status = Statuses.New;
        ObservableCollection<Inquiry> InquiriesData;
        ObservableCollection<InquiryYear> YearsData;
        public InquiriesWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                InquiriesData = InquiryController.GetInquiries(connection, UserData, Statuses.New, DateTime.Now.Year);
                YearsData = InquiryController.GetInquiriesYears(connection, UserData, Statuses.New);
            }
            DataContext = new { UserData };

            viewData = new CollectionViewSource() { Source = InquiriesData };
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = YearsData;
            YearsList.SelectedItem = YearsData.Where(item => item.Year == DateTime.Now.Year).FirstOrDefault();
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
                if (inquiryData.QuotationID == null)
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
                if (inquiryData.QuotationID == null)
                    EditButton.Visibility = AssignButton.Visibility = DeleteButton.Visibility = Visibility.Visible;
                else
                    EditButton.Visibility = AssignButton.Visibility = DeleteButton.Visibility = Visibility.Collapsed;
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            InquiryWindow inquiryWindow = new InquiryWindow()
            {
                UserData = this.UserData,
                WindowMode = Actions.New,
                InquiriesDataToUpdate = InquiriesData,
            };
            inquiryWindow.ShowDialog();
        }
        private void Edit_ClicK(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiry)
            {
                User usedBy;
                Quotation quotation;
                using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
                {
                    usedBy = UserController.CheckUserInquiryID(connection, inquiry.InquiryID);
                    quotation = InquiryController.CheckQuotation(connection, inquiry.InquiryID);

                    if (usedBy == null)
                    {
                        UserData.InquiryID = inquiry.InquiryID;
                        UserController.UpdateInquiryID(connection, UserData);
                    }
                }

                if (quotation != null)
                {
                    if (quotation.QuotationStatus != Statuses.Running.ToString())
                    {
                        CMessageBox.Show($"Access", $"Can't edit this Inquiry!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
                        return;
                    }
                }

                if (usedBy == null || usedBy.UserID == UserData.UserID)
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
                    CMessageBox.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
                }
            }
        }
        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiry)
            {
                User usedBy;
                Quotation quotation;
                using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
                {
                    usedBy = UserController.CheckUserInquiryID(connection, inquiry.InquiryID);
                    quotation = InquiryController.CheckQuotation(connection, inquiry.InquiryID);

                    if (usedBy == null || usedBy.UserID == UserData.UserID)
                    {
                        UserData.InquiryID = inquiry.InquiryID;
                        UserController.UpdateInquiryID(connection, UserData);
                    }
                }

                if (quotation != null)
                {
                    if (quotation.QuotationStatus != Statuses.Running.ToString())
                    {
                        CMessageBox.Show($"Access", $"Can't edit this Inquiry!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
                        return;
                    }
                }

                if (usedBy == null || usedBy.UserID == UserData.UserID)
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
                    CMessageBox.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiry)
            {
                User usedBy;
                Quotation quotation;
                using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
                {
                    usedBy = UserController.CheckUserInquiryID(connection, inquiry.InquiryID);
                    quotation = InquiryController.CheckQuotation(connection, inquiry.InquiryID);

                    if (usedBy == null || usedBy.UserID == UserData.UserID)
                    {
                        UserData.InquiryID = inquiry.InquiryID;
                        UserController.UpdateInquiryID(connection, UserData);
                    }
                }

                if (quotation != null)
                {
                    CMessageBox.Show($"Access", $"Can't delete this Inquiry!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
                    return;
                }

                if (usedBy == null || usedBy.UserID == UserData.UserID)
                {
                    MessageBoxResult result = CMessageBox.Show("Deleting", $"Do you want to Delete Inquiy: \n{inquiry.RegisterCode}?", CMessageBoxButton.YesNo, CMessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
                        {
                            connection.Execute($"Delete From [Inquiry].[Inquiries] Where InquiryID = {inquiry.InquiryID}");
                            connection.Execute($"Delete From [Inquiry].[ProjectsContacts] Where InquiryID = {inquiry.InquiryID}");
                            InquiriesData.Remove(inquiry);
                        }
                    }
                }
                else
                {
                    CMessageBox.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
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
            typeof(Inquiry).GetProperty("EstimationName"),
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
            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                viewData.Source = InquiriesData = InquiryController.GetInquiries(connection, UserData, Statuses.All, DateTime.Now.Year);
                YearsData = InquiryController.GetInquiriesYears(connection, UserData, Statuses.All);
            }
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = YearsData;

            YearsList.SelectedItem = YearsData.FirstOrDefault(i => i.Year == DateTime.Now.Year);
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

            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                viewData.Source = InquiriesData = InquiryController.GetInquiries(connection, UserData, Statuses.New, DateTime.Now.Year);
                YearsData = InquiryController.GetInquiriesYears(connection, UserData, Statuses.New);
            }
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = YearsData;

            YearsList.SelectedItem = YearsData.FirstOrDefault(i => i.Year == DateTime.Now.Year);
            YearValue.Text = DateTime.Now.Year.ToString();

            CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            isLoading = false;
        }
        private void UnderWork_Click(object sender, RoutedEventArgs e)
        {
            DeleteFilter_Click(sender, e);
            isLoading = true;
            status = Statuses.Running;
            StatusName.Text = $"Under Work Inquiries";
            StatusName.Foreground = YearValue.Foreground = Brushes.Blue;
            using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
            {
                viewData.Source = InquiriesData = InquiryController.GetInquiries(connection, UserData, Statuses.Running, DateTime.Now.Year);
                YearsData = InquiryController.GetInquiriesYears(connection, UserData, Statuses.Running);
            }
            InquiriesList.ItemsSource = viewData.View;
            YearsList.ItemsSource = YearsData;

            YearsList.SelectedItem = YearsData.FirstOrDefault(i => i.Year == DateTime.Now.Year);
            YearValue.Text = DateTime.Now.Year.ToString();

            CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            isLoading = false;
        }
        private void Years_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoading)
            {
                if (YearsList.SelectedItem is InquiryYear yearData)
                {
                    YearValue.Text = yearData.Year.ToString();
                    DeleteFilter_Click(sender, e);
                    using (SqlConnection connection = new SqlConnection(DatabaseAI.ConnectionString))
                    {
                        viewData.Source = InquiriesData = InquiryController.GetInquiries(connection, UserData, status, yearData.Year);
                    }
                    InquiriesList.ItemsSource = viewData.View;
                    CollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
                }
            }
        }

    }
}
