﻿using System.Threading.Tasks;

namespace HubSpot.Net
{
    public interface IHubSpotNet
    {
        Task<HubSpotAppTokenModel> CreateAppTokens(string code, string redirectUri);
        Task<HubSpotRefreshTokenInfoModel> GetRefreshTokenInfo(string token);
        Task DeleteRefreshToken(string token);
        Task<HubSpotAppTokenModel> RefreshAccessToken(string token, string redirectUri);
        Task<HubSpotDealModel> CreateDeal(string token, HubSpotCreateDealModel model);
    }
}