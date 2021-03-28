namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuotationArticleData : DbMigration
    {
        public override void Up()
        {
            CreateTable("[Quotation].[_Articles]",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Sort = c.Int(nullable: false),
                    Article = c.String(maxLength:256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);


            Sql($"SET IDENTITY_INSERT [Quotation].[_Articles] ON; " +
                $"INSERT INTO [Quotation].[_Articles] " +
                $"([Id], " +
                $"[Sort], " +
                $"[Article]) " +

                $"SELECT " +
                $"[ID], " +
                $"[Sort], " +
                $"[Article] " +
                $"FROM [Quotation].[Articles] " +
                $"ORDER BY [ID] ASC; " +
                $"SET IDENTITY_INSERT [Quotation].[_Articles] OFF;");
        }
        
        public override void Down()
        {
            DropTable("[Quotation].[_Articles]");
        }
    }
}
