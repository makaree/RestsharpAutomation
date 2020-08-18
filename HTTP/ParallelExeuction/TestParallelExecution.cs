using HTTP.Helper.Request;
using HTTP.Helper.Response;
using HTTP.Model;
using HTTP.Model.JSONmodel;
using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.ParallelExeuction
{
    [TestClass]
    public class TestParallelExecution
    {
        private string delayurl = "http://localhost:8091/laptop-bag/webapi/delay/all";
        private string getdelayurl = "http://localhost:8091/laptop-bag/webapi/delay/find/";
        private string postdelayurl = "http://localhost:8091/laptop-bag/webapi/delay/add";
        private string deldelayurl = "http://localhost:8091/laptop-bag/webapi/delay/delete/";
        private string putdelayurl = "http://localhost:8091/laptop-bag/webapi/delay/update";
        private int id = 3;
        private string xmldataformat = "application/xml";
        private RestResponse restResponse;
        private HttpClientAsyncHelper httpClientAsyncHelper = new HttpClientAsyncHelper();

        private void sendgetrequest()
        {
            Dictionary<string, string> httpheader = new Dictionary<string, string>();
            httpheader.Add("Accept", "application/json");
            restResponse = httpClientAsyncHelper.PerformGetRequest(delayurl, httpheader).GetAwaiter().GetResult();

            //List<JsonRootObject> jsonroot = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
            //Console.WriteLine(jsonroot[0].BrandName.ToString());

            List<JsonRootObject> jsondata = ResponseDataHelper.DeserializeJSONResponse<List<JsonRootObject>>(restResponse.ResponseContent);
            Console.WriteLine(jsondata[1].ToString());
        }

        private void sendpostrequest()
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
            restResponse = httpClientAsyncHelper.PerformPostRequest(postdelayurl, contentxml, xmldataformat, header).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            Console.WriteLine(xmlresponsedata.ToString());
        }

        private void sendputrequest()
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
            restResponse = httpClientAsyncHelper.PerformPostRequest(postdelayurl, contentxml, xmldataformat, header).GetAwaiter().GetResult();
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

            restResponse = httpClientAsyncHelper.PerformPutRequest(putdelayurl, contentxml, xmldataformat, header).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);


            restResponse = httpClientAsyncHelper.PerformGetRequest(getdelayurl + id, header).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);
            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            //Console.WriteLine(xmlresponsedata.Features.Feature.ToString());
            Assert.IsTrue(xmlresponsedata.Features.Feature.Contains("1  TB is added"), "Failed to add data");

        }
        [TestMethod]
        public void testtask()
        {
            Task get = new Task(() =>
           {
               sendgetrequest();
           });
            get.Start();

            Task post = new Task(() =>
            {
                sendpostrequest();
            });
            post.Start();

            Task put = new Task(() =>
            {
                sendputrequest();
            });
            put.Start();

            get.Wait();
            put.Wait();
            post.Wait();
            //


        }

        [TestMethod]
        public void testwithtaskfactory()
        {
            var gettask = Task.Factory.StartNew(() =>
            {
                sendgetrequest();
            });


            var posttask = Task.Factory.StartNew(() =>
            {
                sendpostrequest();
            });


            var puttask = Task.Factory.StartNew(() =>
            {
                sendputrequest();
            });


            gettask.Wait();
            puttask.Wait();
            posttask.Wait();
        }

        [TestMethod]
        public void testwithtaskfactorywithrestresponse()
        {
            Task<RestResponse> gettask = Task.Factory.StartNew<RestResponse>(() =>
            {
                return sendgetrequestwithRestResponse();
            });


            Task<RestResponse> posttask = Task.Factory.StartNew<RestResponse>(() =>
            {
                return sendpostrequestwithRestresponse();
            });


            //gettask.Wait();
            //posttask.Wait();
            Console.WriteLine(gettask.Result.ResponseContent);
            Console.WriteLine(posttask.Result.ResponseContent);

        }




        private RestResponse sendgetrequestwithRestResponse()
        {
            Dictionary<string, string> httpheader = new Dictionary<string, string>();
            httpheader.Add("Accept", "application/json");
            restResponse = httpClientAsyncHelper.PerformGetRequest(delayurl, httpheader).GetAwaiter().GetResult();

            //List<JsonRootObject> jsonroot = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
            //Console.WriteLine(jsonroot[0].BrandName.ToString());

            //List<JsonRootObject> jsondata = ResponseDataHelper.DeserializeJSONResponse<List<JsonRootObject>>(restResponse.ResponseContent);
            //Console.WriteLine(jsondata[1].ToString());
            return restResponse;
        }

        private RestResponse sendpostrequestwithRestresponse()
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
            restResponse = httpClientAsyncHelper.PerformPostRequest(postdelayurl, contentxml, xmldataformat, header).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            //Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            //Console.WriteLine(xmlresponsedata.ToString());
            return restResponse;
        }

    }
}
