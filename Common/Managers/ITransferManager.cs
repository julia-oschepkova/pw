using System;
using Common.Model;

namespace Common.Managers
{
    public interface ITransferManager
    {
        Tuple<Transfer, PwAccount> Transfer(Guid sourceAccountId, Guid destinationAccountId, double amount, string description);
    }
}