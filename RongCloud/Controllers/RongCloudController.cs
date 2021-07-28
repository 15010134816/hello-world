using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud.Controllers
{
    public class RongCloudController : Controller
    {
        public ActionResult Index(string userId = "", string name = "")
        {
            ViewBag.UserId = userId;
            ViewBag.Name = name;
            if (userId != "")
            {
                CookieHelper.SetCookies(CookieHelper.CookiesUserId, userId);
            }
            return View();
        }
        public ActionResult SetCookie(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Json(new { result = false, msg = "userId不能为空" });
            }
            CookieHelper.SetCookies(CookieHelper.CookiesUserId, userId);
            return Json(new { result = true });
        }
    }
}