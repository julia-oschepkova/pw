using Common.Model;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.Models.ClientModels
{
    public static class ClientModelExtensions
    {
        public static PwAccountClientModel ToClient(this PwAccount account)
        {
            return new PwAccountClientModel()
            {
                Id = account.Id,
                Name = account.Name
            };
        }

        public static PwAccountWithBalanceClientModel ToClientWithBalance(this PwAccount account)
        {
            return new PwAccountWithBalanceClientModel()
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance
            };
        }


        public static UserClientModel ToClient(this ApplicationUser user)
        {
            return new UserClientModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.UserName
            };
        }
    }
}