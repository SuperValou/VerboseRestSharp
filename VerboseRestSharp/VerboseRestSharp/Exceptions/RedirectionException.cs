using System;
using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class RedirectionException : WrongHttpStatusCodeException
    {
        public RedirectionException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base(message, request, response, innerException)
        {
        }
    }    
}