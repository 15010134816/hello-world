using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RongCloud.Models;
using Newtonsoft.Json.Converters;
using System.Data.SqlClient;
using System.Data;
using io.rong.models.group;

namespace RongCloud.Controllers
{
    public class RongCloudAPIController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        #region 通用调用方法
#if Debug
#else
        [HttpPost]
#endif
        public string DoWork(string pars = "")
        {
            string text = "";
            try
            {
                LogHelper.WriteLog(this.GetType().ToString(), pars, LogType.Info, LogPath.Logs_RongIM);
                JObject joparam = (JObject)JsonConvert.DeserializeObject(pars);
                if (!checkparam(joparam, "method,proc,pars", out string str))
                {
                    throw new Exception(str);
                }
                switch (joparam["method"].ToString())
                {
                    case "Query":
                        //text = Query(joparam);
                        break;
                    case "Modify":
                        //text = Modify(joparam);
                        break;
                    case "Func":
                        text = DoFunc(joparam);
                        break;
                    default:
                        text = "{\"result\":\"2\",\"msg\":\"无效的方法名称！\"}";
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("code:2000") > -1)
                {
                    text = "{\"result\":\"2000\",\"msg\":\"" + ex.Message.Replace("code:2000|", "") + "\"}";
                }
                else
                {
                    text = "{\"result\":\"1000\",\"msg\":\"" + ex.Message + "\"}";
                }
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
            }

            return text;
        }

        ///<summary>
        ///接口入口
        ///</summary>
        ///<returns></returns>
        public string DoFunc(JObject o)
        {
            string text;
            JsonModel2 result = new JsonModel2();
            result.result = false;
            result.Msg = "";
            string classname = "RongCloud.Controllers.RongCloudAPIController";
            string methodname = o["proc"].ToString();
            JsonResult json = new JsonResult();
            try
            {
                if (methodname == "")
                {
                    throw new Exception("请传入正确的方法名称");
                }
                Type type;                          // 存储类
                Object obj;
                type = Type.GetType(classname);      // 通过类名获取同名类
                obj = System.Activator.CreateInstance(type);       // 创建实例
                MethodInfo md = type.GetMethod(methodname, new Type[] { typeof(string) });      // 获取方法信息
                if (md == null)
                {
                    throw new Exception("找不到" + methodname + "方法,请核查");
                }
                string data = JsonConvert.SerializeObject(o["pars"]).Replace("[", "").Replace("]", "");
                object[] parameters = new object[] { data };
                text = (string)md.Invoke(obj, parameters);  // 调用方法
                return text;

            }
            catch (Exception ex)
            {
                result.result = false;
                result.Msg = ex.Message;
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
            }
            if (result.result)
            {
                text = "{\"result\":\"0\",\"msg\":" + JsonConvert.SerializeObject(result.Data) + "}";
            }
            else
            {
                text = "{\"result\":\"1\",\"msg\":\"" + result.Msg + "\"}";
            }
            return text;
        }

        #endregion

        #region 判断data参数json字符串键值是否完整
        /// <summary>
        /// 判断data参数json字符串键值是否完整
        /// </summary>
        /// <param name="jo">json字符串序列化后的JObject</param>
        /// <param name="paramstr">键值拼接字符串，格式"key1,key2,key3"</param>
        /// <param name="msg">错误时返回的字符串</param>
        /// <returns></returns>
        private bool checkparam(JObject jo, string paramstr, out string msg)
        {
            bool sign = true;
            msg = "data缺少键值：";
            if (paramstr == "")
            {
                return sign;
            }
            string[] sArray = paramstr.Split(',');
            for (int i = 0; i < sArray.Length; i++)
            {
                if (jo.Property(sArray[i]) == null)
                {
                    sign = false;
                    msg += sArray[i] + "、";
                }
            }
            if (jo.Property("proc") != null && jo.Property("proc") != null)
            {
                //if (!CheckPower(jo["proc"].ToString(), jo["method"].ToString()))
                //{
                //    sign = false;
                //    msg = "找不到对应接口！";
                //}
            }
            if (jo.Property("pars") != null)
            {
                try
                {
                    JArray array = (JArray)jo["pars"];
                }
                catch (Exception)
                {
                    sign = false;
                    msg = "json字符串格式错误！";
                }

            }
            msg = msg.Substring(0, msg.Length - 1);
            return sign;
        }

