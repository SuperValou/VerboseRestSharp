using System;
using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class NullResponseObjectException : RequestFailedException
    {
        public Type ExpectedType { get; }

        public NullResponseObjectException(Type expectedType, IRestRequest request, IRestResponse response, Exception innerException) 
            : base($"The expected '{expectedType?.Name}' object was null.", request, response, innerException)
        {
            ExpectedType = expectedType;
        }        
    }
}
