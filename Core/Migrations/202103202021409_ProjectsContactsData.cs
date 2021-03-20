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
                    Id = c.Int(nullable: false),
                    InquiryId = c.Int(nullable: false),
                    ContactId = c.Int(nullable: false),
                    Attention = c.Boolean()
                } );


            Sql($"Insert Into Inquiry._ProjectsContacts " +
                $"(Id, " +
                $"InquiryId, " +
                $"ContactId, " +
                $"Attention) " +
                $"Select " +
                $"RecordID, " +
                $"InquiryId, " +
                $"ContactId, " +
                $"Attention " +
                $"From Inquiry.ProjectsContacts ");

            AlterColumn("Inquiry._ProjectsContacts", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("Inquiry._ProjectsContacts", "Id");
            CreateIndex("Inquiry._ProjectsContacts", "Id");
        }

        public override void Down()
        {
            DropTable("Inquiry._ProjectsContacts");
        }
    }
}
