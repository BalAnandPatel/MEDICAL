using System.Web;

namespace BaseApiApp.Framework
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial interface IWebUtils
    {  
        string GetCurrentIpAddress();
         
        string GetBrowserInfo(); 
    }
}
