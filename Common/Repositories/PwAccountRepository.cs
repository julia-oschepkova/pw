using System;
using System.Linq;
using Common.DbContext;
using Common.Exceptions;
using Common.Model;

namespace Common.Repositories
{
    public class PwAccountRepository : IPwAccountRepository
    {
        private readonly IDbContextProvider contextProvider;

        public PwAccountRepository(IDbContextProvider contextProvider)

        {
            this.contextProvider = contextProvider;
        }

        public PwAccount GetByUserId(Guid id)
        {
            using (var context = contextProvider.Get())
            {
                return GetByUserId(id, context);
            }
        }

        public PwAccount GetByUserId(Guid id, DataContext context)
        {
            var account = context.Accounts.FirstOrDefault(x => x.UserId == id && x.IsDefault);
            if (account == null)
                throw new EntityNotFoundException($"Cannot find for user {id}", typeof (PwAccount));
            return account;
        }

        public PwAccount Read(Guid id)
        {
            using (var context = contextProvider.Get())
            {
                return Read(id, context);
            }
        }

        public PwAccount Read(Guid id, DataContext context)
        {
            var account = context.Accounts.FirstOrDefault(x => x.Id == id);
            if (account == null)
                throw new EntityNotFoundException(id, typeof (PwAccount));
            return account;
        }
    }
}