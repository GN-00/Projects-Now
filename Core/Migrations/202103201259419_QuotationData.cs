namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class QuotationData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Quotation._Quotations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InquiryId = c.Int(nullable: false),
                    Code = c.String(maxLength: 25),
                    Number = c.Int(nullable: false),
                    Revise = c.Int(nullable: false),
                    ReviseDate = c.DateTime(nullable: true),
                    Month = c.Int(nullable: false),
                    Year = c.Int(nullable: false),
                    Status = c.String(maxLength: 25),
                    SubmitDate = c.DateTime(nullable: true),

                    PowerVoltage = c.String(maxLength: 20),
                    Phase = c.String(maxLength: 10),
                    Frequency = c.String(maxLength: 10),
                    NetworkSystem = c.String(maxLength: 10),
                    ControlVoltage = c.String(maxLength: 25),
                    TinPlating = c.String(maxLength: 25),
                    NeutralSize = c.String(maxLength: 25),
                    EarthSize = c.String(maxLength: 25),
                    EarthingSystem = c.String(maxLength: 25),

                    Discount = c.Double(nullable: false),
                    VAT = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.InquiryId);


            Sql($"SET IDENTITY_INSERT [Quotation].[_Quotations] ON; " +
                $"INSERT INTO [Quotation].[_Quotations] " +
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
                $"[VAT]) " +

                $"SELECT [QuotationID], " +
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
                $"[VAT] * 100 " +
                $"FROM [Quotation].[Quotations] "+
                $"ORDER BY [QuotationID] ASC;" +
                $"SET IDENTITY_INSERT [Quotation].[_Quotations] OFF;");
        }

        public override void Down()
        {
            DropTable("Quotation._Quotations");
        }
    }
}
