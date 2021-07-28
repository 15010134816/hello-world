using io.rong.models.push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace RongCloud.Controllers
{
    /// <summary>
    /// 好友控制器
    /// </summary>
    public class FriendShipController : Controller
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public ActionResult All()
        {
            //默认所有注册用户都是好友
            var list = RongTokenHelper.GetAll().Select(p => new FriendShipModel
            {
                user = new UserInfoModel
                {
                    id = p.UserId,
                    nickname = p.Name,
                    portraitUri = p.PortraitUri
                    //createTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                },
                status = 20
            });
            return Json(new { result = list }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取好友资料
        /// </summary>
        /// <returns></returns>
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
                        //createTime = userTokenInfo.CreateTime.ToString(),
                        portraitUri = userTokenInfo.PortraitUri
                        //code = 200
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
    }
}