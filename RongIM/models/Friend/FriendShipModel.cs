using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong.models.push
{
    /// <summary>
    /// 好友关系返回类
    /// </summary>
    public class FriendShipModel
    {
        /// <summary>
        /// 好友信息
        /// </summary>
        public UserInfoModel user { get; set; }
        /// <summary>
        /// 好友关系状态
        /// </summary>
        public int status { get; set; }
        public string displayName { get; set; } = "";
        public string message { get; set; } = "";
        /// <summary>
        /// 时间
        /// </summary>
        public string updatedAt { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long updatedTime { get; set; }
        //public string disPlayNameSpelling { get; set; }
        //public string groupDisplayName { get; set; }
        //public string groupDisplayNameSpelling { get; set; }
    }
}
