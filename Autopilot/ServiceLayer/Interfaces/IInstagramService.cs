using EntitiesLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface IInstagramService
    {
        string Authorize();
        OAuthTokens GetToken(string Code);
    }
}
