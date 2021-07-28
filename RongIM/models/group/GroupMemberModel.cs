using io.rong.models.push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong.models.group
{
    /// <summary>
    /// 群成员信息
    /// </summary>
    public class GroupMemberModel
    {
        public string groupNickname { get; set; } = "";
        public int role { get; set; } = 1;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string createdAt { get; set; }
        /// <summary>
        /// 加群时间
        /// </summary>
        public long createdTime { get; set; }
        public string updatedAt { get; set; }
        public long updatedTime { get; set; }
        public string displayName { get; set; } = "";
        public UserInfoModel user { get; set; }
    }
}
