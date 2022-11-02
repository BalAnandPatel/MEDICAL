using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web; 

namespace BaseApiApp.Framework.Utils
{
    public class ApiWebUtils : IWebUtils
    {
       // private readonly HttpContextBase _httpContext;
        public ApiWebUtils()
        {
            
        }

        public string GetUrlReferrer()
        {
            return "";
        }

        public string GetCurrentIpAddress()
        {
        //////    #if DEBUG
        //       return "127.0.0.1";
        //    // //   #endif

#pragma warning disable CS0162 // Unreachable code detected
            var httpContext = HttpContext.Current;// DependencyResolver.Current.GetService<HttpContextBase>();
#pragma warning restore CS0162 // Unreachable code detected
            if (httpContext == null)
                return "127.0.0.1";
           
            // if httpContext is NULL or if httpContext.Request is null, then return Empty
           // if (httpContext.Request == null)
               // return string.Empty;

            var result = "";
            //if (httpContext.Request.Headers != null)
            //{
                //look for the X-Forwarded-For (XFF) HTTP header field
                //it's used for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer. 
                var xff = httpContext.Request.Headers.AllKeys
                    .Where(x => "X-FORWARDED-FOR".Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => httpContext.Request.Headers[k])
                    .FirstOrDefault();
                //if you want to exclude private IP addresses, then see http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc
                if (!string.IsNullOrEmpty(xff))
                {
                    var lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    result = lastIp;
                }
           // }

            if (string.IsNullOrEmpty(result) && httpContext.Request.UserHostAddress != null)
            {
                result = httpContext.Request.UserHostAddress;
            }

            //some validation
            if (result == "::1")
                result = "127.0.0.1";
            //remove port
            if (string.IsNullOrEmpty(result)) return result;
            var index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
            if (index > 0)
                result = result.Substring(0, index);
            return result;
            
        }

        public string GetThisPageUrl(bool includeQueryString)
        {
            return "";
        }

        public string GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            return "";
        }

        public bool IsCurrentConnectionSecured()
        {
            return false;
        }

        public string ServerVariables(string name)
        {
            return "";
        }

        public bool IsStaticResource(HttpRequest request)
        {
            return false;
        }

        public string MapPath(string path)
        {
            return "";
        }
 
        public bool IsSearchEngine(HttpContextBase context)
        {
            return false;
        }

        public string GetBrowserInfo()
        {
            return "";
        }
    }
}
