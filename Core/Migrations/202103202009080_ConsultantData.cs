namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ConsultantData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customer._Consultants",
                c => new 
                {
                    Id = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 256),
                    Address = c.String(nullable: true, maxLength: 256),
                    Mobile = c.String(nullable: true, maxLength: 20),
                    Email = c.String(nullable: true, maxLength: 100),
                    Company = c.String(nullable: true, maxLength: 100),
                    Website = c.String(nullable: true, maxLength: 100),
                    Job = c.String(nullable: true, maxLength: 100),
                    Note = c.String(nullable: true, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);

            Sql($"SET IDENTITY_INSERT [Customer].[_Consultants] ON; " +
                $"INSERT INTO [Customer].[_Consultants] " +
                $"([Id], " +
                $"[Name], " +
                $"[Address], " +
                $"[Mobile], " +
                $"[Email], " +
                $"[Company], " +
                $"[Website], " +
                $"[Job], " +
                $"[Note]) " +

                $"SELECT " +
                $"[ConsultantID], " +
                $"[ConsultantName], " +
                $"[Address], " +
                $"[Mobile], " +
                $"[Email], " +
                $"[Company], " +
                $"[Website], " +
                $"[Job], " +
                $"[Note] " +
                $"FROM [Customer].[Consultants] " +
                $"ORDER BY [ConsultantID] ASC;" +
                $"SET IDENTITY_INSERT [Customer].[_Customers] OFF;");
        }

        public override void Down()
        {
            DropTable("Customer._Consultants");
        }
    }
}
