using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VerboseRestSharpTests.BadClients
{
    internal class ThrowingClient : FakeClient
    {   
        public override Task<IRestResponse> ExecuteAsync(IRestRequest request, Method httpMethod, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => (IRestResponse) Throw<object>());            
        }

        private IRestResponse<T> Throw<T>()
        {
            throw new Exception("Exception!");
        }

        public override Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.Run(Throw<T>);
        }
    }
}
