namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProjectsContactsView : DbMigration
    {
        public override void Up()
        {
            Sql("Create View Inquiry._ProjectsContactsView As " +
                "SELECT TOP (100) PERCENT Customer._Contacts.Id, Customer._Contacts.CustomerId, Inquiry._ProjectsContacts.InquiryId, Inquiry._ProjectsContacts.Attention, " +
                "Customer._Contacts.Name, Customer._Contacts.Address, Customer._Contacts.Mobile, Customer._Contacts.Email, Customer._Contacts.Job, " +
                "Customer._Contacts.Note " +
                "FROM Inquiry._ProjectsContacts LEFT OUTER JOIN " +
                "Customer._Contacts ON Inquiry._ProjectsContacts.ContactId = Customer._Contacts.Id " +
                "ORDER BY Inquiry._ProjectsContacts.InquiryId, Inquiry._ProjectsContacts.Attention");
        }
        
        public override void Down()
        {
            Sql("Drop View Inquiry._ProjectsContactsView");
        }
    }
}
