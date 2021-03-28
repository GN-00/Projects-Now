using System.ComponentModel;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.QuotationsData
{
    [Table("[Quotation].[_Options]")]
    public class Option : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [Key] 
        public int Id { get; set; }
        public int QuotationId { get; set; }

        private int _Number;
        public int Number
        {
            get { return this._Number; }
            set { if (value != this._Number) { this._Number = value; NotifyPropertyChanged(); NotifyPropertyChanged("Code"); } }
        }

        [Write(false)]
        public string Code
        {
            get
            {
                return (((char)(64 + Number)).ToString());
            }
        }

        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set { if (value != this._Name) { this._Name = value; NotifyPropertyChanged(); } }
        }
    }
}
