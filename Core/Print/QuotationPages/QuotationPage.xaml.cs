using System.Windows.Controls;

namespace Core.Print.QuotationPages
{
    public partial class QuotationPage : UserControl
    {
        public string QuotationCode { get; set; }

        public QuotationPage()
        {
            InitializeComponent();
        }

        public QuotationPage(string quotationCode)
        {
            InitializeComponent();
            QuotationCode = quotationCode;
            DataContext = new { QuotationCode };
        }
    }
}
