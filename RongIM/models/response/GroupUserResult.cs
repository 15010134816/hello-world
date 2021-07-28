using io.rong.models.push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong.models.response
{
    /// <summary>
    /// 群组成员返回类
    /// </summary>
    public class GroupUserResult : Result
    {
        public string groupid { get; set; }
        public UserInfoModel[] users { get; set; }
    }
}
