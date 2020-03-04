using RestSharp;
using System;

namespace VerboseRestSharp.Exceptions
{
    public class WrongHttpStatusCodeException : RequestFailedException
    {
        public WrongHttpStatusCodeException(string message, IRestRequest request, IRestResponse response)
            : base(message, request, response, innerException: null)
        {
        }
    }
}