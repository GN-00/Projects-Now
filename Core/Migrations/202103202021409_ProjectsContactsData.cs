namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProjectsContactsData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inquiry._ProjectsContacts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    InquiryId = c.Int(nullable: false),
                    ContactId = c.Int(nullable: false),
                    Attention = c.Boolean()
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.InquiryId)
                .Index(t => t.ContactId);

            Sql($"SET IDENTITY_INSERT Inquiry._ProjectsContacts ON; " +
                $"Insert Into Inquiry._ProjectsContacts " +
                $"(Id, " +
                $"InquiryId, " +
                $"ContactId, " +
                $"Attention) " +
                $"Select " +
                $"RecordID, " +
                $"InquiryId, " +
                $"ContactId, " +
                $"Attention " +
                $"From Inquiry.ProjectsContacts " +
                $"ORDER BY [RecordID] ASC;" +
                $"SET IDENTITY_INSERT Inquiry._ProjectsContacts OFF;");
        }

        public override void Down()
        {
            DropTable("Inquiry._ProjectsContacts");
        }
    }
}
