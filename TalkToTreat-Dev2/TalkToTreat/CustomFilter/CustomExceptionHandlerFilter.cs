using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalkToTreat.Models;
//using MvcCustomExceptionFilter.Models;

namespace MvcCustomExceptionFilter.CustomFilter
{
    public class CustomExceptionHandlerFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //ExceptionLogger logger = new ExceptionLogger()
            //{
            //    ExceptionMessage = filterContext.Exception.Message,
            //    ExceptionStackTrack = filterContext.Exception.StackTrace,
            //    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
            //    ActionName = filterContext.RouteData.Values["action"].ToString(),
            //    ExceptionLogTime = DateTime.Now
            //};
            //EmployeeContext dbContext = new EmployeeContext();
            //dbContext.ExceptionLoggers.Add(logger);
            //dbContext.SaveChanges();


            _Mysave _mysave = new _Mysave("ExceptionLoggingToDataBase");
            _mysave.AddParameter("@ExceptionMessage", filterContext.Exception.Message);
            _mysave.AddParameter("@ExceptionStackTrack", filterContext.Exception.StackTrace);
            _mysave.AddParameter("@ControllerName", filterContext.RouteData.Values["controller"].ToString());
            _mysave.AddParameter("@ActionName", filterContext.RouteData.Values["action"].ToString());
            _mysave.AddParameter("@ExceptionLogTime", DateTime.Now);

            _mysave.ExecuteSave();


            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"
            };
        }
    }
}