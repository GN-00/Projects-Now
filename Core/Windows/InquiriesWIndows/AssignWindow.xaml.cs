using Core.Data.InquiriesData;
using Core.Data.UserData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Core.Windows.InquiriesWindows
{
    /// <summary>
    /// Interaction logic for AssignWindow.xaml
    /// </summary>
    public partial class AssignWindow : Window
    {
        public User UserData { get; set; }
        public Inquiry InquiryData { get; set; }
        public AssignWindow()
        {
            InitializeComponent();
        }
    }
}
