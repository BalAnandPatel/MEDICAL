
using AutoMapper; 
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; 
using Unity;
using Unity.Injection;
using BaseApiApp.Framework;
using BaseApiApp.Framework.Utils;

namespace TalkToTreat
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            //HttpContext object
            container.RegisterType<HttpContextBase>(new InjectionFactory(_ => new HttpContextWrapper(HttpContext.Current)));

            //Request object
            container.RegisterType<HttpRequestBase>(new InjectionFactory(_ => new HttpRequestWrapper(HttpContext.Current.Request)));

            //Response object for injection
            container.RegisterType<HttpResponseBase>(new InjectionFactory(_ => new HttpResponseWrapper(HttpContext.Current.Response)));

            // register orgapi  services
            //container.RegisterTypes(AllClasses.FromLoadedAssemblies().Where(x => x.FullName.StartsWith("Omnicx.Backend.Framework") && !x.FullName.ToLower().Contains("session")), WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.PerThread);

            
            container.RegisterType<IWebUtils, WebUtils>();
            container.RegisterType<TalkToTreatService.Users.IUserService, TalkToTreatService.Users.UserService>();
            container.RegisterType<TalkToTreatService.Data.IDataContextFactory, TalkToTreatService.Data.DataContextFactory>();
            // container.RegisterType<CategoryController>(new InjectionConstructor());
            //container.RegisterTypes(AllClasses.FromLoadedAssemblies().Where(x => x.FullName.StartsWith("NIDMIMS")
            //        && !x.FullName.ToLower().Contains("cache")
            //        ),
            //        WithMappings.FromMatchingInterface, WithName.Default);

            RegisterMapping();
        }
        #region "Mapping"
        private static void RegisterMapping()
        {

            var types = Assembly.Load("TalkToTreat").GetExportedTypes();
            LoadStandardMappings(types);
            LoadCustomMappings(types);

            //types = Assembly.Load("Omnicx.Web.Framework").GetExportedTypes();
            //LoadStandardMappings(types);
            //LoadCustomMappings(types);

            types = Assembly.GetExecutingAssembly().GetExportedTypes();
            LoadStandardMappings(types);
            LoadCustomMappings(types);
        }
        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(TalkToTreatService.Data.IHaveCustomMappings).IsAssignableFrom(t) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select (TalkToTreatService.Data.IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();
            foreach (var map in maps)
            {
                //TODO: Review the new AutoMapper approach for this. 28-Aug-2018
                //map.CreateMappings(Mapper.Configuration);
            }
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(TalkToTreatService.Data.IMapFrom<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new { Source = i.GetGenericArguments()[0], Destination = t }).ToArray();

            foreach (var map in maps)
            {
                //TODO: Review the new AutoMapper approach for this. 28-Aug-2018
                //Mapper.CreateMap(map.Source, map.Destination);
                Mapper.Map(map.Source, map.Destination);
            }
        }
        #endregion
    }
}