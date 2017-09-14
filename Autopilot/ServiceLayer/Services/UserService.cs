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
        private readonly IUnitOfWork _unitOfWork;


        public UserService( IUnitOfWork unitOfWork, IRepository<SocialMedia> socialMedia)
        {         
            _unitOfWork = unitOfWork;
            _socialMediaRepo = socialMedia;
           
        }
        #endregion


        /// <summary>
        /// Get Service Method for Social Media table by diksha
        /// </summary>
        /// <returns></returns>
        /// 

        public IQueryable<SocialMedia> GetSocialMediaData()
        {
            return _socialMediaRepo.Get();
        }


       



    }
}
