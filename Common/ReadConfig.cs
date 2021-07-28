using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ReadConfig
    {
        /// <summary>
        /// 读取cookiesname
        /// </summary>
        private static string _cookiesname;
        /// <summary>
        /// cookie
        /// </summary>
        public static string CookiesName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cookiesname))
                {
                    _cookiesname = ConfigurationManager.AppSettings["CookiesName"];
                }

                return _cookiesname;
            }
        }
        /// <summary>
        /// 融云AppKey
        /// </summary>
        private static string _AppKey_RongCloud;
        public static string AppKey_RongCloud
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AppKey_RongCloud))
                {
                    _AppKey_RongCloud = ConfigurationManager.AppSettings["AppKey_RongCloud"];
                }
                return _AppKey_RongCloud;
            }
        }

        private static string _AppSecret_RongCloud;
        /// <summary>
        /// 融云AppSecret
        /// </summary>
        public static string AppSecret_RongCloud
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AppSecret_RongCloud))
                {
                    _AppSecret_RongCloud = ConfigurationManager.AppSettings["AppSecret_RongCloud"];
                }
                return _AppSecret_RongCloud;
            }
        }
    }

}
