using System.Web;
using System.Web.Mvc;

namespace RongCloud
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilter());
        }
    }
}
