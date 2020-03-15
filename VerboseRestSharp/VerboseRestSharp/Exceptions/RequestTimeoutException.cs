using System;
using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class RequestTimeoutException : WrongHttpStatusCodeException
    {
        public RequestTimeoutException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base($"The request timed out. {message}", request, response, innerException)
        {
        }
    }
}