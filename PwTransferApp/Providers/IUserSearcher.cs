using System;
using System.Collections.Generic;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.Providers
{
    public interface IUserSearcher
    {
        IEnumerable<Tuple<ApplicationUser, Guid>> Search(string prefix, string userContext);
    }
}