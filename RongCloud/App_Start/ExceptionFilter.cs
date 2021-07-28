using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            Common.LogHelper.WriteLog(this.GetType().ToString(), ex, Common.LogPath.Logs_RongIM);
            var code = new HttpException(null, ex.InnerException).GetHttpCode();
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        code,
                        msg = ex.Message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
                filterContext.Result = new RedirectResult("~/Views/Shared/Error.html");
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}