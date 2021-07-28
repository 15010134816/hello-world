using Common;
using io.rong.models.push;
using io.rong.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud.Areas.APP.Controllers
{
    [RoutePrefix("APP/FriendShip")]
    public class FriendShipController : Controller
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        [Route("All")]
        [HttpGet]
        public ActionResult All()
        {
            var appAuth = CookieHelper.AppAuth;//获取当前登录人cookie信息
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc);
            //默认所有注册用户都是好友
            var list = RongTokenHelper.GetAll().Select(p => new FriendShipModel
            {
                user = new UserInfoModel
                {
                    id = p.UserId,
                    nickname = p.Name,
                    portraitUri = p.PortraitUri
                },
                status = 20,
                updatedAt = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("s") + "Z",
                updatedTime = RongHttpClient.ConvertDateTimeIntMill(p.CreateTime)
            });
            return Json(new { code = 200, result = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取好友资料
        /// </summary>
        /// <returns></returns>
        [Route("{id}/profile")]
        public ActionResult profile(string id)
        {
            var userTokenInfo = RongTokenHelper.GetUserInfo(id);
            if (!string.IsNullOrWhiteSpace(userTokenInfo.UserId))
            {
                var info = new FriendShipModel
                {
                    user = new UserInfoModel
                    {
                        id = userTokenInfo.UserId,
                        nickname = userTokenInfo.Name,
                        portraitUri = userTokenInfo.PortraitUri
                    }
                };
                return Json(new { code = 200, result = info }, JsonRequestBehavior.AllowGet);
            }
            var user = RongCloudHelper.RongCloudInstance.User;
            var userInfo = user.Get(id);
            var result = new FriendShipModel
            {
                user = userInfo
            };
            return Json(new { userInfo.code, result }, JsonRequestBehavior.AllowGet);
        }

        [Route("get_friend_description")]
        [HttpPost]
        public ActionResult Get_friend_description(string friendId)
        {
            //需获取业务数据库中的好友信息，此处返回空值
            return Json(new
            {
                code = 200,
                result = new
                {
                    imageUri = "",
                    displayName = "",
                    phone = "",
                    region = "86",
                    description = ""
                }
            });
        }
    }
}