using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PwTransferApp.Models.ClientModels;
using PwTransferApp.Providers;

namespace PwTransferApp.Controllers.v1
{
    [Authorize]
    [RoutePrefix("api/v1/User")]
    public class UserV1Controller : LoggedInController
    {
        private readonly IUserSearcher userSearcher;

        public UserV1Controller(IUserSearcher userSearcher)
        {
            this.userSearcher = userSearcher;
        }

        [Route("Search")]
        [HttpGet]
        public IEnumerable<UserWithAccountClientModel> Search(string prefix)
        {
            var result = userSearcher.Search(prefix, UserId.ToString());
            return result
                .Select(x => new UserWithAccountClientModel
                {
                    Id = x.Item1.Id,
                    FirstName = x.Item1.FirstName,
                    LastName = x.Item1.LastName,
                    Login = x.Item1.UserName,
                    DefaultAccountId = x.Item2
                });
        }
    }
}