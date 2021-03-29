namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class QuotationPanelsView : DbMigration
    {
        public override void Up()
        {
            Sql($"CREATE VIEW [Quotation].[_PanelsView] As "+
                $"SELECT " +
                $"Quotation._Panels.*, Quotation._PanelsCost.UnitCost " +
                $"FROM " +
                $"Quotation._Panels LEFT OUTER JOIN " +
                $"Quotation._PanelsCost ON Quotation._Panels.Id = Quotation._PanelsCost.PanelId");
        }
        
        public override void Down()
        {
            Sql("Drop View [Quotation].[_PanelsView]");
        }
    }
}
