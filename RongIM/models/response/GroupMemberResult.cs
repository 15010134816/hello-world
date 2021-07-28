using io.rong.models.group;
using io.rong.models.push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong.models.response
{
    /// <summary>
    /// 获取群成员返回类
    /// </summary>
    public class GroupMemberResult : Result
    {
        public List<GroupMemberModel> members { get; set; }
    }
}
