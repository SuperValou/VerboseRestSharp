using RestSharp;
using System;

namespace VerboseRestSharp.Exceptions
{
    public class RestRequestFailedException : VerboseRestSharpException
    {
        public IRestRequest Request { get; }
        public IRestResponse Response { get; }

        public RestRequestFailedException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base(message, innerException)
        {
            Request = request;
            Response = response;
        }        
    }
}
