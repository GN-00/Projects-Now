namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TransferQuotationsData : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [Quotation].[_Quotations] " +
                   $"([Id], " +
                   $"[InquiryId], " +
                   $"[Code], " +
                   $"[Number], " +
                   $"[Revise], " +
                   $"[ReviseDate], " +
                   $"[Month], " +
                   $"[Year], " +
                   $"[Status], " +
                   $"[SubmitDate], " +
                   $"[PowerVoltage], " +
                   $"[Phase], " +
                   $"[Frequency], " +
                   $"[NetworkSystem], " +
                   $"[ControlVoltage], " +
                   $"[TinPlating], " +
                   $"[NeutralSize], " +
                   $"[EarthSize], " +
                   $"[EarthingSystem], " +
                   $"[Discount], " +
                   $"[VAT]) "+

           "SELECT [QuotationID], " +
                   $"[InquiryID], " +
                   $"[QuotationCode], " +
                   $"[QuotationNumber], " +
                   $"[QuotationRevise], " +
                   $"[QuotationReviseDate], " +
                   $"[QuotationYear], " +
                   $"[QuotationMonth], " +
                   $"[QuotationStatus], " +
                   $"[SubmitDate], " +
                   $"[PowerVoltage], " +
                   $"[Phase], " +
                   $"[Frequency], " +
                   $"[NetworkSystem], " +
                   $"[ControlVoltage], " +
                   $"[TinPlating], " +
                   $"[NeutralSize], " +
                   $"[EarthSize], " +
                   $"[EarthingSystem], " +
                   $"[Discount], " +
                   $"[VAT] " +
                   $"FROM [Quotation].[Quotations]");

            AlterColumn("[Quotation].[_Quotations]", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("[Quotation].[_Quotations]", "Id");
            CreateIndex("[Quotation].[_Quotations]", new[] { "Id" });

        }

        public override void Down()
        {
            DropIndex("[Quotation].[_Quotations]", new[] { "Id" });
            DropPrimaryKey("[Quotation].[_Quotations]", "Id");

            AlterColumn("[Quotation].[_Quotations]", "Id", c => c.Int(nullable: false, identity: false));

            Sql("Delete [ProjectsNow].[Quotation].[_Quotations] " +
                "where Id in (Select InquiryID from [ProjectsNow].[Quotation].[Quotations]);");
        }
    }
}
