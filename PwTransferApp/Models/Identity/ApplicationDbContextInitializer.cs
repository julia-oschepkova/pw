using System.Data.Entity;
using Common.Model;

namespace PwTransferApp.Models.Identity
{
    public class ApplicationDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            context.Users.Add(new ApplicationUser()
            {
                Id = PwConstants.RootUserId.ToString(),
                FirstName = "Root",
                Email = "admin@pw.ru",
                UserName = "admin@pw.ru",
            });
                
            context.SaveChanges();
        }
    }
}