        private int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;

            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = (ts.Days * 24 * 60 * 60) + (ts.Hours * 60 * 60) + (ts.Minutes * 60) + ts.Seconds;
            return dateDiff;
        }
        private int DateDiffDay(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Days;
        }

        #endregion

        /// <summary>
        /// 获取融云token
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetToken(string parameters)
        {
            string text = "";
            //DataSet ds = new DataSet();
            try
            {
                JObject joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "userid,name", out string str))
                {
                    throw new Exception(str);
                }
                string userid = TypeHelper.S_ToString(joparam["userid"]); //用户id
                string name = TypeHelper.S_ToString(joparam["name"]); //昵称
                string portrait_path = TypeHelper.S_ToString(joparam["portrait_path"]);  //头像

                //1、先从数据库获取融云token，没有继续往下
                //2、获取融云token，存入数据库

                #region 从数据库获取融云token
                //从数据库获取融云token
                //SqlParameter[] para = {
                //    new SqlParameter("@userid",userid)
                //};
                //ds = DbHelperSQL.GetDataSet("Q_Get_RongToken", para);
                //try
                //{

                //}
                //catch (Exception e)
                //{
                //    text = "{\"result\":\"1\",\"msg\":\"" + e.Message + "\"}";
                //    return text;
                //} 
                //目前从数据字典中取
                var token = RongTokenHelper.GetToken(userid);
                if (!string.IsNullOrEmpty(token))
                {
                    text = JsonConvert.SerializeObject(new { result = 0, msg = token });
                    return text;
                }
                #endregion

                var user = RongCloudHelper.RongCloudInstance.User;
                var result = user.Register(new io.rong.models.push.UserModel
                {
                    Id = userid,
                    Name = name,
                    Portrait = portrait_path
                });
                if (result.code == 200)
                {
                    text = JsonConvert.SerializeObject(new { result = 0, msg = result.Token });
                    //token存入数据库 目前存入txt文件、数据字典
                    RongTokenHelper.Add(new RongTokenInfo
                    {
                        UserId = userid,
                        Name = name,
                        PortraitUri = portrait_path,
                        Token = result.Token,
                        CreateTime = DateTime.Now
                    });
                }
                else
                {
                    text = JsonConvert.SerializeObject(new { result = 1, msg = "获取token失败：" + result.msg });
                }
            }
            catch (Exception ex)
            {
                text = "{\"result\":\"1\",\"msg\":\"" + ex.Message + "\"}";
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
            }

            return text;
        }

        /// <summary>
        /// 获取融云所有用户
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetAllUser(string parameters)
        {
            var text = "";
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "userid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var users = RongTokenHelper.GetAll();
                text = JsonConvert.SerializeObject(new { result = 0, msg = users });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                text = "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
            return text;
        }

        #region 用户
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserInfo(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "userid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var userId = TypeHelper.S_ToString(joparam["userid"]);
                var user = RongCloudHelper.RongCloudInstance.User;
                var userInfo = user.Get(userId);
                var isSuccess = userInfo.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, msg = userInfo });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 获取用户所在群组
        /// </summary>
        /// <returns></returns>
        public string GetMyGroups(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "userid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var userId = TypeHelper.S_ToString(joparam["userid"]);
                var user = RongCloudHelper.RongCloudInstance.User;
                var myGroups = user.GetGroups(userId);
                var isSuccess = myGroups.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, myGroups.groups, myGroups.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 获取用户的黑名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMyBlackList(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "userid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var userId = TypeHelper.S_ToString(joparam["userid"]);
                var user = RongCloudHelper.RongCloudInstance.User;
                var result = user.blackList.GetList(new io.rong.models.push.UserModel
                {
                    Id = userId
                });
                var isSuccess = result.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, users = result.Users, result.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public string SetNickName(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "userid,nickname", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var userId = TypeHelper.S_ToString(joparam["userid"]);
                var nickname = TypeHelper.S_ToString(joparam["nickname"]);
                var user = RongCloudHelper.RongCloudInstance.User;
                var result = user.Update(new io.rong.models.push.UserModel
                {
                    Id = userId,
                    Name = nickname
                });
                var isSuccess = result.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, result.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }
        #endregion

        #region 群组
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="groupname">群名</param>
        /// <param name="userids">群成员userId,以逗号分隔</param>
        /// <returns>成功返回群Id</returns>
        public string CreateGroup(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "groupname,userids", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var name = TypeHelper.S_ToString(joparam["groupname"]);
                var userIds = TypeHelper.S_ToString(joparam["userids"]);
                var group = RongCloudHelper.RongCloudInstance.Group;
                var groupId = Guid.NewGuid().ToString();
                var result = group.Create(new GroupModel
                {
                    Id = groupId,
                    Name = name,
                    Members = userIds.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Select(p => new GroupMember { Id = p }).ToArray()
                });
                //融云不维护群信息，若要保存群主等信息需要存储到业务数据库
                var isSuccess = result.code == 200 ? 0 : 1;
                msg = isSuccess == 0 ? groupId : result.msg;
                return "{\"result\":" + isSuccess + ",\"msg\":\"" + msg + "\"}";
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 查询群信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetGroupInfo(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "groupid,userid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var id = TypeHelper.S_ToString(joparam["groupid"]);
                var userId = TypeHelper.S_ToString(joparam["userid"]);
                var user = RongCloudHelper.RongCloudInstance.User;
                var myGroups = user.GetGroups(userId);
                var groupInfo = myGroups.groups?.Where(p => p.id == id).FirstOrDefault() ?? new io.rong.models.push.GroupInfoModel
                {
                    id = id,
                    name = "讨论组" + id
                }; ;
                //由于群组信息项目未保存，从融云获取个人所有群组后筛选，前端可以保存获取的群组信息自行遍历获取
                return JsonConvert.SerializeObject(new { result = 0, msg = groupInfo });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 查询群成员
        /// </summary>
        /// <param name="groupid">群id</param>
        /// <returns></returns>
        public string GetGroupMembers(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "groupid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var id = TypeHelper.S_ToString(joparam["groupid"]);
                var group = RongCloudHelper.RongCloudInstance.Group;
                var result = group.GetGroupUser(new GroupModel
                {
                    Id = id
                });
                var isSuccess = result.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, result.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 加入群组
        /// </summary>
        /// <param name="groupid">群Id</param>
        /// <param name="userids">加入群的userId,以逗号分隔</param>
        /// <returns></returns>
        public string AddGroup(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "groupid,userids", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var groupId = TypeHelper.S_ToString(joparam["groupid"]);
                var userIds = TypeHelper.S_ToString(joparam["userids"]);
                var group = RongCloudHelper.RongCloudInstance.Group;
                var result = group.Join(new GroupModel
                {
                    Id = groupId,
                    Members = userIds.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Select(p => new GroupMember { Id = p }).ToArray()
                });
                var isSuccess = result.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, result.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="groupid">群Id</param>
        /// <param name="userids">退群的userId，以逗号分隔</param>
        /// <returns></returns>
        public string QuitGroup(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "groupid,userids", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var groupId = TypeHelper.S_ToString(joparam["groupid"]);
                var userIds = TypeHelper.S_ToString(joparam["userids"]);
                var group = RongCloudHelper.RongCloudInstance.Group;
                var result = group.Quit(new GroupModel
                {
                    Id = groupId,
                    Members = userIds.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Select(p => new GroupMember { Id = p }).ToArray()
                });
                var isSuccess = result.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, result.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 解散群组
        /// </summary>
        /// <param name="groupid">群Id</param>
        /// <param name="userid">操作解散群的用户Id</param>
        /// <returns></returns>
        public string DismissGroup(string parameters)
        {
            try
            {
                var joparam = (JObject)JsonConvert.DeserializeObject(parameters);
                if (!checkparam(joparam, "groupid,userid", out var msg))
                {
                    return "{\"result\":1,\"msg\":\"" + msg + "\"}";
                }
                var groupId = TypeHelper.S_ToString(joparam["groupid"]);
                var userId = TypeHelper.S_ToString(joparam["userid"]);
                var group = RongCloudHelper.RongCloudInstance.Group;
                var result = group.Dismiss(new GroupModel
                {
                    Id = groupId,
                    Members = new GroupMember[] { new GroupMember { Id = userId } }
                });
                var isSuccess = result.code == 200 ? 0 : 1;
                return JsonConvert.SerializeObject(new { result = isSuccess, result.msg });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return "{\"result\":1,\"msg\":\"" + ex.Message + "\"}";
            }
        }
        #endregion
    }
}