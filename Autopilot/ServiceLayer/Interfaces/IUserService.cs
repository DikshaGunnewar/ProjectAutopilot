using EntitiesLayer.Entities;
using EntitiesLayer.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Interfaces
{
    public interface IUserService
    {
        IQueryable <SocialMedia> GetSocialMediaData();
        AccessDetails GetTokenForUser(int socialId);
        bool AddBlockTag(string tag, int socailId, bool IsBlocked);
        bool RemoveTags(int tagId);
        List<TagsVM> GetAllTags(int socailId);
        IEnumerable<SocialMediaVM> GetUsersAllAccounts(string userId);
        SubscriptionsPlan GetASubscriptionPlan(string PlanId);
        List<SubscriptionsPlan> GetAllSubscriptionPlan();
        void ApplyUserSubscription(UserAccountSubscription accSubscriptionDetails);
        bool SaveTransactionDetails(PaymentViewModel transactioDetails);

    }
}
