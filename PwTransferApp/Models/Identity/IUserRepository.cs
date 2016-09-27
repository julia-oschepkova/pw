using System;

namespace PwTransferApp.Models.Identity
{
    public interface IUserRepository
    {
        ApplicationUser Read(Guid id);
        ApplicationUser Read(Guid id, IIdentityContext context);
    }
}