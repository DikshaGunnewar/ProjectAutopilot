using EntitiesLayer.Entities;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IRepository<SocialMedia> _socialMediaRepo;
        private readonly IRepository<SubscriptionsPlan> _subscriptionPlanRepo;
        private readonly IUserService _userServices;
        private readonly IRepository<UserAccountSubscription> _userAccountSubscription;
        private readonly IRepository<UserBillingAddress> _userBillingAddressRepo;

        private readonly IUnitOfWork _unitOfWork;


        public AdminServices(IRepository<SocialMedia> socialMediaRepo, IUserService userServices, IRepository<SubscriptionsPlan> subscriptionPlanRepo,
          IUnitOfWork unitOfWork, IRepository<UserAccountSubscription> userAccountSubscription, IRepository<UserBillingAddress> userBillingAddressRepo)
        {
            _socialMediaRepo = socialMediaRepo;
            _subscriptionPlanRepo = subscriptionPlanRepo;
            _userServices = userServices;
            _userBillingAddressRepo = userBillingAddressRepo;
            _userAccountSubscription = userAccountSubscription;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Add subscription plan by admin by diksha
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>

        public bool AddSubscriptionsPlan(SubscriptionsPlan plan)
        {
            try
            {
                _subscriptionPlanRepo.Add(plan);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool UpdateSubscriptionsPlan(SubscriptionsPlan plan)
        {
            try
            {
                _subscriptionPlanRepo.Update(plan);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool RemoveSubscriptionPlan(int planId)
        {
            try
            {
                var plan = _subscriptionPlanRepo.Get().Where(x => x.Id == planId).FirstOrDefault();
                plan.IsDeleted = true;
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
