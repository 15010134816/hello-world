using Common;
using io.rong.models.push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud.Areas.APP.Controllers
{
    [RoutePrefix("APP/User")]
    public class UserController : Controller
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public ActionResult GetInfo(string id = "")
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.Get(id);
            return Json(new { result.code, result = new { result.id, result.nickname, result.portraitUri, result.region, result.phone, result.stAccount, result.gender } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户的黑名单
        /// </summary>
        /// <returns></returns>
        [Route("BlackList")]
        [HttpGet]
        public ActionResult BlackList()
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var userId = CookieHelper.AppAuth;
            var result = user.blackList.GetList(new UserModel
            {
                Id = userId
            });
            var userList = result.Users?.Select(p => new
            {
                user = new UserInfoModel
                {
                    id = p.Id,
                    nickname = ""
                }
            });
            return Json(new { result.code, result.msg, result = userList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// APP获取我的群组
        /// </summary>
        /// <returns></returns>
        [Route("favgroups")]
        [HttpGet]
        public ActionResult FavGroups()
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var userId = CookieHelper.AppAuth;
            var list = new List<GroupInfoModel>();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { code = 200, result = new { limit = 0, offset = 0, total = 0, list } }, JsonRequestBehavior.AllowGet); //"{\"code\":200,\"result\":{\"limit\":null,\"offset\":null,\"total\":0,\"list\":[]}}";
            }
            var result = user.GetGroups(userId);
            if (result.code != 200)
            {
                return Json(new { code = 200, result = new { limit = 0, offset = 0, total = 0, list }, result.msg, JsonRequestBehavior.AllowGet });
            }
            list = result.groups.Select(p => new GroupInfoModel
            {
                id = p.id,
                name = p.name
            }).ToList();
            var count = list.Count;
            return Json(new { code = 200, result = new { limit = 0, offset = 0, total = count, list } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Web获取用户所在群组
        /// </summary>
        /// <returns></returns>
        [Route("Groups")]
        [HttpGet]
        public ActionResult Groups()
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.GetGroups(CookieHelper.AppAuth);
            var groups = result.groups?.Select(p => new { group = p });
            return Json(new { result.code, result.msg, result = groups }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        [HttpPost]
        public ActionResult Logout()
        {
            CookieHelper.DeleteCookies(CookieHelper.CookieAppAuth);
            return Json(new { code = 200 });
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [Route("get_token")]
        [HttpGet]
        public ActionResult GetToken()
        {
            try
            {
                string userid = CookieHelper.AppAuth;
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
                    return Json(new { code = 200, result = new { token = result.Token }, msg = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result.code, result = new { token = "" }, msg = "获取token失败：" + result.msg }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType().ToString(), ex, LogPath.Logs_RongIM);
                return Json(new { code = 500, result = new { token = "" }, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("verify_code_register")]
        [Route("Login")]
        [HttpPost]
        public ActionResult Login(string code, string phone, string region)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return Json(new { code = 201, msg = "请输入用户名" });
            }
            var userid = phone;
            //目前从数据字典中取
            var userInfo = RongTokenHelper.GetUserInfo(userid);
            if (!string.IsNullOrEmpty(userInfo.UserId))
            {
                var token = userInfo.Token;
                if (!string.IsNullOrEmpty(token))
                {
                    CookieHelper.SetCookies("rong_im_auth", userid, 2400);
                    return Json(new { code = 200, result = new { token, id = userid, nickName = userInfo.Name }, msg = "" });
                }
            }
            return Json(new { code = 201, msg = "用户不存在" });
        }

        [Route("get_poke")]
        [HttpGet]
        public ActionResult GetPoke()
        {
            return Json(new { code = 200, result = new { pokeStatus = 1 } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加入黑名单
        /// </summary>
        /// <returns></returns>
        [Route("add_to_blacklist")]
        [HttpPost]
        public ActionResult Add_to_blacklist(string friendId)
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.blackList.Add(new UserModel
            {
                id = CookieHelper.AppAuth,
                Blacklist = new UserModel[] { new UserModel { id = friendId } }
            });
            return Json(result);
        }

        /// <summary>
        /// 移出黑名单
        /// </summary>
        /// <returns></returns>
        [Route("remove_from_blacklist")]
        [HttpPost]
        public ActionResult Remove_from_blacklist(string friendId)
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var result = user.blackList.Remove(new UserModel
            {
                id = CookieHelper.AppAuth,
                Blacklist = new UserModel[] { new UserModel { id = friendId } }
            });
            return Json(result);
        }
    }
}