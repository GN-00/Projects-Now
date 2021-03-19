using System.Windows;
//using ProjectsNow.Database;
using System.Windows.Controls;
using System.Collections.Generic;
//using ProjectsNow.Windows.JobOrderWindows;

namespace ProjectsNow.Windows.MainWindows
{
    public partial class ProjectsControl : UserControl
    {
        //public User UserData { get; set; }
        //public MainWindow MainWindowData { get; set; }

        public ProjectsControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<StackPanel> buttons = new List<StackPanel>();
            //if (UserData.AccessNewJobOrder == true) buttons.Add(NewJobOrder);
            //if (UserData.AccessJobOrders == true) buttons.Add(JobOrders);
            //if (UserData.AccessQuotations == true) buttons.Add(Quotation);
            //if (UserData.AccessCustomers == true) buttons.Add(Customers);
            //if (UserData.AccessConsultants == true) buttons.Add(Consultants);

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

        private void NewJobOrder_Click(object sender, RoutedEventArgs e)
        {
            //NewJobOrderWindow newJobOrderWindow = new NewJobOrderWindow() { UserData = this.UserData };
            //newJobOrderWindow.ShowDialog();
        }

        private void JobOrders_Click(object sender, RoutedEventArgs e)
        {
            //JobOrdersWindow jobOrdersWindow = new JobOrdersWindow() { UserData = this.UserData };
            //jobOrdersWindow.ShowDialog();
        }

        private void JobOrderPanels_Click(object sender, RoutedEventArgs e)
        {
            //JobOrderSelectorWindow jobOrderSelectorWindow = new JobOrderSelectorWindow()
            //{
            //    UserData = this.UserData,
            //    MainWindowData = this.MainWindowData,
            //    WindowToOpen = ProjectWindows.JobOrderpanels
            //};
            //jobOrderSelectorWindow.ShowDialog();
        }


    }
}
