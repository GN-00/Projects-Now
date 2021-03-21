namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InquiriesView : DbMigration
    {
        public override void Up()
        {
            Sql($"Create View Inquiry._InquiriesView As " +
                $"SELECT TOP (100) PERCENT Inquiry._Inquiries.Id, Inquiry._Inquiries.CustomerId, Inquiry._Inquiries.ConsultantId, Inquiry._Inquiries.EstimatorId, Inquiry._Inquiries.SalesmanId, " +
                $"Inquiry._Inquiries.RegisterCode, Inquiry._Inquiries.ProjectName, Inquiry._Inquiries.RegisterDate, Inquiry._Inquiries.DuoDate, Inquiry._Inquiries.Priority, " +
                $"Inquiry._Inquiries.RegisterNumber, Inquiry._Inquiries.RegisterMonth, Inquiry._Inquiries.RegisterYear, Inquiry._Inquiries.DeliveryCondition, " +
                $"Customer._Customers.Name AS CustomerName, [User]._Estimators.EstimatorCode, [User]._Estimators.EstimatorName, " +
                $"CASE WHEN COUNT(Quotation._Quotations.Id) = 0 THEN 'New' ELSE 'Quoting' END AS Status " +
                $"FROM Inquiry._Inquiries LEFT OUTER JOIN " +
                $"[User]._Estimators ON Inquiry._Inquiries.EstimatorId = [User]._Estimators.Id LEFT OUTER JOIN " +
                $"Quotation._Quotations ON Inquiry._Inquiries.Id = Quotation._Quotations.InquiryId LEFT OUTER JOIN " +
                $"Customer._Customers ON Inquiry._Inquiries.CustomerId = Customer._Customers.Id " +
                $"GROUP BY Inquiry._Inquiries.Id, Inquiry._Inquiries.CustomerId, Inquiry._Inquiries.ConsultantId, Inquiry._Inquiries.EstimatorId, Inquiry._Inquiries.SalesmanId, " +
                $"Inquiry._Inquiries.RegisterCode, Inquiry._Inquiries.ProjectName, Inquiry._Inquiries.RegisterDate, Inquiry._Inquiries.DuoDate, Inquiry._Inquiries.Priority, " +
                $"Inquiry._Inquiries.RegisterNumber, Inquiry._Inquiries.RegisterMonth, Inquiry._Inquiries.RegisterYear, Inquiry._Inquiries.DeliveryCondition, " +
                $"Customer._Customers.Name, [User]._Estimators.EstimatorCode, [User]._Estimators.EstimatorName " +
                $"Order By Inquiry._Inquiries.RegisterYear Desc, Inquiry._Inquiries.RegisterMonth Desc, Inquiry._Inquiries.RegisterNumber Desc");
        }
        
        public override void Down()
        {
            Sql("Drop View Inquiry._InquiriesView");
        }
    }
}
