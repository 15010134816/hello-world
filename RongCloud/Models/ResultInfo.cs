using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RongCloud.Models
{
    public class ResultInfo
    {

        public int result { get; set; }
        public string msg { get; set; }
        public object data { get; set; }

        public string url { get; set; }

        /// <summary>
        /// 返回是否成功的结果
        /// </summary>
        /// <returns></returns>
        public ResultInfo()
        {
            this.result = 0;
            this.msg = "成功";
        }
        /// <summary>
        /// 返回页面状态和提示信息
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public ResultInfo(int code, string msg)
        {
            this.result = code;
            this.msg = msg;
        }
        /// <summary>
        /// 返回页面状态和数据
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResultInfo(int code, object data, string url = "")
        {
            this.result = code;
            this.msg = "";
            this.data = data;
            this.url = url;
        }
        /// <summary>
        /// 返回页面成功数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResultInfo(object data)
        {
            this.result = 0;
            this.msg = "";
            this.data = data;
        }
        /// <summary>
        /// 返回页面成功消息和数据
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResultInfo(string msg, object data)
        {
            this.result = 0;
            this.msg = msg;
            this.data = data;
        }
        /// <summary>
        /// 返回页面状态，消息，数据
        /// </summary>
        /// <param name="code">状态</param>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResultInfo(int code, string msg, object data)
        {
            this.result = code;
            this.msg = msg;
            this.data = data;
        }
    }

    /// <summary>
    /// JSON返回结果
    /// </summary>
    public class JsonModel2
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 返回信息 如错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data2 { get; set; }
    }

    /// <summary>
    /// JSON返回结果
    /// </summary>
    public class JsonModel3
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 返回信息 如错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data2 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data3 { get; set; }
    }
    /// <summary>
    /// JSON返回结果
    /// </summary>
    public class JsonModel4
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 返回信息 如错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data2 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data3 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data4 { get; set; }
    }
    /// <summary>
    /// JSON返回结果
    /// </summary>
    public class JsonModel5
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 返回信息 如错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data2 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data3 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data4 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data5 { get; set; }
    }

    /// <summary>
    /// JSON返回结果
    /// </summary>
    public class JsonModel6
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 返回信息 如错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data2 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data3 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data4 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data5 { get; set; }
        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data6 { get; set; }
    }
}