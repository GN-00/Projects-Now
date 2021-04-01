using Dapper;
using System;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using System.Reflection;
using Core.Data.UserData;
using Core.Data.StoreData;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using Core.Windows.MessageWindows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Core.Windows.QuotationsWindows.ItemsWindows
{
    public partial class ItemsWindow : Window
    {
        public Tables TableData;
        public ObservableCollection<Item> ItemsData;
        public ObservableCollection<Item> ItemsDetails;
        public ObservableCollection<Item> ItemsEnclosure;

        public User UserData { get; set; }
        public Panel PanelData { get; set; }
        public Quotation QuotationData { get; set; }
        public PanelsWindow PanelsWindowData { get; set; }
        public List<Reference> ReferencesListData { get; set; }

        public ItemsWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query;
                query = $"Select * From Quotation._Items Where PanelId = {PanelData.Id} And Table = '{Tables.Details}' Order By Sort";
                ItemsDetails = new ObservableCollection<Item>(connection.Query<Item>(query));
                query = $"Select * From Quotation._Items Where PanelId = {PanelData.Id} And Table = '{Tables.Enclosure}' Order By Sort";
                ItemsEnclosure = new ObservableCollection<Item>(connection.Query<Item>(query));
            }

            TableData = Tables.Details;
            ItemsData = ItemsDetails;

            viewData = new CollectionViewSource() { Source = ItemsData };
            viewData.Filter += DataFilter;
            ItemsList.ItemsSource = viewData.View;

            ItemsDetails.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            ItemsEnclosure.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);

            PanelData.UnitCost =
                ItemsDetails.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100)) +
                ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));

            DataContext = new { UserData, PanelData, QuotationData };

            CollectionChanged(null, null);
        }
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            double total = ItemsDetails.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100)) +
                           ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));

            double detailsCost = ItemsDetails.Where(i => i.Article1 != "COPPER").Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));
            double enclosureCost = ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));
            double copperCost = ItemsDetails.Where(i => i.Article1 == "COPPER").Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));

            DetailsCost.Text = $"{detailsCost:N2} ({(detailsCost / total) * 100:N2} %)";
            EnclosureCost.Text = $"{enclosureCost:N2} ({(enclosureCost / total) * 100:N2} %)";
            CopperCost.Text = $"{copperCost:N2} ({(copperCost / total) * 100:N2} %)";
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemType.Text == "Standard Item")
            {
                ItemWindow itemWindow = new ItemWindow()
                {
                    SelectedIndex = this.ItemsData.Count,
                    ActionData = Actions.New,
                    UserData = this.UserData,
                    ItemsWindowData = this,
                    ReferencesListData = this.ReferencesListData,
                };
                itemWindow.ShowDialog();
            }
            else
            {
                NewItemWindow newItemWindow = new NewItemWindow()
                {
                    UserData = this.UserData,
                    ActionData = Actions.New,
                    SelectedIndex = this.ItemsData.Count,
                    ItemsWindowData = this,
                    ReferencesListData = this.ReferencesListData,
                };
                newItemWindow.ShowDialog();
            }
        }
        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                if (itemData.Type == ItemTypes.NewItem.ToString())
                {
                    NewItemWindow newItemWindow = new NewItemWindow()
                    {
                        ActionData = Actions.Edit,
                        UserData = this.UserData,
                        ItemsWindowData = this,
                        ItemData = itemData,
                        ReferencesListData = this.ReferencesListData,
                    };
                    newItemWindow.ShowDialog();
                }
                else
                {
                    ItemWindow itemWindow = new ItemWindow()
                    {
                        ActionData = Actions.Edit,
                        UserData = this.UserData,
                        ItemsWindowData = this,
                        ItemData = itemData,
                        ReferencesListData = this.ReferencesListData,
                    };
                    itemWindow.ShowDialog();
                }
            }
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                Item newItem = new Item();
                newItem.Update(itemData);
                int index;

                if (TableData == Tables.Details)
                    index = ItemsDetails.Count;
                else
                    index = ItemsEnclosure.Count;

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    newItem.Sort = index;
                    newItem.Id = Convert.ToInt32(connection.Insert(newItem));
                }

                if (TableData == Tables.Details)
                    ItemsDetails.Add(newItem);
                else
                    ItemsEnclosure.Add(newItem);

                PanelData.UnitCost =
                        ItemsDetails.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100)) +
                        ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));

                ListChanged();
            }
        }
        private void InsertUp_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                if (ItemType.Text == "Standard Item")
                {
                    ItemWindow itemWindow = new ItemWindow()
                    {
                        SelectedIndex = this.ItemsData.IndexOf(itemData),
                        ActionData = Actions.InsertUp,
                        UserData = this.UserData,
                        ItemsWindowData = this,
                        ReferencesListData = this.ReferencesListData,
                    };
                    itemWindow.ShowDialog();
                }
                else
                {
                    NewItemWindow newItemWindow = new NewItemWindow()
                    {
                        SelectedIndex = this.ItemsData.IndexOf(itemData),
                        ActionData = Actions.InsertUp,
                        UserData = this.UserData,
                        ItemsWindowData = this,
                        ReferencesListData = this.ReferencesListData,
                    };
                    newItemWindow.ShowDialog();
                }
            }
        }
        private void InsertDown_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                if (ItemType.Text == "Standard Item")
                {
                    ItemWindow itemWindow = new ItemWindow()
                    {
                        SelectedIndex = this.ItemsData.IndexOf(itemData) + 1,
                        ActionData = Actions.InsertDown,
                        UserData = this.UserData,
                        ItemsWindowData = this,
                        ReferencesListData = this.ReferencesListData,
                    };
                    itemWindow.ShowDialog();
                }
                else
                {
                    NewItemWindow newItemWindow = new NewItemWindow()
                    {
                        SelectedIndex = this.ItemsData.IndexOf(itemData) + 1,
                        ActionData = Actions.InsertDown,
                        UserData = this.UserData,
                        ItemsWindowData = this,
                        ReferencesListData = this.ReferencesListData,
                    };
                    newItemWindow.ShowDialog();
                }
            }
        }
        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                if (itemData.Sort > 0)
                {
                    itemData.Sort -= 1;
                    ItemsData.Move(ItemsData.IndexOf(itemData), ItemsData.IndexOf(itemData) - 1);

                    Item affectedItem = ItemsData.FirstOrDefault(item => item.Sort == itemData.Sort && item.Id != itemData.Id);
                    ++affectedItem.Sort;

                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        connection.Update(itemData);
                        connection.Update(affectedItem);
                    }
                }
            }
        }
        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                if (itemData.Sort < ItemsData.Count && ItemsData.Count != 1)
                {
                    itemData.Sort += 1;
                    ItemsData.Move(ItemsData.IndexOf(itemData), ItemsData.IndexOf(itemData) + 1);
                    Item affectedItem = ItemsData.FirstOrDefault(item => item.Sort == itemData.Sort && item.Id != itemData.Id);
                    --affectedItem.Sort;

                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        connection.Update(itemData);
                        connection.Update(affectedItem);
                    }
                }
            }
        }
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                MessageBoxResult result = MessageWindow.Show("Deleting", $"Are you sure you want to \ndelete {itemData.PartNumber} ?", MessageWindowButton.YesNo, MessageWindowImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (Item item in ItemsData.Where(i => i.Sort > itemData.Sort))
                    {
                        --item.Sort;
                    }
                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        connection.Delete(itemData);
                        connection.Update(ItemsData);
                    }

                    ItemsData.Remove(itemData);

                    PanelData.UnitCost =
                        ItemsDetails.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100)) +
                        ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));
                }
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0 && e.Key == Key.N)
            {
                AddItem_Click(sender, e);
            }
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            TableData = Tables.Details;
            TableName.Text = TableData.ToString();
            ItemsData = ItemsDetails;

            viewData.Source = ItemsData;
            ItemsList.ItemsSource = viewData.View;
        }
        private void Enclosure_Click(object sender, RoutedEventArgs e)
        {
            TableData = Tables.Enclosure;
            TableName.Text = TableData.ToString();
            ItemsData = ItemsEnclosure;

            viewData.Source = ItemsData;
            ItemsList.ItemsSource = viewData.View;
        }
        private void TableChange_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem is Item itemData)
            {
                foreach (Item item in ItemsData.Where(i => i.Sort > itemData.Sort))
                {
                    --item.Sort;
                }

                if (TableData == Tables.Details)
                {
                    itemData.Table = Tables.Enclosure.ToString();
                    ItemsDetails.Remove(itemData);
                    itemData.Sort = ItemsEnclosure.Count;
                    ItemsEnclosure.Add(itemData);
                }
                else
                {
                    itemData.Table = Tables.Details.ToString();
                    ItemsEnclosure.Remove(itemData);
                    itemData.Sort = ItemsDetails.Count;
                    ItemsDetails.Add(itemData);
                }

                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    connection.Update(itemData);
                    if (TableData == Tables.Details)
                        connection.Update(ItemsDetails);
                    else
                        connection.Update(ItemsEnclosure);
                }
            }
        }

        #region Filters

        CollectionViewSource viewData;
        private readonly List<PropertyInfo> filterProperties = new List<PropertyInfo>()
        {
            typeof(Item).GetProperty("Article1"),
            typeof(Item).GetProperty("Article2"),
            typeof(Item).GetProperty("PartNumber"),
            typeof(Item).GetProperty("Description"),
            typeof(Item).GetProperty("Qty"),
            typeof(Item).GetProperty("Brand"),
            typeof(Item).GetProperty("Discount"),
        };
        private void DataFilter(object sender, FilterEventArgs e)
        {
            try
            {
                e.Accepted = true;
                if (e.Item is Item record)
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

        private void SmartEnclosure_Click(object sender, RoutedEventArgs e)
        {
            EnclosuresWindow enclosuresWindow = new EnclosuresWindow()
            {
                UserData = this.UserData,
                PanelData = this.PanelData,
                ItemsDetails = this.ItemsDetails,
                ItemsEnclosure = this.ItemsEnclosure,
            };
            enclosuresWindow.ShowDialog();
        }
        private void DeleteSmartEnclosure_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageWindow.Show("Delete", "Are you sure you want to delete the enclosure!!", MessageWindowButton.YesNo, MessageWindowImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                List<Item> items = new List<Item>();
                items.AddRange(ItemsDetails.Where(i => i.SelectionGroup == SelectionGroups.SmartEnclosure.ToString()));
                items.AddRange(ItemsEnclosure.Where(i => i.SelectionGroup == SelectionGroups.SmartEnclosure.ToString()));
                int detailsCount = items.Where(i => i.Table == Tables.Details.ToString()).Count();
                int enclosureCount = items.Where(i => i.Table == Tables.Enclosure.ToString()).Count();

                int itemsCounter = items.Count;
                if (itemsCounter != 0)
                {
                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        string query;
                        query = $"Delete From [Quotation].[QuotationsPanelsItems] Where PanelID = {PanelData.Id} And SelectionGroup = '{SelectionGroups.SmartEnclosure}'";
                        query += $"Update [Quotation].[QuotationsPanelsItems] Set Sort = (Sort - {detailsCount}) Where (PanelID = {PanelData.Id}) And (Table = '{Tables.Details}'); ";
                        query += $"Update [Quotation].[QuotationsPanelsItems] Set Sort = (Sort - {enclosureCount}) Where (PanelID = {PanelData.Id}) And (Table = '{Tables.Enclosure}'); ";
                        connection.Execute(query);

                        PanelData.EnclosureType = null;
                        PanelData.EnclosureHeight = null;
                        PanelData.EnclosureWidth = null;
                        PanelData.EnclosureDepth = null;

                        PanelData.EnclosureMetalType = null;
                        PanelData.EnclosureColor = null;
                        PanelData.EnclosureIP = null;
                        PanelData.EnclosureForm = null;
                        PanelData.EnclosureLocation = null;
                        PanelData.EnclosureInstallation = null;
                        PanelData.EnclosureFunctional = null;

                        PanelData.EnclosureName = null;
                        query = Database.UpdateRecord<Panel>();
                        connection.Execute(query, PanelData);
                    }

                    foreach (Item item in items)
                    {
                        if (item.Table == Tables.Details.ToString())
                            ItemsDetails.Remove(item);
                        else
                            ItemsEnclosure.Remove(item);
                    }

                    foreach (Item item in ItemsDetails)
                    {
                        item.Sort -= detailsCount;
                    }

                    foreach (Item item in ItemsEnclosure)
                    {
                        item.Sort -= enclosureCount;
                    }
                }
            }
        }

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (QuotationPanelsWindowData != null)
        //    {
        //        PanelData.UnitCost = ItemsDetails.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100)) +
        //                              ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));

        //        QuotationPanelsWindowData.Visibility = Visibility.Visible;
        //        QuotationData.QuotationCost = Math.Round
        //            (QuotationPanelsWindowData.panelsData.Sum<Panel>(panel => panel.UnitCost * panel.PanelQty / (1 - panel.PanelProfit / 100)), 3);
        //    }
        //}
    }
}
