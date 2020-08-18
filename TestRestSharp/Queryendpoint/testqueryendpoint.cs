using HTTP.Model.xmlmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRestSharp.Queryendpoint
{
    
    [TestClass]
    public class testqueryendpoint
    {

        private string searchurl = "http://localhost:8091/laptop-bag/webapi/api/query";

        [TestMethod]
        public void testquery()
        {
            IRestClient restClient = new RestClient();

            IRestRequest request = new RestRequest()
            {
                Resource =searchurl
            };
            request.AddHeader("Accept", "application/xml");
            request.AddQueryParameter("id", "5");
            request.AddQueryParameter("laptopName", "Alienware M16");
            var restResponse = restClient.Get<Laptop>(request);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.AreEqual("Alienware", restResponse.Data.BrandName);
        }
    }
}
