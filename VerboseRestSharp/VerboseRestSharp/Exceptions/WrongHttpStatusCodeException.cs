using RestSharp;
using System;

namespace VerboseRestSharp.Exceptions
{
    public class WrongHttpStatusCodeException : RestRequestFailedException
    {
        public WrongHttpStatusCodeException(string message, IRestRequest request, IRestResponse response)
            : base(message, request, response, innerException: null)
        {
        }
    }
}