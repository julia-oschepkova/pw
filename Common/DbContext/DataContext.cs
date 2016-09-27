using System.Data.Entity;
using Common.Model;

namespace Common.DbContext
{
    public class DataContext : System.Data.Entity.DbContext , IDataContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<PwAccount> Accounts { get; set; }
        public DbSet<Transfer> Transactions { get; set; }
    }
}