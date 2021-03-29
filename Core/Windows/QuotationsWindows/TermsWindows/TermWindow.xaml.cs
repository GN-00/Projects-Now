using System;
using Core.Data;
using Core.Enums;
using System.Linq;
using System.Windows;
using Core.Data.UserData;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.QuotationsData;
using Dapper.Contrib.Extensions;
using Core.Windows.MessageWindows;

namespace Core.Windows.QuotationsWindows.TermsWindows
{
    public partial class TermWindow : Window
    {
        public User UserData { get; set; }
        public Term TermData { get; set; }
        public Actions ActionsData { get; set; }
        public TermsWindow TermsWindowData { get; set; }

        string termType;
        public TermWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (ActionsData == Actions.New)
            {
                var selectedConditionTab = TermsWindowData.Tabs.SelectedIndex;
                termType = ((TermTypes)selectedConditionTab).ToString();
            }
            else
            {
                termType = TermData.ConditionType;
                Condition.Text = TermData.Condition;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Term term;
            if (string.IsNullOrWhiteSpace(Condition.Text))
            {
                MessageWindow.Show("Error", "Please write correct term!", MessageWindowButton.OK, MessageWindowImage.Warning);
            }
            else
            {
                if (ActionsData == Actions.New)
                {
                    term = new Term()
                    {
                        QuotationId = TermsWindowData.QuotationData.Id,
                        Condition = Condition.Text,
                        ConditionType = termType,
                        IsUsed = true,
                        IsDefault = false,
                        Sort = TermsWindowData.terms.Where<Term>(item => item.ConditionType == termType).Max<Term>(item => item.Sort) + 1,
                    };

                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        term.Id = Convert.ToInt32(connection.Insert(term));
                    }

                    TermsWindowData.terms.Add(term);
                    var listBoxName = termType;
                    var listBox = TermsWindowData.FindName(listBoxName) as ListBox;
                    listBox.ItemsSource = TermsWindowData.terms.Where<Term>(item => item.ConditionType == termType);

                }
                else
                {
                    TermData.Condition = Condition.Text;
                    using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                    {
                        connection.Update(TermData);
                    }
                }

                this.Close();

            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Condition_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0 && e.Key == Key.V)
                Condition.Text.Insert(Condition.CaretIndex, Clipboard.GetText());
        }
    }
}
