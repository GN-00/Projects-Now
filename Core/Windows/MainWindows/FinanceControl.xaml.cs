using System.Windows;
using Core.Data.UserData;
using System.Windows.Controls;
using System.Collections.Generic;
//using ProjectsNow.Windows.FinanceWindows;

namespace Core.Windows.MainWindows
{
    public partial class FinanceControl : UserControl
    {
        public User UserData { get; set; }
        public MainWindow MainWindowData { get; set; }
        public FinanceControl()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<StackPanel> buttons = new List<StackPanel>();
            //if (UserData.AccessCompanyAccount == true) buttons.Add(Accounts);
            //if (UserData.AccessJobordersFinance == true) buttons.Add(JobOrders);
            //if (UserData.AccessStatements == true) buttons.Add(Statements);
            //if (UserData.AccessSuppliersInvoices == true) buttons.Add(SuppliersInvoices);
            //if (UserData.AccessTransportation == true) buttons.Add(Transportation);

            if (buttons.Count == 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int ii = 0; ii < 2; ii++)
                    {
                        ResourcesArea.Children.Remove(buttons[ii + (i * 2)]);
                        ((StackPanel)FindName($"Area{i}")).Children.Add(buttons[ii + (i * 2)]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int ii = 0; ii < 3; ii++)
                    {
                        if ((ii + (i * 3)) >= buttons.Count) break;
                        ResourcesArea.Children.Remove(buttons[ii + (i * 3)]);
                        ((StackPanel)FindName($"Area{i}")).Children.Add(buttons[ii + (i * 3)]);
                    }
                }
            }
        }

        private void Accounts_Click(object sender, RoutedEventArgs e)
        {
            //CompanyAccountsWindow companyAccountsWindow = new CompanyAccountsWindow() { UserData = this.UserData };
            //companyAccountsWindow.ShowDialog();
        }

        private void JobOrders_Click(object sender, RoutedEventArgs e)
        {
            //FinanceWindows.JobOrdersWindows.JobOrdersWindow jobOrdersWindow = new FinanceWindows.JobOrdersWindows.JobOrdersWindow() { UserData = this.UserData };
            //jobOrdersWindow.ShowDialog();
        }

        private void Statements_Click(object sender, RoutedEventArgs e)
        {
            //FinanceWindows.CustomersWindows.CustomersWindow customersWindow = new FinanceWindows.CustomersWindows.CustomersWindow() { UserData = UserData };
            //customersWindow.ShowDialog();
        }

        private void SuppliersInvoices_Click(object sender, RoutedEventArgs e)
        {
            //FinanceWindows.SuppliersInvoicesWindows.InvoicesWindow invoicesWindow = new FinanceWindows.SuppliersInvoicesWindows.InvoicesWindow() { UserData = UserData };
            //invoicesWindow.ShowDialog();
        }

        private void Transportation_Click(object sender, RoutedEventArgs e)
        {
            //FinanceWindows.TransportationWindows.TransportationWindow transportationWindow = new FinanceWindows.TransportationWindows.TransportationWindow() { UserData = UserData };
            //transportationWindow.ShowDialog();
        }
    }
}
