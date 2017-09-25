using Autopilot.Helper;
using EntitiesLayer.ViewModels;
using Microsoft.AspNet.Identity;
using PayPal.Api;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Autopilot.Controllers
{
    public class PaymentController : ApiController
    {

        private readonly PaypalPaymentHelper _paymentheader;
        private PayPal.Api.Payment payment;

        private readonly IUserService _userService;
        public PaymentController(IUserService userService)
        {
            _userService = userService;

        }




        //[AcceptVerbs("Get", "Post")]
        [AcceptVerbs("HttpVerbs.Get", "HttpVerbs.Post")]
        public IHttpActionResult PaymentWithPaypal()
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
             
                string planId = HttpContext.Current.Request.Params["planId"];
                string socailIds = HttpContext.Current.Request.Params["socialIds"];
                string payerId = HttpContext.Current.Request.Params["PayerID"];



                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    //var url = ConfigurationSettings.AppSettings["BaseURL"] + RequestRequest.Url.Authority + "/Payment/PaymentWithPayPal?";


                    string baseURI = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                "/Payment/PaymentWithPayPal?";

                    // string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid + "&planId=" + planId + "&socialIds=" + socailIds, planId, socailIds);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    HttpContext.Current.Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment
                    var guid = HttpContext.Current.Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Current.Session[guid] as string);
                    PaymentViewModel vm = new PaymentViewModel()
                    {
                        planId = planId,
                        socialIds = socailIds,
                        Amount = executedPayment.transactions[0].amount.total,
                        TransactionId = executedPayment.transactions[0].related_resources[0].sale.id,
                        userId = User.Identity.GetUserId(),
                        Currency = executedPayment.transactions[0].related_resources[0].sale.amount.currency,
                        Status = executedPayment.state,
                        InvoiceId = executedPayment.transactions[0].invoice_number,
                        Date = DateTime.Parse(executedPayment.create_time)
                    };
                    _userService.SaveTransactionDetails(vm);

                    if (executedPayment.state.ToLower() == "approved")
                    {

                        //return Redirect("PaymentSuccess"vm);
                        return Redirect(" PaymentSuccess");

                    }
                    else
                    {
                        return Ok("false");
                    }
                }
            }
            catch (Exception)
            {

                return Ok("false");

            }

        }


        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string planId, string socialIds)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            //int subscriptionPlanId = Int16.Parse(planId);
            var plan = _userService.GetASubscriptionPlan(planId);
            int quantity = socialIds.Split(',').Count();
            itemList.items.Add(new Item()
            {
                name = plan.Title,
                currency = "USD",
                price = plan.Price.ToString(),
                quantity = quantity.ToString(),

            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "1",
                subtotal = (quantity * plan.Price).ToString()
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (double.Parse(details.tax) + double.Parse(details.subtotal)).ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            Random generator = new Random();
            string random = generator.Next(0, 1000000).ToString("D6");
            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = random,
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public IHttpActionResult PaymentSuccess(PaymentViewModel vm)
        {

            //var acc = _paymentheader.CreateCustomer(model);
            vm.planDetails = _userService.GetASubscriptionPlan(vm.planId);


            return Ok(vm);
        }

    }
}
