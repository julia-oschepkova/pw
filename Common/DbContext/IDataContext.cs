using System.Data.Entity;
using Common.Model;

namespace Common.DbContext
{
    public interface IDataContext
    {
        DbSet<PwAccount> Accounts { get; set; }
        DbSet<Transfer> Transactions { get; set; }

        int SaveChanges();
    }
}