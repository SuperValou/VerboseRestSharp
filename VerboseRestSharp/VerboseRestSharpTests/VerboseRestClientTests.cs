using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using VerboseRestSharp;
using VerboseRestSharp.Exceptions;
using VerboseRestSharpTests.BadClients;
using VerboseRestSharpTests.JsonResponses;

namespace VerboseRestSharpTests
{
    public class VerboseRestClientTests
    {
        private const string TestURl = @"https://httpbin.org";
        private const string Get = "get";

        private IRestRequest CreateRequest()
        {
            var request = new RestRequest(Get);
            request.AddParameter(new Parameter("some-param", 1, ParameterType.UrlSegment));
            return request;
        }

        // Constructor

        [Test]
        public void Constructor_Empty_DoesntThrows()
        {
            Assert.DoesNotThrow(() => new VerboseRestClient());
        }

        [Test]
        public void Constructor_Url_DoesntThrows()
        {
            Assert.DoesNotThrow(() => new VerboseRestClient(TestURl));
        }

        [Test]
        public void Constructor_RestClient_DoesntThrows()
        {
            Assert.DoesNotThrow(() => new VerboseRestClient(new RestClient()));
        }

        [Test]
        public void Constructor_NullRestClient_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new VerboseRestClient((RestClient)null));
        }


        // Get Rest response

        [Test]
        public async Task ExecuteAndGetRestResponseAsync_ValidRequest_ReturnsValidResponse()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            var request = CreateRequest();
            var response = await verboseRestClient.ExecuteAndGetRestResponseAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_InvalidRequest_ThrowsException()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            var request = new RestRequest(Get);
            request.Method = Method.POST;
            request.AddParameter(new Parameter("some-param", 1, ParameterType.UrlSegment));
            
            var exception = Assert.Throws<BadRequestException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            
            Console.WriteLine(exception);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_NullRequest_ThrowsException()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            Assert.Throws<ArgumentNullException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(null).GetAwaiter().GetResult());
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_EmptyBaseUrl_ThrowsException()
        {
            var restClient = new RestClient();
            var verboseRestClient = new VerboseRestClient(restClient);
            var request = CreateRequest();

            Assert.Throws<RequestFailedException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
        }

        [TestCase(300)]
        [TestCase(301)]
        [TestCase(302)]
        [TestCase(303)]
        [TestCase(304)]
        [TestCase(305)]
        [TestCase(306)]
        [TestCase(307)]
        public void ExecuteAndGetRestResponseAsync_300RedirectedResponse_ThrowsException(int httpCode)
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode)httpCode);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<RedirectionException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }

        [TestCase(400)]
        [TestCase(401)]
        [TestCase(402)]
        [TestCase(403)]
        [TestCase(405)]
        [TestCase(406)]
        [TestCase(407)]        
        [TestCase(409)]
        [TestCase(410)]
        [TestCase(411)]
        [TestCase(412)]
        [TestCase(413)]
        [TestCase(414)]
        [TestCase(415)]
        [TestCase(416)]
        [TestCase(417)]
        [TestCase(426)]
        public void ExecuteAndGetRestResponseAsync_400BadRequestResponse_ThrowsException(int httpCode)
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode)httpCode);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<BadRequestException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_404NotFoundResponse_ThrowsException()
        {
            IRestClient badClient = new BadHttpStatusCodeClient(HttpStatusCode.NotFound);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<ResourceNotFoundException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_408RequestTimeoutResponse_ThrowsException()
        {
            IRestClient badClient = new BadHttpStatusCodeClient(HttpStatusCode.RequestTimeout);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<RequestTimeoutException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }

        [TestCase(500)]
        [TestCase(501)]
        [TestCase(502)]
        [TestCase(503)]
        [TestCase(504)]
        [TestCase(505)]
        public void ExecuteAndGetRestResponseAsync_500InternalServerResponse_ThrowsException(int httpCode)
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode)httpCode);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<InternalServerErrorException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }
        
        [Test]
        public void ExecuteAndGetRestResponseAsync_UndefinedHttpStatusCodeResponse_ThrowsException()
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode) int.MaxValue);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<WrongHttpStatusCodeException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_RequestThrowingException_ThrowsException()
        {
            IRestClient badClient = new ThrowingClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<RequestFailedException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNull(exception.Response);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_NullResponse_ThrowsException()
        {
            IRestClient badClient = new NullResponseClient() { ReturnNullResponse = true };
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<RequestFailedException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNull(exception.Response);
        }

        [Test]
        public void ExecuteAndGetRestResponseAsync_AccessDenied_ThrowsException()
        {
            IRestClient badClient = new AccessDeniedClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<SocketErrorException>(() => verboseRestClient.ExecuteAndGetRestResponseAsync(request).GetAwaiter().GetResult());
            Assert.AreEqual(SocketError.AccessDenied, exception.SocketError);
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }


        // Get String response

        [Test]
        public async Task ExecuteAndGetStringAsync_ValidRequest_ReturnsValidStringResponse()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            var request = CreateRequest();
            string response = await verboseRestClient.ExecuteAndGetStringAsync(request);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(string.Empty, response);
        }

        [Test]
        public void ExecuteAndGetStringAsync_NullObjectInResponse_ThrowsException()
        {
            IRestClient badClient = new NullResponseClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<NullResponseObjectException>(() => verboseRestClient.ExecuteAndGetStringAsync(request).GetAwaiter().GetResult());
            Assert.AreEqual(typeof(string), exception.ExpectedType);
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }


        // Get byte[] response

        [Test]
        public async Task ExecuteAndGetRawBytesAsync_ValidRequest_ReturnsValidByteArray()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            var request = CreateRequest();
            byte[] response = await verboseRestClient.ExecuteAndGetRawBytesAsync(request);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, response.Length);
        }

        [Test]
        public void ExecuteAndGetRawBytesAsync_NullObjectInResponse_ThrowsException()
        {
            IRestClient badClient = new NullResponseClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<NullResponseObjectException>(() => verboseRestClient.ExecuteAndGetRawBytesAsync(request).GetAwaiter().GetResult());
            Assert.AreEqual(typeof(byte[]), exception.ExpectedType);
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }


        // Get T object response

        [Test]
        public async Task ExecuteAndGetAsync_ValidRequest_ReturnsValidObject()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            var request = CreateRequest();
            var response = await verboseRestClient.ExecuteAndGetAsync<ValidJsonResponse>(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Origin);
            Assert.IsNotNull(response.Url);
        }

        [Test]
        public void ExecuteAndGetAsync_NullRequest_ThrowsException()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            Assert.Throws<ArgumentNullException>(() => verboseRestClient.ExecuteAndGetAsync<ValidJsonResponse>(null).GetAwaiter().GetResult());
        }

        [Test]
        public void ExecuteAndGetAsync_RequestThrowingException_ThrowsException()
        {
            IRestClient badClient = new ThrowingClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<RequestFailedException>(() => verboseRestClient.ExecuteAndGetAsync<ValidJsonResponse>(request).GetAwaiter().GetResult());
            Assert.IsNotNull(exception.Request);
            Assert.IsNull(exception.Response);
        }

        [Test]
        public void ExecuteAndGetAsync_NullObjectInResponse_ThrowsException()
        {
            IRestClient badClient = new NullResponseClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = CreateRequest();

            var exception = Assert.Throws<NullResponseObjectException>(() => verboseRestClient.ExecuteAndGetAsync<ValidJsonResponse>(request).GetAwaiter().GetResult());
            Assert.AreEqual(typeof(ValidJsonResponse), exception.ExpectedType);
            Assert.IsNotNull(exception.Request);
            Assert.IsNotNull(exception.Response);
        }
    }
}