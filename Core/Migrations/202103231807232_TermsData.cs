namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TermsData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Quotation._Terms&Conditions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    QuotationId = c.Int(nullable: false),
                    Sort = c.Int(nullable: false),
                    IsDefault = c.Boolean(nullable: false),
                    IsUsed = c.Boolean(),
                    Condition = c.String(nullable: true),
                    ConditionType = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuotationId);


            Sql($"SET IDENTITY_INSERT [Quotation].[_Terms&Conditions] ON; " +
                $"Insert Into [Quotation].[_Terms&Conditions] " +
                $"(Id, " +
                $"QuotationId, " +
                $"Sort, " +
                $"IsDefault, " +
                $"IsUsed, " +
                $"Condition, " +
                $"ConditionType) " +

                $"SELECT " +
                $"[TermID], " +
                $"[QuotationID], " +
                $"[Sort], " +
                $"[IsDefault], " +
                $"[IsUsed], " +
                $"[Condition], " +
                $"[ConditionType] " +
                $"FROM [Quotation].[Terms&Conditions] " +
                $"ORDER BY [TermID] ASC;" +

                $"SET IDENTITY_INSERT [Quotation].[_Terms&Conditions] OFF;");
        }

        public override void Down()
        {
            Sql("Drop View [Quotation].[_Terms&Conditions]");
        }
    }
}
