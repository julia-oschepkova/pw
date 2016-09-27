using System.Web.Http;
using Common.Repositories;
using PwTransferApp.Models.ClientModels;

namespace PwTransferApp.Controllers.v1
{
    [RoutePrefix("api/v1/PwAccount")]
    public class PwAccountV1Controller : LoggedInController
    {
        private readonly IPwAccountRepository pwAccountRepository;

        public PwAccountV1Controller(IPwAccountRepository pwAccountRepository)
        {
            this.pwAccountRepository = pwAccountRepository;
        }

        [Route("")]
        public PwAccountWithBalanceClientModel Get()
        {
            return pwAccountRepository.GetByUserId(UserId).ToClientWithBalance();
        }
    }
}