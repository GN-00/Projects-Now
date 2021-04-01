using Core.Enums;
using System.ComponentModel;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.StoreData
{
	[Table("[Reference].[_References]")]
	public class Reference : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[Key] 
	    public int Id { get; set; }

		private string _Category;
		public string Category
		{
			get { return this._Category; }
			set { if (value != this._Category) { this._Category = value; NotifyPropertyChanged(); NotifyPropertyChanged("PartNumber"); } }
		}

		private string _Code;
		public string Code
		{
			get { return this._Code; }
			set { if (value != this._Code) { this._Code = value; NotifyPropertyChanged(); NotifyPropertyChanged("PartNumber"); } }
		}

		[Write(false)]
		public string PartNumber
		{ get { return ($"{Category}{Code}"); } }

		private string _Description;
		public string Description
		{
			get { return this._Description; }
			set { if (value != this._Description) { this._Description = value; NotifyPropertyChanged(); } }
		}

		public string Article1 { get; set; }
		public string Article2 { get; set; }

		private string _Brand;
		public string Brand
		{
			get { return this._Brand; }
			set { if (value != this._Brand) { this._Brand = value; NotifyPropertyChanged(); } }
		}
		public string Remarks { get; set; }

		private string _Unit;
		public string Unit
		{
			get { return this._Unit; }
			set { if (value != this._Unit) { this._Unit = value; NotifyPropertyChanged(); } }
		}

		private double _Cost;
		public double Cost
		{
			get { return this._Cost; }
			set { if (value != this._Cost) { this._Cost = value; NotifyPropertyChanged(); } }
		}

		private double _Discount;
		public double Discount
		{
			get { return this._Discount; }
			set { if (value != this._Discount) { this._Discount = value; NotifyPropertyChanged(); } }
		}

		[Write(false)]
		public string ItemType { get; set; } = ItemTypes.Standard.ToString();
		public string Type { get; set; } 
	}
}
