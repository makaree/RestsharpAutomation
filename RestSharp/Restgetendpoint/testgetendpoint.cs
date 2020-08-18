using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;


namespace RestSharpTest.Restgetendpoint
{
    [TestClass]
    public class testgetendpoint
    {
        private string url = "http://localhost:8091/laptop-bag/webapi/api/all";

        [TestMethod]
        public void testgetusingRestSharp()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(url);
            restClient.Get(restRequest);
        }
    }
}
