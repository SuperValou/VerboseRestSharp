using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using VerboseRestSharp;
using VerboseRestSharp.Exceptions;
using VerboseRestSharpTests.BadClients;

namespace VerboseRestSharpTests
{
    public class VerboseRestClientTests
    {
        private const string TestURl = @"https://httpbin.org";
        private const string Get = "get";
         

        [Test]
        public void Constructor_NullRestClient_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new VerboseRestClient((RestClient)null));
        }

        [Test]
        public void ExecuteAndGetRestResponse_ValidRequest_ReturnsValidResponse()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            var request = new RestRequest(Get);
            var response = verboseRestClient.ExecuteAndGetRestResponse(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Content);
        }

        [Test]
        public void ExecuteAndGetRestResponse_NullRequest_ThrowsException()
        {
            var restClient = new RestClient(TestURl);
            var verboseRestClient = new VerboseRestClient(restClient);

            Assert.Throws<ArgumentNullException>(() => verboseRestClient.ExecuteAndGetRestResponse(null));
        }

        [TestCase(300)]
        [TestCase(301)]
        [TestCase(302)]
        [TestCase(303)]
        [TestCase(304)]
        [TestCase(305)]
        [TestCase(306)]
        [TestCase(307)]
        public void ExecuteAndGetRestResponse_300RedirectedResponse_ThrowsException(int httpCode)
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode)httpCode);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = new RestRequest(Get);

            Assert.Throws<RedirectionException>(() => verboseRestClient.ExecuteAndGetRestResponse(request));
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
        public void ExecuteAndGetRestResponse_400BadRequestResponse_ThrowsException(int httpCode)
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode)httpCode);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = new RestRequest(Get);

            Assert.Throws<BadRequestException>(() => verboseRestClient.ExecuteAndGetRestResponse(request));
        }

        [Test]
        public void ExecuteAndGetRestResponse_404NotFoundResponse_ThrowsException()
        {
            IRestClient badClient = new BadHttpStatusCodeClient(HttpStatusCode.NotFound);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = new RestRequest(Get);

            Assert.Throws<ResourceNotFoundException>(() => verboseRestClient.ExecuteAndGetRestResponse(request));
        }

        [Test]
        public void ExecuteAndGetRestResponse_408RequestTimeoutResponse_ThrowsException()
        {
            IRestClient badClient = new BadHttpStatusCodeClient(HttpStatusCode.RequestTimeout);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = new RestRequest(Get);

            Assert.Throws<RequestTimeoutException>(() => verboseRestClient.ExecuteAndGetRestResponse(request));
        }

        [TestCase(500)]
        [TestCase(501)]
        [TestCase(502)]
        [TestCase(503)]
        [TestCase(504)]
        [TestCase(505)]
        public void ExecuteAndGetRestResponse_500InternalServerResponse_ThrowsException(int httpCode)
        {
            IRestClient badClient = new BadHttpStatusCodeClient((HttpStatusCode)httpCode);
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = new RestRequest(Get);

            Assert.Throws<InternalServerErrorException>(() => verboseRestClient.ExecuteAndGetRestResponse(request));
        }

        [Test]
        public void ExecuteAndGetRestResponse_UnableToConnectToServer_ThrowsException()
        {
            IRestClient badClient = new ThrowingClient();
            var verboseRestClient = new VerboseRestClient(badClient);

            var request = new RestRequest(Get);

            Assert.Throws<RequestFailedException>(() => verboseRestClient.ExecuteAndGetRestResponse(request));
        }
    }
}