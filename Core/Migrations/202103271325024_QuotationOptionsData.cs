namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuotationOptionsData : DbMigration
    {
        public override void Up()
        {
            CreateTable("[Quotation].[_Options]",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    QuotationId = c.Int(nullable: false),
                    Number = c.Int(nullable:false),
                    Name = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.QuotationId);


            Sql($"SET IDENTITY_INSERT [Quotation].[_Options] ON; " +
                $"INSERT INTO [Quotation].[_Options] " +
                $"([Id], " +
                $"[QuotationId], " +
                $"[Number], " +
                $"[Name]) " +

                $"SELECT " +
                $"[ID], " +
                $"[QuotationID], " +
                $"[Number], " +
                $"[Name] " +
                $"FROM [Quotation].[QuotationsOptions] " + 
                $"ORDER BY [ID] ASC; " +
                $"SET IDENTITY_INSERT [Quotation].[_Options] OFF;");


            CreateTable("[Quotation].[_OptionsPanels]",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OptionId = c.Int(nullable: false),
                    PanelId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.PanelId)
                .Index(t => t.OptionId);


            Sql($"SET IDENTITY_INSERT [Quotation].[_OptionsPanels] ON; " +
                $"INSERT INTO [Quotation].[_OptionsPanels] " +
                $"([Id], " +
                $"[OptionId], " +
                $"[PanelId]) " +

                $"SELECT " +
                $"[ID], " +
                $"[OptionID], " +
                $"[PanelID] " +
                $"FROM [Quotation].[QuotationsOptionsPanels] " +
                $"ORDER BY [ID] ASC; " +
                $"SET IDENTITY_INSERT [Quotation].[_OptionsPanels] OFF;");
        }

        public override void Down()
        {
            DropTable("[Quotation].[_Options]");
            DropTable("[Quotation].[_OptionsPanels]");
        }
    }
}
