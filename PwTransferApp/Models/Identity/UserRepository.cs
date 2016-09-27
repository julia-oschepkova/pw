using System;
using System.Linq;
using Common.Exceptions;

namespace PwTransferApp.Models.Identity
{
    public class UserRepository : IUserRepository
    {
        private readonly IIdentityDbContextProvider contextProvider;

        public UserRepository(IIdentityDbContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public ApplicationUser Read(Guid id, IIdentityContext context)
        {
            var entity = context.Users.FirstOrDefault(x => x.Id == id.ToString());
            if (entity == null)
                throw new EntityNotFoundException(id, typeof (ApplicationUser));
            return entity;
        }

        public ApplicationUser Read(Guid id)
        {
            using (var context = contextProvider.Get())
            {
                return Read(id, context);
            }
        }
    }
}