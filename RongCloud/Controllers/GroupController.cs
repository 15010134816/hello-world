using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using io.rong.models.group;

namespace RongCloud.Controllers
{
    /// <summary>
    /// 群组控制器
    /// </summary>
    public class GroupController : Controller
    {
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="name">群名</param>
        /// <param name="memberIds">群成员userId</param>
        /// <returns></returns>
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
            //融云不维护群信息，若要保存群主等信息需要存储到业务数据库
            return Json(new { result.code, result = new { id = groupId } });
        }

        /// <summary>
        /// 查询群信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetGroupInfo(string id)
        {
            var user = RongCloudHelper.RongCloudInstance.User;
            var myGroups = user.GetGroups(CookieHelper.UserId);
            var result = myGroups.groups?.Where(p => p.id == id).FirstOrDefault() ?? new io.rong.models.push.GroupInfoModel
            {
                id = id,
                name = "讨论组" + id
            }; ;
            //由于群组信息项目未保存，从融云获取个人所有群组后筛选，前端可以保存获取的群组信息自行遍历获取 
            return Json(new { code = 200, result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询群成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Members(string id)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.GetGroupUser(new GroupModel
            {
                Id = id
            });
            return Json(new { result.code, result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加入群组
        /// </summary>
        /// <param name="groupId">群Id</param>
        /// <param name="memberIds">群成员userId</param>
        /// <returns></returns>
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
        public ActionResult Quit(string groupId)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.Quit(new GroupModel
            {
                Id = groupId,
                Members = new GroupMember[] { new GroupMember { Id = CookieHelper.UserId } }
            });
            return Json(result);
        }

        /// <summary>
        /// 解散群组
        /// </summary>
        /// <param name="groupId">群Id</param>
        /// <returns></returns>
        public ActionResult Dismiss(string groupId)
        {
            var group = RongCloudHelper.RongCloudInstance.Group;
            var result = group.Dismiss(new GroupModel
            {
                Id = groupId,
                Members = new GroupMember[] { new GroupMember { Id = CookieHelper.UserId } }
            });
            return Json(result);
        }
    }
}