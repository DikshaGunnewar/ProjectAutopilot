using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer.Interfaces;
using System.Collections.Generic;
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
        /// get business category
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusinessCategory> GetBusinessCategory()
        {
            var categories = _businessCategory.Get().ToList();
            return categories;
        }


    }
}
