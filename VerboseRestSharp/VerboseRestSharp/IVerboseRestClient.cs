using RestSharp;

namespace VerboseRestSharp
{
    public interface IVerboseRestClient
    {
        /// <summary>
        /// Executes the given request and returns the rest response. Any error will throw an exception containing as much information as possible.
        /// </summary>
        IRestResponse ExecuteAndGetRestResponse(IRestRequest request);
    }
}
