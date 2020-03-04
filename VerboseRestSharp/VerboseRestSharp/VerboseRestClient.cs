using RestSharp;
using System;

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

        public IRestResponse ExecuteAndGetRestResponse(IRestRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            throw new NotImplementedException();
        }
    }
}
