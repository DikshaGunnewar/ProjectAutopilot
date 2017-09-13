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
        private readonly IRepository<BusinessCategory> _businessCategory;
        private readonly IRepository<SocialMedia> _socialMediaRepo;
        private readonly IRepository<SubscriptionsPlan> _subscriptionPlanRepo;
        private readonly IRepository<OrderDetails> _orderDetailsRepo;
        private readonly IRepository<UserAccountSubscription> _accountSubscriptionRepo;

        private readonly IRepository<Languages> _languageRepo;
        private readonly IRepository<Tags> _tagRepo;
        private readonly IRepository<Activities> _activityRepo;
        private readonly IRepository<Conversions> _conversionRepo;
        private readonly IRepository<SuperTargetUser> _superTargetUserRepo;
        private readonly IRepository<UserManagement> _userManagementRepo;
        private readonly IRepository<FollowersGraph> _followersGraphRepo;
        private readonly IRepository<UserBillingAddress> _userBillingAddressRepo;

        private readonly IUnitOfWork _unitOfWork;


        public UserService(IRepository<BusinessCategory> businessCategory,
            IUnitOfWork unitOfWork,
            IRepository<SocialMedia> socialMedia,
            IRepository<Languages> languages,
            IRepository<Activities> activityRepo,
            IRepository<Conversions> conversionRepo,
            IRepository<UserManagement> userManagementRepo,
            IRepository<SuperTargetUser> superTargetUserRepo,
            IRepository<Tags> tagsRepo, IRepository<FollowersGraph> followersGraphRepo,
            IRepository<SubscriptionsPlan> subscriptionPlanRepo,
            IRepository<UserBillingAddress> userBillingAddressRepo,
            IRepository<OrderDetails> orderDetailsRepo,
        IRepository<UserAccountSubscription> accountSubscriptionRepo)
        {
            _businessCategory = businessCategory;
            _unitOfWork = unitOfWork;
            _tagRepo = tagsRepo;
            _superTargetUserRepo = superTargetUserRepo;
            _userManagementRepo = userManagementRepo;
            _conversionRepo = conversionRepo;
            _activityRepo = activityRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _accountSubscriptionRepo = accountSubscriptionRepo;
            _userBillingAddressRepo = userBillingAddressRepo;
            _socialMediaRepo = socialMedia;
            _languageRepo = languages;
            _subscriptionPlanRepo = subscriptionPlanRepo;
            _followersGraphRepo = followersGraphRepo;
        }
        #endregion



        /// <summary>
        /// Get all socail account associated with a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public IEnumerable<SocialMediaVM> GetUsersAllAccounts(string userId)
        //{
        //    //var accounts = _socialMediaRepo.Get().Where(x => x.UserId == userId).ToList();
        //    var accounts = _userManagementRepo.Get().ToList()
        //        .Join(_socialMediaRepo.Get().Include(x => x.AccSettings).Where(x => x.IsDeleted == false).ToList(), um => um.AccSettingId, sm => sm.AccSettings.Id, (um, sm) => new { um, sm })
        //        .Where(x => x.um.userId == userId)
        //        .Select(m => new
        //        {
        //            m.sm.Id,
        //            m.sm.SMId,
        //            m.sm.Status,
        //            m.sm.Followers,
        //            m.sm.Provider,
        //            m.sm.UserId,
        //            m.sm.UserName,
        //            m.sm.ProfilepicUrl,
        //            m.sm.IsInvalid
        //        }).ToList();
        //    List<SocialMediaVM> accountList = new List<SocialMediaVM>();
        //    foreach (var acc in accounts)
        //    {
        //        accountList.Add(new SocialMediaVM { Id = acc.Id, SMId = acc.SMId, Status = acc.Status, Followers = acc.Followers, Provider = acc.Provider, UserId = acc.UserId, UserName = acc.UserName, ProfilepicUrl = acc.ProfilepicUrl, IsInvalid = acc.IsInvalid });

        //    }

        //    return accountList;
        //}


        /// <summary>
        /// Getting all the social accounts in the application
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<SocialMediaVM> GetAllAccounts()
        //{
        //    var accounts = _socialMediaRepo.Get().Include(x => x.AccSettings).Include(x => x.AccessDetails).Include(x => x.AccSettings.Tags).Include(x => x.AccSettings.SuperTargetUser).Where(x => x.IsDeleted == false).ToList();
        //    var today = DateTime.UtcNow;
        //    List<SocialMediaVM> accountList = new List<SocialMediaVM>();
        //    foreach (var acc in accounts)
        //    {
        //        var subscription = _accountSubscriptionRepo.Get().Where(x => x.socialIds == acc.Id.ToString() && x.ExpiresOn > today).FirstOrDefault();

        //        var accountDetails = new SocialMediaVM()
        //        {
        //            Id = acc.Id,
        //            SMId = acc.SMId,
        //            Status = acc.Status,
        //            Followers = acc.Followers,
        //            Provider = acc.Provider,
        //            UserId = acc.UserId,
        //            UserName = acc.UserName,
        //            ProfilepicUrl = acc.ProfilepicUrl,
        //            AccessDetails = acc.AccessDetails,
        //            IsInvalid = acc.IsInvalid,
        //            AccSettings = acc.AccSettings
        //        };
        //        if (subscription == null) //checking if account is under subscription or in trail
        //        {
        //            var CheckForTrail = _accountSubscriptionRepo.Get().Where(x => x.userId == acc.UserId && x.ExpiresOn > today && x.IsTrail == true).FirstOrDefault();
        //            if (CheckForTrail != null)
        //            {
        //                accountDetails.IsTrail = true;
        //                accountDetails.IsSubscribed = false;
        //            }
        //        }
        //        else
        //        {
        //            accountDetails.IsTrail = false;
        //            accountDetails.IsSubscribed = true;
        //            accountDetails.planId = subscription.PlanId.ToString();
        //        }
        //        accountList.Add(accountDetails);
        //    }

        //    return accountList;
        //}



        /// <summary>
        /// Get application user's profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public UserProfileVM GetUserprofile(string userId)
        //{
        //    try
        //    {
        //        UserProfileVM profile = new UserProfileVM();
        //        var accounts = GetUsersAllAccounts(userId);
        //        var billingAddress = _userBillingAddressRepo.Get().Where(x => x.UserId == userId).FirstOrDefault();
        //        profile.UserId = userId;
        //        profile.UserAccounts = accounts.ToList();
        //        if (billingAddress != null)
        //        {
        //            profile.BillingAddress = billingAddress;
        //        }
        //        else
        //        {
        //            profile.BillingAddress = new UserBillingAddress();
        //        }
        //        return profile;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}



    }
}
