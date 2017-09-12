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
    public class InstagramController : ApiController
    {
        private readonly IInstagramService _instagramService;
        private readonly IUserService _userService;
        public InstagramController(IUserService userService, IInstagramService instagramService)
        {
            _instagramService = instagramService;
            _userService = userService;
        }

        /// <summary>
        /// Method for Authenticate and get URL path
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult InstaAuth()
        {
            try
            {
                var URI = _instagramService.Authorize();

                //return Ok(URI, false);
                return Ok(URI);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method for save account details
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult AuthCallback(string code)
        {
            try
            {
                var tokens = _instagramService.GetToken(code);
                //var response = _instagramService.SaveAccountDeatils(tokens, User.Identity.GetUserId(), User.Identity.Name);
                //return RedirectToAction("Dashboard", "Users", new { Message = response });
                return Ok(tokens);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
