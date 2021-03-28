namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class QuotationOptionsPanelsViewData : DbMigration
    {
        public override void Up()
        {
            Sql("Create View Quotation._OptionsPanelsView As " +
                "SELECT " +
                "Quotation._OptionsPanels.Id, Quotation._OptionsPanels.OptionId, " +
                "Quotation._OptionsPanels.PanelId, Quotation._Panels.SN, Quotation._Panels.Name, " +
                "Quotation._Panels.Qty, Quotation._Panels.EnclosureName " +
                "FROM " +
                "Quotation._Panels RIGHT OUTER JOIN " +
                "Quotation._OptionsPanels ON Quotation._Panels.Id = Quotation._OptionsPanels.PanelId");
        }
        
        public override void Down()
        {
            Sql("Drop View Quotation._OptionsPanelsView");
        }
    }
}
