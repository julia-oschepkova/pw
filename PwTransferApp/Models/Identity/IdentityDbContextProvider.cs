namespace PwTransferApp.Models.Identity
{
    public class IdentityDbContextProvider : IIdentityDbContextProvider
    {
        public IIdentityContext Get()
        {
            return new ApplicationDbContext();
        }
    }
}