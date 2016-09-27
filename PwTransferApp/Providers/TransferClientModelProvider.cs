using System;
using System.Collections.Generic;
using System.Linq;
using Common.DbContext;
using Common.Model;
using Common.Repositories;
using PwTransferApp.Models.ClientModels;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.Providers
{
    public class TransferClientModelProvider
    {
        private readonly IUserRepository userRepository;
        private readonly IPwAccountRepository accountRepository;
        private readonly IDbContextProvider contextProvider;
        private readonly IIdentityDbContextProvider identityDbContextProvider;
        private readonly ITransferRepository transferRepository;

        public TransferClientModelProvider(IUserRepository userRepository,
            IPwAccountRepository accountRepository,
            IDbContextProvider contextProvider,
            IIdentityDbContextProvider identityDbContextProvider,
            ITransferRepository transferRepository)
        {
            this.userRepository = userRepository;
            this.accountRepository = accountRepository;
            this.contextProvider = contextProvider;
            this.identityDbContextProvider = identityDbContextProvider;
            this.transferRepository = transferRepository;
        }

        public TransferClientModel Get(Transfer transfer, PwAccount account)
        {
            using (var context = contextProvider.Get())
            {
                var counterPartAccount = accountRepository.Read(transfer.DestinationAccountId, context);
                var model = new TransferClientModel
                {
                    Amount = transfer.Amount,
                    Id = transfer.Id,
                    Balance = account.Balance,
                    Direction = TransferDirection.Out,
                    CounterpartAccount = counterPartAccount.ToClient(),
                    Counterpart = userRepository.Read(counterPartAccount.UserId).ToClient(),
                    Description = transfer.Description
                };
                return model;
            }
        }

        //todo test it
        public List<TransferClientModel> Get(PwAccount account)
        {
            using (var context = contextProvider.Get())
            {
                var transfers = context.Transactions
                    .Where(x => x.SourceAccountId == account.Id || x.DestinationAccountId == account.Id)
                    .ToList()
                    .OrderBy(x => x.TransferDateTime);
                return SelectTransferClientModels(account, transfers, context);
            }
        }

        private List<TransferClientModel> SelectTransferClientModels(PwAccount account, IEnumerable<Transfer> transfers,
            DataContext context)
        {
            using (var identityContext = identityDbContextProvider.Get())
            {
                var lastAmount = account.Balance;

                var result = new List<TransferClientModel>();
                foreach (var transfer in transfers)
                {
                    var model = new TransferClientModel
                    {
                        Id = transfer.Id,
                        Amount = transfer.Amount,
                        Balance = lastAmount,
                        Description = transfer.Description,
                        Direction =
                            transfer.DestinationAccountId == account.Id
                                ? TransferDirection.In
                                : TransferDirection.Out
                    };
                    FillCounterpart(
                        model.Direction == TransferDirection.In
                            ? transfer.SourceAccountId
                            : transfer.DestinationAccountId, context, identityContext, model);
                    result.Add(model);

                    lastAmount = model.Direction == TransferDirection.In
                        ? lastAmount - transfer.Amount
                        : lastAmount + transfer.Amount;
                }

                return result;
            }
        }

        private void FillCounterpart(Guid accountId, DataContext context, IIdentityContext identityContext,
            TransferClientModel model)
        {
            var counterPartAccount = accountRepository.Read(accountId, context);
            model.CounterpartAccount = counterPartAccount.ToClient();
            var counterpart = userRepository.Read(counterPartAccount.UserId, identityContext);
            model.Counterpart = counterpart.ToClient();
        }
    }
}