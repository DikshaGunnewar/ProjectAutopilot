using EntitiesLayer.Entities;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer.Interfaces;
using System;
using System.Configuration;
using TweetSharp;

namespace ServiceLayer.Services
{
    public class TwitterServices:ITwitterServices
    {
        private readonly IRepository<SocialMedia> _socialMediaRepo;
        private readonly IRepository<AccessDetails> _accessDetailRepo;
        private readonly IRepository<SuperTargetUser> _targetUserRepo;
        private readonly IRepository<Activities> _activityRepo;
        private readonly IRepository<SuperTargetUser> _superTargetUserRepo;
        private readonly IRepository<FollowersGraph> _followersGraphRepo;

        private readonly IRepository<Tags> _tagRepo;

        private readonly IUserService _userService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly string consumerKey;
        private readonly string consumerSecret;


        public TwitterServices(IRepository<SocialMedia> socialMedia,
           IUnitOfWork unitOfWork,
           IRepository<AccessDetails> accessDetail,
           IRepository<SuperTargetUser> targetUserRepo,
           IRepository<Activities> activityRepo, IUserService userService, IRepository<Tags> tagRepo,
           IRepository<SuperTargetUser> superTargetUserRepo, IRepository<FollowersGraph> followersGraphRepo)
        {
            _socialMediaRepo = socialMedia;
            _unitOfWork = unitOfWork;
            _targetUserRepo = targetUserRepo;
            _accessDetailRepo = accessDetail;
            _activityRepo = activityRepo;
            _superTargetUserRepo = superTargetUserRepo;
            _tagRepo = tagRepo;
            _followersGraphRepo = followersGraphRepo;
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

                OAuthRequestToken requestToken = service.GetRequestToken(url + "/Twitter/TwitterAuthCallback");

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




    }
}
