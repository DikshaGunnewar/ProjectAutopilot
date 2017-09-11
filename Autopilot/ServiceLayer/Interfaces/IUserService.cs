using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IUserService
    {
        IEnumerable<BusinessCategory> GetBusinessCategory();
       
    }
}
