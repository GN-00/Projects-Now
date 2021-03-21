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
                    Id = c.Int(nullable: false, identity: true),
                    CustomerId = c.Int(nullable: false),
                    ConsultantId = c.Int(nullable: true),
                    EstimatorId = c.Int(nullable: true),
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
                })
                  .PrimaryKey(t => t.Id)
                  .Index(t => t.Id)
                  .Index(t => t.CustomerId)
                  .Index(t => t.ConsultantId)
                  .Index(t => t.EstimatorId)
                  .Index(t => t.SalesmanId); 

            Sql($"SET IDENTITY_INSERT [Inquiry].[_Inquiries] ON; " +
                $"Insert Into [Inquiry].[_Inquiries] " +
                $"(Id, " +
                $"CustomerId, " +
                $"ConsultantId, " +
                $"EstimatorId, " +
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
                $"FROM [Inquiry].[Inquiries] " +
                $"ORDER BY [InquiryID] ASC;" +
                $"SET IDENTITY_INSERT [Inquiry].[_Inquiries] OFF;");
        }

        public override void Down()
        {
            DropTable("Inquiry._Inquiries");
        }
    }
}
