namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class QuotationItemsData : DbMigration
    {
		public override void Up()
		{
			CreateTable("[Quotation].[_Items]",
				c => new {
					Id = c.Int(nullable: false, identity: true),
					PanelId = c.Int(nullable: false),
					Article1 = c.String(nullable: true, maxLength: 100),
					Article2 = c.String(nullable: true, maxLength: 100),
					Category = c.String(nullable: true, maxLength: 50),
					Code = c.String(nullable: true, maxLength: 50),
					Description = c.String(nullable: true, maxLength: 256),
					Unit = c.String(nullable: true, maxLength: 50),
					Qty = c.Int(nullable: false),
					Brand = c.String(nullable: true, maxLength: 100),
					Remarks = c.String(nullable: true, maxLength: 256),
					UnitCost = c.Double(nullable: false),
					Discount = c.Double(nullable: false),
					Table = c.String(nullable: true, maxLength: 25),
					Type = c.String(nullable: true, maxLength: 100),
					Sort = c.Int(),
					SelectionGroup = c.String(nullable: true, maxLength: 100),
				})
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.PanelId);

            Sql($"SET IDENTITY_INSERT [Quotation].[_Items] ON; " +
                $"INSERT INTO [Quotation].[_Items] " +
                $"([Id], " +
                $"[PanelId], " +
                $"[Article1], " +
                $"[Article2], " +
                $"[Category], " +
                $"[Code], " +
                $"[Description], " +
                $"[Unit], " +
                $"[Qty], " +
                $"[Brand], " +
                $"[Remarks], " +
                $"[UnitCost], " +
                $"[Discount], " +
                $"[Table], " +
                $"[Type], " +
                $"[Sort], " +
                $"[SelectionGroup]) " +

                $"SELECT " +
                $"[ItemID], " +
                $"[PanelID], " +
                $"[Article1], " +
                $"[Article2], " +
                $"[Category], " +
                $"[Code], " +
                $"[Description], " +
                $"[Unit], " +
                $"[ItemQty], " +
                $"[Brand], " +
                $"[Remarks], " +
                $"[ItemCost], " +
                $"[ItemDiscount], " +
                $"[ItemTable], " +
                $"[ItemType], " +
                $"[ItemSort], " +
                $"[SelectionGroup] " +
                $"FROM [Quotation].[QuotationsPanelsItems] " +
                $"ORDER BY [ItemID] ASC; " +
                $"SET IDENTITY_INSERT [Quotation].[_Items] OFF;");

        }
        
        public override void Down()
        {
			DropTable("[Quotation].[_Items]");
        }
    }
}
