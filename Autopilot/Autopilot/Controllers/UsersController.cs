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
   //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
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
            return Ok(_userService.GetSocialMediaData());

        }

    

        /// <summary>
        /// get tags
        /// </summary>
        /// <param name="socialId"></param>
        /// <returns></returns>

        public IHttpActionResult GetTags(int socialId)
        {
            try
            {
                var tagList = _userService.GetAllTags(socialId);
                return Ok(tagList);

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// remove tags
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public IHttpActionResult RemoveTag(int tagId)
        {
            try
            {
                var result = _userService.RemoveTags(tagId);
                return Ok(new { status = result });
            }
            catch (Exception)
            {
                return Ok(new { status = false });
            }
        }


        public IHttpActionResult SideMenu()
        {
            var accounts = _userService.GetUsersAllAccounts(User.Identity.GetUserId());
            return Ok(accounts);
        }





    }
}
