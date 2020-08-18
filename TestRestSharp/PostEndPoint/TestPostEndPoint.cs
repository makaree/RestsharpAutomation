using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRestSharp.HelperClass.Request;

namespace TestRestSharp.PostEndPoint
{

    [TestClass]
    public class TestPostEndPoint
    {
        private string posturl = "http://localhost:8091/laptop-bag/webapi/api/add";
        private int id = 2;
        [TestMethod]
        public void testWithJsonData()
        {

            string jsondata = "{" +
        "\"BrandName\": \"Alienware\"," +
        "\"Features\": {" +
                "\"Feature\": [" +
                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                "\"Windows 10 Home 64-bit English\"," +
                "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
            "]" +
        "}," +
        "\"Id\": " + id + "," +
        "\"LaptopName\": \"Alienware M16\"" +
    "}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = posturl
            };
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.AddJsonBody(jsondata);
            IRestResponse restResponse = restClient.Post(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content);

        }

        private Laptop getlaptopobject()
        {
            Laptop laptop = new Laptop();
            laptop.BrandName = "Sample Brand Name";
            laptop.LaptopName = "Sample Laptop Name";
            Features features = new Features();
            List<string> featurelist = new List<string>()
            { "Sample Feature"};
            features.Feature = featurelist;
            laptop.Id = id.ToString();
            laptop.Features = features;
            return laptop;
        }

        [TestMethod]
        public void testPostwithModelObject()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = posturl
            };
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.AddJsonBody(getlaptopobject());
            //restRequest.AddObject(getlaptopobject());
            IRestResponse restResponse = restClient.Post(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content);
        }

        [TestMethod]
        public void testPostwithModelObject_helperclass()
        {
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {"Content-Type", "application/json" },
                {"Accept", "application/xml" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> restresponse = restClientHelper.PerformPostRequest<Laptop>(posturl, header, getlaptopobject(), DataFormat.Json);



            Assert.AreEqual(200, (int)restresponse.StatusCode);
            Assert.IsNotNull(restresponse.Data, "Rest respnse is null");
        }

        [TestMethod]
        public void testPostwithxmldata()
        {
            string contentxml = "<Laptop>" +
    "<BrandName>Alienware</BrandName>" +
    "<Features>" +
       "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
        "<Feature>Windows 10 Home 64-bit English</Feature>" +
        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
    "</Features>" +
    "<Id>" + id.ToString() + "</Id>" +
    "<LaptopName>Alienware M16</LaptopName>" +
"</Laptop>";
        
        IRestClient restClient = new RestClient();
        IRestRequest restRequest = new RestRequest()
        {
            Resource = posturl
        };
        restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.AddParameter("xmlbody",contentxml,ParameterType.RequestBody);
            IRestResponse<Laptop> restResponse = restClient.Post<Laptop>(restRequest);
        Assert.AreEqual(200, (int) restResponse.StatusCode);
        Assert.IsNotNull(restResponse.Content);
    }

        [TestMethod]
        public void testPostwith_complexpayload()
        {
            string contentxml = "<Laptop>" +
    "<BrandName>Alienware</BrandName>" +
    "<Features>" +
       "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
        "<Feature>Windows 10 Home 64-bit English</Feature>" +
        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
    "</Features>" +
    "<Id>" + id.ToString() + "</Id>" +
    "<LaptopName>Alienware M16</LaptopName>" +
"</Laptop>";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = posturl
            };
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.RequestFormat = DataFormat.Xml;
            restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            restRequest.AddParameter("xmlbody", restRequest.XmlSerializer.Serialize(getlaptopobject()), ParameterType.RequestBody);
            IRestResponse<Laptop> restResponse = restClient.Post<Laptop>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Content);
        }

        [TestMethod]
        public void testPostwithxml_helperclass()
        {
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {"Content-Type", "application/xml" },
                {"Accept", "application/xml" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> restresponse = restClientHelper.PerformPostRequest<Laptop>(posturl, header, getlaptopobject(), DataFormat.Xml);



            Assert.AreEqual(200, (int)restresponse.StatusCode);
            Assert.IsNotNull(restresponse.Data, "Rest respnse is null");
        }
    }
}
