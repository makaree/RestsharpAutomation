using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Model
{
    public class RestResponse
    {
        private int statusCode;
        private string responseData;
        public RestResponse(int statusCode, string responseData)
        {
            this.statusCode = statusCode;
            this.responseData = responseData;
        }

        public int StatusCode { get {return statusCode; } }
        public string ResponseContent { get { return responseData; } }
        public override string ToString()
        {
            return string.Format("Status Code {0}  Response Content {1}", statusCode,responseData);
        }
    }
}
