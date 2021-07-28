using Common;
using io.rong.models.group;
using io.rong.models.response;
using io.rong.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RongCloud.Areas.APP.Controllers
{
    [RoutePrefix("APP/Group")]
    public class GroupController : Controller
    {
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="name">群名</param>
        /// <param name="memberIds">群成员userId</param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(string name, string[] memberIds)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var groupId = Guid.NewGuid().ToString();
            var result = group.Create(new GroupModel
            {
                Id = groupId,
                Name = name,
                Members = memberIds.Select(p => new GroupMember { Id = p }).ToArray()
            });
            var userStatus = memberIds.Select(p => new { id = p, status = 1 });//用户状态，默认1
            //融云不维护群信息，若要保存群主等信息需要存储到业务数据库
            return Json(new { result.code, result = new { id = groupId, userStatus } });
        }

        /// <summary>
        /// 查询群信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(string id)
        {
            //因为demo没有业务数据库存储群信息，此处先获取群成员，再获取第1个群成员的所有信息，遍历得到群信息
            var user = RongCloudHelper.RongCloudInstance.User;
            var group = RongCloudHelper.RongCloudInstance.Group;
            var groupUsers = group.Get(new io.rong.models.group.GroupModel { Id = id });
            var userId = "";
            var memberCount = 0;
            if (groupUsers.code == 200)
            {
                memberCount = groupUsers.Members.Count;
                if (memberCount > 0)
                {
                    var groupUser = groupUsers.Members[0];
                    userId = groupUser.Id;
                }
            }
            var myGroups = user.GetGroups(userId);
            var result = myGroups.groups?.Where(p => p.id == id).FirstOrDefault() ?? new io.rong.models.push.GroupInfoModel
            {
                id = id,
                name = "讨论组" + id
            };
            result.memberCount = memberCount;
            result.creatorId = userId;//默认每个人都是群主，可以解散群聊
            //由于群组信息项目未保存，从融云获取个人所有群组后筛选，前端可以保存获取的群组信息自行遍历获取 
            return Json(new { code = 200, result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询群成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}/Members")]
        [HttpGet]
        public ActionResult Members(string id)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.GetGroupUser(new GroupModel
            {
                Id = id
            });
            var memberResult = new GroupMemberResult()
            {
                code = result.code,
                msg = result.msg,
                members = new List<GroupMemberModel>()
            };
            if (result.code == 200 && result.users != null && result.users.Length > 0)
            {
                var userInfoList = RongTokenHelper.GetAll();//从缓存的个人信息中获取个人详细信息
                var date = DateTime.Now;
                var i = 0;
                foreach (var item in result.users)
                {
                    var userId = item.id;
                    var userInfo = userInfoList.Where(p => p.UserId == userId).OrderByDescending(p => p.CreateTime).FirstOrDefault() ?? new RongTokenInfo();
                    var groupMember = new GroupMemberModel
                    {
                        user = new io.rong.models.push.UserInfoModel
                        {
                            id = userId,
                            nickname = userInfo.Name,
                            portraitUri = userInfo.PortraitUri
                        },
                        createdTime = RongHttpClient.ConvertDateTimeIntMill(date),
                        createdAt = TimeZoneInfo.ConvertTimeToUtc(date).ToString("s") + "Z",
                        updatedAt = TimeZoneInfo.ConvertTimeToUtc(date).ToString("s") + "Z",
                        updatedTime = RongHttpClient.ConvertDateTimeIntMill(date),
                        role = i == 0 ? 0 : 1
                    };
                    memberResult.members.Add(groupMember);
                    i++;
                }
            }
            return Json(new { result.code, result = memberResult.members }, JsonRequestBehavior.AllowGet);
        }

        [Route("get_regular_clear")]
        [HttpPost]
        public ActionResult Get_regular_clear()
        {
            return Json(new { code = 200, result = 0 });
        }

        /// <summary>
        /// 加入群组
        /// </summary>
        /// <param name="groupId">群Id</param>
        /// <param name="memberIds">群成员userId</param>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public ActionResult Add(string groupId, string[] memberIds)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.Join(new GroupModel
            {
                Id = groupId,
                Members = memberIds.Select(p => new GroupMember { Id = p }).ToArray()
            });
            return Json(result);
        }

        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="groupId">群Id</param>
        /// <returns></returns>
        [Route("Quit")]
        [HttpPost]
        public ActionResult Quit(string groupId)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.Quit(new GroupModel
            {
                Id = groupId,
                Members = new GroupMember[] { new GroupMember { Id = CookieHelper.AppAuth } }
            });
            return Json(result);
        }

        /// <summary>
        /// 解散群组
        /// </summary>
        /// <param name="groupId">群Id</param>
        /// <returns></returns>
        [Route("Dismiss")]
        [HttpPost]
        public ActionResult Dismiss(string groupId)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.Dismiss(new GroupModel
            {
                Id = groupId,
                Members = new GroupMember[] { new GroupMember { Id = CookieHelper.AppAuth } }
            });
            return Json(result);
        }

        /// <summary>
        /// 群公告
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("get_bulletin")]
        [HttpGet]
        public ActionResult Get_bulletin(string groupId)
        {
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 群员个人信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [Route("get_member_info")]
        [HttpPost]
        public ActionResult Get_member_info(string groupId, string memberId)
        {
            //获取业务平台保存到群员群名片
            return Json(new
            {
                code = 200,
                result = new
                {
                    groupNickname = "",
                    WeChat = "",
                    memberDesc = "",
                    isDeleted = 0,
                    Alipay = "",
                    phone = "",
                    region = ""
                }
            });
        }

        [Route("notice_info")]
        [HttpGet]
        public ActionResult Notice_info()
        {
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除群员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="memberIds"></param>
        /// <returns></returns>
        [Route("kick")]
        [HttpPost]
        public ActionResult Kick(string groupId, string[] memberIds)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.Quit(new GroupModel
            {
                Id = groupId,
                Members = memberIds.Select(p => new GroupMember { Id = p }).ToArray()
            });
            return Json(result);
        }
    }
}