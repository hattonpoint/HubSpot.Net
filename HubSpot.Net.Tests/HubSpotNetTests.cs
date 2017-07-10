using HubSpot.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            var apiKey = "91f0e333-203c-45d1-b2ff-2666d046b981";

            var properties = new List<string>();
            properties.Add("firstname");
            properties.Add("lastname");
            properties.Add("email");

            var contacts = await _hubSpot.GetRecentContacts(apiKey, DateTime.Now.AddHours(-6), properties);            
        }

        
        [TestMethod]
        public async Task BatchUpdateContacts_NewContact_ContactInserted()
        {
            //this takes a little setup against the api
            //using the testing framework for verification
            //assumption is the hubspot account contacts is empty

            //arrange
            var apiKey = "54a308cc-4048-403c-81d8-a3110bff4743";                                                

            var request = new List<HubSpotUpdateContactModel>();
            var contact1 = new HubSpotUpdateContactModel();
            contact1.Email = "test@test.com";
                 
            var firstName = new HubSpotPropertyUpdateModel()
            {
                Property = "firstname",
                Value = "Billy"
            };

            var lastName = new HubSpotPropertyUpdateModel()
            {
                Property = "lastname",
                Value = "Test"
            };

            var email = new HubSpotPropertyUpdateModel()
            {
                Property = "email",
                Value = "test@test.com"
            };

            contact1.Properties.Add(firstName);
            contact1.Properties.Add(lastName);
            contact1.Properties.Add(email);

            request.Add(contact1);            

            //act
            await _hubSpot.BatchUpdateContactsByEmail(apiKey, request);            

            //assert
        }

        [TestMethod]
        public async Task BatchUpdateContacts_UpdateContact()
        {
            //assumption here is current hubspot account has no contacts in it

            //arrange
            var apiKey = "54a308cc-4048-403c-81d8-a3110bff4743";
            var contact1Email = "mike@test.com";

            ////create the contact
            var contacts = new List<HubSpotUpdateContactModel>();
            var contact1 = new HubSpotUpdateContactModel();
            contact1.Email = contact1Email;
            var phone = new HubSpotPropertyUpdateModel()
            {
                Property = "phone",
                Value = "303-555-7777"
            };            

            contact1.Properties.Add(phone);            
            contacts.Add(contact1);

            ////act
            await _hubSpot.BatchUpdateContactsByEmail(apiKey, contacts);

            ////change property on contact and update via batch update



            ////_hubSpot.BatchUpdateContactsByEmail()                                                
            ////act
            //await _hubSpot.BatchUpdateContactsByEmail(apiKey, request);
            //var updatedContacts = await _hubSpot.GetContact(apiKey, contact1Email);

            //assert

        }
    }
}
