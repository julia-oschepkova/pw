using System;
using System.Data;
using System.Data.Entity;
using Common.DbContext;
using Common.Exceptions;
using Common.Model;
using Common.Repositories;
using log4net;

namespace Common.Managers
{
    public class TransferManager : ITransferManager
    {
        private ILog logger = LogManager.GetLogger(typeof (TransferManager));
        private readonly IPwAccountRepository pwAccountRepository;
        private readonly ITransferRepository transferRepository;
        private readonly IDbContextProvider dbContextProvider;

        public TransferManager(IPwAccountRepository pwAccountRepository,
            ITransferRepository transferRepository,
            IDbContextProvider dbContextProvider)
        {
            this.pwAccountRepository = pwAccountRepository;
            this.transferRepository = transferRepository;
            this.dbContextProvider = dbContextProvider;
        }

        public Tuple<Transfer, PwAccount> Transfer(Guid sourceAccountId, Guid destinationAccountId, double amount,
            string description)
        {
            var transfer = transferRepository.Create(sourceAccountId, destinationAccountId, amount, description);
            PwAccount sourceAccount;
            try
            {
                using (var context = dbContextProvider.Get())
                {
                    sourceAccount = pwAccountRepository.Read(sourceAccountId, context);
                    var destAccount = pwAccountRepository.Read(destinationAccountId, context);

                    Validate(sourceAccount, amount);

                    sourceAccount.Balance = sourceAccount.Balance - amount;
                    destAccount.Balance = destAccount.Balance + amount;
                    context.Entry(sourceAccount).State = EntityState.Modified;
                    context.Entry(destAccount).State = EntityState.Modified;

                    transfer.Status = TransferStatus.Successed;
                    context.Entry(transfer).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (DBConcurrencyException ex)
            {
                transferRepository.MarkTransferAsFailed(transfer);
                logger.Error(
                    $"Self {sourceAccountId} or recipient {destinationAccountId} account was changed during transfer",
                    ex);
                throw new AccountWasChangedException();
            }
            catch (Exception ex)
            {
                transferRepository.MarkTransferAsFailed(transfer);
                logger.Error($"Transfer {transfer.Id} from {sourceAccountId} to {destinationAccountId} failed", ex);
                throw;
            }

            return Tuple.Create(transfer, sourceAccount);
        }

        private void Validate(PwAccount source, double amount)
        {
            if (source.Balance < amount)
            {
                throw new NotEnoughtPwForTransferException();
            }
        }
    }
}