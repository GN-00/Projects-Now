using Core.Enums;
using System.ComponentModel;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Core.Data.QuotationsData
{
    [Table("[Quotation].[_Terms&Conditions]")]
    public class Term : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Key] 
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int Sort { get; set; }

        private string _Condition;
        public string Condition
        {
            get { return this._Condition; }
            set { if (value != this._Condition) { this._Condition = value; NotifyPropertyChanged(); } }
        }
        public string ConditionType { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDefault { get; set; }

        public static void GetDefaultTerms(SqlConnection connection, int quotationId)
        {
            var list = new List<Term>()
            {
                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "This offer consists of LV Switchgear, according to the attached Bill of Prices and the technical part that complement one another.",
                    ConditionType = TermsTypes.ScopeOfSupply.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 2,
                    Condition = "Our offer is limited to the details mentioned; any changes which could be requested by the purchaser will be subject to further offer.",
                    ConditionType = TermsTypes.ScopeOfSupply.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "In Saudi riyals currency of account and payment.",
                    ConditionType = TermsTypes.TotalPrice.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 2,
                    Condition = "Firm within the validity and delivery periods as defined hereunder.",
                    ConditionType = TermsTypes.TotalPrice.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "50 % of the total price along with the order as down payment by cheque, cash or bank transfer.",
                    ConditionType = TermsTypes.PaymentConditions.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 2,
                    Condition = "50 % of the total price before delivery by cheque, cash or bank transfer.",
                    ConditionType = TermsTypes.PaymentConditions.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "This offer is valid for (1) week from offer date.",
                    ConditionType = TermsTypes.ValidityPeriod.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 2,
                    Condition = "Should extension of validity be required, please contact us.",
                    ConditionType = TermsTypes.ValidityPeriod.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "Will be submitted upon the period which will be mentioned in our order acknowledgment after receiving down payment.",
                    ConditionType = TermsTypes.ShopDrawingSubmittals.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "Ex-Store, Jeddah",
                    ConditionType = TermsTypes.Delivery.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 2,
                    Condition = "TBA, from the date of: [P.O or contract], down payment & shop drawing approval.",
                    ConditionType = TermsTypes.Delivery.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },

                new Term()
                {
                    QuotationId = quotationId,
                    Sort = 1,
                    Condition = "The Guarantee period will be 12 months from the date of delivery, for material defect and Malfunction except damage unintentionally done by customer. ",
                    ConditionType = TermsTypes.Guarantee.ToString(),
                    IsUsed = true,
                    IsDefault = true
                },
            };

            connection.Insert(list);
        }
    }
}
