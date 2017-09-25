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
   //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
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
        /// Get method to Authenticate Social media Twitter by Diksha
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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

        /// <summary>
        /// Twitter callback method to get access token and store in Db by diksha
        /// </summary>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_verifier"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult TwitterAuthCallback(string oauth_token, string oauth_verifier)
       {
            var tokens = _twitterService.GetTokensOAuth(oauth_token, oauth_verifier);
            var response = _twitterService.SaveAccountDeatils(tokens, User.Identity.GetUserId(), User.Identity.Name);            
            return Redirect("http://localhost:4200/dashboard");
             //return Ok(response);     

        }


        /// <summary>
        /// Add Location
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IHttpActionResult AddLocation(int tagId, string location)
        {
            try
            {
                var result = _twitterService.AddLocationToTag(tagId, location);
                return Ok(new { status = result });
            }
            catch (Exception)
            {
                return Ok(new { status = false });
            }
        }

        public IHttpActionResult RemoveLocation(int tagId)
        {
            try
            {
                var result = _twitterService.RemoveLocation(tagId);
                return Ok(new { status = result });
            }
            catch (Exception)
            {
                return Ok(new { status = false });

            }
        }
    }



}

