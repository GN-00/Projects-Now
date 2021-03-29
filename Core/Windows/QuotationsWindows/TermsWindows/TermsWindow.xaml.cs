using Dapper;
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
using System.Collections.ObjectModel;

namespace Core.Windows.QuotationsWindows.TermsWindows
{
    public partial class TermsWindow : Window
    {
        public User UserData { get; set; }
        public Quotation QuotationData { get; set; }
        public PanelsWindow PanelsWindow { get; set; } 

        ListBox listBox;
        public ObservableCollection<Term> terms;

        public TermsWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                string query = $"Select * From Quotation._Terms&Conditions Where QuotationId = {QuotationData.Id}";
                terms = new ObservableCollection<Term>(connection.Query<Term>(query));
            }

            ScopeOfSupply.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.ScopeOfSupply.ToString()).OrderBy(t => t.Sort);
            TotalPrice.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.TotalPrice.ToString()).OrderBy(t => t.Sort);
            PaymentConditions.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.PaymentConditions.ToString()).OrderBy(t => t.Sort);
            ValidityPeriod.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.ValidityPeriod.ToString()).OrderBy(t => t.Sort);
            ShopDrawingSubmittals.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.ShopDrawingSubmittals.ToString()).OrderBy(t => t.Sort);
            Delivery.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.Delivery.ToString()).OrderBy(t => t.Sort);
            Guarantee.ItemsSource = terms.Where<Term>(term => term.ConditionType == TermTypes.Guarantee.ToString()).OrderBy(t => t.Sort);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Update(terms);
            }
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListBox_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            listBox = ((ListBox)sender);
            if (e.OriginalSource.GetType() != typeof(ScrollViewer))
            {
                if (listBox.SelectedItem is Term term)
                {
                    if (term.IsDefault)
                        listBox.ContextMenu = this.Resources["DefaultItemContextMenu"] as ContextMenu;
                    else
                        listBox.ContextMenu = this.Resources["ItemContextMenu"] as ContextMenu;
                }
                else
                {
                    listBox.ContextMenu = this.Resources["NoItemContextMenu"] as ContextMenu;
                }
            }
            else
            {
                listBox.ContextMenu = this.Resources["NoItemContextMenu"] as ContextMenu;
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            TermWindow termWindow = new TermWindow()
            {
                UserData = this.UserData,
                ActionsData = Actions.New,
                TermsWindowData = this,
            };
            termWindow.ShowDialog();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (listBox != null)
            {
                if (listBox.SelectedItem is Term term)
                {
                    TermWindow termWindow = new TermWindow()
                    {
                        UserData = this.UserData,
                        ActionsData = Actions.Edit,
                        TermsWindowData = this,
                        TermData = term,
                    };
                    termWindow.ShowDialog();
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox != null)
            {
                if (listBox.SelectedItem is Term term)
                {
                    if (!term.IsDefault)
                    {
                        MessageBoxResult result = MessageWindow.Show("Deleting", "Are you Sure to delete this term?", MessageWindowButton.YesNo, MessageWindowImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                            {
                                connection.Delete(term);
                            }

                            var conditionType = term.ConditionType;
                            terms.Remove(term);

                            var listBoxName = conditionType;
                            var listBox = FindName(listBoxName) as ListBox;
                            listBox.ItemsSource = terms.Where<Term>(item => item.ConditionType == conditionType);
                        }
                    }
                }
            }
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is Term term)
                Clipboard.SetText(term.Condition);
        }
        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (listBox != null)
            {
                if (listBox.SelectedItem is Term term)
                {
                    if (term.Sort > 1)
                    {
                        term.Sort--;
                        foreach (Term termData in terms.Where(t => t.Sort == term.Sort && t.ConditionType == term.ConditionType && t.Id != term.Id))
                        {
                            ++termData.Sort;
                        }
                        ((ListBox)FindName(term.ConditionType)).ItemsSource = terms.Where(t => t.ConditionType == term.ConditionType).OrderBy(t => t.Sort);
                    }
                }
            }
        }
        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (listBox != null)
            {
                if (listBox.SelectedItem is Term term)
                {
                    int listCont = terms.Where(t => t.ConditionType == term.ConditionType).Count();
                    if (term.Sort < listCont && listCont != 1)
                    {
                        term.Sort++;
                        foreach (Term termData in terms.Where(t => t.Sort == term.Sort && t.ConditionType == term.ConditionType && t.Id != term.Id))
                        {
                            --termData.Sort;
                        }
                        ((ListBox)FindName(term.ConditionType)).ItemsSource = terms.Where(t => t.ConditionType == term.ConditionType).OrderBy(t => t.Sort);
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(PanelsWindow == null)
            {
                using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
                {
                    UserData.QuotationId = null;
                    connection.UserAccessUpdate(UserData, nameof(UserData.QuotationId));
                }
            }
        }
    }
}
