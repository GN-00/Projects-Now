namespace Core.Migrations
{
    using Core.Data.CustomersData;

    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Customer._Contacts",
                c => new 
                {
                    Id = c.Int(nullable: false),
                    CustomerId = c.Int(nullable: true),
                    Name = c.String(nullable: false, maxLength: 256),
                    Address = c.String(nullable: true, maxLength: 256),
                    Mobile = c.String(nullable: true, maxLength: 20),
                    Email = c.String(nullable: true, maxLength: 100),
                    Job = c.String(nullable: true, maxLength: 100),
                    Note = c.String(nullable: true, maxLength: 256),
                });

            Sql($"INSERT INTO [Customer].[_Contacts] " +
                $"([Id], " +
                $"[CustomerId], " +
                $"[Name], " +
                $"[Address], " +
                $"[Mobile], " +
                $"[Email], " +
                $"[Job], " +
                $"[Note]) " +

                $"SELECT " +
                $"[ContactID], " +
                $"[CustomerID], " +
                $"[ContactName], " +
                $"[Address], " +
                $"[Mobile], " +
                $"[Email], " +
                $"[Job], " +
                $"[Note] " +
                $"FROM [Customer].[Contacts]");

            AlterColumn("Customer._Contacts", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("Customer._Contacts", "Id");
            CreateIndex("Customer._Contacts", "Id");
        }
        
        public override void Down()
        {
            DropTable("Customer._Contacts");
        }
    }
}
