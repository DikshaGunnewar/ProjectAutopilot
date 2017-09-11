﻿using ServiceLayer.Interfaces;
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
            return new RedirectResult(URI, false /*permanent*/);

        }

        public IHttpActionResult Reconnect()
        {
            try
            {
                return RedirectToAction("TwitterAuth");
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
