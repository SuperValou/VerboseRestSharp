using RestSharp;
using System;
using System.Net.Sockets;

namespace VerboseRestSharp.Exceptions
{
    public class SocketErrorException : RequestFailedException
    {
        public SocketError SocketError { get; }

        public SocketErrorException(SocketError socketError, IRestRequest request, IRestResponse response, Exception innerException) 
            : base($"The connection to the target server failed: '{socketError}'. It looks like the service you're trying to access is not running, or there is a network issue.",
                  request, response, innerException)
        {
            SocketError = socketError;
        }
    }
}