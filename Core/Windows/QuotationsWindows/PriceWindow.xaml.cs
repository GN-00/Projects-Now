using Core.Data;
using Core.Enums;
using System.Windows;
using Core.Data.UserData;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;
using Core.Windows.MessageWindows;

namespace Core.Windows.QuotationsWindows
{
    public partial class PriceWindow : Window
    {
        public User UserData { get; set; }
        public Quotation QuotationData { get; set; }
        public PanelsWindow PanelsWindow { get; set; }


        Quotation newQuotationData;
        public PriceWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            newQuotationData = new Quotation();
            newQuotationData.Update(QuotationData);
            DataContext = newQuotationData;

            if (UserData.QuotationsDiscount == true && QuotationData.Status == Statuses.Running.ToString())
            {
                DiscountInput.IsReadOnly = false;
                DiscountInput.Focusable = true;
                DiscountInput.IsHitTestVisible = true;

                DiscountValueInput.IsReadOnly = false;
                DiscountValueInput.Focusable = true;
                DiscountValueInput.IsHitTestVisible = true;

                Save.Visibility = Visibility.Visible;
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Update(newQuotationData);
            }

            QuotationData.Update(newQuotationData);
            this.Close();
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Input.DoubleOnly(e);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PanelsWindow == null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    UserData.QuotationId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.QuotationId));
                }
            }
        }

        private void DiscountInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DiscountInput.Text))
            {
                newQuotationData.Discount = double.Parse(((TextBox)sender).Text);
                if (newQuotationData.Discount > UserData.QuotationsDiscountValue)
                {
                    MessageWindow.Show("Discount", $"Max discount can be apply is {UserData.QuotationsDiscountValue}!!", MessageWindowButton.OK, MessageWindowImage.Warning);
                    newQuotationData.Discount = 0;
                    DiscountInput.Text = (0).ToString("N3");
                    DiscountValueInput.Text = newQuotationData.DiscountValue.ToString("N3");
                }
                else
                {
                    DiscountValueInput.Text = newQuotationData.DiscountValue.ToString("N3");
                }
            }
            else
            {
                newQuotationData.Discount = 0;
                DiscountInput.Text = (0).ToString("N3");
                DiscountValueInput.Text = newQuotationData.DiscountValue.ToString("N3");
            }
        }
        private void DiscountValueInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DiscountValueInput.Text))
            {
                double value = double.Parse(DiscountValueInput.Text);
                if (value != newQuotationData.DiscountValue)
                {
                    newQuotationData.Discount = (value / newQuotationData.Cost) * 100;
                    if (newQuotationData.Discount > UserData.QuotationsDiscountValue)
                    {
                        MessageWindow.Show("Discount", $"Max discount can be apply is {UserData.QuotationsDiscountValue}!!", MessageWindowButton.OK, MessageWindowImage.Warning);
                        newQuotationData.Discount = 0;
                        DiscountInput.Text = (0).ToString("N3");
                        DiscountValueInput.Text = (0).ToString("N3");
                    }
                    else
                    {
                        DiscountValueInput.Text = newQuotationData.DiscountValue.ToString("N3");
                    }
                }
            }
            else
            {
                newQuotationData.Discount = 0;
                DiscountInput.Text = (0).ToString("N3");
                DiscountValueInput.Text = (0).ToString("N3");
            }
        }
    }
}
