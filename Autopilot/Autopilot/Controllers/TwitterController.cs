using EntitiesLayer.Entities;
using Microsoft.AspNet.Identity;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Autopilot.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class TwitterController : ApiController
    {
        private readonly ITwitterServices _twitterService;

        private readonly IUserService _userService;

        public TwitterController(ITwitterServices twitterService, IUserService userService)
        {
            _twitterService = twitterService;
            _userService = userService;
        }

        /// <summary>
        /// trial get data from social media class
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetData()
        {

            return Ok(_twitterService.GetTwitterData());

        }




        // GET: /SocialMedia/

        public IHttpActionResult TwitterAuth()
        {
            var URI = _twitterService.Authorize();
            //return Redirect(URI);
            return Ok(URI);

        }


        public IHttpActionResult Reconnect()
        {
            try
            {
                return Redirect("TwitterAuth");
            }
            catch (Exception)
            {

                throw;
            }
        }

  
        public IHttpActionResult TwitterAuthCallback(string oauth_token, string oauth_verifier)
       {
            var tokens = _twitterService.GetTokensOAuth(oauth_token, oauth_verifier);
            var response = _twitterService.SaveAccountDeatils(tokens, User.Identity.GetUserId(), User.Identity.Name);
            //var url = ConfigurationSettings.AppSettings["BaseURL"];
            //return Redirect(url+ "/api/Users/Dashboard"+response);       
            return Ok(response);
            
            //return Ok(tokens);

        }


      
    }
}
