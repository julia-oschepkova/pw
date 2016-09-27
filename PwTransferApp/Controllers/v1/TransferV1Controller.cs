using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common.Exceptions;
using Common.Managers;
using Common.Model;
using Common.Repositories;
using PwTransferApp.Models.ClientModels;
using PwTransferApp.Models.Forms;
using PwTransferApp.Providers;

namespace PwTransferApp.Controllers.v1
{
    [Authorize]
    [RoutePrefix("api/v1/Transfer")]
    public class TransferV1Controller : LoggedInController
    {
        private readonly ITransferManager transferManager;
        private readonly IPwAccountRepository pwAccountRepository;
        private readonly TransferClientModelProvider clientModelProvider;
        private readonly ITransferRepository transferRepository;

        public TransferV1Controller(ITransferManager transferManager,
            IPwAccountRepository pwAccountRepository,
            TransferClientModelProvider clientModelProvider,
            ITransferRepository transferRepository)
        {
            this.transferManager = transferManager;
            this.pwAccountRepository = pwAccountRepository;
            this.clientModelProvider = clientModelProvider;
            this.transferRepository = transferRepository;
        }

        // GET: api/Transfer
        [Route]
        [HttpGet]
        public List<TransferClientModel> Get()
        {
            return clientModelProvider.Get(pwAccountRepository.GetByUserId(UserId));
        }
        
        // POST: api/Transfer
        /// <summary>
        /// add new transfer
        /// </summary>
        /// <param name="form">transfer data </param>
        [Route]
        [HttpPost]
        public TransferClientModel Post([FromBody] CreateTransactionForm form)
        {
            var userAccount = pwAccountRepository.GetByUserId(UserId);
            var errors = GetTransferModelErrors(form, userAccount);

            if (errors.Any())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Join(";", errors)),
                });
            }
            Tuple<Transfer, PwAccount> transferData = null;
            try
            {
                transferData = transferManager.Transfer(userAccount.Id, form.RecipientAccountId, form.Amount,
                    form.Description);
            }
            catch (AccountWasChangedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent("Self or recipient account was changed during transfer"),
                });
            }
            catch (NotEnoughtPwForTransferException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Not enought PW to make transfer"),
                });
            }
            catch (EntityNotFoundException ex)
            {
                if (ex.EntityType == typeof(PwAccount))
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Recipient account is invalid"),
                    });
            }
            return clientModelProvider.Get(transferData?.Item1, transferData?.Item2);
        }

        private List<string> GetTransferModelErrors(CreateTransactionForm form, PwAccount account)
        {
            var errors = new List<string>();
            if (form.RecipientAccountId == Guid.Empty)
                errors.Add("Recipient account is invalid");
            if (form.Amount <= 0)
                errors.Add("Amount should be greater than 0");
            if (form.Amount >= account.Balance)
                errors.Add("Not enought PW to make transfer");
            if (form.RecipientAccountId == account.Id)
                errors.Add("Cannot make transfer to oneself");
            return errors;
        }
    }
}