using System;
using Dapper.Contrib.Extensions;

namespace Core.Data.UserData
{
    [Table("User._Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public int IdNumber { get; set; }
        public int? PassportNumber { get; set; }

        public string Title { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime? BirthDate { get; set; }

        public string Mobile { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public string Job { get; set; }
        public DateTime? HiringDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndReason { get; set; }
        public bool IsActive
        {
            get
            {
                if (EndDate is null)
                    return true;
                else
                    return false;
            }
        }

        public double BasicSalary { get; set; }
        public double HousingAllowance { get; set; }
        public double TransportationAllowance { get; set; }
        public double OtherAllowance { get; set; }
    }
}
