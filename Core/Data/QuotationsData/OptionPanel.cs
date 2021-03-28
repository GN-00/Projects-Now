using Dapper.Contrib.Extensions;

namespace Core.Data.QuotationsData
{
    [Table("[Quotation].[_OptionsPanels]")]
    public class OptionPanel
    {
        [Key]
        public int Id { get; set; }
        public int OptionId { get; set; }
        public int PanelId { get; set; }
        public int SN { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public string EnclosureName { get; set; }
    }
}
