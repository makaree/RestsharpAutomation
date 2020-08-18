using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestRestSharp.HelperClass.Request;

namespace TestRestSharp.DeleteEndPoint
{
    [TestClass]
    public class TestDeleteEndPoint
    {
        private string posturl = "http://localhost:8091/laptop-bag/webapi/api/add";
        private int id = 7;
        private string delurl = "http://localhost:8091/laptop-bag/webapi/api/delete/";

        [TestMethod]
        public void testdeleteendpoint()
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
            IRestResponse<Laptop> restresponse = restClientHelper.PerformPostRequest<Laptop>(posturl, header, jsondata, DataFormat.Json);
            Assert.AreEqual(200, (int)restresponse.StatusCode);

            IRestClient restClient = new RestClient();
            IRestRequest restrequest = new RestRequest()
            {
                Resource = delurl + id
            };
            restrequest.AddHeader("Accept", "*/*");
            IRestResponse restResponse1 = restClient.Delete(restrequest);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);

            restResponse1 = restClient.Delete(restrequest);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);
        }

        [TestMethod]
        public void testdeleteendpoint_helperclass()
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
            IRestResponse<Laptop> restresponse = restClientHelper.PerformPostRequest<Laptop>(posturl, header, jsondata, DataFormat.Json);
            Assert.AreEqual(200, (int)restresponse.StatusCode);

            header = new Dictionary<string, string>()
            {

                {"Accept", "*/*" }
            };
            IRestResponse restResponse1 = restClientHelper.PerformDeleteRequest(delurl + id, header);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);

            restResponse1 = restClientHelper.PerformDeleteRequest(delurl + id, header);
            Assert.AreEqual(404, (int)restResponse1.StatusCode);
        }

    }
}
