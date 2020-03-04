using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerboseRestSharp.Exceptions;

namespace VerboseRestSharp
{
    public class VerboseRestClient : IVerboseRestClient
    {
        private readonly IRestClient _restClient;

        public VerboseRestClient() : this(new RestClient())
        {
        }

        public VerboseRestClient(string url) : this (new RestClient(url))
        {            
        }

        public VerboseRestClient(IRestClient restClient)
        {
            _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
        }

        public async Task<IRestResponse> ExecuteAndGetRestResponseAsync(IRestRequest request, CancellationToken token = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // execute request
            IRestResponse response = null;
            try
            {
                response = await _restClient.ExecuteAsync(request, request.Method, token);
            }
            catch (Exception e)
            {
                string errorMessage = BuildDetailedErrorMessage(request, response);
                throw new RequestFailedException($"Exception occured during request execution. Details: {errorMessage}", request, response, e);
            }

            // validate response
            Validate(request, response);

            return response;
        }

        public async Task<TObject> ExecuteAndGetAsync<TObject>(IRestRequest request, CancellationToken token = default, bool throwIfNull = true)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // execute request
            IRestResponse<TObject> response = null;
            try
            {
                response = await _restClient.ExecuteAsync<TObject>(request);
            }
            catch (Exception e)
            {
                string errorMessage = BuildDetailedErrorMessage(request, response);
                throw new RequestFailedException($"Exception occured during request execution. Details: {errorMessage}", request, response, e);
            }

            // validate response
            Validate(request, response);

            if (response.Data == null && throwIfNull)
            {
                throw new NullResponseObjectException(typeof(TObject));
            }

            return response.Data;
        }

        public async Task<string> ExecuteAndGetStringAsync(IRestRequest request, CancellationToken token = default, bool throwIfNull = true)
        {
            IRestResponse response = await ExecuteAndGetRestResponseAsync(request, token);
            if (response.Content == null && throwIfNull)
            {
                throw new NullResponseObjectException(typeof(string));
            }

            return response.Content;
        }

        public async Task<byte[]> ExecuteAndGetRawBytesAsync(IRestRequest request, CancellationToken token = default, bool throwIfNull = true)
        {
            IRestResponse response = await ExecuteAndGetRestResponseAsync(request, token);
            if (response.RawBytes == null && throwIfNull)
            {
                throw new NullResponseObjectException(typeof(byte[])); ;
            }

            return response.RawBytes;
        }

        private void Validate(IRestRequest request, IRestResponse response)
        {
            // ensure response is fine
            if (response == null || response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response?.ErrorException?.InnerException is SocketException socketException
                    && socketException.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    throw new ConnectionRefusedException(request, response, socketException);
                }

                string errorMessage = BuildDetailedErrorMessage(request, response);
                throw new RequestFailedException($"Exception occured while getting response. Details: {errorMessage}", request, response, response?.ErrorException);
            }

            // ensure response code is fine
            if ((int)response.StatusCode >= 300)
            {
                string errorMessage = BuildDetailedErrorMessage(request, response);

                switch (response.StatusCode)
                {
                    // 3xx - Redirection
                    case HttpStatusCode.Ambiguous:
                    case HttpStatusCode.Moved:
                    case HttpStatusCode.Redirect:
                    case HttpStatusCode.RedirectMethod:
                    case HttpStatusCode.NotModified:
                    case HttpStatusCode.UseProxy:
                    case HttpStatusCode.Unused:
                    case HttpStatusCode.TemporaryRedirect:
                        throw new RedirectionException(errorMessage, request, response);


                    // 4xx - Client error
                    case HttpStatusCode.NotFound:
                        throw new ResourceNotFoundException(errorMessage, request, response);

                    case HttpStatusCode.RequestTimeout:
                        throw new RequestTimeoutException(errorMessage, request, response);

                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.PaymentRequired:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.MethodNotAllowed:
                    case HttpStatusCode.NotAcceptable:
                    case HttpStatusCode.ProxyAuthenticationRequired:

                    case HttpStatusCode.Conflict:
                    case HttpStatusCode.Gone:
                    case HttpStatusCode.LengthRequired:
                    case HttpStatusCode.PreconditionFailed:
                    case HttpStatusCode.RequestEntityTooLarge:
                    case HttpStatusCode.RequestUriTooLong:
                    case HttpStatusCode.UnsupportedMediaType:
                    case HttpStatusCode.RequestedRangeNotSatisfiable:
                    case HttpStatusCode.ExpectationFailed:
                    case HttpStatusCode.UpgradeRequired:
                        throw new BadRequestException(errorMessage, request, response);

                    // 5xx - Server error
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.NotImplemented:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                    case HttpStatusCode.HttpVersionNotSupported:
                        throw new InternalServerErrorException(errorMessage, request, response);
                }

                throw new RequestFailedException(errorMessage, request, response, response?.ErrorException);
            }
        }

        private string BuildDetailedErrorMessage(IRestRequest restRequest, IRestResponse restResponse)
        {
            var builder = new StringBuilder();

            // request full uri
            string fullUri;
            if (string.IsNullOrEmpty(_restClient.BaseUrl?.ToString()))
            {
                fullUri = restRequest.Resource;
            }
            else
            {
                try
                {
                    fullUri = _restClient.BuildUri(restRequest).ToString();
                }
                catch (Exception)
                {
                    fullUri = _restClient.BaseUrl.ToString() + restRequest.Resource;
                }                
            }

            builder.AppendLine($"Resource: {restRequest.Method.ToString()} {fullUri}");

            // request parameters
            ICollection<Parameter> parameters = restRequest.Parameters;
            if (parameters != null && parameters.Count > 0)
            {
                builder.AppendLine("Parameters:");
                
                foreach (var parameter in parameters.Where(p => p != null).OrderBy(p => p.Type))
                {
                    builder.AppendLine($"\t({parameter.Type}) {parameter.Name}: {parameter.Value}");
                }
            }

            // response
            if (restResponse != null)
            {
                builder.AppendLine($"Response code: {(int)restResponse.StatusCode} {restResponse.StatusCode.ToString()}");
                builder.AppendLine($"Response status: {restResponse.StatusDescription}");                
                builder.AppendLine($"Response error message: {restResponse.ErrorMessage}");
                builder.AppendLine($"Response exception: {restResponse.ErrorException}");
                builder.AppendLine($"Response content type: {restResponse.ContentType}");
                builder.AppendLine($"Response content: {restResponse.Content}");
            }

            return builder.ToString();
        }

    }
}
