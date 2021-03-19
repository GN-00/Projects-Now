namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransferUsersData : DbMigration
    {
        public override void Up()
        {
            Sql($"Insert Into [User].[_Users] " +
                $"(Id, " +
                $"UserName, " +
                $"Password, " +
                $"UserCode, " +
                $"Administrator, " +
                $"AccessTendaring, " +
                $"AccessProjects," +
                $"AccessItems," +
                $"AccessFinance) " +

                $"Select " +
                $"UserID,  " +
                $"UserName, " +
                $"Password," +
                $"UserCode, " +
                $"Administrator, " +
                $"AccessTendaring, " +
                $"AccessProjects, " +
                $"AccessItems, " +
                $"AccessFinance " +
                $"From [User].[Users]");

            AlterColumn("User._Users", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("User._Users", "Id");
            CreateIndex("User._Users", new[] { "Id" });

        }

        public override void Down()
        {
            DropIndex("User._Users", new[] { "Id" });
            DropPrimaryKey("User._Users", "Id");

            AlterColumn("User._Users", "Id", c => c.Int(nullable: false, identity: false));

            Sql("Delete [ProjectsNow].[User]._Users " +
                "where Id in (Select UserID from[ProjectsNow].[User].[Users]);");
        }
    }
}
