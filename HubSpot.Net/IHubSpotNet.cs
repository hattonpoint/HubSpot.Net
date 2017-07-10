using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Net
{
    public interface IHubSpotNet
    {
        Task<HubSpotAppTokenModel> CreateAppTokens(string code, string redirectUri);
        Task<HubSpotRefreshTokenInfoModel> GetRefreshTokenInfo(string token);
        Task DeleteRefreshToken(string token);
        Task<HubSpotAppTokenModel> RefreshAccessToken(string token, string redirectUri);
        Task<HubSpotDealModel> CreateDeal(string token, HubSpotCreateDealModel model);
        Task<List<HubSpotContactModel>> GetRecentContacts(string apiKey, DateTime timeOffset, List<string> properties = null);
        Task<List<HubSpotContactModel>> GetContacts(string apiKey, List<string> properties);
        Task<HubSpotContactModel> GetContact(string apiKey, string email);
        Task UpdateContact(string apiKey, HubSpotUpdateContactModel model, string[] param);
        Task UpdateContacts(string apiKey, List<HubSpotUpdateContactModel> model);
        Task BatchUpdateContactsByEmail(string apiKey, List<HubSpotUpdateContactModel> model);
        Task DeleteContact(string apiKey, string vid);        
    }
}