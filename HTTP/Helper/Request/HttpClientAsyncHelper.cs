using HTTP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Helper.Request
{
    public class HttpClientAsyncHelper
    {
        private  HttpClient httpClient;
        private  HttpRequestMessage httpRequestMessage;
        private  RestResponse restResponse;
        
        private  HttpClient AddHeadersAndCreateHttpClient(Dictionary<string, string> httpheader)
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

        //private  HttpRequestMessage createHttpRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        //{
        //    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);
        //    if (!(httpMethod == HttpMethod.Get) || (httpMethod == HttpMethod.Delete))
        //    {
        //        httpRequestMessage.Content = httpContent;
        //    }
        //    return httpRequestMessage;
        //}

        //private  RestResponse sendRequest(string requestUrl, HttpMethod httpMethod, HttpContent httpContent, Dictionary<string, string> httpheader)
        //{
        //    httpClient = AddHeadersAndCreateHttpClient(httpheader);
        //    httpRequestMessage = createHttpRequestMessage(requestUrl, httpMethod, httpContent);
        //    try
        //    {
        //        Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
        //        restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
        //    }
        //    catch (Exception err)
        //    {
        //        restResponse = new RestResponse(500, err.Message);
        //    }
        //    finally
        //    {
        //        httpRequestMessage?.Dispose();
        //        httpClient?.Dispose();
        //    }
        //    return restResponse;
        //}

        public async Task<RestResponse> PerformGetRequest(string requestUrl, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(requestUrl);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }

        public async Task<RestResponse> PerformPostRequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            HttpResponseMessage responseMessage = await httpClient.PostAsync(requestUrl,httpContent);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }

        public async Task<RestResponse> PerformPostRequest(string requestUrl, string data, string mediaType, Dictionary<string, string> httpheader)
        {
            
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediaType);
            HttpResponseMessage responseMessage = await httpClient.PostAsync(requestUrl, httpContent);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }

        public async Task<RestResponse> PerformPutRequest(string requestUrl, string content, string mediaType, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            HttpResponseMessage responseMessage = await httpClient.PutAsync(requestUrl, httpContent);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }

        public async Task<RestResponse> PerformPutRequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            HttpResponseMessage responseMessage = await httpClient.PutAsync(requestUrl, httpContent);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }

        public async Task<RestResponse> PerformDeleteRequest(string requestUrl)
        {
            httpClient = AddHeadersAndCreateHttpClient(null);
            HttpResponseMessage responseMessage = await httpClient.DeleteAsync(requestUrl);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }

        public async Task<RestResponse> PerformDeleteRequest(string requestUrl, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            HttpResponseMessage responseMessage = await httpClient.DeleteAsync(requestUrl);
            int statuscode = (int)responseMessage.StatusCode;
            var responsedata = await responseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statuscode, responsedata);
        }
    }
}
