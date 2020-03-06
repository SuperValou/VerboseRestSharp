using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace VerboseRestSharpTests.BadClients
{
    internal class AccessDeniedClient : FakeClient
    {
        public override Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                IRestResponse<T> response = new RestResponse<T>();
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseStatus = ResponseStatus.Error;
                response.ErrorException = new Exception("Exception!", new SocketException(10013));
                return response;
            });
        }

        public override Task<IRestResponse> ExecuteAsync(IRestRequest request, Method httpMethod, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                IRestResponse response = new RestResponse();
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseStatus = ResponseStatus.Error;
                response.ErrorException = new Exception("Exception!", new SocketException(10013));
                return response;
            });
        }
    }
}