namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InquiriesYears : DbMigration
    {
        public override void Up()
        {
            Sql($"Create View Inquiry._Years As " +
                $"SELECT TOP (100) PERCENT RegisterYear AS Value, Status " +
                $"FROM Inquiry._InquiriesView " +
                $"GROUP BY RegisterYear, Status " +
                $"ORDER BY Status DESC, RegisterYear DESC");
        }
        
        public override void Down()
        {
            Sql("Drop View Inquiry._Years");
        }
    }
}
