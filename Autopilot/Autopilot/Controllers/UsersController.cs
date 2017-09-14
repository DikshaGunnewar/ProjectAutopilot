using Autopilot.Models;
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
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ITwitterServices _twitterService;      
        private readonly ApplicationDbContext _accountDb;

        public UsersController(IUserService userService, ITwitterServices twitterService)
        {
            _userService = userService;
            _twitterService = twitterService;
            _accountDb = new ApplicationDbContext();
                     
        }




        /// <summary>
        ///Get method to retrive data on browser by diksha
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetData()
        {
            //var url = _userService.GetSocialMediaData();
            //return Redirect("http://localhost:4200/dashboard");

            return Ok(_userService.GetSocialMediaData());

        }

    

    }
}
