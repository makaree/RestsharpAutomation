using HTTP.Model.JSONmodel;
using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRestSharp.HelperClass.Request;

namespace TestRestSharp.PutEndpoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string posturl = "http://localhost:8091/laptop-bag/webapi/api/add";
        private string geturl = "http://localhost:8091/laptop-bag/webapi/api/find/";
        private string puturl = "http://localhost:8091/laptop-bag/webapi/api/update";
        private int id = 11;
        [TestMethod]
        public void Testputendpointwithjson()
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
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {"Content-Type", "application/json" },
                {"Accept", "application/json" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restresponse = restClientHelper.PerformPostRequest(posturl, header, jsondata, DataFormat.Json);
            Assert.AreEqual(200, (int)restresponse.StatusCode);


            jsondata = "{" +
        "\"BrandName\": \"Alienware\"," +
        "\"Features\": {" +
                "\"Feature\": [" +
                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                "\"Windows 10 Home 64-bit English\"," +
                "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                 "\"New feature added\"" +
            "]" +
        "}," +
        "\"Id\": " + id + "," +
        "\"LaptopName\": \"Alienware M16\"" +
    "}";
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = puturl
            };
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(jsondata);
            IRestResponse<JsonRootObject> restResponse = restClient.Put<JsonRootObject>(restRequest);
            Assert.IsTrue(restResponse.Data.Features.Feature.Contains("New feature added"), "Data is not present");

            header = new Dictionary<string, string>
            {
                {"Accept","application/json"  }
            };
            restClientHelper = new RestClientHelper();
            restResponse = restClientHelper.PerformGetRequest<JsonRootObject>(geturl+id, header);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsTrue(restResponse.Data.Features.Feature.Contains("New feature added"), "Data is not present");

        }

        [TestMethod]
        public void Testputendpointwithxml()
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
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {"Content-Type", "application/xml" },
                {"Accept", "application/xml" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restresponse = restClientHelper.PerformPostRequest(posturl, header, contentxml, DataFormat.Xml);
            Assert.AreEqual(200, (int)restresponse.StatusCode);

            contentxml = "<Laptop>" +
      "<BrandName>Alienware</BrandName>" +
      "<Features>" +
         "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
          "<Feature>Windows 10 Home 64-bit English</Feature>" +
          "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
          "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
          "<Feature>New Feature</Feature>" +
      "</Features>" +
      "<Id>" + id.ToString() + "</Id>" +
      "<LaptopName>Alienware M16</LaptopName>" +
  "</Laptop>";
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = puturl
            };
            restRequest.AddHeader("Content-Type", "application/xml");
            restRequest.AddHeader("Accept", "application/xml");
            restRequest.RequestFormat = DataFormat.Xml;
            restRequest.AddParameter("xmlbody", contentxml, ParameterType.RequestBody);
            IRestResponse restResponse1 = restClient.Put(restRequest);
            var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            var laptop = deserializer.Deserialize<Laptop>(restResponse1);
            Assert.IsTrue(laptop.Features.Feature.Contains("New Feature"), "Data is not updated");

            header = new Dictionary<string, string>
            {
                {"Accept","application/xml"  }
            };
            restClientHelper = new RestClientHelper();
            IRestResponse<Laptop> restResponse2 = restClientHelper.PerformGetRequest<Laptop>(geturl + id, header);
            Assert.AreEqual(200, (int)restResponse2.StatusCode);
            Assert.IsTrue(restResponse2.Data.Features.Feature.Contains("New Feature"), "Data is not present");



        }

        [TestMethod]
        public void Testputendpointwithxml_helperclass()
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
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {"Content-Type", "application/xml" },
                {"Accept", "application/xml" }
            };

            RestClientHelper restClientHelper = new RestClientHelper();
            IRestResponse restresponse = restClientHelper.PerformPostRequest(posturl, header, contentxml, DataFormat.Xml);
            Assert.AreEqual(200, (int)restresponse.StatusCode);

            contentxml = "<Laptop>" +
      "<BrandName>Alienware</BrandName>" +
      "<Features>" +
         "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
          "<Feature>Windows 10 Home 64-bit English</Feature>" +
          "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
          "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
          "<Feature>New Feature edited</Feature>" +
      "</Features>" +
      "<Id>" + id.ToString() + "</Id>" +
      "<LaptopName>Alienware M16</LaptopName>" +
  "</Laptop>";
            //restClientHelper = new RestClientHelper();
            var restrespones3 = restClientHelper.PerformPutRequest<Laptop>(puturl, header,contentxml,DataFormat.Xml);
            

            //Assert.IsTrue(restrespones3.Data.Features.Feature.Contains("New Feature edited"), "Data is not updated");

            header = new Dictionary<string, string>
            {
                {"Accept","application/xml"  }
            };
            //restClientHelper = new RestClientHelper();
            var restResponse2 = restClientHelper.PerformGetRequest<Laptop>(geturl + id, header);
            Assert.AreEqual(200, (int)restResponse2.StatusCode);
            Assert.IsTrue(restResponse2.Data.Features.Feature.Contains("New Feature edited"), "Data is not present");

        }
    }

    }
    

