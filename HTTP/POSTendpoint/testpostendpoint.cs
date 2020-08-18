using HTTP.Helper.Converter;
using HTTP.Helper.Request;
using HTTP.Helper.Response;
using HTTP.Model;
using HTTP.Model.JSONmodel;
using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTP.POSTendpoint
{
    [TestClass]
    public class testpostendpoint
    {
        private string posturl = "http://localhost:8091/laptop-bag/webapi/api/add";
        private string jsondataformat = "application/json";
        private RestResponse restResponse;
        private int id = 3;
        private RestResponse GetResponse;
        private string geturl = "http://localhost:8091/laptop-bag/webapi/api/find/";
        private string xmldataformat = "application/xml";
        private string securegeturl = "http://localhost:8091/laptop-bag/webapi/secure/find/";
        private string secureposturl = "http://localhost:8091/laptop-bag/webapi/secure/add";

        [TestMethod]
        public void testpostasyncjson()
        {
            string content = "{" +
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
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, jsondataformat);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(posturl, httpContent);
                HttpStatusCode responseStatus = httpResponseMessage.Result.StatusCode;
                HttpContent responseContent = httpResponseMessage.Result.Content;
                string responsedata = responseContent.ReadAsStringAsync().Result;
                restResponse = new RestResponse((int)responseStatus, responsedata);

                Assert.AreEqual(200, restResponse.StatusCode, "Status code are not equal");
                Assert.IsNotNull(restResponse.ResponseContent);

                Task<HttpResponseMessage> httpgetMessage = httpClient.GetAsync(geturl + id);
                GetResponse = new RestResponse((int)httpgetMessage.Result.StatusCode, httpgetMessage.Result.Content.ReadAsStringAsync().Result);

                JsonRootObject jsonrootdeserelize = JsonConvert.DeserializeObject<JsonRootObject>(GetResponse.ResponseContent);
                Assert.AreEqual(id, jsonrootdeserelize.Id, "Id are not equal");
                Assert.AreEqual("Alienware M16", jsonrootdeserelize.LaptopName, "Laptop Name are not same");
            }
        }

        [TestMethod]
        public void testpostasyncxml()
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
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(contentxml, Encoding.UTF8, xmldataformat);
                Task<HttpResponseMessage> posthttpresponse = httpClient.PostAsync(posturl, httpContent);
                GetResponse = new RestResponse((int)posthttpresponse.Result.StatusCode, posthttpresponse.Result.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(200, GetResponse.StatusCode, "Status code are not equal");
                Assert.IsNotNull(GetResponse.ResponseContent);

                posthttpresponse = httpClient.GetAsync(geturl + id);
                if (!posthttpresponse.Result.IsSuccessStatusCode)
                {
                    Assert.Fail("The status code is not 200/successful");
                }

                GetResponse = new RestResponse((int)posthttpresponse.Result.StatusCode, posthttpresponse.Result.Content.ReadAsStringAsync().Result);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Laptop));
                TextReader textreader = new StringReader(GetResponse.ResponseContent);
                Laptop xmlobj = (Laptop)xmlSerializer.Deserialize(textreader);
                Assert.IsTrue(xmlobj.Features.Feature.Contains("8GB, 2x4GB, DDR4, 2666MHz"),"item not present in the list");

            }

        }

        [TestMethod]
        public void PostUsingHTTPClientHelper()
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
            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Accept","application/xml" }
            };
            restResponse =  HttpClientHelper.PerformPostRequest(posturl,contentxml,xmldataformat,header);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlresponsedata =ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            Console.WriteLine(xmlresponsedata.ToString());
        }

        [TestMethod]
        public void SecurePostUsingHTTPClientHelper()
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

            restResponse = HttpClientHelper.PerformGetRequest(securegeturl+id, header);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            Console.WriteLine(xmlresponsedata.ToString());
            Assert.AreEqual("Alienware", xmlresponsedata.BrandName);
        }
    }
}
