namespace PwTransferApp.Models.Identity
{
    public interface IIdentityDbContextProvider
    {
        IIdentityContext Get();
    }
}