using System.Data.Entity;

namespace Core.Data
{
    public class Database: DbContext
    {
        public Database() 
            : base(ConnectionString)
        {

        }

        public static string ConnectionString
        {
            get
            {
                if (App.ComputerName == "HASAN2-PC")
                    return @"Data Source=hasan2-pc\PN;Initial Catalog=ProjectsNow;Integrated Security=False;User ID=sa;Password=Wing00Gundam;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";

                else if (App.ComputerName == "WG-0")
                    return @"Data Source=WG-0\PN;Initial Catalog=ProjectsNow;Integrated Security=False;User ID=sa;Password=Wing00Gundam;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";

                else
                    return @"Data Source=PCAPSSYSTEM\PROJECTSNOW;Initial Catalog=ProjectsNow;Integrated Security=False;User ID=sa;Password=Wing00Gundam;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False"; ;
            }
        }
    }
}
