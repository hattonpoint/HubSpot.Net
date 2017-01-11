using System;
using System.Net;

namespace HubSpot.Net
{
    [Serializable]
    public class HubSpotException : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public HubSpotException()
        {
        }

        public HubSpotException(HttpStatusCode httpStatusCode, string message)
          : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}