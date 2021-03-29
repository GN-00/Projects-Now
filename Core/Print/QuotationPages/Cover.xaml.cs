using Dapper;
using Core.Data;
using Core.Data.UserData;
using System.Data.SqlClient;
using System.Windows.Controls;
using Core.Data.CustomersData;
using Core.Data.QuotationsData;
using System.Collections.Generic;

namespace Core.Print.QuotationPages
{
    public partial class Cover : UserControl
    {
        public User UserData { get; set; }
        public Quotation QuotationData { get; set; }
        public List<Content> Contents { get; set; }

        Contact ContactData { get; set; }
        public Cover()
        {
            InitializeComponent();
        }

        public Cover(User user, Quotation quotation, List<Content> contents)
        {
            InitializeComponent();
            UserData = user;
            QuotationData = quotation;
            using (SqlConnection connection = new SqlConnection(Database.ConnectionString))
            {
                var query = $"Select * From Inquiry._ProjectsContacts Where InquiryId = {quotation.InquiryId} And Attention = 'True'";
                ContactData = connection.QueryFirstOrDefault<Contact>(query);
            }

            Contents = contents;
            DataContext = new { UserData, QuotationData, ContactData, Contents };
        }
    }
}
