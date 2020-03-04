using RestSharp;

namespace VerboseRestSharp.Exceptions
{
    public class ResourceNotFoundException : WrongHttpStatusCodeException
    {
        public ResourceNotFoundException(string message, IRestRequest request, IRestResponse response)
            : base($"The resource was not found. Did you make a spelling mistake? {message}", request, response)
        {
        }
    }
}