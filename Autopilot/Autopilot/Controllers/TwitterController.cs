using Microsoft.AspNet.Identity;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Autopilot.Controllers
{
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
        public IHttpActionResult TwitterAuth()
        {
            var URI = _twitterService.Authorize();
            return Redirect(URI);

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
            //return RedirectToAction("Dashboard", "Users", new { Message = response });
            return Ok(response);
   
        }

    }
}
