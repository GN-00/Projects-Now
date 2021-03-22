using Dapper;
using Core.Data.UserData;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Core.Data
{
    public static class  Access
    {
        public static User AccessValidation(this SqlConnection connection, string property, int id)
        {
            User record = connection.QueryFirstOrDefault<User>($"Select * From [User].[_Users] Where {property} = {id}");
            return record;
        }

        public static void UserAccessUpdate(this SqlConnection connection, User user, string property)
        {
            var propertyValue = typeof(User).GetProperty(property).GetValue(user);
            if (propertyValue == null)
                connection.Execute($"Update [User].[_Users] Set {property} = NULL Where Id = {user.Id}");
            else
                connection.Execute($"Update [User].[_Users] Set {property} = {propertyValue} Where Id = {user.Id}");
        }

        public static void UserAccessReset(this SqlConnection connection, User user)
        {
            string query = "Update[User].[_Users] Set ";

            foreach (string property in AccessProperties)
                query += $"{property} = NULL ,";

            query = query.Substring(0, query.Length - 1) + $" Where Id = {user.Id}";

            connection.Execute(query);
        }

        static User User;
        private static List<string> AccessProperties = new List<string>()
        {
              nameof(User.InquiryId),
              nameof(User.QuotationId),
              nameof(User.JobOrderId),
              nameof(User.CustomerId),
              nameof(User.ContactId),
              nameof(User.ConsultantId),
              nameof(User.SupplierId),
              nameof(User.AccountId),
              nameof(User.FinanceOrderId),
        };
    }
}
