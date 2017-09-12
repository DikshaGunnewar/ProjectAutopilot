using Microsoft.AspNet.Identity;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
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

        // GET: /SocialMedia/
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

        [HttpGet]
        public IHttpActionResult TwitterAuthCallback(string oauth_token, string oauth_verifier)
        {
            var tokens = _twitterService.GetTokensOAuth(oauth_token, oauth_verifier);
            //var response = _twitterService.SaveAccountDeatils(tokens, User.Identity.GetUserId(), User.Identity.Name);
            //return RedirectToAction("Dashboard", "Users", new { Message = response });
            //return Ok(response);
            return Ok(tokens);
   
        }

    }
}
