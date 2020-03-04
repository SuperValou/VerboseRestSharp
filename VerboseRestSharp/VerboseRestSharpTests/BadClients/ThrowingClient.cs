using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VerboseRestSharpTests.BadClients
{
    internal class ThrowingClient : FakeClient
    {   
        public override IRestResponse Execute(IRestRequest request)
        {
            throw new Exception("Exception!");
        }

        public override Task<IRestResponse> ExecuteAsync(IRestRequest request, Method httpMethod, CancellationToken cancellationToken = default)
        {
            throw new Exception("Exception!");
        }
    }
}
