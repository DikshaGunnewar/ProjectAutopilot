using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {

        #region Initialization

        private readonly IRepository<SocialMedia> _socialMediaRepo;     
        private readonly IRepository<UserAccountSubscription> _accountSubscriptionRepo;
        private readonly IRepository<UserManagement> _userManagementRepo;  
        private readonly IUnitOfWork _unitOfWork;


        public UserService( IUnitOfWork unitOfWork,IRepository<SocialMedia> socialMedia,IRepository<UserManagement> userManagementRepo,
        IRepository<UserAccountSubscription> accountSubscriptionRepo)
        {
           
            _unitOfWork = unitOfWork;     
            _socialMediaRepo = socialMedia;
  
        }
        #endregion

       

        /// <summary>
        /// Get all social account associated with a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SocialMediaVM> GetUsersAllAccounts(string userId)
        {       
            var Accounts = _userManagementRepo.Get().ToList()
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
            foreach (var acc in Accounts)
            {
                accountList.Add(new SocialMediaVM { Id = acc.Id, SMId = acc.SMId, Status = acc.Status, Followers = acc.Followers, Provider = acc.Provider, UserId = acc.UserId, UserName = acc.UserName, ProfilepicUrl = acc.ProfilepicUrl, IsInvalid = acc.IsInvalid });

            }

            return accountList;
        }


        
    }
}
