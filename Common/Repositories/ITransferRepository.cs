using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Model;

namespace Common.Repositories
{
    public interface ITransferRepository 
    {
        Task<List<Transfer>> GetAsync(TransferRepository.Filter filter);
        Transfer Create(Guid sourceAccountId, Guid destinationAccountId, double amount, string description);
        void MarkTransferAsFailed(Transfer transfer);
    }
}