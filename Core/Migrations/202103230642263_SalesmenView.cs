namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesmenView : DbMigration
    {
        public override void Up()
        {
            Sql($"Update [User].[_Users] set IsSeller = 1 " +
                $"Where Id In (0, 1, 2, 4, 5, 6); ");

            Sql($"CREATE VIEW [User].[_Salesmen] As " +
                $"SELECT TOP (100) PERCENT [User].[_Users].Id, [User].[_Employees].Name " +
                $"FROM [User]._Users INNER JOIN " +
                $"[User].[_Employees] ON [User].[_Users].Id = [User]._Employees.UserId " +
                $"WHERE ([User].[_Users].IsSeller = 1) " +
                $"Order By [User].[_Employees].Name");
        }

        public override void Down()
        {
            Sql($"Update [User].[_Users] set IsSeller = 0 " +
                $"Where Id In (0, 1, 2, 4, 5, 6); ");

            Sql("Drop View [User].[_Salesmen]");
        }
    }
}
