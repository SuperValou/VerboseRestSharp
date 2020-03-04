using RestSharp;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace VerboseRestSharpTests.BadClients
{
    internal class BadHttpStatusCodeClient : FakeClient
    {
        private readonly HttpStatusCode _codeToReturn;

        public BadHttpStatusCodeClient(HttpStatusCode codeToReturn)
        {
            _codeToReturn = codeToReturn;
        }

        public override Task<IRestResponse> ExecuteAsync(IRestRequest request, Method httpMethod, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                IRestResponse response = new RestResponse();
                response.StatusCode = _codeToReturn;
                response.ResponseStatus = ResponseStatus.Completed;
                return response;
            });
        }

        public override Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                IRestResponse<T> response = new RestResponse<T>();
                response.StatusCode = _codeToReturn;
                response.ResponseStatus = ResponseStatus.Completed;
                return response;
            });
        }
    }
}
