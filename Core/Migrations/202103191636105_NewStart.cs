namespace Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NewStart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "User._Employees",
                c => new
                {
                    Id = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                    IdNumber = c.Int(nullable: false),
                    PassportNumber = c.Int(),
                    Title = c.String(maxLength: 10),
                    FirstName = c.String(maxLength: 50),
                    LastName = c.String(maxLength: 50),
                    Gender = c.String(maxLength: 10),
                    Nationality = c.String(maxLength: 50),
                    BirthDate = c.DateTime(),
                    Mobile = c.String(maxLength: 10),
                    Email = c.String(maxLength: 50),
                    City = c.String(maxLength: 50),
                    Address = c.String(maxLength: 256),
                    Job = c.String(maxLength: 256),
                    HiringDate = c.DateTime(),
                    EndDate = c.DateTime(),
                    EndReason = c.String(),
                    BasicSalary = c.Double(nullable: false),
                    HousingAllowance = c.Double(nullable: false),
                    TransportationAllowance = c.Double(nullable: false),
                    OtherAllowance = c.Double(nullable: false),
                });

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
            CreateIndex("User._Employees", "Id");


            CreateTable(
                "User._Users",
                c => new
                {
                    Id = c.Int(nullable: false),
                    UserName = c.String(maxLength: 50),
                    Password = c.String(maxLength: 50),
                    UserCode = c.String(maxLength: 4),
                    Administrator = c.Boolean(),
                    InquiryId = c.Int(),
                    QuotationId = c.Int(),
                    JobOrderId = c.Int(),
                    CustomerId = c.Int(),
                    ContactId = c.Int(),
                    ConsultantId = c.Int(),
                    SupplierId = c.Int(),
                    AccountId = c.Int(),
                    FinanceOrderId = c.Int(),
                    IsEstimator = c.Boolean(),
                    IsSeller = c.Boolean(),
                    AccessTendaring = c.Boolean(),
                    AccessProjects = c.Boolean(),
                    AccessItems = c.Boolean(),
                    AccessFinance = c.Boolean(),
                });

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
            CreateIndex("User._Users", "Id");
        }
        
        public override void Down()
        {
            DropTable("User._Users");
            DropTable("User._Employees");
        }
    }
}
