using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class RedirectionException : WrongHttpStatusCodeException
    {
        public RedirectionException(string message, IRestRequest request, IRestResponse response)
            : base(message, request, response)
        {
        }
    }    
}