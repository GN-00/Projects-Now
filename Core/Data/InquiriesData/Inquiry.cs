using System;
using System.ComponentModel;
using Dapper.Contrib.Extensions;
using System.Runtime.CompilerServices;

namespace Core.Data.InquiriesData
{
	[Table("[Inquiry].[_Inquiries]")]
	public class Inquiry : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[Key] 
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public int ConsultantId { get; set; }
		public int EstimationId { get; set; }
		public int SalesmanId { get; set; }

		private int? _QuotationId;
		[Write(false)]
		public int? QuotationId
		{
			get { return this._QuotationId; }
			set { if (value != this._QuotationId) { this._QuotationId = value; NotifyPropertyChanged(); NotifyPropertyChanged("Status"); } }
		}

		private string _RegisterCode;
		public string RegisterCode
		{
			get { return this._RegisterCode; }
			set { if (value != this._RegisterCode) { this._RegisterCode = value; NotifyPropertyChanged(); } }
		}

		private string _ProjectName;
		public string ProjectName
		{
			get { return this._ProjectName; }
			set { if (value != this._ProjectName) { this._ProjectName = value; NotifyPropertyChanged(); } }
		}

		private DateTime _RegisterDate = DateTime.Now;
		public DateTime RegisterDate
		{
			get { return this._RegisterDate; }
			set { if (value != this._RegisterDate) { this._RegisterDate = value; NotifyPropertyChanged(); } }
		}

		private DateTime _DuoDate = DateTime.Now.AddDays(7);
		public DateTime DuoDate
		{
			get { return this._DuoDate; }
			set { if (value != this._DuoDate) { this._DuoDate = value; NotifyPropertyChanged(); } }
		}

		private string _Priority = "Normal";
		public string Priority
		{
			get { return this._Priority; }
			set { if (value != this._Priority) { this._Priority = value; NotifyPropertyChanged(); } }
		}

		public int RegisterNumber { get; set; }
		public int RegisterMonth { get; set; }
		public int RegisterYear { get; set; }
		public string DeliveryCondition { get; set; } = "Ex-Factory";

		//Join
		private string _CustomerName;
		[Write(false)]
		public string CustomerName
		{
			get { return this._CustomerName; }
			set { if (value != this._CustomerName) { this._CustomerName = value; NotifyPropertyChanged(); } }
		}

		private string _EstimationName;
		[Write(false)]
		public string EstimationName
		{
			get { return this._EstimationName; }
			set { if (value != this._EstimationName) { this._EstimationName = value; NotifyPropertyChanged(); } }
		}

		private string _EstimationCode;
		[Write(false)]
		public string EstimationCode
		{
			get { return this._EstimationCode; }
			set { if (value != this._EstimationCode) { this._EstimationCode = value; NotifyPropertyChanged(); } }
		}

		[Write(false)]
		public string Status { get { return (QuotationId == null ? "New" : "Quoting"); } }
	}
}
