namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class QuotationsViewData : DbMigration
    {
        public override void Up()
        {
            Sql($"CREATE VIEW [Quotation].[_QuotationsView] As " +
                $"SELECT " +
                $"Quotation._Quotations.Id, Quotation._Quotations.InquiryId, Quotation._Quotations.Code, Quotation._Quotations.Number, " +
                $"Quotation._Quotations.Revise, Quotation._Quotations.ReviseDate, Quotation._Quotations.Month, Quotation._Quotations.Year, " +
                $"Quotation._Quotations.Status, Quotation._Quotations.SubmitDate, Quotation._Quotations.PowerVoltage, Quotation._Quotations.Phase, " +
                $"Quotation._Quotations.Frequency, Quotation._Quotations.NetworkSystem, Quotation._Quotations.ControlVoltage, " +
                $"Quotation._Quotations.TinPlating, Quotation._Quotations.NeutralSize, Quotation._Quotations.EarthingSystem, " +
                $"Quotation._Quotations.EarthSize, Quotation._Quotations.Discount, Quotation._Quotations.VAT, Quotation._Cost.Cost, " +
                $"Quotation._Cost.Price, Quotation._Cost.DiscountValue, Quotation._Cost.VAT_Value, Quotation._Cost.PriceWithVAT " +
                $"FROM " +
                $"Quotation._Quotations LEFT OUTER JOIN " +
                $"Quotation._Cost ON Quotation._Quotations.Id = Quotation._Cost.QuotationId ");
        }
        
        public override void Down()
        {
            Sql("Drop View [Quotation].[_QuotationsView]");
        }
    }
}
