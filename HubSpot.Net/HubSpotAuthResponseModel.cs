using System.Net;
namespace HubSpot.Net
{
    public class HubSpotAuthResponseModel
    {
        public string Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
