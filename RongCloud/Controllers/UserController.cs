using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace RongCloud.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : Controller
    {
        public ActionResult Get_token()
        {
            try
            {
                string userid = CookieHelper.UserId;
                string name = ""; //昵称
                string portrait_path = "";  //头像

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
                var userInfo = RongTokenHelper.GetUserInfo(userid);
                if (!string.IsNullOrEmpty(userInfo.UserId))
                {
                    var token = userInfo.Token;
                    if (!string.IsNullOrEmpty(token))
                    {
                        return Json(new { code = 200, result = new { token }, msg = "" });
                    }
                    name = userInfo.Name;
                    portrait_path = userInfo.PortraitUri;
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
                    //token存入数据库 目前存入txt文件、数据字典
                    RongTokenHelper.Add(new RongTokenInfo
                    {
                        UserId = userid,
                        Name = name,
                        PortraitUri = portrait_path,
                        Token = result.Token,
                        CreateTime = DateTime.Now
                    });
                    return Json(new { code = 200, result = new { token = result.Token }, msg = "" });
                }
                else
                {
                    return Json(new { result.code, result = new { token = "" }, msg = "获取token失败：" + result.msg });
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return Json(new { code = 500, result = new { token = "" }, msg = ex.Message });
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            return Json(new { code = 200 });
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetInfo(string id = "")
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.Get(id);
            return Json(new { result.code, result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户所在群组
        /// </summary>
        /// <returns></returns>
        public ActionResult Groups()
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.GetGroups(CookieHelper.UserId);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户的黑名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BlackList(string id = "")
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.blackList.GetList(new io.rong.models.push.UserModel
            {
                Id = CookieHelper.UserId
            });
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public ActionResult Set_NickName(string nickname)
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.Update(new io.rong.models.push.UserModel
            {
                Id = CookieHelper.UserId,
                Name = nickname
            });
            return Json(new { result });
        }

        public ActionResult Test(string id)
        {
            var test = TestHelper.Instance.GetTest(id);
            return Json("用户" + id + test);
        }

        public ActionResult ResetCount(int total = 10)
        {
            CacheHelper.Set("Num", total);
            TestHelper.total = total;
            TestHelper.queueAll.Clear();
            return Json(total);
        }
    }
}