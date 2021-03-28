namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class QuotationsCostView : DbMigration
    {
        public override void Up()
        {
            Sql("Create View Quotation._Cost As " +
                "SELECT " +
                "QuotationId, " +
                "Cost, " +
                "(Cost * (1 - Discount / 100)) AS Price, " +
                "(Cost * Discount / 100) AS DiscountValue, " +
                "(Cost * (1 - Discount / 100)) * VAT / 100 AS VAT_Value, " +
                "(Cost * (1 - Discount / 100)) * (1 + VAT / 100) AS PriceWithVAT " +
                "FROM " +
                "(SELECT " +
                "Quotation._Panels.QuotationId, " +
                "SUM((Quotation._PanelsCost.UnitCost * Quotation._Panels.Qty) * (1 / (1 - Quotation._Panels.Profit / 100))) AS Cost, " +
                "Quotation._Quotations.Discount, " +
                "Quotation._Quotations.VAT, " +
                "Quotation._Quotations.Id " +
                "FROM " +
                "Quotation._Quotations LEFT OUTER JOIN " +
                "Quotation._Panels ON Quotation._Quotations.Id = Quotation._Panels.QuotationId LEFT OUTER JOIN " +
                "Quotation._PanelsCost ON Quotation._Panels.Id = Quotation._PanelsCost.PanelId " +
                "GROUP BY Quotation._Panels.QuotationId, Quotation._Quotations.Discount, Quotation._Quotations.VAT, Quotation._Quotations.Id) AS QuotationInformation");
        }

        public override void Down()
        {
            Sql("Drop View Quotation._Cost");
        }
    }
}
