using RestSharp;
using System.Threading;
using System.Threading.Tasks;
using VerboseRestSharp.Exceptions;

namespace VerboseRestSharp
{
    public interface IVerboseRestClient
    {
        /// <summary>
        /// Executes the given request and returns the desired deserialized object. Any error will throw an exception containing as much information as possible.
        /// </summary>
        /// <typeparam name="TObject">Type of the object to deserialize from the response.</typeparam>
        /// <param name="request">Request to execute.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="throwIfNull">Makes the method throw a <see cref="NullDeserializedObjectException"/> if the desired deserialized object is actually null. True by default.</param>
        /// <returns>Deserialized object from the response content.</returns>
        Task<TObject> ExecuteAndGetAsync<TObject>(IRestRequest request, CancellationToken token = default, bool throwIfNull = true);

        /// <summary>
        /// Executes the given request and returns the rest response itself. Any error will throw an exception containing as much information as possible.
        /// </summary>
        /// <param name="request">Request to execute.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Response to the given request.</returns>
        Task<IRestResponse> ExecuteAndGetRestResponseAsync(IRestRequest request, CancellationToken token = default);

        /// <summary>
        /// Executes the given request and returns the response content as a string. Any error will throw an exception containing as much information as possible.
        /// </summary>
        /// <param name="request">Request to execute.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="throwIfNull">Makes the method throw a <see cref="NullDeserializedObjectException"/> if the desired deserialized object is actually null. True by default.</param>
        /// <returns>Content of the response as a string.</returns>
        Task<string> ExecuteAndGetStringAsync(IRestRequest request, CancellationToken token = default, bool throwIfNull = true);

        /// <summary>
        /// Executes the given request and returns the response content as a byte array. Any error will throw an exception containing as much information as possible.
        /// </summary>
        /// <param name="request">Request to execute.</param>
        /// <param name="token">Cancellation token.</param>
        /// <param name="throwIfNull">Makes the method throw a <see cref="NullDeserializedObjectException"/> if the desired deserialized object is actually null. True by default.</param>
        /// <returns>Content of the response as a byte array.</returns>
        Task<byte[]> ExecuteAndGetRawBytesAsync(IRestRequest request, CancellationToken token = default, bool throwIfNull = true);
    }
}
