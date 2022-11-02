using System.Web.Mvc;
using BaseApiApp.Framework;
using BaseApiApp.Framework.Utils;
using Microsoft.Practices.Unity;
using Service.MasterData;
using TalkToTreatService.Users; 
using Unity.Mvc4;

namespace BaseApiApp
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IWebUtils, ApiWebUtils>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IMasterService, MasterService>();
            container.RegisterType<IHospitalService, HospitalService>();
            container.RegisterType<IDoctorsService, DoctorsService>();
        }
  }
}