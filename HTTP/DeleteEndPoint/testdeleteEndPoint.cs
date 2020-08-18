using HTTP.Helper.Converter;
using HTTP.Helper.Request;
using HTTP.Helper.Response;
using HTTP.Model;
using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.DeleteEndPoint
{
    [TestClass]
    public class testdeleteEndPoint
    {
        private string posturl = "http://localhost:8091/laptop-bag/webapi/api/add";
        
        private RestResponse restResponse;
        private int id = 8;
       private string delurl = "http://localhost:8091/laptop-bag/webapi/api/delete/";
        private string securedelurl = "http://localhost:8091/laptop-bag/webapi/secure/delete/";
        private string xmldataformat = "application/xml";
        
        [TestMethod]
        public void TestDeleteEndpoint()
        {
            int idno = this.id;
            postdata(idno);
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> delresponse = httpClient.DeleteAsync(delurl + idno);
                Assert.AreEqual(200,(int)delresponse.Result.StatusCode,"The data is deleted");

                delresponse = httpClient.DeleteAsync(delurl + idno);
                Assert.AreEqual(404, (int)delresponse.Result.StatusCode, "The data is not deleted");
            }
        }

        public void postdata(int id)
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
            restResponse = HttpClientHelper.PerformPostRequest(posturl, contentxml, xmldataformat, header);
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlresponsedata = ResponseDataHelper.DeserializeXMLResponse<Laptop>(restResponse.ResponseContent);
            Console.WriteLine(xmlresponsedata.ToString());
        }


        [TestMethod]
        public void TestDeletehttpclienthelper()
        {
            int idno = this.id;
            postdata(idno);
            
                restResponse = HttpClientHelper.PerformDeleteRequest(delurl + idno);
                Assert.AreEqual(200, restResponse.StatusCode, "The data is deleted");

            restResponse = HttpClientHelper.PerformDeleteRequest(delurl + idno);
            Assert.AreEqual(404, restResponse.StatusCode, "The data is deleted");
        }

        [TestMethod]
        public void TestSecureDeletehttpclienthelper()
        {
            int idno = this.id;
            postdata(idno);
            string authheader = Base64StringConverter.getBase64String("admin", "welcome");
            authheader = "Basic " + authheader;
            Dictionary<string, string> header = new Dictionary<string, string>
            {
                {"Authorization",authheader }
            };
            restResponse = HttpClientHelper.PerformDeleteRequest(securedelurl + idno,header);
            Assert.AreEqual(200, restResponse.StatusCode, "The data is deleted");

            restResponse = HttpClientHelper.PerformDeleteRequest(securedelurl + idno,header);
            Assert.AreEqual(404, restResponse.StatusCode, "The data is deleted");
        }
    }
}
