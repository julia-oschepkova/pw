using Microsoft.AspNet.Identity.EntityFramework;

namespace PwTransferApp.Models.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IIdentityContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}