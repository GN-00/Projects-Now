using System;
using System.ComponentModel;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.CustomersData
{
    [Table("Customer._Customers")]
    public class Customer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Key] public int Id { get; set; }
        public int SalesmanId { get; set; }
        public int BankId { get; set; }

        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set { if (value != this._Name) { this._Name = value; NotifyPropertyChanged(); } }
        }

        private string _City;
        public string City
        {
            get { return this._City; }
            set { if (value != this._City) { this._City = value; NotifyPropertyChanged(); } }
        }

        private string _Address;
        public string Address
        {
            get { return this._Address; }
            set { if (value != this._Address) { this._Address = value; NotifyPropertyChanged(); } }

        }
        private int? _POBox;
        public int? POBox
        {
            get { return this._POBox; }
            set { if (value != this._POBox) { this._POBox = value; NotifyPropertyChanged(); } }
        }

        private string _Phone;
        public string Phone
        {
            get { return this._Phone; }
            set { if (value != this._Phone) { this._Phone = value; NotifyPropertyChanged(); } }
        }

        private string _Email;
        public string Email
        {
            get { return this._Email; }
            set { if (value != this._Email) { this._Email = value; NotifyPropertyChanged(); } }
        }

        private string _Website;
        public string Website
        {
            get { return this._Website; }
            set { if (value != this._Website) { this._Website = value; NotifyPropertyChanged(); } }
        }

        private DateTime? _StartRelation = DateTime.Now;
        public DateTime? StartRelation
        {
            get { return this._StartRelation; }
            set { if (value != this._StartRelation) { this._StartRelation = value; NotifyPropertyChanged(); } }
        }

        private string _VATNumber;
        public string VATNumber
        {
            get { return this._VATNumber; }
            set { if (value != this._VATNumber) { this._VATNumber = value; NotifyPropertyChanged(); } }
        }

        private string _Note;
        public string Note
        {
            get { return this._Note; }
            set { if (value != this._Note) { this._Note = value; NotifyPropertyChanged(); } }
        }

        private string _SalesmanName;
        [Write(false)]
        public string SalesmanName
        {
            get { return this._SalesmanName; }
            set { if (value != this._SalesmanName) { this._SalesmanName = value; NotifyPropertyChanged(); } }
        }
    }
}
