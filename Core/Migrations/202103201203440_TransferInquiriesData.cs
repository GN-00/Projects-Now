namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TransferInquiriesData : DbMigration
    {
        public override void Up()
        {
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

            AlterColumn("[Inquiry].[_Inquiries]", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("[Inquiry].[_Inquiries]", "Id");
            CreateIndex("[Inquiry].[_Inquiries]", new[] { "Id" });

        }

        public override void Down()
        {
            DropIndex("[Inquiry].[_Inquiries]", new[] { "Id" });
            DropPrimaryKey("[Inquiry].[_Inquiries]", "Id");

            AlterColumn("[Inquiry].[_Inquiries]", "Id", c => c.Int(nullable: false, identity: false));

            Sql("Delete [ProjectsNow].[Inquiry].[_Inquiries] " +
                "where Id in (Select InquiryID from [ProjectsNow].[Inquiry].[Inquiries]);");
        }
    }
}
