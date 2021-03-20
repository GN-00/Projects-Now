using System;
using Core.Enums;
using System.ComponentModel;
using Core.Data.InquiriesData;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.QuotationsData
{
    [Table("[Quotation].[_Quotations]")]
    public class Quotation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Key]
        public int Id { get; set; }
        public int InquiryId { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }
        public int Revise { get; set; } = 0;
        public DateTime? ReviseDate { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;
        public int Month { get; set; } = DateTime.Now.Month;

        private string _Status = Statuses.Running.ToString();
        public string Status
        {
            get { return this._Status; }
            set { if (value != this._Status) { this._Status = value; NotifyPropertyChanged(); } }
        }

        private DateTime? _SubmitDate;
        public DateTime? SubmitDate
        {
            get { return this._SubmitDate; }
            set { if (value != this._SubmitDate) { this._SubmitDate = value; NotifyPropertyChanged(); } }
        }

        public string PowerVoltage { get; set; } = "220-380V";
        public string Phase { get; set; } = "3P + N";
        public string Frequency { get; set; } = "60Hz";
        public string NetworkSystem { get; set; } = "AC";
        public string ControlVoltage { get; set; } = "230V AC";
        public string TinPlating { get; set; } = "Bare Copper";
        public string NeutralSize { get; set; } = "Full of Phase";
        public string EarthSize { get; set; } = "Half of Neutral";
        public string EarthingSystem { get; set; } = "TNS";

        public double Discount { get; set; } = 0;
        public double VAT { get; set; } = Constants.VAT;
        

        //Join Data
        [Write(false)]
        public double Cost { get; set; }

        [Write(false)]
        public double Price { get; set; }

        [Write(false)]
        public double VAT_Value { get; set; }
        
        [Write(false)]
        public double DiscountValue { get; set; }
        
        [Write(false)]
        public double PriceWithVAT { get; set; }

        [Write(false)]
        public Inquiry Inquiry { get; set; }
    }
}
