using HTTP.Helper.Converter;
using HTTP.Helper.Request;
using HTTP.Helper.Response;
using HTTP.Model;
using HTTP.Model.JSONmodel;
using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.PutEndPoint
{
    [TestClass]
    public class testPutEndpoint
    {
        private string posturl = "http://localhost:8091/laptop-bag/webapi/api/add";
        private string jsondataformat = "application/json";
        private RestResponse restResponse;
        private int id = 8;
        private RestResponse GetResponse;
        private string geturl = "http://localhost:8091/laptop-bag/webapi/api/find/";
        private string xmldataformat = "application/xml";
        private string puturl = "http://localhost:8091/laptop-bag/webapi/api/update";
        private string secureputurl = "http://localhost:8091/laptop-bag/webapi/secure/update";
        private string securegeturl = "http://localhost:8091/laptop-bag/webapi/secure/find/";
        private string secureposturl = "http://localhost:8091/laptop-bag/webapi/secure/add";


        [TestMethod]
        public void testPutUsingxml()
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

            Dictionary<string, string> header = new Dictionary<string, string>{
                {"Accept","application/xml" }
            };
            restResponse = HttpClientHelper.PerformPostRequest(posturl, contentxml, xmldataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            contentxml = "<Laptop>" +
    "<BrandName>Alienware</BrandName>" +
    "<Features>" +
       "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
        "<Feature>Windows 10 Home 64-bit English</Feature>" +
        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
        "<Feature>1  TB is added</Feature>" +
    "</Features>" +
    "<Id>" + id.ToString() + "</Id>" +
    "<LaptopName>Alienware M16</LaptopName>" +
"</Laptop>";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(contentxml,Encoding.UTF8,xmldataformat);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(puturl,httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200,restResponse.StatusCode);
            }
            
            
            restResponse = HttpClientHelper.PerformGetRequest(geturl+id, header);
            Assert.AreEqual(200, restResponse.StatusCode);
            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            //Console.WriteLine(xmlresponsedata.Features.Feature.ToString());
            Assert.IsTrue(xmlresponsedata.Features.Feature.Contains("1  TB is added"), "Failed to add data");
            
        }

        [TestMethod]
        public void testPutUsingJSON()
        {
            string contentjson = "{" +
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

            Dictionary<string, string> header = new Dictionary<string, string>{
                {"Accept","application/json" }
            };
            restResponse = HttpClientHelper.PerformPostRequest(posturl, contentjson, jsondataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            contentjson = "{" +
        "\"BrandName\": \"Alienware\"," +
        "\"Features\": {" +
                "\"Feature\": [" +
                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                "\"Windows 10 Home 64-bit English\"," +
                "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                "\"1 TB SSD added\"" +
            "]" +
        "}," +
        "\"Id\": " + id + "," +
        "\"LaptopName\": \"Alienware M16\"" +
    "}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(contentjson, Encoding.UTF8, jsondataformat);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(puturl, httpContent);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200, restResponse.StatusCode);
            }


            restResponse = HttpClientHelper.PerformGetRequest(geturl + id, header);
            Assert.AreEqual(200, restResponse.StatusCode);
            //Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            ////Console.WriteLine(xmlresponsedata.Features.Feature.ToString());
            //Assert.IsTrue(xmlresponsedata.Features.Feature.Contains("1  TB is added"), "Failed to add data");

            JsonRootObject JSONresponsedata = ResponseDataHelper.DeserializeJSONResponse<JsonRootObject>(restResponse.ResponseContent);
            Console.WriteLine(JSONresponsedata.Features.Feature);
            Assert.IsTrue(JSONresponsedata.Features.Feature.Contains("1 TB SSD added"));
        }


        [TestMethod]
        public void testHTTPClientHelper_JSON()
        {
            string contentjson = "{" +
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

            Dictionary<string, string> header = new Dictionary<string, string>{
                {"Accept","application/json" }
            };
            restResponse = HttpClientHelper.PerformPostRequest(posturl, contentjson, jsondataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            contentjson = "{" +
        "\"BrandName\": \"Alienware\"," +
        "\"Features\": {" +
                "\"Feature\": [" +
                    "\"8th Generation Intel® Core™ i5-8300H\"," +
                "\"Windows 10 Home 64-bit English\"," +
                "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                "\"8GB, 2x4GB, DDR4, 2666MHz\"," +
                "\"1 TB SSD added\"" +
            "]" +
        "}," +
        "\"Id\": " + id + "," +
        "\"LaptopName\": \"Alienware M16\"" +
    "}";

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    HttpContent httpContent = new StringContent(contentjson, Encoding.UTF8, jsondataformat);
            //    Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(puturl, httpContent);
            //    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            //    Assert.AreEqual(200, restResponse.StatusCode);
            //}

            restResponse = HttpClientHelper.PerformPutRequest(puturl,contentjson,jsondataformat,header);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HttpClientHelper.PerformGetRequest(geturl + id, header);
            Assert.AreEqual(200, restResponse.StatusCode);
            //Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            ////Console.WriteLine(xmlresponsedata.Features.Feature.ToString());
            //Assert.IsTrue(xmlresponsedata.Features.Feature.Contains("1  TB is added"), "Failed to add data");

            JsonRootObject JSONresponsedata = ResponseDataHelper.DeserializeJSONResponse<JsonRootObject>(restResponse.ResponseContent);
            Console.WriteLine(JSONresponsedata.Features.Feature);
            Assert.IsTrue(JSONresponsedata.Features.Feature.Contains("1 TB SSD added"));
        }


        [TestMethod]
        public void testHTTPClientHelper_xml()
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

            Dictionary<string, string> header = new Dictionary<string, string>{
                {"Accept","application/xml" }
            };
            restResponse = HttpClientHelper.PerformPostRequest(posturl, contentxml, xmldataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            contentxml = "<Laptop>" +
    "<BrandName>Alienware</BrandName>" +
    "<Features>" +
       "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
        "<Feature>Windows 10 Home 64-bit English</Feature>" +
        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
        "<Feature>1  TB is added</Feature>" +
    "</Features>" +
    "<Id>" + id.ToString() + "</Id>" +
    "<LaptopName>Alienware M16</LaptopName>" +
"</Laptop>";

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    HttpContent httpContent = new StringContent(contentxml, Encoding.UTF8, xmldataformat);
            //    Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(puturl, httpContent);
            //    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            //    Assert.AreEqual(200, restResponse.StatusCode);
            //}

            restResponse = HttpClientHelper.PerformPutRequest(puturl, contentxml, xmldataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            restResponse = HttpClientHelper.PerformGetRequest(geturl + id, header);
            Assert.AreEqual(200, restResponse.StatusCode);
            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            //Console.WriteLine(xmlresponsedata.Features.Feature.ToString());
            Assert.IsTrue(xmlresponsedata.Features.Feature.Contains("1  TB is added"), "Failed to add data");
        }

        [TestMethod]
        public void testSecureHTTPClientHelper_xml()
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

            string authheader = Base64StringConverter.getBase64String("admin", "welcome");
            authheader = "Basic " + authheader;
            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Accept","application/xml" },
                {"Authorization",authheader }
            };
            restResponse = HttpClientHelper.PerformPostRequest(secureposturl, contentxml, xmldataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            contentxml = "<Laptop>" +
    "<BrandName>Alienware</BrandName>" +
    "<Features>" +
       "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>" +
        "<Feature>Windows 10 Home 64-bit English</Feature>" +
        "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>" +
        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
        "<Feature>1  TB is added</Feature>" +
    "</Features>" +
    "<Id>" + id.ToString() + "</Id>" +
    "<LaptopName>Alienware M16</LaptopName>" +
"</Laptop>";

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    HttpContent httpContent = new StringContent(contentxml, Encoding.UTF8, xmldataformat);
            //    Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(puturl, httpContent);
            //    restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            //    Assert.AreEqual(200, restResponse.StatusCode);
            //}

            restResponse = HttpClientHelper.PerformPutRequest(secureputurl, contentxml, xmldataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);


            restResponse = HttpClientHelper.PerformGetRequest(securegeturl + id, header);
            Assert.AreEqual(200, restResponse.StatusCode);
            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            //Console.WriteLine(xmlresponsedata.Features.Feature.ToString());
            Assert.IsTrue(xmlresponsedata.Features.Feature.Contains("1  TB is added"), "Failed to add data");
        }

    }
    }
