using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace PwTransferApp.Models.Identity
{
    public interface IRegistrationManager
    {
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password, ApplicationUserManager userManager);
    }
}