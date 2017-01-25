using HubSpot.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

        // Not a unit test, just used to test service calls
        [TestMethod]
        public async Task TestSomeStuff()
        {
            var apiKey = "b6485fd4-41b3-406e-b925-a5a99309b68b";

            var contacts = await _hubSpot.GetRecentContacts(apiKey, DateTime.Now.AddHours(-1));            
        }

        
        [TestMethod]
        public async Task UpdateContactTest()
        {
            var apiKey = "b6485fd4-41b3-406e-b925-a5a99309b68b";
            //var email = ""


            var request = new HubSpotUpdateContactModel();

            var contactEmail = new HubSpotPropertyUpdateModel()
            {
                Property = "email",
                Value = "mike@test.com"
            };

            var firstName = new HubSpotPropertyUpdateModel()
            {
                Property = "firstname",
                Value = "Mikey"
            };

            var lastName = new HubSpotPropertyUpdateModel()
            {
                Property = "lastname",
                Value = "Test"
            };

            //request.Properties.Add(contactEmail);
            request.Properties.Add(firstName);
            request.Properties.Add(lastName);
            request.Email = "mike@test.com";

            await _hubSpot.UpdateContact(apiKey, request);
        }
    }
}
