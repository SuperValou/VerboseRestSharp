using NUnit.Framework;
using RestSharp;
using System;
using VerboseRestSharp;

namespace VerboseRestSharpTests
{
    public class Tests
    {
        private const string TestURl = @"https://httpbin.org/get";

        private IVerboseRestClient _restClient;
                
        [SetUp]
        public void Setup()
        {
            var restClient = new RestClient(TestURl);
            _restClient = new VerboseRestClient(restClient);
        }

        [Test]
        public void Constructor_NullRestClient_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new VerboseRestClient((RestClient)null));
        }

        [Test]
        public void Constructor_NullRestClient_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new VerboseRestClient((RestClient)null));
        }
    }
}