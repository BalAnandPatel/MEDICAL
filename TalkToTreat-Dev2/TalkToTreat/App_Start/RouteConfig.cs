using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TalkToTreat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(name: "Hospitals", url: "Hospitalslist/{country}/{city}", defaults: new { controller = "Hospitals", action = "Index", country = UrlParameter.Optional, city = UrlParameter.Optional });

            routes.MapRoute(name: "Doctors", url: "Doctorslist/{country}/{city}", defaults: new { controller = "Doctors", action = "Index", country = UrlParameter.Optional, city = UrlParameter.Optional });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
