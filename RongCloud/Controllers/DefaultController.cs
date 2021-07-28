using Common;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace RongCloud.Controllers
{
    public class DefaultController : Controller
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ILog log = LogManager.GetLogger("DefaultController");
        private ILog logError = LogManager.GetLogger("logerror");
        // GET: Default
        public ActionResult Index()
        {
            log.Info("项目启动");
            logError.Info("测试错误记录器");
            logger.Info("项目启动2");
            return View();
        }
        /// <summary>
        /// 秒杀
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult Seckill(string userId, int goodsId)
        {
            var result = SeckillHelper.Instance.Seckill(userId, goodsId);
            return Json(result);
        }
        /// <summary>
        /// 重置库存
        /// 从数据库读取库存更新缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetStock()
        {
            SeckillHelper.Instance.ResetStock();
            return Json(true);
        }
        /// <summary>
        /// 批量添加用户
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public ActionResult AddUser(int num = 0)
        {
            var trueCount = 0;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < num; i++)
            {
                //var userId = Guid.NewGuid();
                //var dtIndex = SubMeterHelper.Instance.GetDtIndex(userId);
                ////LogHelper.WriteLog("AddUserDb", string.Format("i:{0};userId:{1};dtIndex:{2}", i, userId, dtIndex), LogType.Info, LogPath.Logs);
                //var sql = string.Format("insert into userinfo_{0}(userId,userName) values(@userId,@userName)", dtIndex);
                //trueCount += DbHelperSQL.ExecuteSql(sql, new SqlParameter[]{
                //    new SqlParameter("userId",userId),
                //    new SqlParameter("userName",i)
                //});
                //LogHelper.WriteLog(this.GetType().ToString(), i.ToString(), LogType.Info, LogPath.Logs);
                var j = i;
                //Task.Factory.StartNew(() =>
                Task.Run(() =>
                {
                    var userId = Guid.NewGuid();
                    var dtIndex = SubMeterHelper.Instance.GetDtIndex(userId);
                    LogHelper.WriteLog4Net("AddUserDb", string.Format("i:{0};userId:{1};dtIndex:{2},threadID:{3}", j, userId, dtIndex, Thread.CurrentThread.ManagedThreadId), LogType.Info, LogPath.Logs);
                    var sql = string.Format("insert into userinfo_{0}(userId,userName) values(@userId,@userName)", dtIndex);
                    var para = new SqlParameter[]{
                        new SqlParameter("userId",userId),
                        new SqlParameter("userName",j)
                    };
                    DbHelperSQL.ExecuteSql(sql, para);
                });
            }
            sw.Stop();
            var times = sw.ElapsedMilliseconds;
            return Json(new { trueCount, times });
        }
        /// <summary>
        /// 异步批量添加用户
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task<ActionResult> AddUserAsync(int num = 0)
        {
            var trueCount = 0;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < num; i++)
            {
                var j = i;
                trueCount += await AddUserDb(j);
            }
            sw.Stop();
            var times = sw.ElapsedMilliseconds;
            return Json(new { trueCount, times });
        }
        public ActionResult AddUserAsyncNoWait(int num = 0)
        {
            var trueCount = 0;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < num; i++)
            {
                var j = i;
                Task.Run(() => AddUserDb(j));
            }
            sw.Stop();
            var times = sw.ElapsedMilliseconds;
            return Json(new { trueCount, times });
        }
        /// <summary>
        /// 异步数据库添加用户
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private async Task<int> AddUserDb(int i)
        {
            var userId = Guid.NewGuid();
            var dtIndex = SubMeterHelper.Instance.GetDtIndex(userId);
            var sql = string.Format("insert into userinfo_{0}(userId,userName) values(@userId,@userName)", dtIndex);
            var para = new SqlParameter[]{
                    new SqlParameter("userId",userId),
                    new SqlParameter("userName",i)
            };
            //LogHelper.WriteLogAsync("AddUserDb", string.Format("i:{0};userId:{1};dtIndex:{2},WriteLog ThreadId:{3}\r\n" + sql + "\r\n" + JsonConvert.SerializeObject(para), i, userId, dtIndex, Thread.CurrentThread.ManagedThreadId), LogType.Info, LogPath.Logs);
            log.Info(string.Format("i:{0};userId:{1};dtIndex:{2},WriteLog ThreadId:{3}\r\n" + sql + "\r\n" + JsonConvert.SerializeObject(para), i, userId, dtIndex, Thread.CurrentThread.ManagedThreadId));
            var rowCount = await ExecuteSqlAsync(sql, para);
            return rowCount;
        }
        /// <summary>
        /// 异步执行sql
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(DbHelperSQL.connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    DbHelperSQL.PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    int rows = await cmd.ExecuteNonQueryAsync();
                    cmd.Parameters.Clear();
                    return rows;
                }
            }
        }

        public ActionResult Test()
        {
            var i = 0;
            var n = 10 / i;
            return Json(new { n });
        }

        public ActionResult TestLog()
        {
            var th = new Thread(() =>
            {
                log.Info("多线程写入日志");
            });
            th.Start();
            Task.Factory.StartNew(() =>
            {
                log.Info("测试异步并发日志");
            });
            Task.Factory.StartNew(() =>
            {
                log.Info("测试异步并发日志2");
            });
            Task.Factory.StartNew(() =>
            {
                log.Info("测试异步并发日志3");
            });
            return Json(new { result = true });
        }
    }
}