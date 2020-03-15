using System;
using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class BadRequestException : WrongHttpStatusCodeException
    {
        public BadRequestException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base($"The request is malformed. Did you forget some headers? {message}", request, response, innerException)
        {
        }
    }
    
}