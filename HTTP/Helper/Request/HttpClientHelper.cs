using HTTP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Helper.Request
{
    public class HttpClientHelper
    {
        private static HttpClient httpClient;
        private static HttpRequestMessage httpRequestMessage;
        private static RestResponse restResponse;

        private static HttpClient AddHeadersAndCreateHttpClient(Dictionary<string,string> httpheader)
        {
            HttpClient httpClient = new HttpClient();
            if (null != httpheader)
            {
                foreach (string key in httpheader.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, httpheader[key]);
                }
            }
            return httpClient;
        }

        private static HttpRequestMessage createHttpRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);
            if(!(httpMethod==HttpMethod.Get)|| (httpMethod == HttpMethod.Delete))
            {
                httpRequestMessage.Content = httpContent;
            }
            return httpRequestMessage;
        }

        private static RestResponse sendRequest(string requestUrl, HttpMethod httpMethod, HttpContent httpContent, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            httpRequestMessage = createHttpRequestMessage(requestUrl, httpMethod, httpContent);
            try
            {
                Task<HttpResponseMessage> httpResponseMessage =httpClient.SendAsync(httpRequestMessage);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            }
            catch(Exception err)
            {
                restResponse = new RestResponse(500, err.Message);
                    }
            finally 
            {
                httpRequestMessage?.Dispose();
                httpClient?.Dispose();
            }
            return restResponse;
        }

        public static RestResponse PerformGetRequest(string requestUrl,  Dictionary<string, string> httpheader)
        {
            return sendRequest(requestUrl, HttpMethod.Get, null, httpheader);
        }

        public static RestResponse PerformPostRequest(string requestUrl,HttpContent httpContent, Dictionary<string, string> httpheader)
        {
            return sendRequest(requestUrl, HttpMethod.Post, httpContent, httpheader);
        }

        public static RestResponse PerformPostRequest(string requestUrl,string data, string mediaType, Dictionary<string, string> httpheader)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);
            return PerformPostRequest(requestUrl, httpContent, httpheader);
        }

        public static RestResponse PerformPutRequest(string requestUrl, string content, string mediaType, Dictionary<string, string> httpheader)
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            return sendRequest(requestUrl, HttpMethod.Put, httpContent, httpheader);
        }

        public static RestResponse PerformPutRequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpheader)
        {
            return sendRequest(requestUrl, HttpMethod.Put, httpContent, httpheader);
        }

        public static RestResponse PerformDeleteRequest(string requestUrl)
        {
            return sendRequest(requestUrl, HttpMethod.Delete, null, null);
        }

        public static RestResponse PerformDeleteRequest(string requestUrl, Dictionary<string, string> httpheader)
        {
            return sendRequest(requestUrl, HttpMethod.Delete, null, httpheader);
        }
    }
}
