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
            return Task.Run(() =>
            {
                throw new Exception("Exception!");

                // force lambda to expose its return type
#pragma warning disable CS0162 // Unreachable code detected
                IRestResponse response = null;
#pragma warning restore CS0162 // Unreachable code detected
                return response;
            });            
        }
    }
}
