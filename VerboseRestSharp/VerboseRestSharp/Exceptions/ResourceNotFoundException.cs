using System;
using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class ResourceNotFoundException : WrongHttpStatusCodeException
    {
        public ResourceNotFoundException(string message, IRestRequest request, IRestResponse response, Exception innerException)
            : base($"The resource was not found. Did you make a spelling mistake? {message}", request, response, innerException)
        {
        }
    }
}