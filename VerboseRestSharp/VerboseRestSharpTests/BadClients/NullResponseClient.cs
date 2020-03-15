using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace VerboseRestSharpTests.BadClients
{
    internal class NullResponseClient : FakeClient
    {
        public bool ReturnNullResponse { get; set; } = false;

        public override Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                if (ReturnNullResponse)
                {
                    return null;
                }

                IRestResponse<T> response = new RestResponse<T>();
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseStatus = ResponseStatus.Completed;
                response.Content = null;
                response.Data = (T) ((object) null);
                
                return response;
            });
        }

        public override Task<IRestResponse> ExecuteAsync(IRestRequest request, Method httpMethod, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                if (ReturnNullResponse)
                {
                    return null;
                }

                IRestResponse response = new NullContentRestResponse();
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseStatus = ResponseStatus.Completed;
                response.Content = null;

                return response;
            });
        }

        // RestResponse class of RestSharp actually never has a null value for the Content property
        private class NullContentRestResponse : IRestResponse
        {
            public IRestRequest Request { get; set; }
            public string ContentType { get; set; }
            public long ContentLength { get; set; }
            public string ContentEncoding { get; set; }
            public string Content { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public bool IsSuccessful { get; }
            public string StatusDescription { get; set; }
            public byte[] RawBytes { get; set; }
            public Uri ResponseUri { get; set; }
            public string Server { get; set; }

            [Obsolete]
            public IList<RestResponseCookie> Cookies { get; }
            public IList<Parameter> Headers { get; }
            public ResponseStatus ResponseStatus { get; set; }
            public string ErrorMessage { get; set; }
            public Exception ErrorException { get; set; }
            public Version ProtocolVersion { get; set; }
        }
    }

    
}