using RestSharp;
using System;

namespace VerboseRestSharp.Exceptions
{
    public class RequestFailedException : VerboseRestSharpException
    {
        public IRestRequest Request { get; }
        public IRestResponse Response { get; }

        public RequestFailedException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base(message, innerException)
        {
            Request = request;
            Response = response;
        }        
    }
}
