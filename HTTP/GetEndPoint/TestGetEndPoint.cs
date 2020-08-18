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
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTP.GetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string url = "http://localhost:8091/laptop-bag/webapi/api/all";
        private string secureurl = "http://localhost:8091/laptop-bag/webapi/secure/all";
        private string delayurl = "http://localhost:8091/laptop-bag/webapi/delay/all";
        [TestMethod]
        public void EndpointCheckwithUrl()
        {
            //create http client
            HttpClient httpClient = new HttpClient();

            //create http request and execute
            httpClient.GetAsync(url);

            // end http request
            httpClient.Dispose();
        }

        [TestMethod]
        public void CheckwithUri()
        {
            //create http client
            HttpClient httpClient = new HttpClient();

            //create http request and execute
            Uri geturi = new Uri(url);
            Task<HttpResponseMessage> httprespnsemessage = httpClient.GetAsync(geturi);
            HttpResponseMessage reponseResult = httprespnsemessage.Result;
            Console.WriteLine(reponseResult);


            //get response status code
            HttpStatusCode statuscode = reponseResult.StatusCode;
            Console.WriteLine("StatusCode: " + statuscode);
            Console.WriteLine("StatusCode: " + (int)statuscode);


            //get response data
            HttpContent responseContent = reponseResult.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);
            Assert.AreEqual((int)statuscode, 200, "Not equal");
            Assert.IsNotNull(data);

            //exit the client
            httpClient.Dispose();
        }


        [TestMethod]
        public void CheckwithIncorrectUri()
        {
            //create http client
            HttpClient httpClient = new HttpClient();

            //create http request and execute
            Uri geturi = new Uri(url);
            Task<HttpResponseMessage> httprespnsemessage = httpClient.GetAsync(geturi + "/app");
            HttpResponseMessage reponseResult = httprespnsemessage.Result;
            Console.WriteLine(reponseResult);


            //get response status code
            HttpStatusCode statuscode = reponseResult.StatusCode;
            Console.WriteLine("StatusCode: " + statuscode);
            Console.WriteLine("StatusCode: " + (int)statuscode);


            //get response data
            HttpContent responseContent = reponseResult.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //exit the client
            httpClient.Dispose();
        }

        [TestMethod]
        public void ResultInJson()
        {
            //create http client
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/json");


            //create http request and execute
            Uri geturi = new Uri(url);
            Task<HttpResponseMessage> httprespnsemessage = httpClient.GetAsync(geturi);
            HttpResponseMessage reponseResult = httprespnsemessage.Result;
            Console.WriteLine(reponseResult);


            //get response status code
            HttpStatusCode statuscode = reponseResult.StatusCode;
            Console.WriteLine("StatusCode: " + statuscode);
            Console.WriteLine("StatusCode: " + (int)statuscode);


            //get response data
            HttpContent responseContent = reponseResult.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //exit the client
            httpClient.Dispose();
        }

        [TestMethod]
        public void UsingSendAsync()
        {
            //Uri uri = new Uri(url);
            //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            //requestMessage.Headers.Add("Accept", "application/xml");

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(url);
            requestMessage.Method = HttpMethod.Get;
            requestMessage.Headers.Add("Accept", "application/json");

            //create http client
            HttpClient httpClient = new HttpClient();

            //HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            //requestHeaders.Add("Accept", "application/json");


            //create http request and execute
            //Uri geturi = new Uri(url);
            Task<HttpResponseMessage> httprespnsemessage = httpClient.SendAsync(requestMessage);
            HttpResponseMessage reponseResult = httprespnsemessage.Result;
            Console.WriteLine(reponseResult);


            //get response status code
            HttpStatusCode statuscode = reponseResult.StatusCode;
            Console.WriteLine("StatusCode: " + statuscode);
            Console.WriteLine("StatusCode: " + (int)statuscode);


            //get response data
            HttpContent responseContent = reponseResult.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //exit the client
            httpClient.Dispose();
        }

        [TestMethod]
        public void CheckUsingSendAsync()
        {
            using (HttpClient httpclient = new HttpClient())
            {
                using (HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    requestMessage.RequestUri = new Uri(url);
                    requestMessage.Method = HttpMethod.Get;
                    requestMessage.Headers.Add("Accept", "application/json");
                    Task<HttpResponseMessage> httprespnsemessage = httpclient.SendAsync(requestMessage);
                    using (HttpResponseMessage reponseResult = httprespnsemessage.Result)
                    {
                        Console.WriteLine(reponseResult);

                        //get response status code
                        HttpStatusCode statuscode = reponseResult.StatusCode;
                        Console.WriteLine("StatusCode: " + statuscode);
                        Console.WriteLine("StatusCode: " + (int)statuscode);

                        //get response data
                        HttpContent responseContent = reponseResult.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        Console.WriteLine(data);
                    }
                }
            }
        }
        [TestMethod]
        public void JsonDeserialize()
        {
            using (HttpClient httpclient = new HttpClient())
            {
                using (HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    requestMessage.RequestUri = new Uri(url);
                    requestMessage.Method = HttpMethod.Get;
                    requestMessage.Headers.Add("Accept", "application/json");
                    Task<HttpResponseMessage> httprespnsemessage = httpclient.SendAsync(requestMessage);
                    using (HttpResponseMessage reponseResult = httprespnsemessage.Result)
                    {
                        Console.WriteLine(reponseResult);

                        //get response status code
                        HttpStatusCode statuscode = reponseResult.StatusCode;
                        //Console.WriteLine("StatusCode: " + statuscode);
                        //Console.WriteLine("StatusCode: " + (int)statuscode);

                        //get response data
                        HttpContent responseContent = reponseResult.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        //Console.WriteLine(data);

                        RestResponse restResponse = new RestResponse((int)statuscode, data);
                        //Console.WriteLine(restResponse);
                        //JsonRootObject jsonrootobject = JsonConvert.DeserializeObject<JsonRootObject>(restResponse.ResponseContent);
                        List<JsonRootObject> jsonroot = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
                        Console.WriteLine(jsonroot[0].BrandName.ToString());

                    }
                }
            }
        }

        [TestMethod]
        public void xmldeserialize()
        {
            using (HttpClient httpclient = new HttpClient())
            {
                using (HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    requestMessage.RequestUri = new Uri(url);
                    requestMessage.Method = HttpMethod.Get;
                    requestMessage.Headers.Add("Accept", "application/xml");
                    Task<HttpResponseMessage> httprespnsemessage = httpclient.SendAsync(requestMessage);
                    using (HttpResponseMessage reponseResult = httprespnsemessage.Result)
                    {
                        Console.WriteLine(reponseResult);

                        //get response status code
                        HttpStatusCode statuscode = reponseResult.StatusCode;
                        Console.WriteLine("StatusCode: " + statuscode);
                        Console.WriteLine("StatusCode: " + (int)statuscode);

                        //get response data
                        HttpContent responseContent = reponseResult.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();
                        string data = responseData.Result;
                        //Console.WriteLine(data);
                        RestResponse restResponse = new RestResponse((int)statuscode, data);

                        //step1
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetailss));
                        //step 2
                        TextReader textreader = new StringReader(restResponse.ResponseContent);

                        //step3
                        LaptopDetailss xmldata = (LaptopDetailss)xmlSerializer.Deserialize(textreader);
                        Console.WriteLine(xmldata.Laptop.ToString());


                    }
                }
            }
        }

        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string, string> httpheader = new Dictionary<string, string>();
            httpheader.Add("Accept", "application/json");
            RestResponse restResponse = HttpClientHelper.PerformGetRequest(url, httpheader);

            //List<JsonRootObject> jsonroot = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
            //Console.WriteLine(jsonroot[0].BrandName.ToString());

            List<JsonRootObject> jsondata = ResponseDataHelper.DeserializeJSONResponse<List<JsonRootObject>>(restResponse.ResponseContent);
            Console.WriteLine(jsondata[1].ToString());
        }

        [TestMethod]
        public void TestSecureGetUsingHelperMethod()
        {
            Dictionary<string, string> httpheader = new Dictionary<string, string>();
            httpheader.Add("Accept", "application/json");
            //httpheader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==");
            string authheader = Base64StringConverter.getBase64String("admin", "welcome");
            authheader = "Basic " + authheader;
            httpheader.Add("Authorization", authheader);

            RestResponse restResponse = HttpClientHelper.PerformGetRequest(secureurl, httpheader);
            Assert.AreEqual(200, restResponse.StatusCode, "Status code is not connected");
            //List<JsonRootObject> jsonroot = JsonConvert.DeserializeObject<List<JsonRootObject>>(restResponse.ResponseContent);
            //Console.WriteLine(jsonroot[0].BrandName.ToString());

            List<JsonRootObject> jsondata = ResponseDataHelper.DeserializeJSONResponse<List<JsonRootObject>>(restResponse.ResponseContent);
            Console.WriteLine(jsondata[1].ToString());
        }

        [TestMethod]
        public void TestDelayGetMethod_sync()
        {
            HttpClientHelper.PerformGetRequest("http://localhost:8091/laptop-bag/webapi/delay/all", null);

            HttpClientHelper.PerformGetRequest("http://localhost:8091/laptop-bag/webapi/delay/all", null);
            HttpClientHelper.PerformGetRequest("http://localhost:8091/laptop-bag/webapi/delay/all", null);
            HttpClientHelper.PerformGetRequest("http://localhost:8091/laptop-bag/webapi/delay/all", null);

        }

        [TestMethod]
        public void getendpoint_async()
        {
            Task t1 = new Task(getendpoint());
            t1.Start();

            Task t2 = new Task(getendpoint());
            t2.Start();

            Task t3 = new Task(getendpoint());
            t3.Start();

            Task t4 = new Task(getendpoint());
            t4.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();
        }

        private Action  getendpoint()
        {
            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Accept","application/xml" }
            };
            return new Action(() =>
            {
                RestResponse restResponse = HttpClientHelper.PerformGetRequest(delayurl, header);
                Assert.AreEqual(200, restResponse.StatusCode);
            });
        }
    }
}
