using System;
using System.Threading.Tasks;
using Common.DbContext;
using Common.Model;
using Microsoft.AspNet.Identity;

namespace PwTransferApp.Models.Identity
{
    public class RegistrationManager : IRegistrationManager
    {
        private readonly IDbContextProvider contextProvider;

        public RegistrationManager(IDbContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password, ApplicationUserManager userManager)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                CreateAccount(Guid.Parse(user.Id));
            }

            return result;
        }

        private void CreateAccount(Guid userId)
        {
            using (var context = contextProvider.Get())
            {
                var account = context.Set<PwAccount>().Add(new PwAccount()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Balance = PwConstants.InitialSum,
                    Name = "Default account",
                    IsDefault = true
                });
                context.Set<Transfer>().Add(new Transfer()
                {
                    Id = Guid.NewGuid(),
                    Amount = PwConstants.InitialSum,
                    SourceAccountId = PwConstants.UnlimitedAccountId,
                    DestinationAccountId = account.Id,
                    Description = "Initial grant",
                    Status = TransferStatus.Successed,
                    TransferDateTime = DateTimeOffset.UtcNow
                });
                context.SaveChanges();
            }
        }
    }
}