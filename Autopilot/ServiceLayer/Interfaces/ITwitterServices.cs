using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace ServiceLayer.Interfaces
{
    public interface ITwitterServices
    {
        string Authorize();
        OAuthAccessToken GetTokensOAuth(string oauth_token, string oauth_verifier);
        string SaveAccountDeatils(OAuthAccessToken tokens, string userId, string Email);
        TwitterUser GetUserprofile(AccessDetails accessToken);

    }
}
