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


         // GET: /User/
        // [HttpGet]
        //public IHttpActionResult Dashboard(string Message)
        //{
        //    var userId = User.Identity.GetUserId();
        //    //ViewBag.EmailConfirmed = _accountDb.Users.Where(x => x.Id == userId).FirstOrDefault().EmailConfirmed;
        //    //ViewBag.Message = Message;
        //    var accounts = _userService.GetUsersAllAccounts(User.Identity.GetUserId());
        //    //return Ok(accounts);
        //    return Ok(accounts);
        //}

        //[HttpGet]

        //public IHttpActionResult Profile()
        //{
        //    try
        //    {
        //        var profile = _userService.GetUserprofile(User.Identity.GetUserId());
        //        var applicationUserDetail = _accountDb.Users.ToList().Where(x => x.Id == profile.UserId).FirstOrDefault();
        //        profile.Username = applicationUserDetail.UserName;
        //        profile.Email = applicationUserDetail.Email;
        //        profile.EmailVerified = applicationUserDetail.EmailConfirmed;
        //        return Ok(profile);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}




    }
}
