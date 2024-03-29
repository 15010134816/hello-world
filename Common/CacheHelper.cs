﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Common
{
    /// <summary>
    /// HttpRuntime Cache读取设置缓存信息帮助类
    /// 使用描述：给缓存赋值使用HttpRuntimeCache.Set(key,value....)等参数(第三个参数可以传递文件的路径(HttpContext.Current.Server.MapPath()))
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 设置缓存时间(秒)
        /// </summary>
        private const double Seconds = 30 * 24 * 60 * 60;

        /// <summary>
        /// 缓存指定对象，设置缓存
        /// 默认缓存一个月
        /// </summary>
        public static bool Set(string key, object value)
        {
            return Set(key, value, null, DateTime.Now.AddSeconds(Seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
        }
        /// <summary>
        /// 缓存指定对象，设置缓存
        /// </summary>
        public static bool Set(string key, object value, DateTime expireTime)
        {
            return Set(key, value, null, expireTime, Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
        }
        /// <summary>
        ///  缓存指定对象，设置缓存
        /// </summary>
        public static bool Set(string key, object value, string path)
        {
            try
            {
                var cacheDependency = new CacheDependency(path);
                return Set(key, value, cacheDependency);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 缓存指定对象，设置缓存
        /// </summary>
        public static bool Set(string key, object value, CacheDependency cacheDependency)
        {
            return Set(key, value, cacheDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
        }

        /// <summary>
        /// 缓存指定对象，设置缓存
        /// </summary>
        public static bool Set(string key, object value, double seconds, bool isAbsulute)
        {
            return Set(key, value, null, (isAbsulute ? DateTime.Now.AddSeconds(seconds) : Cache.NoAbsoluteExpiration),
                (isAbsulute ? Cache.NoSlidingExpiration : TimeSpan.FromSeconds(seconds)), CacheItemPriority.Default,
                null);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        public static object Get(string key)
        {
            return GetCacheValue(key);
        }

        /// <summary>
        /// 判断缓存中是否含有缓存该键
        /// </summary>
        public static bool Exists(string key)
        {
            return (GetCacheValue(key) != null);
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            HttpRuntime.Cache.Remove(key);
            return true;
        }

        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns></returns>
        public static bool RemoveAll()
        {
            IDictionaryEnumerator iDictionaryEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (iDictionaryEnumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove(Convert.ToString(iDictionaryEnumerator.Key));
            }
            return true;
        }

        //------------------------提供给上面方法进行调用-----------------------------------
        /// <summary>
        /// 设置缓存
        /// </summary>
        public static bool Set(string key, object value, CacheDependency cacheDependency, DateTime dateTime,
            TimeSpan timeSpan, CacheItemPriority cacheItemPriority, CacheItemRemovedCallback cacheItemRemovedCallback)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return false;
            }
            HttpRuntime.Cache.Insert(key, value, cacheDependency, dateTime, timeSpan, cacheItemPriority,
                cacheItemRemovedCallback);
            return true;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        private static object GetCacheValue(string key)
        {
            return string.IsNullOrEmpty(key) ? null : HttpRuntime.Cache.Get(key);
        }
    }
}
