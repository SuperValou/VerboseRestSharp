using RestSharp;
using System;

namespace VerboseRestSharp.Exceptions
{
    public class WrongHttpStatusCodeException : RequestFailedException
    {
        public WrongHttpStatusCodeException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base(message, request, response, innerException)
        {
        }
    }
}