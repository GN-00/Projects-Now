namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CustomerData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customer._Customers",
                c => new
                {
                    Id = c.Int(nullable: false),
                    SalesmanId = c.Int(nullable: true),
                    BankId = c.Int(nullable: true),
                    Name = c.String(nullable: false, maxLength: 256),
                    City = c.String(nullable: true, maxLength: 100),
                    Address = c.String(nullable: true, maxLength: 256),
                    POBox = c.String(nullable: true, maxLength: 20),
                    Phone = c.String(nullable: true, maxLength: 20),
                    Email = c.String(nullable: true, maxLength: 100),
                    Website = c.String(nullable: true, maxLength: 100),
                    StartRelation = c.DateTime(nullable: false),
                    VATNumber = c.String(nullable: true, maxLength: 20),
                    Note = c.String(nullable: true, maxLength: 256),

                });

            Sql($"INSERT INTO [Customer].[_Customers] " +
                $"([Id], " +
                $"[SalesmanId], " +
                $"[BankID], " +
                $"[Name], " +
                $"[City], " +
                $"[Address], " +
                $"[POBox], " +
                $"[Phone], " +
                $"[Email], " +
                $"[Website], " +
                $"[StartRelation], " +
                $"[VATNumber], " +
                $"[Note]) " +

                $"SELECT " +
                $"[CustomerID], " +
                $"[SalesmanID], " +
                $"[BankID], " +
                $"[CustomerName], " +
                $"[City], " +
                $"[Address], " +
                $"[POBox], " +
                $"[Phone], " +
                $"[Email], " +
                $"[Website], " +
                $"[StartRelation], " +
                $"[VATNumber], " +
                $"[Note] " +
                $"FROM [Customer].[Customers]");

            AlterColumn("Customer._Customers", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("Customer._Customers", "Id");
            CreateIndex("Customer._Customers", "Id");
        }

        public override void Down()
        {
            DropTable("Customer._Customers");
        }
    }
}
