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

            Sql($"Insert Into [Inquiry].[_Inquiries] " +
                $"(Id, " +
                $"CustomerId, " +
                $"ConsultantId, " +
                $"EstimationId, " +
                $"SalesmanId, " +
                $"RegisterCode, " +
                $"ProjectName, " +
                $"RegisterDate, " +
                $"DuoDate, " +
                $"Priority, " +
                $"RegisterNumber, " +
                $"RegisterMonth, " +
                $"RegisterYear, " +
                $"DeliveryCondition) " +

                $"SELECT " +
                $"[InquiryID], " +
                $"[CustomerID], " +
                $"[ConsultantID], " +
                $"[EstimationID], " +
                $"[SalesmanID], " +
                $"[RegisterCode], " +
                $"[ProjectName], " +
                $"[RegisterDate], " +
                $"[DuoDate], " +
                $"[Priority], " +
                $"[RegisterNumber], " +
                $"[RegisterMonth], " +
                $"[RegisterYear], " +
                $"[DeliveryCondition] " +
                $"FROM [Inquiry].[Inquiries]");

            AlterColumn("Inquiry._Inquiries", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("Inquiry._Inquiries", "Id");
            CreateIndex("Inquiry._Inquiries", "Id");
        }

        public override void Down()
        {
            DropTable("Inquiry._Inquiries");
        }
    }
}
