using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IInstagramService
    {
        string Authorize();
        OAuthTokens GetToken(string Code);
        string SaveAccountDeatils(OAuthTokens tokens, string userId, string Email);
        InstaUserList GetUserprofile(AccessDetails Token);
        bool Logout();
    }
}
