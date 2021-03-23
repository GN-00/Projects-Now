using System;
using Dapper;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Core.Windows.MessageWindows;
using Core.Data.UserData;
using Core.Data.InquiriesData;
using Core.Data;
using Core.Enums;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;

namespace Core.Windows.QuotationsWindows
{
    public partial class QuoteWindow : Window
    {
        public User UserData { get; set; }

        ObservableCollection<Inquiry> inquiriesData;
       
        public QuoteWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query = $"Select * From [Inquiry].[_InquiriesView] " +
                               $"Where EstimatorId = {UserData.Id} And Status = {Statuses.New} "; 
                inquiriesData = new ObservableCollection<Inquiry>(connection.Query<Inquiry>(query));
            }

            DataContext = new { UserData };

            viewData = new CollectionViewSource() { Source = inquiriesData };
            viewData.Filter += DataFilter;

            InquiriesList.ItemsSource = viewData.View;
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(DataGrid_CollectionChanged);

        }
        private void Quote_Click(object sender, RoutedEventArgs e)
        {
            if (InquiriesList.SelectedItem is Inquiry inquiryData)
            {
                User usedBy;
                Quotation quotationData = new Quotation() { InquiryId = inquiryData.Id, Inquiry = inquiryData };

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    usedBy = connection.AccessValidation(nameof(quotationData.InquiryId), inquiryData.Id);

                    if (usedBy == null)
                    {
                        string query = $"Select MAX(Number) as Number From [Quotation].[_Qoutations] Where Year = {DateTime.Now.Year}";
                        quotationData.Number = connection.QueryFirstOrDefault<Quotation>(query).Number + 1;
                        quotationData.Year = DateTime.Now.Year;
                        quotationData.Month = DateTime.Now.Month;
                        quotationData.Code =
                            $"ER-{quotationData.Number:000}/{UserData.UserCode}/{quotationData.Month}/{quotationData.Year}/R00";
                        quotationData.ReviseDate = DateTime.Now;

                        quotationData.Id = Convert.ToInt32(connection.Insert<Quotation>(quotationData));
                        TermController.DefaultTerms(connection, quotationData.Id);

                        UserData.InquiryID = inquiryData.InquiryID;
                        UserController.UpdateInquiryID(connection, UserData);

                        UserData.Id = quotationData.Id;
                        UserController.UpdateId(connection, UserData);
                    }
                }

                if (usedBy == null)
                {
                    var quotationWindow = new QuotationWindow()
                    {
                        UserData = this.UserData,
                        OpenPanelsWindow = true,
                        QuotationData = quotationData
                    };
                    this.Close();
                    quotationWindow.ShowDialog();
                }
                else
                {
                    MessageWindow.Show($"Access", $"This inquiry underwork by {usedBy.UserName}!", MessageWindowButton.OK, MessageWindowImage.Warning);
                }
            }
        }

        private void InquiriesList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selectedIndex = InquiriesList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Inquiries: {viewData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Inquiry: {selectedIndex + 1} / {viewData.View.Cast<object>().Count()}";
        }
        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = InquiriesList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Inquiries: {viewData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Inquiry: {selectedIndex + 1} / {viewData.View.Cast<object>().Count()}";
        }

        #region Filters

        CollectionViewSource viewData;
        private readonly List<PropertyInfo> filterProperties = new List<PropertyInfo>()
        {
            (typeof(Inquiry)).GetProperty("RegisterCode"),
            (typeof(Inquiry)).GetProperty("CustomerName"),
            (typeof(Inquiry)).GetProperty("ProjectName"),
            (typeof(Inquiry)).GetProperty("EstimatorCode"),
            (typeof(Inquiry)).GetProperty("RegisterDate"),
            (typeof(Inquiry)).GetProperty("DuoDate"),
            (typeof(Inquiry)).GetProperty("Priority"),
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
    }
}
