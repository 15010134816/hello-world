using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class CookieHelper
    {
        #region cookie名称
        public const string CookiesUserId = "UserId";
        /// <summary>
        /// 暂时只存放UserId的值
        /// </summary>
        public const string CookieAppAuth = "rong_im_auth";
        #endregion

        public static string UserId { get => GetCookies(CookiesUserId); }
        /// <summary>
        /// 暂时只存放UserId
        /// </summary>
        public static string AppAuth { get => GetCookies(CookieAppAuth); }

        #region 设置cookie信息
        /// <summary>
        /// 设置cookie信息
        /// </summary>
        /// <param name="cookName">cookie名称</param>
        /// <param name="value">cookie值</param>
        /// <param name="days">过期时间 默认7天</param>
        /// <param name="isJm">是否加密</param>
        public static void SetCookies(string cookName, string value, int hour = 168, bool isJm = true)
        {
            HttpCookie cokie = new HttpCookie(cookName)
            {
                Value = isJm ? SecureHelper.Encrypt(value) : value,
                Expires = DateTime.Now.AddHours(hour)
            };
            HttpContext.Current.Response.Cookies.Add(cokie);
        }
        #endregion

        #region 获取Cookie
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookName">cookie名称</param>
        /// <param name="isJm"></param>
        /// <returns></returns>
        public static string GetCookies(string cookName, bool isJm = true)
        {
            HttpCookie htpCokie = System.Web.HttpContext.Current.Request.Cookies[cookName];
            string rest = "";
            try
            {
                if (htpCokie != null)
                {
                    rest = isJm ? SecureHelper.Decrypt(htpCokie.Value) : htpCokie.Value;
                }
            }
            catch { rest = ""; }
            return rest;
        }
        #endregion

        #region 删除Cookie
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookName">Cookie名称</param>
        public static void DeleteCookies(string cookName)
        {
            TimeSpan ts = new TimeSpan(-7, 0, 0, 0);
            HttpCookie cokie = System.Web.HttpContext.Current.Request.Cookies[cookName];
            if (cokie != null)
            {
                cokie.Expires = DateTime.Now.Add(ts);
                System.Web.HttpContext.Current.Response.Cookies.Add(cokie);
            }
        }
        #endregion

        #region 获得指定名称的Cookie中特定键的值
        /// <summary>
        /// 获得指定名称的Cookie中特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetCookie(string name, string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null && cookie.HasKeys)
            {
                string v = cookie[key];
                if (v != null)
                    return v;
            }
            return string.Empty;
        }
        #endregion

        #region 设置指定名称的Cookie特定键的值
        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间 分钟 0表示不设置过期时间</param>
        public static void SetCookie(string name, string key, string value, double expires = 0)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);
            cookie[key] = value;
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion
    }
}
