using HubSpot.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HubSpotNetTests
{
    [TestClass]
    public class HubSpotNetTests
    {
        private IHubSpotNet _hubSpot;

        [TestInitialize]
        public void Init()
        {
            _hubSpot = new HubSpotNet();
        }

        [TestMethod]
        public async Task Get_Refresh_Token_Succeeds()
        {
            // We've got a test refresh token stashed in the app.config
            var res =
                await
                    _hubSpot.GetRefreshTokenInfo(
                        System.Configuration.ConfigurationManager.AppSettings["HubSpotTestRefreshToken"]);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        [ExpectedException(typeof(HubSpotException))]
        public async Task Get_Refresh_Token_Throws()
        {
            // We've got a test refresh token stashed in the app.config
            var res = await _hubSpot.GetRefreshTokenInfo(string.Empty);
            Assert.IsNotNull(res);
        }
    }
}
