using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Interfaces
{
    public interface IUserService
    {
         IQueryable <SocialMedia> GetSocialMediaData();

      
    }
}
