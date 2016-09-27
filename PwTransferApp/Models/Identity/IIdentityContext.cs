using System;
using System.Data.Entity;

namespace PwTransferApp.Models.Identity
{
    public interface IIdentityContext : IDisposable
    {
        IDbSet<ApplicationUser> Users { get; set; }
    }
}