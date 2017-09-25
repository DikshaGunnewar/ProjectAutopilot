using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {

        #region Initialization      
        private readonly IRepository<SocialMedia> _socialMediaRepo;
        private readonly IRepository<SubscriptionsPlan> _subscriptionPlanRepo;
        private readonly IRepository<Tags> _tagRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserManagement> _userManagementRepo;
        private readonly IRepository<OrderDetails> _orderDetailsRepo;
        private readonly IRepository<UserAccountSubscription> _accountSubscriptionRepo;



        public UserService( IUnitOfWork unitOfWork, 
            IRepository<SocialMedia> socialMedia, 
            IRepository<SubscriptionsPlan> subscriptionPlanRepo,
            IRepository<Tags> tagsRepo, IRepository<UserManagement> userManagementRepo,
            IRepository<OrderDetails> orderDetailsRepo,
            IRepository<UserAccountSubscription> accountSubscriptionRepo)
        {         
            _unitOfWork = unitOfWork;
            _socialMediaRepo = socialMedia;
            _subscriptionPlanRepo = subscriptionPlanRepo;
            _tagRepo = tagsRepo;
            _userManagementRepo = userManagementRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _accountSubscriptionRepo = accountSubscriptionRepo;




        }
        #endregion


        /// <summary>
        /// Get Service Method for Social Media table by diksha
        /// </summary>
        /// <returns></returns>
       

        public IQueryable<SocialMedia> GetSocialMediaData()
        {
            return _socialMediaRepo.Get();
        }




        /// <summary>
        /// Get all socail account associated with a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SocialMediaVM> GetUsersAllAccounts(string userId)
        {
            //var accounts = _socialMediaRepo.Get().Where(x => x.UserId == userId).ToList();
            var accounts = _userManagementRepo.Get().ToList()
                .Join(_socialMediaRepo.Get().Include(x => x.AccSettings).Where(x => x.IsDeleted == false).ToList(), um => um.AccSettingId, sm => sm.AccSettings.Id, (um, sm) => new { um, sm })
                .Where(x => x.um.userId == userId)
                .Select(m => new
                {
                    m.sm.Id,
                    m.sm.SMId,
                    m.sm.Status,
                    m.sm.Followers,
                    m.sm.Provider,
                    m.sm.UserId,
                    m.sm.UserName,
                    m.sm.ProfilepicUrl,
                    m.sm.IsInvalid
                }).ToList();
            List<SocialMediaVM> accountList = new List<SocialMediaVM>();
            foreach (var acc in accounts)
            {
                accountList.Add(new SocialMediaVM { Id = acc.Id, SMId = acc.SMId, Status = acc.Status, Followers = acc.Followers, Provider = acc.Provider, UserId = acc.UserId, UserName = acc.UserName, ProfilepicUrl = acc.ProfilepicUrl, IsInvalid = acc.IsInvalid });

            }

            return accountList;
        }







        /// <summary>
        /// Get Token for User social account
        /// </summary>
        /// <param name="socialId"></param>
        /// <returns></returns>
        public AccessDetails GetTokenForUser(int socialId)
        {
            try
            {
                var accessToken = _socialMediaRepo.Get().Include(x => x.AccessDetails).Where(x => x.Id == socialId).Select(x => x.AccessDetails).FirstOrDefault();
                return accessToken;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Add/block tags
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="socailId"></param>
        /// <param name="IsBlocked"></param>
        /// <returns></returns>
        public bool AddBlockTag(string tag, int socailId, bool IsBlocked)
        {
            try
            {
                var acc = _socialMediaRepo.Get().Include(x => x.AccSettings).Include(x => x.AccSettings.Tags).Where(x => x.Id == socailId).FirstOrDefault();
                if (acc.AccSettings == null)
                {
                    acc.AccSettings = new AccSettings();
                }
                var check = acc.AccSettings.Tags.Where(x => x.TagName.ToLower() == tag.ToLower()).FirstOrDefault();
                if (check == null)
                {
                    acc.AccSettings.Tags.Add(new Tags { IsBlocked = IsBlocked, TagName = tag });
                    _unitOfWork.Commit();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Remove Tags
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public bool RemoveTags(int tagId)
        {
            try
            {
                var tagItem = _tagRepo.Get().Where(x => x.Id == tagId).FirstOrDefault();
                _tagRepo.Remove(tagItem);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// Get all tags (blocked as well as unblocked)
        /// </summary>
        /// <param name="socialId"></param>
        /// <returns></returns>
        public List<TagsVM> GetAllTags(int socialId)
        {
            try
            {
                var acc = _socialMediaRepo.Get().Include(x => x.AccSettings).Include(x => x.AccSettings.Tags).Where(x => x.Id == socialId).FirstOrDefault();
                List<TagsVM> tagList = new List<TagsVM>();
                if (acc.AccSettings != null)
                {
                    foreach (var item in acc.AccSettings.Tags)
                    {
                        tagList.Add(new TagsVM { AccSettingId = item.AccSettingId, Id = item.Id, IsBlocked = item.IsBlocked, TagName = item.TagName, Location = item.Location });
                    }
                }

                return tagList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get all subscription plans
        /// </summary>
        /// <returns></returns>
        public List<SubscriptionsPlan> GetAllSubscriptionPlan()
        {
            try
            {
                var plans = _subscriptionPlanRepo.Get().Where(x => x.IsDeleted == false).ToList();
                return plans;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Get a particular subscription plan
        /// </summary>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        public SubscriptionsPlan GetASubscriptionPlan(string PlanId)
        {
            try
            {
                var pId = Int16.Parse(PlanId);
                var plans = _subscriptionPlanRepo.Get().Where(x => x.IsDeleted == false && x.Id == pId).FirstOrDefault();
                return plans;
            }
            catch (Exception)
            {

                throw;
            }
        }

      

        /// <summary>
        /// Save payment/transaction details
        /// </summary>
        /// <param name="transactioDetails"></param>
        /// <returns></returns>
        public bool SaveTransactionDetails(PaymentViewModel transactioDetails)
        {
            try
            {

                OrderDetails order = new OrderDetails()
                {
                    Amount = transactioDetails.Amount,
                    Date = DateTime.UtcNow,
                    PaymentMethod = "Paypal",
                    SubscriptionPlanId = transactioDetails.planId,
                    TransactionId = transactioDetails.TransactionId,
                    UserId = transactioDetails.userId,
                    Status = transactioDetails.Status,
                    Currency = transactioDetails.Currency,
                    InvoiceId = transactioDetails.InvoiceId,
                    SocialId = transactioDetails.socialIds
                };   //preparing order details object
                var addedOrder = _orderDetailsRepo.AddAndReturn(order); //add order details

                var pId = Int16.Parse(transactioDetails.planId);
                var planDetails = _subscriptionPlanRepo.Get().Where(x => x.Id == pId).FirstOrDefault();
                DateTime planExpiresDate = transactioDetails.Date;
                if (planDetails.BillingFrequency == "Monthly") { planExpiresDate = transactioDetails.Date.AddDays(30); }
                else if (planDetails.BillingFrequency == "Yearly") { planExpiresDate = transactioDetails.Date.AddDays(365); }
                if (transactioDetails.Status.ToLower() == "approved") // transcation is approved/success then apply subscription for user account
                {
                    foreach (var item in transactioDetails.socialIds.Split(','))
                    {

                        ApplyUserSubscription(new UserAccountSubscription { socialIds = item, userId = transactioDetails.userId, ExpiresOn = planExpiresDate, IsTrail = false, PlanId = Int16.Parse(transactioDetails.planId), OrderId = addedOrder.Id.ToString() });
                    }
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }

        /// <summary>
        /// Apply/Add subscription for particular social account
        /// </summary>
        /// <param name="accSubscriptionDetails"></param>
        public void ApplyUserSubscription(UserAccountSubscription accSubscriptionDetails)
        {
            try
            {
                _accountSubscriptionRepo.Add(accSubscriptionDetails);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
