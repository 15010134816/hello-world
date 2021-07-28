using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace io.rong.models.push
{
    /// <summary>
    /// 我的群组返回类
    /// </summary>
    public class MyGroupsModel : Result
    {
        public GroupInfoModel[] groups { get; set; }
    }
    /// <summary>
    /// 群信息
    /// </summary>
    public class GroupInfoModel
    {
        public string id { get; set; }

        public string name { get; set; }
        public string portraitUri { get; set; }
        public int memberCount { get; set; }
        public int maxMemberCount { get; set; } = 500;
        public string creatorId { get; set; }
        public string bulletin { get; set; }
        public string deletedAt { get; set; }
        public int isMute { get; set; } = 0;
        public int certiStatus { get; set; } = 1;
        public int memberProtection { get; set; } = 1;
    }
}
