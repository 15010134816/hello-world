using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud.Areas.APP.Controllers
{
    [RoutePrefix("APP/MISC")]
    public class MiscController : Controller
    {
        [Route("client_version")]
        [Route("mobile_version")]
        [HttpGet]
        public string Client_VerSion()
        {
            var result = "{\"iOS\":{\"version\":\"5.1.3\",\"build\":\"202106252001\",\"url\":\"https://cdn.ronghub.com/app_sealtalk.plist?513\"},\"Android\":{\"version\":\"5.1.3\",\"url\":\"https://downloads.rongcloud.cn/SealTalk_by_RongCloud_Android_v5_1_3.apk\"}}";
            //return Json(new { result }, JsonRequestBehavior.AllowGet);
            return result;
        }
        
        [Route("demo_square")]
        [HttpGet]
        public string Demo_Square()
        {
            var result = "{\"code\":200,\"result\":[{\"id\":\"GC2lr3GPu\",\"type\":\"group\",\"name\":\"大大的动物小小的人\",\"portraitUri\":\"http://7xogjk.com1.z0.glb.clouddn.com/CzUCEKg211524556100714103027\",\"memberCount\":0,\"maxMemberCount\":500},{\"id\":\"xwpzGeb8X\",\"type\":\"group\",\"name\":\"测试租组\",\"portraitUri\":\"\",\"memberCount\":2,\"maxMemberCount\":500},{\"id\":\"NxgwO7nJm\",\"type\":\"group\",\"name\":\"10064\",\"portraitUri\":\"\",\"memberCount\":5,\"maxMemberCount\":500},{\"id\":\"OIBbeKlkx\",\"type\":\"chatroom\",\"name\":\"聊天室 I\",\"portraitUri\":null,\"memberCount\":0,\"maxMemberCount\":0},{\"id\":\"675NdFjkx\",\"type\":\"chatroom\",\"name\":\"聊天室 II\",\"portraitUri\":null,\"memberCount\":0,\"maxMemberCount\":0},{\"id\":\"MfgILRowx\",\"type\":\"chatroom\",\"name\":\"聊天室 III\",\"portraitUri\":null,\"memberCount\":0,\"maxMemberCount\":0},{\"id\":\"lFVuoM7Jx\",\"type\":\"chatroom\",\"name\":\"聊天室 IV\",\"portraitUri\":null,\"memberCount\":0,\"maxMemberCount\":0}]}";
            //return Json(new { result }, JsonRequestBehavior.AllowGet);
            return result;
        }
        [Route("get_screen_capture")]
        [HttpPost]
        public ActionResult Get_screen_capture(int conversationType, string targetId)
        {
            return Json(new { code = 200, result = new { status = 0 } });
        }
    }
}