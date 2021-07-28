using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LogType
    {
        Success = 1,
        Info = 2,
        Error = 99
    }

    /// <summary>
    /// 日志路径
    /// </summary>
    public enum LogPath
    {
        /// <summary>
        /// 默认日志文件夹
        /// </summary>
        Logs = 1,
        /// <summary>
        /// 融云IM日志文件夹
        /// </summary>
        Logs_RongIM = 2
    }

    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 写入异常日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="objErr">Exception错误信息</param>
        /// <param name="logPath">日志路径</param>
        /// <param name="isAsync">是否异步写入 默认异步</param>
        public static void WriteLog(string className, Exception objErr, LogPath logPath = LogPath.Logs, bool isAsync = true)
        {
            DateTime currentTime = DateTime.Now;
            string msg = "发生时间：" + currentTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
            msg += "发生异常页：" + System.Web.HttpContext.Current.Request.Url.ToString() + "\r\n";
            msg += "异常信息：" + objErr.Message + objErr.InnerException?.Message + "\r\n";
            var param = System.Web.HttpContext.Current.Request.Form;
            var dic = new Dictionary<string, string>();
            foreach (var item in param)
            {
                dic.Add(item.ToString(), param[item.ToString()]);
            }
            if (dic.Count > 0)
            {
                msg += "请求参数：" + Newtonsoft.Json.JsonConvert.SerializeObject(dic) + "\r\n";
            }
            msg += "错误源：" + objErr.Source + "\r\n";
            msg += "堆栈信息：" + objErr.StackTrace + "\r\n";
            WriteLog(className, msg, LogType.Error, logPath, isAsync);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="content">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="logPath">日志路径</param>
        /// <param name="isAsync">是否异步写入 默认异步</param>
        public static void WriteLog(string className, string content, LogType logType, LogPath logPath = LogPath.Logs, bool isAsync = true)
        {
            var type = logType.ToString();
            var path = AppDomain.CurrentDomain.BaseDirectory + logPath.ToString() + "/" + type;
            if (isAsync)
            {
                WriteLogAsync(type, className, content, path);
            }
            else
            {
                WriteLog(type, className, content, path);
            }
        }

        /// <summary>
        /// 异步写入日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="className">类名</param>
        /// <param name="content">日志内容</param>
        /// <param name="path">保存日志的绝对路径</param>
        protected static void WriteLogAsync(string type, string className, string content, string path)
        {
            Task.Run(() =>
            {
                WriteLog(type, className, content, path);
            });
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="className">类名</param>
        /// <param name="content">日志内容</param>
        /// <param name="path">保存日志的绝对路径</param>
        protected static void WriteLog(string type, string className, string content, string path)
        {
            if (!Directory.Exists(path)) //如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); //获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"; //用日期对日志文件命名

            string filename2 = path + "/" + DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd") + ".log"; //获取三个月前的文件
            if (System.IO.File.Exists(filename2))
            {
                File.Delete(filename2); //删除三个月前的日志文件
            }

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);

            //向日志文件写入内容
            string write_content = time + " " + type + " " + className + ": " + content;
            mySw.WriteLine(write_content);
            mySw.WriteLine();

            //关闭日志文件
            mySw.Close();
        }
    }
}
