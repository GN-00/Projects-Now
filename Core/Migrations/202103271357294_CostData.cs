namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostData : DbMigration
    {
        public override void Up()
        {
            Sql("Create View Quotation._PanelsCost As " +
                "SELECT " +
                "PanelId, " +
                "SUM((Qty * UnitCost) * (1 - Discount / 100)) As UnitCost " +
                "FROM " +
                "Quotation._Items " +
                "GROUP BY PanelId");
        }

        public override void Down()
        {
            Sql("Drop View Quotation._PanelsCost");
        }
    }
}
