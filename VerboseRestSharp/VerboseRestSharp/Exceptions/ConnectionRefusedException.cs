using RestSharp;
using System;

namespace VerboseRestSharp.Exceptions
{
    public class ConnectionRefusedException : RequestFailedException
    {
        public ConnectionRefusedException(IRestRequest request, IRestResponse response, Exception innerException) 
            : base("The connection to the target server was refused: it looks like the service you're trying to access is not running, or you don't have the rights to access it.",
                  request, response, innerException)
        {
        }
    }
}