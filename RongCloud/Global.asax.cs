using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RongCloud
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            Newtonsoft.Json.JsonConvert.DefaultSettings = new Func<Newtonsoft.Json.JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                ////空值处理
                //setting.NullValueHandling = NullValueHandling.Ignore;

                ////高级用法九中的Bool类型转换 设置
                //setting.Converters.Add(new BoolConvert("是,否"));

                return setting;
            });
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //var ex = Server.GetLastError();
            ////Log.Error(ex); //记录日志信息  
            //var httpStatusCode = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误  
            //var httpContext = ((MvcApplication)sender).Context;

            //httpContext.Response.StatusCode = httpStatusCode;
            //var shouldHandleException = true;
            //HandleErrorInfo errorModel;

            //var routeData = new RouteData();
            //routeData.Values["controller"] = "Home";

            //switch (httpStatusCode)
            //{
            //    case 404:
            //        routeData.Values["action"] = "NotFoundPage";
            //        errorModel = new HandleErrorInfo(new Exception(string.Format("No page Found", httpContext.Request.UrlReferrer), ex), "Home", "NotFoundPage");
            //        break;

            //    default:
            //        //routeData.Values["action"] = "Error";
            //        //Exception exceptionToReplace = null; //这里使用了EntLib的异常处理模块的一些功能  
            //        //shouldHandleException = ExceptionPolicy.HandleException(ex, "LogAndReplace", out exceptionToReplace);  
            //        errorModel = new HandleErrorInfo(new Exception(string.Format("No page Found", httpContext.Request.UrlReferrer), ex), "Home", "NotFoundPage");
            //        shouldHandleException = false;
            //        break;
            //}

            //if (shouldHandleException)
            //{
            //    httpContext.ClearError();
            //    httpContext.Response.Clear();
            //    var controller = new Controllers.HomeController();
            //    controller.ViewData.Model = errorModel; //通过代码路由到指定的路径  
            //    ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            //}
        }
    }
}
