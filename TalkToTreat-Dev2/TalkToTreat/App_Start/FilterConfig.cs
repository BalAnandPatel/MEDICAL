using MvcCustomExceptionFilter.CustomFilter;
using System.Web;
using System.Web.Mvc;

namespace TalkToTreat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExceptionHandlerFilter());

        }       
    }
}
