using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace VerboseRestSharp
{
    public interface IVerboseRestClient
    {
        Task<TObject> ExecuteAndGetAsync<TObject>(IRestRequest request, CancellationToken token = default, bool throwIfNull = true);

        /// <summary>
        /// Executes the given request and returns the rest response. Any error will throw an exception containing as much information as possible.
        /// </summary>
        Task<IRestResponse> ExecuteAndGetRestResponseAsync(IRestRequest request, CancellationToken token = default);

        /// <summary>
        /// Executes the given request and returns the response as a string. Any error will throw an exception containing as much information as possible.
        /// </summary>
        Task<string> ExecuteAndGetStringAsync(IRestRequest request, CancellationToken token = default, bool throwIfNull = true);

        /// <summary>
        /// Executes the given request and returns the bytes of the response. Any error will throw an exception containing as much information as possible.
        /// </summary>
        Task<byte[]> ExecuteAndGetRawBytesAsync(IRestRequest request, CancellationToken token = default, bool throwIfNull = true);
    }
}
