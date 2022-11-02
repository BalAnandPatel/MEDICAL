using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BaseApiApp.Models
{
    public class ResponseModel<T>
    {
        HttpStatusCode _statusCode = HttpStatusCode.OK;
        public HttpStatusCode StatusCode { get { return _statusCode; } set { _statusCode = value; } }

        public string Status { get { return _statusCode.ToString(); } }

        public string Error { get; set; }

        public string ErrorId { get; set; }

        public string Message { get; set; }

        public string MessageCode { get; set; }
        public string SessionToken { get; set; }

        public T Result { get; set; }
    }
}