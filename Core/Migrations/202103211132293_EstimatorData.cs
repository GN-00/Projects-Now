namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstimatorData : DbMigration
    {
        public override void Up()
        {
            Sql($"Update [User].[_Users] set IsEstimator = 1 " +
                $"Where Id In (0, 1, 2, 4, 5); ");

            Sql($"CREATE VIEW [User]._Estimators As " +
                $"SELECT TOP (100) PERCENT [User]._Users.Id, [User]._Users.UserCode As EstimatorCode, [User]._Employees.Name As EstimatorName " +
                $"FROM [User]._Users INNER JOIN " +
                $"[User]._Employees ON[User]._Users.Id = [User]._Employees.UserId " +
                $"WHERE ([User]._Users.IsEstimator = 1) " +
                $"Order By [User]._Employees.Name");
        }
        
        public override void Down()
        {
            Sql($"Update [User].[_Users] set IsEstimator = 0 " +
                $"Where Id In (0, 1, 2, 4, 5); ");

            Sql("Drop View [User]._Estimators");
        }
    }
}
