using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileHelper
    {
        private static FileHelper _instance = null;
        private static readonly object lockobj = new object();
        private FileHelper()
        {

        }
        public static FileHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new FileHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns></returns>
        public string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("文件不存在");
            }
            var content = string.Empty;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = sr.ReadToEnd();
                }
            }
            return content;
        }
        /// <summary>
        /// 异步读取文件内容
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns></returns>
        public async Task<string> ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("文件不存在");
            }
            var content = string.Empty;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    content = await sr.ReadToEndAsync();
                }
            }
            return content;
        }

        /// <summary>
        /// 远程异步读取文件内容
        /// </summary>
        /// <param name="filePath">远程文件路径</param>
        /// <returns></returns>
        public async Task<string> ReadFileHttpAsync(string filePath)
        {
            var content = string.Empty;
            using (var wc = new WebClient())
            {
                using (var s = wc.OpenRead(new Uri(filePath)))
                {
                    var sr = new StreamReader(s);
                    content = await sr.ReadToEndAsync();
                }
            }
            return content;
        }

        /// <summary>
        /// 写入文件
        /// 文件不存在，则创建新文件
        /// 文件存在，写入覆盖原有内容
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="content">待写入内容</param>
        public void WriteFile(string filePath, string content)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(content);
                    sw.Flush();
                }
            }
        }
        /// <summary>
        /// 异步写入文件
        /// 文件不存在，则创建新文件;
        /// 文件存在，写入覆盖原有内容
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="content">待写入内容</param>
        public async Task WriteFileAsync(string filePath, string content)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    await sw.WriteAsync(content);
                    sw.Flush();
                }
            }
        }
        /// <summary>
        /// 远程异步写入文件
        /// 文件不存在，则创建新文件;
        /// 文件存在，写入覆盖原有内容
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="content">待写入内容</param>
        public async Task WriteFileHttpAsync(string filePath, string content)
        {
            using (var wc = new WebClient())
            {
                using (var s = wc.OpenWrite(filePath))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        await sw.WriteAsync(content);
                        sw.Flush();
                    }
                }
            }
        }
    }
}