using Autopilot.Models;
using EntitiesLayer.Entities;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Autopilot.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IAdminServices _adminServices;
        private readonly IUserService _userServices;
        private readonly ApplicationDbContext _accountDb;

        public AdminController(IAdminServices adminServices, IUserService userServices)
        {
            _adminServices = adminServices;
            _userServices = userServices;
            _accountDb = new ApplicationDbContext();
        }

        /// <summary>
        /// manage Subscription plans
        /// </summary>
        /// <returns></returns>
        //public IHttpActionResult ManagePlans()
        //{
        //    try
        //    {
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}



        public IHttpActionResult GetAllPlan()
        {
            try
            {
                var plans = _userServices.GetAllSubscriptionPlan();
                return Json(plans);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public IHttpActionResult GetAPlan(string PlanId)
        {
            try
            {
                var plan = _userServices.GetASubscriptionPlan(PlanId);
                return Json(new { status = true, plan = plan });
            }
            catch (Exception)
            {
                return Json(new { status = false });

            }
        }
        /// <summary>
        /// Add plans By Admin
        /// </summary>
        /// <param name="planObject"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPlan(SubscriptionsPlan planObject)
        {
            try
            {
                var result = _adminServices.AddSubscriptionsPlan(planObject);
                return Json(result);
            }
            catch (Exception)
            {

                return Json(false);

            }
        }


        public IHttpActionResult UpdatePlan(SubscriptionsPlan planObject)
        {
            try
            {
                var result = _adminServices.UpdateSubscriptionsPlan(planObject);
                return Json(result);
            }
            catch (Exception)
            {

                return Json(false);

            }
        }


        public IHttpActionResult RemovePlan(int planId)
        {
            try
            {
                var result = _adminServices.RemoveSubscriptionPlan(planId);
                return Json(result);
            }
            catch (Exception)
            {

                return Json(false);

            }
        }




    }
}
