namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InquiryData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inquiry._Inquiries",
                c => new
                {
                    Id = c.Int(nullable: false),
                    CustomerId = c.Int(nullable: false),
                    ConsultantId = c.Int(nullable: true),
                    EstimationId = c.Int(nullable: true),
                    SalesmanId = c.Int(nullable: true),
                    RegisterCode = c.String(maxLength: 25),
                    ProjectName = c.String(maxLength: 256),
                    RegisterDate = c.DateTime(nullable: false),
                    DuoDate = c.DateTime(nullable: false),
                    Priority = c.String(maxLength: 10),
                    RegisterNumber = c.Int(nullable: false),
                    RegisterMonth = c.Int(nullable: false),
                    RegisterYear = c.Int(nullable: false),
                    DeliveryCondition = c.String(maxLength: 10),
                });
        }

        public override void Down()
        {
            DropTable("Inquiry._Inquiries");
        }
    }
}
