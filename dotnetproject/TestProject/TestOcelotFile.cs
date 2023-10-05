using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Ocelot.Tests
{
    [TestFixture]
    public class OcelotJsonTests
    {
        private JObject ocelotJson;

        [SetUp]
        public void Setup()
        {
            // Load the Ocelot.json file for testing (replace with your actual file path)
            string jsonFilePath = @"/home/coder/project/workspace/dotnetproject/dotnetapigateway/Ocelot.json";
            string jsonText = System.IO.File.ReadAllText(jsonFilePath);
            ocelotJson = JObject.Parse(jsonText);
        }

       [Test]
        public void VerifyProductRoute()
        {
            var ProductRoute = ocelotJson["Routes"][0];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Product"));
            Assert.That(ProductRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(ProductRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(ProductRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8080));
        }

        [Test]
        public void VerifyProductIDRoute()
        {
            var OrderRoute = ocelotJson["Routes"][1];

            Assert.That(OrderRoute, Is.Not.Null);
            Assert.That(OrderRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Product/{id}"));
            Assert.That(OrderRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(OrderRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(OrderRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8080));
        }
[Test]
        public void VerifyOrderRoute()
        {
            var OrderRoute = ocelotJson["Routes"][2];

            Assert.That(OrderRoute, Is.Not.Null);
            Assert.That(OrderRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Order"));
            Assert.That(OrderRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(OrderRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(OrderRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8081));
        }

        [Test]
        public void VerifyOrderIDRoute()
        {
            var OrderRoute = ocelotJson["Routes"][3];

            Assert.That(OrderRoute, Is.Not.Null);
            Assert.That(OrderRoute["DownstreamPathTemplate"].Value<string>(), Is.EqualTo("/api/Order/{id}"));
            Assert.That(OrderRoute["DownstreamScheme"].Value<string>(), Is.EqualTo("http"));
            Assert.That(OrderRoute["DownstreamHostAndPorts"][0]["Host"].Value<string>(), Is.EqualTo("localhost"));
            Assert.That(OrderRoute["DownstreamHostAndPorts"][0]["Port"].Value<int>(), Is.EqualTo(8081));
        }
        [Test]
        public void VerifyProductRouteUpstreamPath()
        {
            var ProductRoute = ocelotJson["Routes"][0];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Product"));
        }
        [Test]
        public void VerifyProductIDRouteUpstreamPath()
        {
            var ProductRoute = ocelotJson["Routes"][1];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Product/{id}"));
        }
        [Test]
        public void VerifyOrderRouteUpstreamPath()
        {
            var ProductRoute = ocelotJson["Routes"][2];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Order"));
        }
        [Test]
        public void VerifyOrderNamesRouteUpstreamPath()
        {
            var ProductRoute = ocelotJson["Routes"][3];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["UpstreamPathTemplate"].Value<string>(), Is.EqualTo("/gateway/Order/{id}"));
        }

        [Test]
        public void VerifyProductRouteHttpMethods()
        {
            var ProductRoute = ocelotJson["Routes"][0];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "POST", "GET" }));
        }
        [Test]
        public void VerifyProductIDRouteHttpMethods()
        {
            var ProductRoute = ocelotJson["Routes"][1];

            Assert.That(ProductRoute, Is.Not.Null);
            Assert.That(ProductRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "DELETE", "GET" }));
        }
        [Test]
        public void VerifyOrderRouteHttpMethods()
        {
            var OrderRoute = ocelotJson["Routes"][2];

            Assert.That(OrderRoute, Is.Not.Null);
            Assert.That(OrderRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "POST", "GET" }));
        }

        [Test]
        public void VerifyOrderDeleteIDRouteHttpMethods()
        {
            var OrderRoute = ocelotJson["Routes"][3];

            Assert.That(OrderRoute, Is.Not.Null);
            Assert.That(OrderRoute["UpstreamHttpMethod"].ToObject<string[]>(), Is.EquivalentTo(new[] { "DELETE" }));
        }
}
}