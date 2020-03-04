using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class InternalServerErrorException : WrongHttpStatusCodeException
    {
        public InternalServerErrorException(string message, IRestRequest request, IRestResponse response)
            : base($"The distant server had an internal issue while handling the request. Can you contact the server administrator? {message}", request, response)
        {
        }
    }
    
}