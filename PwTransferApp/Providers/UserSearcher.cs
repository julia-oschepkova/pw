using System;
using System.Collections.Generic;
using System.Linq;
using Common.DbContext;
using Common.Model;
using Common.Repositories;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.Providers
{
    public class UserSearcher : IUserSearcher
    {
        private readonly IPwAccountRepository pwAccountRepository;
        private readonly IIdentityDbContextProvider contextProvider;
        private readonly IDbContextProvider dataContextProvider;

        public UserSearcher(IPwAccountRepository pwAccountRepository,
            IDbContextProvider dataContextProvider,
            IIdentityDbContextProvider contextProvider)
        {
            this.pwAccountRepository = pwAccountRepository;
            this.contextProvider = contextProvider;
            this.dataContextProvider = dataContextProvider;
        }

        public IEnumerable<Tuple<ApplicationUser, Guid>> Search(string prefix, string userContext)
        {
            List<ApplicationUser> users;

            using (var context = contextProvider.Get())
            {
                if (string.IsNullOrEmpty(prefix))
                    users = context.Users.ToList();
                else
                {
                    prefix = prefix.ToLower();
                    users = context.Users
                        .Where(x => (x.FirstName.ToLower().Contains(prefix) || x.LastName.ToLower().StartsWith(prefix))
                                    && x.Id != userContext
                                    && x.Id != PwConstants.RootUserId.ToString())
                        .ToList();
                }
            }
            using (var dataContext = dataContextProvider.Get())
            {
                return
                    users.Select(
                        user => Tuple.Create(user, pwAccountRepository.GetByUserId(Guid.Parse(user.Id), dataContext).Id))
                        .ToList();
            }
        }
    }
}