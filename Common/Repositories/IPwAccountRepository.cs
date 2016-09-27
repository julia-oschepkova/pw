using System;
using Common.DbContext;
using Common.Model;

namespace Common.Repositories
{
    public interface IPwAccountRepository
    {
        PwAccount GetByUserId(Guid id);
        PwAccount Read(Guid id);
        PwAccount Read(Guid id, DataContext context);
        PwAccount GetByUserId(Guid id, DataContext context);
    }
}