﻿using Core.Data.UserData;
using Core.Data.QuotationsData;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Data.SqlClient;
using Core.Data;
using Dapper;

namespace Core.Windows.QuotationsWindows
{
    public partial class PanelsWindow : Window
    {
        public User UserData { get; set; }
        public Quotation QuotationData { get; set; }
        public ObservableCollection<Panel> PanelsData { get; set; }
        public ObservableCollection<Quotation> QuotationsData { get; set; }

        public PanelsWindow()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (UserData.QuotationsDiscount)
                DiscountButton.Visibility = Visibility.Visible;

            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query = $"Select * From Quotation._PanelsView Where QuotationId = {QuotationData.Id} Order By SN";
                PanelsData = new ObservableCollection<Panel>(connection.Query<Panel>(query));
            }

            viewData = new CollectionViewSource() { Source = PanelsData };
            viewData.Filter += DataFilter;

            PanelsList.ItemsSource = viewData.View;
            viewData.View.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            DataContext = new { UserData, QuotationData };
        }


        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var selectedIndex = PanelsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Panels: {viewData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Panel: {selectedIndex + 1} / {viewData.View.Cast<object>().Count()}";
        }
        private void PanelsList_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            var selectedIndex = PanelsList.SelectedIndex;
            if (selectedIndex == -1)
                NavigationPanel.Text = $"Panels: {viewData.View.Cast<object>().Count()}";
            else
                NavigationPanel.Text = $"Panel: {selectedIndex + 1} / {viewData.View.Cast<object>().Count()}";
        }

        private void Information_Click(object sender, RoutedEventArgs e)
        {
            var quotationWindow = new QuotationWindow()
            {
                UserData = this.UserData,
                QuotationData = QuotationData,
            };
            quotationWindow.ShowDialog();
        }
        private void TC_Click(object sender, RoutedEventArgs e)
        {
            var termsWindow = new TermsWindows.TermsWindow()
            {
                UserData = this.UserData,
                QuotationData = QuotationData,
                PanelsWindow = this,
            };
            termsWindow.ShowDialog();
        }
        private void Prices_Click(object sender, RoutedEventArgs e)
        {
            var PriceWindow = new PriceWindow()
            {
                UserData = this.UserData,
                QuotationData = QuotationData,
                PanelsWindow = this,
            };
            PriceWindow.ShowDialog();
        }
        private void Printer_Click(object sender, RoutedEventArgs e)
        {
            PrintWindow printWindow = new PrintWindow()
            {
                QuotationData = QuotationData,
                UserData = this.UserData,
            };
            printWindow.ShowDialog();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = CMessageBox.Show("Submit", $"Are you sure to submit \nQ.Code: {QuotationData.QuotationCode}", CMessageBoxButton.YesNo, CMessageBoxImage.Information);
            //if (result == MessageBoxResult.Yes)
            //{
            //    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //    {
            //        QuotationData.QuotationStatus = Statuses.Submitted.ToString();
            //        QuotationData.SubmitDate = DateTime.Now;
            //        var query = Database.UpdateRecord<Quotation>();
            //        connection.Execute(query, QuotationData);
            //    }

            //    if (QuotationsData != null)
            //        QuotationsData.Remove(QuotationData);

            //    this.Close();
            //}
        }

        private void AddPanel_Click(object sender, RoutedEventArgs e)
        {
            //var qPanelWindow = new QPanelWindow()
            //{
            //    ActionData = Actions.New,
            //    QPanelsData = this.PanelsData,
            //    SelectedIndex = this.PanelsData.Count,
            //    QuotationData = this.QuotationData,
            //};
            //qPanelWindow.ShowDialog();
        }
        private void InsertUp_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panel)
            //{
            //    var qPanelWindow = new QPanelWindow()
            //    {
            //        ActionData = Actions.InsertUp,
            //        QPanelsData = this.PanelsData,
            //        SelectedIndex = this.PanelsData.IndexOf(panel),
            //        QuotationData = this.QuotationData,
            //    };
            //    qPanelWindow.ShowDialog();
            //}
        }
        private void InsertDown_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panel)
            //{
            //    var qPanelWindow = new QPanelWindow()
            //    {
            //        ActionData = Actions.InsertDown,
            //        QPanelsData = this.PanelsData,
            //        SelectedIndex = this.PanelsData.IndexOf(panel) + 1,
            //        QuotationData = this.QuotationData,
            //    };
            //    qPanelWindow.ShowDialog();
            //}
        }
        private void EditPanel_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panel)
            //{
            //    var qPanelWindow = new QPanelWindow()
            //    {
            //        ActionData = Actions.Edit,
            //        QPanelData = panel,
            //        QPanelsData = this.PanelsData,
            //        QuotationData = this.QuotationData,
            //    };
            //    qPanelWindow.ShowDialog();
            //}
        }
        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panelData)
            //{
            //    if (panelData.PanelSN > 1)
            //    {
            //        panelData.PanelSN -= 1;
            //        PanelsData.Move(PanelsData.IndexOf(panelData), PanelsData.IndexOf(panelData) - 1);
            //        foreach (QPanel panel in PanelsData.Where(p => p.PanelSN == panelData.PanelSN && p.PanelID != panelData.PanelID))
            //        {
            //            ++panel.PanelSN;
            //        }
            //        string query = $"Update [Quotation].[QuotationsPanels] Set PanelSN = PanelSN + 1 Where PanelSN = {panelData.PanelSN} And QuotationID = {panelData.QuotationID} And PanelID != {panelData.PanelID}; " +
            //                       $"Update [Quotation].[QuotationsPanels] Set PanelSN = PanelSN - 1 Where PanelID = {panelData.PanelID}; ";
            //        using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //        {
            //            connection.Execute(query);
            //        }
            //    }
            //}
        }
        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panelData)
            //{
            //    if (panelData.PanelSN < PanelsData.Count && PanelsData.Count != 1)
            //    {
            //        panelData.PanelSN += 1;
            //        PanelsData.Move(PanelsData.IndexOf(panelData), PanelsData.IndexOf(panelData) + 1);
            //        foreach (QPanel panel in PanelsData.Where(p => p.PanelSN == panelData.PanelSN && p.PanelID != panelData.PanelID))
            //        {
            //            --panel.PanelSN;
            //        }
            //        string query = $"Update [Quotation].[QuotationsPanels] Set PanelSN = PanelSN - 1 Where PanelSN = {panelData.PanelSN} And QuotationID = {panelData.QuotationID} And PanelID != {panelData.PanelID}; " +
            //                       $"Update [Quotation].[QuotationsPanels] Set PanelSN = PanelSN + 1 Where PanelID = {panelData.PanelID}; ";
            //        using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //        {
            //            connection.Execute(query);
            //        }
            //    }
            //}
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panelData)
            //{
            //    MessageBoxResult result = CMessageBox.Show("Deleting", $"Are you sure you want to \ndelete {panelData.PanelName} ?", CMessageBoxButton.YesNo, CMessageBoxImage.Warning);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //        {
            //            string query = $"Delete From [Quotation].[QuotationsPanels] Where PanelID = {panelData.PanelID}; " +
            //                           $"Delete From [Quotation].[QuotationsPanelsItems] Where PanelID = {panelData.PanelID}; " +
            //                           $"Delete From [Quotation].[QuotationsOptionsPanels] Where PanelID = {panelData.PanelID}; " +
            //                           $"Update [Quotation].[QuotationsPanels] Set PanelSN = PanelSN - 1 Where PanelSN > {panelData.PanelSN} And QuotationID = {panelData.QuotationID}; ";
            //            connection.Execute(query);
            //        }

            //        foreach (QPanel panel in PanelsData.Where(p => p.PanelSN > panelData.PanelSN))
            //        {
            //            --panel.PanelSN;
            //        }

            //        PanelsData.Remove(panelData);
            //        QuotationData.QuotationCost = PanelsData.Sum(p => p.PanelsPrice);
            //    }
            //}
        }
        private void Material_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panelData)
            //{
            //    QuotationPanelItemsWindow panelItemsWindow = new QuotationPanelItemsWindow()
            //    {
            //        UserData = this.UserData,
            //        PanelData = panelData,
            //        QuotationData = this.QuotationData,
            //        QuotationPanelsWindowData = this,
            //    };
            //    panelItemsWindow.ShowDialog();
            //}
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panelData)
            //{
            //    QuotationPanelNameWindow panelNameWindow = new QuotationPanelNameWindow()
            //    {
            //        PanelData = panelData,
            //        PanelsData = PanelsData,
            //    };
            //    panelNameWindow.ShowDialog();
            //}
        }
        private void TargetPrice_Click(object sender, RoutedEventArgs e)
        {
            Price.PlacementTarget = (sender as System.Windows.Controls.Button);
            Price.IsOpen = true;
        }
        private void NewProfit_Click(object sender, RoutedEventArgs e)
        {
            //double.TryParse(TargetPriceValue.Text, out double targetPrice);
            //if (targetPrice == 0)
            //{
            //    Price.IsOpen = false;
            //    return;
            //}

            //if (PanelsData.Sum(p => p.PanelsPrice) == 0)
            //{
            //    Price.IsOpen = false;
            //    CMessageBox.Show("Price Error", "Can't change zero price", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //    return;
            //}

            //if (PanelsData.Count() == 0)
            //{
            //    Price.IsOpen = false;
            //    return;
            //}

            //if (PanelsData.Where(p => p.PanelType != "Ready Made Panel").Count() != 0)
            //{
            //    string query = "";
            //    double quotationPrice = PanelsData.Sum(p => p.PanelsPrice) * (1 - QuotationData.Discount / 100);
            //    double readyMadePanelsPrice = PanelsData.Where(p => p.PanelType == "Ready Made Panel").Sum(p => p.PanelsPrice) * (1 - QuotationData.Discount / 100);
            //    double otherpanelsPrice = PanelsData.Where(p => p.PanelType != "Ready Made Panel").Sum(p => p.PanelsPrice) * (1 - QuotationData.Discount / 100);

            //    double targetPriceWithoutReadyMadePanels = targetPrice - readyMadePanelsPrice;

            //    double pp = targetPriceWithoutReadyMadePanels / otherpanelsPrice;

            //    foreach (QPanel panelData in PanelsData.Where(p => p.PanelType != "Ready Made Panel" && p.PanelsPrice != 0))
            //    {
            //        panelData.PanelProfit = 100 - (100 / pp) * (1 - panelData.PanelProfit / 100);
            //        query += $"Update [Quotation].[QuotationsPanels] set PanelProfit = {panelData.PanelProfit} Where PanelID = {panelData.PanelID}; ";
            //    }

            //    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //        connection.Execute(query);

            //    QuotationData.QuotationCost = PanelsData.Sum(p => p.PanelsPrice);
            //    Price.IsOpen = false;
            //}
        }
        private void TargetPriceValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Input.DoubleOnly(e);
        }
        private void BillPriceOptions_Click(object sender, RoutedEventArgs e)
        {
            //QuotationBillPriceOptionsWindow quotationBillPriceOptionsWindow = new QuotationBillPriceOptionsWindow()
            //{
            //    UserData = this.UserData,
            //    QuotationData = this.QuotationData,
            //};
            //quotationBillPriceOptionsWindow.ShowDialog();
        }

        #region Filters

        CollectionViewSource viewData;
        private readonly List<PropertyInfo> filterProperties = new List<PropertyInfo>()
        {
            //(typeof(QPanel)).GetProperty("PanelSN"),
            //(typeof(QPanel)).GetProperty("PanelName"),
            //(typeof(QPanel)).GetProperty("PanelQty"),
            //(typeof(QPanel)).GetProperty("EnclosureType"),
            //(typeof(QPanel)).GetProperty("EnclosureHeight"),
            //(typeof(QPanel)).GetProperty("EnclosureWidth"),
            //(typeof(QPanel)).GetProperty("EnclosureDepth"),
            //(typeof(QPanel)).GetProperty("EnclosureIP"),
            //(typeof(QPanel)).GetProperty("PanelProfit"),
            //(typeof(QPanel)).GetProperty("PanelCost"),
            //(typeof(QPanel)).GetProperty("PanelPrice"),
            //(typeof(QPanel)).GetProperty("PanelsCost"),
            //(typeof(QPanel)).GetProperty("PanelsPrice"),
        };
        private void DataFilter(object sender, FilterEventArgs e)
        {
            try
            {
                //e.Accepted = true;
                //if (e.Item is QPanel record)
                //{
                //    string columnName;
                //    foreach (PropertyInfo property in filterProperties)
                //    {
                //        columnName = property.Name;
                //        string value;
                //        if (property.PropertyType == typeof(DateTime))
                //            value = $"{record.GetType().GetProperty(columnName).GetValue(record):dd/MM/yyyy}";
                //        else
                //            value = $"{record.GetType().GetProperty(columnName).GetValue(record)}".ToUpper();


                //        if (!value.Contains(((TextBox)FindName(property.Name)).Text.ToUpper()))
                //        {
                //            e.Accepted = false;
                //            return;
                //        }
                //    }
                //}
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
            //foreach (PropertyInfo property in filterProperties)
            //{
            //    ((TextBox)FindName(property.Name)).Text = null;
            //}
            //viewData.View.Refresh();
        }

        #endregion

        private void Library_Click(object sender, RoutedEventArgs e)
        {
            //LibraryOfPanelsWindow libraryOfPanelsWindow = new LibraryOfPanelsWindow()
            //{
            //    UserData = UserData,
            //    QuotationToUpdate = QuotationData,
            //    PanelsData = PanelsData,
            //};
            //libraryOfPanelsWindow.ShowDialog();
        }

        private void Recalculate_Click(object sender, RoutedEventArgs e)
        {
            //LoadingControl.Visibility = Visibility.Visible;
            //Events.DoingEvent.DoEvents();

            //using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //{
            //    var items = QItemController.QuotationRecalculateItems(connection, QuotationData.QuotationID);

            //    if (items.Count == 0)
            //    {
            //        LoadingControl.Visibility = Visibility.Collapsed;
            //        return;
            //    }

            //    string query = "";
            //    foreach (QItem item in items)
            //    {
            //        query += $"Update [Quotation].[QuotationsPanelsItems] Set ItemCost = {item.ReferenceCost}, ItemDiscount = {item.ReferenceDiscount} where ItemID = {item.ItemID} ;";
            //    }

            //    connection.Execute(query);

            //    this.Window_Loaded(sender, e);
            //}

            //LoadingControl.Visibility = Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //{
            //    UserData.QuotationID = null;
            //    UserController.UpdateQuotationID(connection, UserData);
            //}
        }

        private void QuotationItems_ClicK(object sender, RoutedEventArgs e)
        {
            //List<QItem> items;
            //using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //{
            //    string query = $"SELECT Quotation.QuotationsPanels.QuotationID, Quotation.QuotationsPanelsItems.Category, Quotation.QuotationsPanelsItems.Code, Quotation.QuotationsPanelsItems.Description, " +
            //                   $"SUM(Quotation.QuotationsPanelsItems.ItemQty) AS ItemQty " +
            //                   $"FROM Quotation.QuotationsPanelsItems INNER JOIN " +
            //                   $"Quotation.QuotationsPanels ON Quotation.QuotationsPanelsItems.PanelID = Quotation.QuotationsPanels.PanelID " +
            //                   $"WHERE(Quotation.QuotationsPanelsItems.ItemCost <> 0) " +
            //                   $"GROUP BY Quotation.QuotationsPanels.QuotationID, Quotation.QuotationsPanelsItems.Description, Quotation.QuotationsPanelsItems.Code, Quotation.QuotationsPanelsItems.Category " +
            //                   $"HAVING (Quotation.QuotationsPanels.QuotationID = {QuotationData.QuotationID}) " +
            //                   $"ORDER BY Quotation.QuotationsPanelsItems.Code";

            //    items = connection.Query<QItem>(query).ToList();
            //}

            //for (int i = 1; i <= items.Count; i++)
            //    items[i - 1].ItemSort = i;

            //double pagesNumber = (items.Count) / 45d;
            //if (pagesNumber - Convert.ToInt32(pagesNumber) != 0)
            //    pagesNumber = Convert.ToInt32(pagesNumber) + 1;

            //List<FrameworkElement> elements = new List<FrameworkElement>();
            //if (pagesNumber != 0)
            //{
            //    for (int i = 1; i <= pagesNumber; i++)
            //    {
            //        Printing.QuotationsItems quotationsItems = new Printing.QuotationsItems() { QuotationData = QuotationData, Items = items.Where(p => p.ItemSort > ((i - 1) * 45) && p.ItemSort <= ((i) * 45)).ToList(), Page = i, Pages = Convert.ToInt32(pagesNumber) };
            //        elements.Add(quotationsItems);
            //    }

            //    Printing.Print.PrintPreview(elements, $"Quotation {QuotationData.QuotationCode} Items");
            //}
            //else
            //{
            //    CMessageBox.Show("Items", "There is no items!!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //}
        }

        private void DiscountRecalculate_Click(object sender, RoutedEventArgs e)
        {
            //CategoriesDiscountsWindow categoriesDiscountsWindow = new CategoriesDiscountsWindow()
            //{
            //    QuotationPanelsWindowData = this,
            //    QuotationID = QuotationData.QuotationID
            //};
            //categoriesDiscountsWindow.ShowDialog();
        }

        private void PanelItems_ClicK(object sender, RoutedEventArgs e)
        {
            //if (PanelsList.SelectedItem is QPanel panel)
            //{
            //    List<QItem> items;
            //    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            //    {
            //        string query = $"SELECT Quotation.QuotationsPanels.QuotationID, Quotation.QuotationsPanelsItems.PanelID, Quotation.QuotationsPanelsItems.Category, Quotation.QuotationsPanelsItems.Code, Quotation.QuotationsPanelsItems.Description, " +
            //                       $"SUM(Quotation.QuotationsPanelsItems.ItemQty) AS ItemQty " +
            //                       $"FROM Quotation.QuotationsPanelsItems INNER JOIN " +
            //                       $"Quotation.QuotationsPanels ON Quotation.QuotationsPanelsItems.PanelID = Quotation.QuotationsPanels.PanelID " +
            //                       $"WHERE(Quotation.QuotationsPanelsItems.ItemCost <> 0) " +
            //                       $"GROUP BY Quotation.QuotationsPanels.QuotationID, Quotation.QuotationsPanelsItems.PanelID, Quotation.QuotationsPanelsItems.Description, Quotation.QuotationsPanelsItems.Code, Quotation.QuotationsPanelsItems.Category " +
            //                       $"HAVING (Quotation.QuotationsPanels.QuotationID = {QuotationData.QuotationID}) AND (Quotation.QuotationsPanelsItems.PanelID = {panel.PanelID})" +
            //                       $"ORDER BY Quotation.QuotationsPanelsItems.Code";

            //        items = connection.Query<QItem>(query).ToList();
            //    }

            //    for (int i = 1; i <= items.Count; i++)
            //        items[i - 1].ItemSort = i;

            //    double pagesNumber = (items.Count) / 45d;
            //    if (pagesNumber - Convert.ToInt32(pagesNumber) != 0)
            //        pagesNumber = Convert.ToInt32(pagesNumber) + 1;

            //    List<FrameworkElement> elements = new List<FrameworkElement>();
            //    if (pagesNumber != 0)
            //    {
            //        for (int i = 1; i <= pagesNumber; i++)
            //        {
            //            Printing.PanelsItems panelsItems = new Printing.PanelsItems() { QuotationData = QuotationData, PanelData = panel, Items = items.Where(p => p.ItemSort > ((i - 1) * 45) && p.ItemSort <= ((i) * 45)).ToList(), Page = i, Pages = Convert.ToInt32(pagesNumber) };
            //            elements.Add(panelsItems);
            //        }

            //        Printing.Print.PrintPreview(elements, $"Panel {panel.PanelSN}-{panel.PanelName} Items");
            //    }
            //    else
            //    {
            //        CMessageBox.Show("Items", "There is no items!!", CMessageBoxButton.OK, CMessageBoxImage.Warning);
            //    }
            //}
        }
    }
}
