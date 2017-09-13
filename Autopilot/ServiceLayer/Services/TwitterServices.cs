using EntitiesLayer.Entities;
using Newtonsoft.Json.Linq;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer.EnumStore;
using ServiceLayer.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using TweetSharp;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity;

namespace ServiceLayer.Services
{
    public class TwitterServices : ITwitterServices
    {


        private readonly IRepository<SocialMedia> _socialMediaRepo;
        private readonly IRepository<AccessDetails> _accessDetailRepo;
        private readonly IUserService _userService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly string consumerKey;
        private readonly string consumerSecret;

        public TwitterServices(IRepository<SocialMedia> socialMedia,
           IUnitOfWork unitOfWork,
           IRepository<AccessDetails> accessDetail,     
           IUserService userService)
        {
            _socialMediaRepo = socialMedia;
            _unitOfWork = unitOfWork;
            _accessDetailRepo = accessDetail;
            _userService = userService;

            consumerKey = ConfigurationSettings.AppSettings["twitterConsumerKey"];
            consumerSecret = ConfigurationSettings.AppSettings["twitterConsumerSecret"];
        }


        /// <summary>
        /// Method to get the uri for authorizing user in Twitter
        /// </summary>
        /// <returns></returns>
        public string Authorize()
        {
            try
            {
                var url = ConfigurationSettings.AppSettings["BaseURL"];

                // Step 1 - Retrieve an OAuth Request Token
                TwitterService service = new TwitterService(consumerKey, consumerSecret);

                //OAuthAccessToken access = service.GetAccessTokenWithXAuth("", "");
                // This is the registered callback URL

                OAuthRequestToken requestToken = service.GetRequestToken(url + "/api/Twitter/TwitterAuthCallback");

                //// Step 2 - Redirect to the OAuth Authorization URL
                Uri uri = service.GetAuthorizationUri(requestToken);
                return uri.ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }



        /// <summary>
        /// Method to get OAuth token for authenticate user
        /// </summary>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_verifier"></param>
        /// <returns></returns>
        public OAuthAccessToken GetTokensOAuth(string oauth_token, string oauth_verifier)
        {
            try
            {
                TwitterService service = new TwitterService(consumerKey, consumerSecret);
                var requestToken = new OAuthRequestToken { Token = oauth_token };
                OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);
                return accessToken;
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Save accounts details like token,followers count & other details by diksha
        /// </summary>

        public string SaveAccountDeatils(OAuthAccessToken tokens, string userId, string Email)
        {
            try
            {
                AccessDetails accessTokens = new AccessDetails() { AccessToken = tokens.Token, AccessTokenSecret = tokens.TokenSecret };
                var returnMessage = string.Empty;
                TwitterUser profile = GetUserprofile(accessTokens);
                var checkAccountIsAvail = _socialMediaRepo.Get().Include(x => x.AccessDetails).Where(x => x.SMId == profile.Id.ToString() && x.IsDeleted == false).FirstOrDefault();
                //var checkAccountIsAvail = _socialMediaRepo.Get().Where(x => x.SMId == profile.Id.ToString() && x.IsDeleted == false).Include(x => x.AccessDetails).FirstOrDefault();
                if (checkAccountIsAvail == null)
                {
                    SocialMedia socialDetails = new SocialMedia()
                    {
                        UserId = userId,
                        Provider = SocialMediaProviders.Twitter.ToString(),
                        AccessDetails = new AccessDetails { AccessToken = tokens.Token, AccessTokenSecret = tokens.TokenSecret },
                        ProfilepicUrl = profile.ProfileImageUrlHttps,
                        Followers = profile.FollowersCount,
                        SMId = profile.Id.ToString(),
                        Status = true,
                        UserName = profile.ScreenName,
                        AccSettings = new AccSettings()
                    };

                    socialDetails.AccSettings.UserManagement.Add(new UserManagement { Email = Email, userId = userId, Role = "Owner" });
                    _socialMediaRepo.Add(socialDetails);
                }
                else if (checkAccountIsAvail.UserId == userId)
                {
                    checkAccountIsAvail.AccessDetails.AccessToken = tokens.Token;
                    checkAccountIsAvail.AccessDetails.AccessTokenSecret = tokens.TokenSecret;
                    checkAccountIsAvail.IsInvalid = false;
                    checkAccountIsAvail.Status = true;
                    returnMessage = "Already added.";
                }
                else
                {
                    checkAccountIsAvail.AccessDetails.AccessToken = tokens.Token;
                    checkAccountIsAvail.AccessDetails.AccessTokenSecret = tokens.TokenSecret;
                    checkAccountIsAvail.IsInvalid = false;
                    checkAccountIsAvail.Status = true;
                    returnMessage = "Cannot add this account, as already added by other user.";
                }
                _unitOfWork.Commit();
                return returnMessage;
            }
            catch (Exception)
            {

                return "Something went wrong.";
            }
        }

       


        /// <summary>
        /// Get current user profile pic by diksha
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public TwitterUser GetUserprofile(AccessDetails accessToken)
        {
            try
            {
                TwitterService service = new TwitterService(consumerKey, consumerSecret);
                service.AuthenticateWith(accessToken.AccessToken, accessToken.AccessTokenSecret);
                var profile = service.GetUserProfile(new GetUserProfileOptions { IncludeEntities = true });
                return profile;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
