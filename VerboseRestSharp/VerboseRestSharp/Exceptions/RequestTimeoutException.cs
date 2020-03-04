using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class RequestTimeoutException : WrongHttpStatusCodeException
    {
        public RequestTimeoutException(string message, IRestRequest request, IRestResponse response)
            : base($"The request timed out. {message}", request, response)
        {
        }
    }
}