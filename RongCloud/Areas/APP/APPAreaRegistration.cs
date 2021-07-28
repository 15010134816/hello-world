using System.Web.Mvc;

namespace RongCloud.Areas.APP
{
    public class APPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "APP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();
            context.MapRoute(
                "APP_default",
                "APP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "RongCloud.Areas.APP.Controllers" }
            );
        }
    }
}