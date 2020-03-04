using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace VerboseRestSharp
{
    public interface IVerboseRestClient
    {
        /// <summary>
        /// Executes the given request and returns the rest response. Any error will throw an exception containing as much information as possible.
        /// </summary>
        Task<IRestResponse> ExecuteAndGetRestResponseAsync(IRestRequest request, CancellationToken token = default);
    }
}
