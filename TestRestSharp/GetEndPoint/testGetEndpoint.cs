using HTTP.Model.JSONmodel;
using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestRestSharp.HelperClass.Request;

namespace TestRestSharp.GetEndPoint
{
    [TestClass]
    public class testGetEndpoint
    {
        private string url = "http://localhost:8091/laptop-bag/webapi/api/all";
        private string secureurl = "http://localhost:8091/laptop-bag/webapi/secure/all";
        [TestMethod]
        public void testgetusingrestsharp()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(url);
            IRestResponse restResponse = restClient.Get(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.StatusCode);
                Console.WriteLine(restResponse.Content);
            }
        }

        [TestMethod]
        public void testgetinxml()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(url);
            restRequest.AddHeader("Accept", "Application/xml");
            IRestResponse restResponse = restClient.Get(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.StatusCode);
                Console.WriteLine(restResponse.Content);
            }
        }

        [TestMethod]
        public void testgetinJson()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(url);
            restRequest.AddHeader("Accept", "Application/json");
            IRestResponse restResponse = restClient.Get(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.StatusCode);
                Console.WriteLine(restResponse.Content);
            }
        }

        [TestMethod]
        public void testgetinJson_deseraialize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(url);
            restRequest.AddHeader("Accept", "application/json");
            //IRestResponse restResponse = restClient.Get(restRequest);
            //IRestResponse<List<JsonRootObject>> restResponse = restClient.Get<List<JsonRootObject>>(restRequest);
            IRestResponse<List<JsonRootObject>> restResponse = restClient.Get<List<JsonRootObject>>(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                Console.WriteLine(restResponse.Content);
                Console.WriteLine(restResponse.Data.Count);
                List<JsonRootObject> jsondata = restResponse.Data;

                JsonRootObject data = jsondata.Find((x) => {
                    return x.Id ==3;
                });
                Assert.IsTrue(data.Features.Feature.Contains("1  TB is added"),"Such entry does not exists");

            }
            else
            {
                Console.WriteLine(restResponse.IsSuccessful);
                Console.WriteLine(restResponse.ErrorMessage);
                Console.WriteLine(restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void testgetinxml_deseraialize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(url);
            restRequest.AddHeader("Accept", "application/xml");
            //IRestResponse restResponse = restClient.Get(restRequest);
            //IRestResponse<List<JsonRootObject>> restResponse = restClient.Get<List<JsonRootObject>>(restRequest);
            //IRestResponse<List<JsonRootObject>> restResponse = restClient.Get<List<JsonRootObject>>(restRequest);
            var dotnetxmldeserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                Console.WriteLine(restResponse.Content);

                LaptopDetailss xmldata = dotnetxmldeserializer.Deserialize<LaptopDetailss>(restResponse);
                Console.WriteLine("no of data" + xmldata.Laptop.Count);

                Laptop laptop = xmldata.Laptop.Find((x) =>
                {
                    return x.Id.Equals("3",StringComparison.OrdinalIgnoreCase);
                });

                //List<JsonRootObject> jsondata = restResponse.Data;

                Assert.AreEqual(laptop.LaptopName, "Alienware M16");
                Assert.IsTrue(laptop.Features.Feature.Contains("1  TB is added"), "Such entry does not exists");

            }
            else
            {
                Console.WriteLine(restResponse.IsSuccessful);
                Console.WriteLine(restResponse.ErrorMessage);
                Console.WriteLine(restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void testgetwithexecute()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Method = Method.GET,
            Resource = url

            };
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse<List<Laptop>> restResponse = restClient.Execute<List<Laptop>>(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine(restResponse.StatusCode);
                Console.WriteLine(restResponse.Content);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                Assert.IsNotNull(restResponse.Data, "Is not null");
            }
            else
            {
                Console.WriteLine(restResponse.IsSuccessful);
                Console.WriteLine(restResponse.ErrorMessage);
                Console.WriteLine(restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void TestGetWithXMLUsingHelperClass()
        {
            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Accept","application/xml"  }
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(url, header);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "It is not null/empty");

            IRestResponse<LaptopDetailss> restResponse1 = restClientHelper.PerformGetRequest<LaptopDetailss>(url, header);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Content, "It is not null/empty");

        }

        [TestMethod]
        public void TestGetWithJsonUsingHelperClass()
        {
            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Accept","application/json"  }
            };
            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restResponse = restClientHelper.PerformGetRequest(url, header);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content, "It is not null/empty");

            IRestResponse<List<Laptop>> restResponse1 = restClientHelper.PerformGetRequest<List<Laptop>>(url, header);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Assert.IsNotNull(restResponse1.Content, "It is not null/empty");

        }
        [TestMethod]
        public void TestGetWithsecureurl()
        {
            IRestClient restClient = new RestClient();
            restClient.Authenticator = new HttpBasicAuthenticator("admin", "welcome");
            IRestRequest request = new RestRequest()
            {
                Resource = secureurl
            };
            IRestResponse restResponse= restClient.Get(request);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

    }
}
