using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Unity;

namespace BaseApiApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            Bootstrapper.Initialise();
            //using (var container = new UnityContainer())
            //{
            //    //foreach (var task in container.ResolveAll<IRunAtInit>())
            //    //{
            //    //    task.Execute();
            //    //}



            //    foreach (var task in container.ResolveAll<IRunAtStartup>())
            //    {
            //        task.Execute();
            //    }
            //}
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
