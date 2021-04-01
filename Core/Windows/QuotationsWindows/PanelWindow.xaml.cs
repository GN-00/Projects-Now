using Dapper;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;
using Core.Windows.MessageWindows;
using System.Collections.ObjectModel;

namespace Core.Windows.QuotationsWindows
{
    public partial class PanelWindow : Window
    {
        public int SelectedIndex { get; set; }
        public Actions ActionData { get; set; }
        public Data.QuotationsData.Panel PanelData { get; set; }
        public Quotation QuotationData { get; set; }
        public ObservableCollection<Data.QuotationsData.Panel> PanelsData { get; set; }


        Data.QuotationsData.Panel newPanelData;
        public PanelWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActionData != Actions.Edit)
            {
                newPanelData = new Data.QuotationsData.Panel()
                {
                    Source = QuotationData.PowerVoltage,
                    Frequency = QuotationData.Frequency,
                    Busbar = QuotationData.TinPlating,
                    NeutralSize = QuotationData.NeutralSize,
                    EarthSize = QuotationData.EarthSize,
                    EarthingSystem = QuotationData.EarthingSystem,
                    QuotationId = QuotationData.Id,
                };
            }
            else
            {
                newPanelData = new Data.QuotationsData.Panel();
                newPanelData.Update(PanelData);
            }

            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                EnclosureType.ItemsSource = connection.Query("Select EnclosureType From [Quotation].[_Panels] Where EnclosureType Is Not Null Group By EnclosureType");
                EnclosureMetalType.ItemsSource = connection.Query("Select EnclosureMetalType From [Quotation].[_Panels] Where EnclosureMetalType Is Not Null Group By EnclosureMetalType ");
                EnclosureColor.ItemsSource = connection.Query("Select EnclosureColor From [Quotation].[_Panels] Where EnclosureColor Is Not Null Group By EnclosureColor");
                EnclosureIP.ItemsSource = connection.Query("Select EnclosureIP From [Quotation].[_Panels] Where EnclosureIP Is Not Null Group By EnclosureIP");
                EnclosureForm.ItemsSource = connection.Query("Select EnclosureForm From [Quotation].[_Panels] Where EnclosureForm Is Not Null Group By EnclosureForm ");
                EnclosureFunctional.ItemsSource = connection.Query("Select EnclosureFunctional From [Quotation].[_Panels] Where EnclosureFunctional Is Not Null Group By EnclosureFunctional");
                EnclosureDoor.ItemsSource = connection.Query("Select EnclosureDoor From [Quotation].[_Panels] Where EnclosureDoor Is Not Null Group By EnclosureDoor");
                Source.ItemsSource = connection.Query("Select Source From [Quotation].[_Panels] Where Source Is Not Null Group By Source");
            }

            DataContext = new { newPanelData };
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Data.QuotationsData.Panel checkName;
            if (ActionData == Actions.New) checkName = PanelsData.Where(p => p.Name == newPanelData.Name).FirstOrDefault();
            else checkName = PanelsData.Where(p => p.Name == newPanelData.Name && p.Id != PanelData.Id).FirstOrDefault();

            if (checkName != null)
            {
                MessageWindow.Show("Name Error", $"Panel name is already exist!\nPanel SN ({checkName.SN})", MessageWindowButton.OK, MessageWindowImage.Warning);
                return;
            }

            bool isReady = true;
            string message = "Please Enter:";
            if (newPanelData.Name == null || newPanelData.Name == "") { message += $"\n Panel Name."; isReady = false; }
            if (newPanelData.Qty == 0) { message += $"\n Panel Qty."; isReady = false; }
            if (newPanelData.Type == null || newPanelData.Type == "") { message += $"\n Panel Type."; isReady = false; }

            if (isReady == true)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    if (ActionData == Actions.Edit)
                    {
                        connection.Update(newPanelData);
                        PanelData.Update(newPanelData);
                    }
                    else
                    {
                        newPanelData.SN = SelectedIndex + 1;

                        if (ActionData == Actions.New)
                        {
                            PanelsData.Add(newPanelData);
                        }
                        else
                        {
                            foreach (Data.QuotationsData.Panel panel in PanelsData.Where(panel => panel.SN >= newPanelData.SN))
                            {
                                panel.SN++;
                            }
                            connection.Update(PanelsData);
                            PanelsData.Insert(SelectedIndex, newPanelData);
                        }

                        newPanelData.Id = System.Convert.ToInt32(connection.Insert(newPanelData));
                    }
                }
                this.Close();
            }
            else
            {
                MessageWindow.Show("Missing Data", message, MessageWindowButton.OK, MessageWindowImage.Warning);
            }

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void IntOnly_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Data.Input.IntOnly(e, 4);
        }
        private void DoubleOnly_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Data.Input.DoubleOnly(e);
        }
        private void QTY_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "" || textBox.Text == null || textBox.Text == "0")
            {
                newPanelData.Qty = 1;
                textBox.Text = "1";
            }
        }
        private void Profit_Discount_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text == null || text == "")
                ((TextBox)sender).Text = "0";
            else
            {
                double value = double.Parse(text);
                if (value > 100)
                {
                    ((TextBox)sender).Text = (100).ToString();
                }
                else if (value < 0)
                {
                    ((TextBox)sender).Text = (0).ToString();
                }
                else
                {
                    ((TextBox)sender).Text = (value).ToString();
                }
            }
        }
    }
}
