using System;
using System.Collections.Generic;
using System;
using Dapper;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using Core.Data.UserData;
using Core.Data.StoreData;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using Core.Windows.MessageWindows;

namespace Core.Windows.QuotationsWindows.ItemsWindows
{
    public partial class NewItemWindow : Window
    {
        public User UserData { get; set; }
        public Item ItemData { get; set; }
        public int SelectedIndex { get; set; }
        public Actions ActionData { get; set; }
        public Reference ReferenceData { get; set; }
        public List<Reference> ReferencesListData { get; set; }
        public ItemsWindow ItemsWindowData { get; set; }

        Item newItemData;
        public NewItemWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query;
                if (ReferencesListData == null)
                {
                    query = $"Select * From Reference._References";
                    ReferencesListData = connection.Query<Reference>(query).ToList();
                }
                query = "Select Article From Quotation._Articles Order By Sort";
                Article1.ItemsSource = connection.Query<string>(query);
                query = "Select Article2 From Quotation._Items Where Article2 is Not Null And Article2 <> ''  Group By Article2 Order By Article2 ";
                Article2.ItemsSource = connection.Query<string>(query);
            }

            if (ActionData == Actions.Edit)
            {
                newItemData = new Item();
                newItemData.Update(ItemData);

                PartNumbersList.Text = newItemData.PartNumber;
                Description.Text = newItemData.Description;
                Article1.Text = newItemData.Article1;
                Article2.Text = newItemData.Article2;
                Brand.Text = newItemData.Brand;
                Table.Text = newItemData.Table;
                Remarks.Text = newItemData.Remarks;
                Discount.Text = newItemData.Discount.ToString("N2");
                Cost.Text = newItemData.UnitCost.ToString("N2");
                Qty.Text = newItemData.Qty.ToString("N2");
                Unit.Text = newItemData.Unit;
            }
            else
            {
                newItemData = new Item();
                Table.Text = ItemsWindowData.TableData.ToString();
            }

            CostCalculator();
            PartNumbersList.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isReady = true;
            string message = "Please select correct reference!";

            newItemData.Code = PartNumbersList.Text;
            newItemData.Description = Description.Text;
            newItemData.Article1 = Article1.Text;
            if (string.IsNullOrWhiteSpace(Article2.Text)) newItemData.Article2 = null;
            else newItemData.Article2 = Article2.Text;
            newItemData.Brand = Brand.Text;
            newItemData.Table = Table.Text;
            newItemData.Qty = double.Parse(Qty.Text);
            newItemData.Discount = double.Parse(Discount.Text);
            newItemData.UnitCost = double.Parse(Cost.Text);
            newItemData.Type = ItemTypes.NewItem.ToString();
            newItemData.Unit = Unit.Text;
            newItemData.Remarks = Remarks.Text;

            if (newItemData.PartNumber == "") { isReady = false; message += $"\n*Part Number."; }
            if (newItemData.Description == "") { isReady = false; message += $"\n *Deacription."; }
            if (newItemData.Article1 == "") { isReady = false; message += $"\n *Article."; }
            if (string.IsNullOrWhiteSpace(newItemData.Unit)) { isReady = false; message += $"\n *Unit."; }
            if (newItemData.Unit == "Set" || newItemData.Unit == "No" || newItemData.Unit == "Roll")
            {
                bool isInt = (double)(int)newItemData.Qty == newItemData.Qty;
                if (!isInt) { isReady = false; message += $"\n *Qty must be Integer."; }
            }

            if (isReady)
            {
                string query;
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    if (ActionData == Actions.Edit)
                    {
                        connection.Update(newItemData);
                        ItemData.Update(newItemData);
                    }
                    else
                    {
                        newItemData.Sort = SelectedIndex;
                        newItemData.PanelId = ItemsWindowData.PanelData.Id;

                        if (ActionData == Actions.New)
                        {
                            ItemsWindowData.ItemsData.Add(newItemData);
                        }
                        else
                        {
                            connection.Execute($"Update [Quotation].[_Items] Set Sort = Sort + 1 Where Sort >= {newItemData.Sort} And PanelId ={newItemData.PanelId} And Table = '{ItemsWindowData.TableData}'");
                            foreach (Item item in ItemsWindowData.ItemsData.Where(item => item.Sort >= newItemData.Sort))
                            {
                                item.Sort++;
                            }
                            ItemsWindowData.ItemsData.Insert(SelectedIndex, newItemData);
                        }

                        newItemData.Id = Convert.ToInt32(connection.Insert(newItemData));
                    }
                }

                ItemsWindowData.PanelData.UnitCost =
                    ItemsWindowData.ItemsDetails.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100)) +
                    ItemsWindowData.ItemsEnclosure.Sum<Item>(item => item.UnitCost * item.Qty * (1 - item.Discount / 100));

                this.Close();
            }
            else
            {
                MessageWindow.Show("Error", message, MessageWindowButton.OK, MessageWindowImage.Warning);
            }

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Table_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Input.ArrowsOnly(e);
        }
        private void Qty_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Unit.Text == "No" || Unit.Text == "Set")
                Input.IntOnly(e, 4);
            else
                Input.DoubleOnly(e);
        }
        private void Qty_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Qty.Text) || Qty.Text == "0")
                Qty.Text = "1";

            CostCalculator();
        }

        private void Cost_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Input.DoubleOnly(e);
        }

        private void Cost_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Cost.Text))
                Cost.Text = "0";

            CostCalculator();
        }

        private void Discount_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Input.DoubleOnly(e);
        }
        private void Discount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Discount.Text)) Discount.Text = "0";
            if (double.Parse(Discount.Text) > 100) Discount.Text = "0";
            CostCalculator();
        }
        private void CostCalculator()
        {
            double qty, discount, cost;
            if (string.IsNullOrWhiteSpace(Qty.Text)) qty = 0;
            else qty = double.Parse(Qty.Text);

            if (string.IsNullOrWhiteSpace(Discount.Text)) discount = 1;
            else discount = (1 - double.Parse(Discount.Text) / 100);

            if (string.IsNullOrWhiteSpace(Cost.Text)) cost = 0;
            else cost = double.Parse(Cost.Text);

            TotalCost.Text = (cost * qty).ToString("N2");
            TotalPrice.Text = (cost * qty * discount).ToString("N2");
        }

        private void NewItemCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ActionData == Actions.Edit)
            {
                MessageWindow.Show("Items", "Can't change this Item type!!", MessageWindowButton.OK, MessageWindowImage.Warning);
            }
            else
            {
                ItemWindow itemWindow = new ItemWindow()
                {
                    UserData = UserData,
                    ActionData = ActionData,
                    SelectedIndex = SelectedIndex,
                    ItemsWindowData = ItemsWindowData,
                    ReferencesListData = ReferencesListData,
                };
                this.Close();
                itemWindow.ShowDialog();
            }
        }
    }
}
