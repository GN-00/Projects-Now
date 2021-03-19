using Dapper.Contrib.Extensions;

namespace Core.Data.UserData
{
    [Table("User._Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserCode { get; set; }

        [Write(false)]
        public string Title { get; set; }
        [Write(false)]
        public string FirstName { get; set; }
        [Write(false)]
        public string LastName { get; set; }
        [Write(false)]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public bool? Administrator { get; set; }

        public int? InquiryId { get; set; }
        public int? QuotationId { get; set; }
        public int? JobOrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? ContactId { get; set; }
        public int? ConsultantId { get; set; }
        public int? SupplierId { get; set; }
        public int? AccountId { get; set; }
        public int? FinanceOrderId { get; set; }

        public bool? IsEstimator { get; set; }
        public bool? IsSeller { get; set; }

        public bool? AccessTendaring { get; set; }
        public bool? AccessProjects { get; set; }
        public bool? AccessItems { get; set; }
        public bool? AccessFinance { get; set; }


        #region Tendaring
        public bool AccessInquiries { get; set; }
        public bool AccessQuote { get; set; }
        public bool AccessQuotations { get; set; }
        public bool AccessCustomers { get; set; }
        public bool AccessContacts { get; set; }
        public bool AccessConsultants { get; set; }

        public bool QuotationsDiscount { get; set; }
        public double QuotationsDiscountValue { get; set; }

        //public bool QuoteManager { get; set; }
        //public bool QuotationItemsDiscount { get; set; }
        //public bool QuotationsManage { get; set; }

        //public bool CustomersWindowAdd { get; set; }
        //public bool CustomersWindowEdit { get; set; }
        //public bool CustomersWindowDelete { get; set; }
        //public bool CustomersWindowHistory { get; set; }

        //public bool ConsultantsWindowAdd { get; set; }
        //public bool ConsultantsWindowEdit { get; set; }
        //public bool ConsultantsWindowDelete { get; set; }

        //public bool ContactsWindowAdd { get; set; }
        //public bool ContactsWindowEdit { get; set; }
        //public bool ContactsWindowDelete { get; set; }
        #endregion 

        //#region Projects
        //public bool AccessProjects { get; set; }
        //public bool AccessNewJobOrder { get; set; }
        //public bool AccessJobOrders { get; set; }
        //public bool JobOrderInformation { get; set; }
        //public bool JobOrderAcknowledgement { get; set; }
        //public bool JobOrderPurchaseOrders { get; set; }
        //public bool JobOrderPanels { get; set; }
        //public bool JobOrderPosting { get; set; }
        //public bool JobOrderInvoicing { get; set; }


        //#endregion

        //#region Items
        //public bool AccessItems { get; set; }
        //public bool AccessReferences { get; set; }
        //public bool ReferencesDiscount { get; set; }
        //public bool AccessStore { get; set; }
        //#endregion

        //#region Finance
        //public bool AccessFinance { get; set; }
        //public bool AccessCompanyAccount { get; set; }
        //public bool AccessJobordersFinance { get; set; }
        //public bool AccessStatements { get; set; }
        //public bool AccessSuppliersInvoices { get; set; }
        //public bool AccessTransportation { get; set; }
        //#endregion

    }
}
