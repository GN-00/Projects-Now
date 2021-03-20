namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpgradeUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("User._Users", "AccessInquiries", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "AccessQuote", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "AccessQuotations", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "AccessCustomers", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "AccessContacts", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "AccessConsultants", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "QuotationsDiscount", c => c.Boolean(nullable: true));
            AddColumn("User._Users", "QuotationsDiscountValue", c => c.Double());

            Sql($"Update [User].[_Users] Set " +
                $"_Users.AccessInquiries = Users.AccessInquiries, " +
                $"_Users.AccessQuote = Users.AccessQuote, " +
                $"_Users.AccessQuotations = Users.AccessQuotations, " +
                $"_Users.AccessCustomers = Users.AccessCustomers, " +
                $"_Users.AccessContacts = Users.AccessContacts, " +
                $"_Users.AccessConsultants = Users.AccessConsultants, " +
                $"_Users.QuotationsDiscount = Users.QuotationsDiscount, " +
                $"_Users.QuotationsDiscountValue = Users.QuotationsDiscountValue " +
                $"From [User].[_Users] " +
                $"Inner Join [User].[Users] On  _Users.Id = Users.UserID");
        }

        public override void Down()
        {
            DropColumn("User._Users", "AccessInquiries");
            DropColumn("User._Users", "AccessQuote");
            DropColumn("User._Users", "AccessQuotations");
            DropColumn("User._Users", "AccessCustomers");
            DropColumn("User._Users", "AccessContacts");
            DropColumn("User._Users", "AccessConsultants");
            DropColumn("User._Users", "QuotationsDiscount");
            DropColumn("User._Users", "QuotationsDiscountValue");
        }
    }
}
