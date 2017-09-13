using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using RestSharp;
using RestSharp.Deserializers;
using ServiceLayer.EnumStore;
using ServiceLayer.Helper;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class InstagramService : IInstagramService
    {

        private readonly IRepository<SocialMedia> _socialMedia;
        private readonly IRepository<AccessDetails> _accessDetail;
        private readonly IRepository<SuperTargetUser> _targetUserRepo;
        private readonly IRepository<Tags> _tagRepo;
        private readonly IRepository<Activities> _activityRepo;
        private readonly IRepository<FollowersGraph> _followersGraphRepo;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string consumerKey;
        private readonly string consumerSecret;
        private readonly string redirecturl;
        private readonly string ApiURL;


        public InstagramService(IRepository<SocialMedia> socialMedia, IUnitOfWork unitOfWork, IRepository<AccessDetails> accessDetail, IRepository<Activities> activityRepo,
          IUserService userService, IRepository<SuperTargetUser> targetUserRepo, IRepository<Tags> tagRepo, IRepository<FollowersGraph> followersGraphRepo)
        {
            _socialMedia = socialMedia;
            _unitOfWork = unitOfWork;
            _accessDetail = accessDetail;
            _activityRepo = activityRepo;
            _userService = userService;
            _targetUserRepo = targetUserRepo;
            _tagRepo = tagRepo;
            _followersGraphRepo = followersGraphRepo;

            consumerKey = ConfigurationSettings.AppSettings["InstaID"];
            consumerSecret = ConfigurationSettings.AppSettings["InstaSecret"];
            redirecturl = ConfigurationSettings.AppSettings["InstaRedirectURL"];
            ApiURL = "https://api.instagram.com/";
        }


        public InstagramService(string consumerKey, string consumerSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }
        /// <summary>
        /// authentication method
        /// </summary>
        /// <returns></returns>
        public string Authorize()
        {
            try
            {
                var request = new RestRequest("oauth/authorize", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("scope", "public_content basic follower_list comments relationships likes");
                request.AddParameter("client_id", consumerKey);
                request.AddParameter("redirect_uri", redirecturl);
                request.AddParameter("response_type", "Code");
                var response = WebServiceHelper.WebRequest(request, ApiURL);
                var uri = response.ResponseUri.AbsoluteUri;
                return uri;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// access tocken method
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public OAuthTokens GetToken(string Code)
        {
            var request = new RestRequest("oauth/access_token", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", "OAuth " + token);
            request.AddParameter("client_id", consumerKey);
            request.AddParameter("client_secret", consumerSecret);
            request.AddParameter("code", Code);
            request.AddParameter("redirect_uri", redirecturl);
            request.AddParameter("grant_type", "authorization_code");
            var response = WebServiceHelper.WebRequest(request, ApiURL);
            JsonDeserializer deserial = new JsonDeserializer();
            OAuthTokens result = deserial.Deserialize<OAuthTokens>(response);
            return result;
        }




        /// <summary>
        /// Method to save account details of authorize user in instagram
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="userId"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public string SaveAccountDeatils(OAuthTokens tokens, string userId, string Email)
        {
            try
            {
                AccessDetails accessToken = new AccessDetails() { AccessToken = tokens.access_token };
                var returnMessage = string.Empty;
                InstaUserList profile = GetUserprofile(accessToken);
                var checkAccountIsAvail = _socialMedia.Get().Include(x => x.AccessDetails).Where(x => x.SMId == profile.id.ToString() && x.IsDeleted == false).FirstOrDefault();
                if (checkAccountIsAvail == null)
                {
                    SocialMedia socialDetails = new SocialMedia()
                    {
                        UserId = userId,
                        Provider = SocialMediaProviders.Instagram.ToString(),
                        AccessDetails = new AccessDetails { AccessToken = tokens.access_token },
                        ProfilepicUrl = profile.profile_picture,
                        SMId = profile.id.ToString(),
                        Status = true,
                        UserName = profile.username,
                        Followers = profile.counts.followed_by,
                        AccSettings = new AccSettings()
                    };
                    socialDetails.AccSettings.UserManagement.Add(new UserManagement { Email = Email, userId = userId, Role = "Owner" });
                    _socialMedia.Add(socialDetails);
                    returnMessage = "Account added successfully.";
                }
                else if (checkAccountIsAvail.UserId == userId)
                {
                    checkAccountIsAvail.AccessDetails.AccessToken = tokens.access_token;
                    checkAccountIsAvail.IsInvalid = false;
                    checkAccountIsAvail.Status = true;
                    returnMessage = "Already added.";
                }
                else
                {
                    checkAccountIsAvail.AccessDetails.AccessToken = tokens.access_token;
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
        /// Method to get user profile using access token of authenticate user
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public InstaUserList GetUserprofile(AccessDetails Token)
        {
            try
            {

                var request = new RestRequest("/v1/users/self", Method.GET);
                request.AddParameter("format", "json");
                request.AddParameter("access_token", Token.AccessToken);

                var response = WebServiceHelper.WebRequest(request, ApiURL);
                JsonDeserializer deserial = new JsonDeserializer();
                var userAuth = WebServiceHelper.CheckTokenInValid(response);
                InstaUserProfile result = new InstaUserProfile() { data = new InstaUserList() };
                if (userAuth == true)
                {
                    result = deserial.Deserialize<InstaUserProfile>(response);
                }
                else
                {
                    result.data.IsAccountValid = false;

                }
                return result.data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method to get user profile using access token and userId 
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public InstaUserList GetUserprofile(AccessDetails Token, string userId)
        {
            try
            {
                var request = new RestRequest("/v1/users/" + userId + "", Method.GET);
                request.AddParameter("format", "json");
                request.AddParameter("access_token", Token.AccessToken);
                var response = WebServiceHelper.WebRequest(request, ApiURL);
                JsonDeserializer deserial = new JsonDeserializer();
                InstaUserProfile result = deserial.Deserialize<InstaUserProfile>(response);
                return result.data;
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Method to logout user in Instagram
        /// </summary>
        /// <returns></returns>                                                                 
        public bool Logout()
        {
            try
            {
                var request = new RestRequest("accounts/logout", Method.GET);
                var response = WebServiceHelper.WebRequest(request, "http://instagram.com/");
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
