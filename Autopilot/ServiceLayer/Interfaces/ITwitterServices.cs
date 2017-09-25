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
        //IQueryable <SocialMedia> GetTwitterData();
        string Authorize();
        OAuthAccessToken GetTokensOAuth(string oauth_token, string oauth_verifier);
        string SaveAccountDeatils(OAuthAccessToken tokens, string userId, string Email);
        bool UpdateProfile(AccessDetails tokens, int socialId);
        TwitterUser GetUserprofile(AccessDetails accessToken);
        IEnumerable<TwitterUser> SearchUser(string query, AccessDetails _accessToken);
        bool LikeTweet(long tweetId, AccessDetails _accessToken);
        bool AddLocationToTag(int tagId, string location);
        bool RemoveLocation(int tagId);
    }
}
