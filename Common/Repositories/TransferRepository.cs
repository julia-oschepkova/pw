using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Common.DbContext;
using Common.Model;

namespace Common.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly IDbContextProvider contextProvider;

        public TransferRepository(IDbContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public Transfer Create(Guid sourceAccountId, Guid destinationAccountId, double amount, string description)
        {
            var transfer = new Transfer()
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                SourceAccountId = sourceAccountId,
                DestinationAccountId = destinationAccountId,
                Description = description,
                Status = TransferStatus.InProcess,
                TransferDateTime = DateTimeOffset.UtcNow
            };
            using (var context = contextProvider.Get())
            {
                context.Transactions.Add(transfer);
                context.SaveChanges();
            }
            return transfer;
        }

        public void MarkTransferAsFailed(Transfer transfer)
        {
            using (var context = contextProvider.Get())
            {
                transfer.Status = TransferStatus.Failed;
                context.Entry(transfer).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public async Task<List<Transfer>> GetAsync(Filter filter)
        {
            using (var context = contextProvider.Get())
            {
                var result = context.Transactions.Where(
                    x =>
                        (x.SourceAccountId == filter.SelfAccountId || x.DestinationAccountId == filter.SelfAccountId)
                        && x.Status == TransferStatus.Successed
                        && (filter.Amount == null || Math.Abs(x.Amount - filter.Amount.Value) < 0.01)
                        &&
                        (filter.AccountId == null ||
                         (x.SourceAccountId == filter.AccountId.Value ||
                          x.DestinationAccountId == filter.AccountId.Value))
                        &&
                        (filter.Date == null ||
                         (x.TransferDateTime >= filter.Date.Value.Date &&
                          x.TransferDateTime < filter.Date.Value.Date.AddDays(1))));


                return await result.ToListAsync();
            }
        }

        public class Filter
        {
            public Guid SelfAccountId { get; set; }

            public Guid? AccountId { get; set; }

            public DateTimeOffset? Date { get; set; }

            public double? Amount { get; set; }
        }
    }


}