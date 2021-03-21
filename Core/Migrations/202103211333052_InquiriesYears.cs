namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InquiriesYears : DbMigration
    {
        public override void Up()
        {
            Sql($"Create View Inquiry._Years As " +
                $"SELECT TOP (100) PERCENT RegisterYear AS Value " +
                $"FROM Inquiry._Inquiries " +
                $"GROUP BY RegisterYear " +
                $"ORDER BY Value");
        }
        
        public override void Down()
        {
            Sql("Drop View Inquiry._Years");
        }
    }
}
