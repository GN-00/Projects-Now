namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class VATData : DbMigration
    {
        public override void Up()
        {
            CreateTable("Finance._VAT",
                c => new 
                {
                    Id = c.Int(nullable: false, identity: true),
                    Value = c.Double(nullable: false),
                    Date = c.DateTime(nullable: false)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);

            Sql($"Insert Into Finance._VAT (Value, Date) Values (15, '2021-01-01')");

            Sql("CREATE VIEW [Finance].[_LastVAT] AS " +
                "SELECT Finance._VAT.Value " +
                "FROM Finance._VAT INNER JOIN " +
                "(SELECT MAX(Date) AS LastDate " +
                "FROM Finance._VAT AS VAT_1 " +
                "WHERE (Date <= GETDATE())) AS LastUpdate ON Finance._VAT.Date = LastUpdate.LastDate ");
        }
        
        public override void Down()
        {
            DropTable("Finance._VAT");
            Sql("Drop View Finance._LAstVAT");
        }
    }
}
