using System.ComponentModel;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.QuotationsData
{
	[Table("[Quotation].[_Items]")]
	public class Item : INotifyPropertyChanged 
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[Key]
		public int Id { get; set; }
		public int PanelId { get; set; }

		private string _Article1;
		public string Article1
		{
			get { return this._Article1; }
			set { if (value != this._Article1) { this._Article1 = value; NotifyPropertyChanged(); } }
		}

		private string _Article2;
		public string Article2
		{
			get { return this._Article2; }
			set { if (value != this._Article2) { this._Article2 = value; NotifyPropertyChanged(); } }
		}

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

		public string Unit { get; set; } = "No";

		private double _Qty;
		public double Qty
		{
			get { return this._Qty; }
			set
			{
				if (value != this._Qty)
				{
					this._Qty = value;

					NotifyPropertyChanged();
					NotifyPropertyChanged("TotalCost");
					NotifyPropertyChanged("TotalPrice");
				}
			}
		}

		private string _Brand;
		public string Brand
		{
			get { return this._Brand; }
			set { if (value != this._Brand) { this._Brand = value; NotifyPropertyChanged(); } }
		}

		private string _Remarks;
		public string Remarks
		{
			get { return this._Remarks; }
			set { if (value != this._Remarks) { this._Remarks = value; NotifyPropertyChanged(); } }
		}

		private double _UnitCost;
		public double UnitCost
		{
			get { return this._UnitCost; }
			set
			{
				if (value != this._UnitCost)
				{
					if (value < 0)
						this._UnitCost = 0;
					else
						this._UnitCost = value;

					NotifyPropertyChanged();
					NotifyPropertyChanged("UnitPrice");
					NotifyPropertyChanged("TotalCost");
					NotifyPropertyChanged("TotalPrice");
				}
			}
		}

		private double _Discount;
		public double Discount
		{
			get { return this._Discount; }
			set
			{
				if (value != this._Discount)
				{
					if (value > 100)
						this._Discount = 0;
					else
						this._Discount = value;

					NotifyPropertyChanged();
					NotifyPropertyChanged("UnitPrice");
					NotifyPropertyChanged("TotalPrice");
				}
			}
		}

		[Write(false)]
		public double UnitPrice
		{
			get { return this._UnitCost * (1 - (this._Discount / 100)); }
		}

		[Write(false)]
		public double TotalCost
		{
			get { return (this._UnitCost * this._Qty); }
		}

		[Write(false)]
		public double TotalPrice
		{
			get { return (this._UnitCost * this._Qty * (1 - (this._Discount / 100))); }
		}

		public string Table { get; set; }

		private string _Type;
		public string Type
		{
			get { return this._Type; }
			set { if (value != this._Type) { this._Type = value; NotifyPropertyChanged(); } }
		}

		public int Sort { get; set; }
		public string SelectionGroup { get; set; }


		//RecalculateItems Only
		[Write(false)]
		public double ReferenceCost { get; set; }
		[Write(false)]
		public double ReferenceDiscount { get; set; }

	}
}
