using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using RestSharp;
using RestSharp.Deserializers;
using ServiceLayer.Helper;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class InstagramService : IInstagramService
    {

        private readonly IRepository<SocialMedia> _socialMedia;
        private readonly IRepository<AccessDetails> _accessDetail;       
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
            _userService = userService;
           

            consumerKey = ConfigurationSettings.AppSettings["InstaID"];
            consumerSecret = ConfigurationSettings.AppSettings["InstaSecret"];
            redirecturl = ConfigurationSettings.AppSettings["InstaRedirectURL"];
            ApiURL = "https://api.instagram.com/";
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
    }
}
