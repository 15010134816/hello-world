using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud
{
    /// <summary>
    /// 跨域过滤器
    /// </summary>
    public class CrossDomainFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "http://test.rongim.com");
            filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            //filterContext.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Token");
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}