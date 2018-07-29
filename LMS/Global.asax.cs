using AutoMapper;
using LMS.Models;
using LMS.ViewModels.Activity;
using LMS.ViewModels.Module;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Activity, ActivityEditViewModel>();
                cfg.CreateMap<Activity, ActivityCreateViewModel>();
                cfg.CreateMap<Module, ModuleCreateViewModel>();
                cfg.CreateMap<Module, ModuleEditViewModel>();
            });
        }
    }
}
