using System.Data.Entity;
using Common.Model;

namespace Common.DbContext
{
    public class DataContextInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);

            context.Accounts.Add(new PwAccount()
            {
                Id = PwConstants.UnlimitedAccountId,
                Name = "Unlimited magic pw-account",
                UserId = PwConstants.RootUserId,
                Balance = double.MaxValue,
                IsDefault = true
            });
            context.SaveChanges();
        }
    }
}