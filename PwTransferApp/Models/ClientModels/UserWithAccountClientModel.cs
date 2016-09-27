using System;

namespace PwTransferApp.Models.ClientModels
{
    public class UserWithAccountClientModel : UserClientModel
    {
        public Guid DefaultAccountId { get; set; }
    }
}