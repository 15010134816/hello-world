using io.rong.models.push;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.rong.models.response
{
    /// <summary>
    /// 个人信息返回类
    /// </summary>
    public class UserInfoResult : UserInfoModel
    {
        public int code { get; set; }
        public string msg { get; set; }
    }
}
