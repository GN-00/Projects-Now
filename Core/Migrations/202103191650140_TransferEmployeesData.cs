namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TransferEmployeesData : DbMigration
    {
        public override void Up()
        {
            Sql($"Insert Into [User].[_Employees] " +
                $"(Id, " +
                $"UserId, " +
                $"IdNumber, " +
                $"FirstName, " +
                $"Job, " +
                $"BasicSalary, " +
                $"HousingAllowance, " +
                $"TransportationAllowance, " +
                $"OtherAllowance) " +

                $"Select " +
                $"EmployeeID,  " +
                $"EmployeeID As UserId,  " +
                $"EmployeeID As IdNumber,  " +
                $"EmployeeName, " +
                $"EmployeeJob," +
                $"0," +
                $"0," +
                $"0," +
                $"0 " +
                $"From [User].[Employees]");

            AlterColumn("User._Employees", "Id", c => c.Int(nullable: false, identity: true));

            AddPrimaryKey("User._Employees", "Id");
            CreateIndex("User._Employees", new[] { "Id" });

        }

        public override void Down()
        {
            DropIndex("User._Employees", new[] { "Id" });
            DropPrimaryKey("User._Employees", "Id");

            AlterColumn("User._Employees", "Id", c => c.Int(nullable: false, identity: false));

            Sql("Delete [ProjectsNow].[User]._Employees " +
                "Where Id in (Select EmployeeID from[ProjectsNow].[User].[Employees]);");
        }
    }
}
