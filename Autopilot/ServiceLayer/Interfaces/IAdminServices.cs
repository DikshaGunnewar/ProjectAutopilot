using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IAdminServices
    {
        bool AddSubscriptionsPlan(SubscriptionsPlan plan);
        bool RemoveSubscriptionPlan(int planId);
        bool UpdateSubscriptionsPlan(SubscriptionsPlan plan);
    }
}
