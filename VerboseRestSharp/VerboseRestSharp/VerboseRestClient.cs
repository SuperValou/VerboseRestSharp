using RestSharp;
using System;
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

        public IRestResponse ExecuteAndGetRestResponse(IRestRequest restRequest)
        {
            if (restRequest == null)
            {
                throw new ArgumentNullException(nameof(restRequest));
            }

            IRestResponse restResponse = null;
            try
            {
                restResponse = _restClient.Execute(restRequest);
            }
            catch (Exception e)
            {
                string errorMessage = BuildDetailedErrorMessage(restRequest, restResponse);
                throw new RestRequestFailedException($"Exception occured while executing Rest request. {errorMessage}", restRequest, restResponse, e);
            }

            throw new NotImplementedException();
        }

        private string BuildDetailedErrorMessage(IRestRequest restRequest, IRestResponse restResponse)
        {
            throw new NotImplementedException();
        }
    }
}
