using System.ComponentModel;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.CustomersData
{
	[Table("Customer._Contacts")]
	public class Contact : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[Key] public int Id { get; set; }
		public int CustomerId { get; set; }

		private string _Name;
		public string Name
		{
			get { return this._Name; }
			set { if (value != this._Name) { this._Name = value; NotifyPropertyChanged(); } }
		}

		private string _Address;
		public string Address
		{
			get { return this._Address; }
			set { if (value != this._Address) { this._Address = value; NotifyPropertyChanged(); } }
		}

		private string _Mobile;
		public string Mobile
		{
			get { return this._Mobile; }
			set { if (value != this._Mobile) { this._Mobile = value; NotifyPropertyChanged(); } }
		}

		private string _Email;
		public string Email
		{
			get { return this._Email; }
			set { if (value != this._Email) { this._Email = value; NotifyPropertyChanged(); } }
		}

		private string _Job;
		public string Job
		{
			get { return this._Job; }
			set { if (value != this._Job) { this._Job = value; NotifyPropertyChanged(); } }
		}

		private string _Note;
		public string Note
		{
			get { return this._Note; }
			set { if (value != this._Note) { this._Note = value; NotifyPropertyChanged(); } }
		}

		private bool _Attention;
		[Write(false)]
		public bool Attention
		{
			get { return this._Attention; }
			set { if (value != this._Attention) { this._Attention = value; NotifyPropertyChanged(); } }
		}

		private string _CustomerName;
		[Write(false)]
		public string CustomerName
		{
			get { return this._CustomerName; }
			set { if (value != this._CustomerName) { this._CustomerName = value; NotifyPropertyChanged(); } }
		}
	}
}
