using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 融云token帮助类
    /// </summary>
    public class RongTokenHelper
    {
        /// <summary>
        /// 融云token字典
        /// 暂不维护修改，只管新增
        /// </summary>
        public static Dictionary<string, RongTokenInfo> TokenDic { get; set; }
        private static readonly string filePath = AppDomain.CurrentDomain.BaseDirectory + "RongIMToken.txt";

        public RongTokenHelper()
        {
        }
        static RongTokenHelper()
        {
            ReadFile();
            LogHelper.WriteLog("RongTokenHelper", "静态构造函数", LogType.Info);
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetToken(string userId)
        {
            return GetUserInfo(userId).Token;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static RongTokenInfo GetUserInfo(string userId)
        {
            if (TokenDic.ContainsKey(userId))
            {
                return TokenDic[userId];
            }
            return new RongTokenInfo();
        }

        /// <summary>
        /// 获取所有人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<RongTokenInfo> GetAll()
        {
            return TokenDic.Values.ToList();
        }

        /// <summary>
        /// 加入token数据字典
        /// </summary>
        /// <param name="token"></param>
        public static void Add(RongTokenInfo token)
        {
            Dictionary_Add(token);
            WriteFile(JsonConvert.SerializeObject(token));
        }

        /// <summary>
        /// 加入token数据字典 暂时只保留最新的
        /// </summary>
        /// <param name="token"></param>
        private static void Dictionary_Add(RongTokenInfo token)
        {
            var userId = token.UserId;
            if (TokenDic.ContainsKey(userId))//暂时只保留最新的
            {
                TokenDic.Remove(userId);
                TokenDic.Add(userId, token);
            }
            else
                TokenDic.Add(userId, token);
        }

        /// <summary>
        /// 读取token信息文件存入静态字典中
        /// </summary>
        private static void ReadFile()
        {
            TokenDic = new Dictionary<string, RongTokenInfo>();
            using (var fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                var token = JsonConvert.DeserializeObject<RongTokenInfo>(line);
                                Dictionary_Add(token);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteLog("RongTokenHelper.ReadFile", ex, LogPath.Logs_RongIM);
                                continue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Token信息写入文件
        /// </summary>
        /// <param name="content">token信息</param>
        protected static void WriteFile(string content)
        {
            Task.Run(() =>
            {
                using (var sw = File.AppendText(filePath))
                {
                    sw.WriteLine(content);
                }
            });
        }
    }
    /// <summary>
    /// 融云token信息
    /// </summary>
    public class RongTokenInfo
    {
        public string UserId { get; set; }
        /// <summary>
        /// 融云Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string PortraitUri { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
