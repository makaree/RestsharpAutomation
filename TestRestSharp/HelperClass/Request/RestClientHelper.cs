using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestRestSharp.HelperClass.Request
{
    public class RestClientHelper
    {
        private IRestClient GetRestClient()
        {
            IRestClient restClient = new RestClient();
            return restClient;
        }

        private IRestRequest GetRestRequest(string url, Dictionary<string,string> header, Method method, object body, DataFormat dataformat)
        {
            IRestRequest restRequest = new RestRequest()
            {
                Method = method,
            Resource = url
            };

            if (header != null)
            {
                foreach (string key in header.Keys)
                {
                    restRequest.AddHeader(key, header[key]);
                }
            }
            if(body!=null)
            {
                restRequest.RequestFormat = dataformat;
                switch(dataformat)
                {
                    case DataFormat.Json:
                        restRequest.AddBody(body);
                        break;
                    case DataFormat.Xml:
                        restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
                        restRequest.AddParameter("xmlbody", body.GetType().Equals(typeof(string))? body:restRequest.XmlSerializer.Serialize(body), ParameterType.RequestBody);
                        break;
                }
                
            }
        return restRequest;
        }

        private IRestResponse SendRequest(IRestRequest restRequest)
        {
            IRestClient restClient = GetRestClient();
            IRestResponse restResponse = restClient.Execute(restRequest);
            return restResponse;
        }

        private IRestResponse<T> SendRequest<T>(IRestRequest restRequest) where T: new()
        {
            IRestClient restClient = GetRestClient();
            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);
            if(restResponse.ContentType.Equals("application/xml"))
            {
                var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
                restResponse.Data = deserializer.Deserialize<T>(restResponse);
            }
            return restResponse;
        }

        public IRestResponse PerformGetRequest(string url, Dictionary<string, string> header)
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.GET,null,DataFormat.None);
            IRestResponse restresponse = SendRequest(restrequest);
            return restresponse;
        }

        public IRestResponse<T> PerformGetRequest<T>(string url, Dictionary<string, string> header) where T: new()
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.GET, null, DataFormat.None);
            IRestResponse<T> restresponse = SendRequest<T>(restrequest);
            return restresponse;
        }

        public IRestResponse<T> PerformPostRequest<T>(string url, Dictionary<string, string> header,object body,DataFormat dataformat) where T : new()
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.POST, body, dataformat);
            IRestResponse<T> restresponse = SendRequest<T>(restrequest);
            return restresponse;
        }

        public IRestResponse PerformPostRequest(string url, Dictionary<string, string> header, object body, DataFormat dataformat)
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.POST, body, dataformat);
            IRestResponse restresponse = SendRequest(restrequest);
            return restresponse;
        }

        public IRestResponse<T> PerformPutRequest<T>(string url, Dictionary<string, string> header, object body, DataFormat dataformat) where T : new()
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.PUT, body, dataformat);
            IRestResponse<T> restresponse = SendRequest<T>(restrequest);
            return restresponse;
        }

        public IRestResponse PerformPutRequest(string url, Dictionary<string, string> header, object body, DataFormat dataformat)
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.PUT, body, dataformat);
            IRestResponse restresponse = SendRequest(restrequest);
            return restresponse;
        }

        public IRestResponse PerformDeleteRequest(string url, Dictionary<string, string> header)
        {
            IRestRequest restrequest = GetRestRequest(url, header, Method.DELETE, null, DataFormat.None); 
            IRestResponse restresponse = SendRequest(restrequest);
            return restresponse;
        }
    }
}
