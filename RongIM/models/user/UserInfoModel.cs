using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace io.rong.models.push
{
    /// <summary>
    /// 个人信息返回类
    /// </summary>
    public class UserInfoModel //: Result
    {
        public string id { get; set; }
        [JsonProperty("userName")]
        public string nickname { get; set; } = "";
        [JsonProperty("userPortrait")]
        public string portraitUri { get; set; }
        //public string createTime { get; set; }
        public string phone { get; set; } = "1";
        public string gender { get; set; } = "male";
        public string stAccount { get; set; } = "";
        public string region { get; set; } = "86";
        //public string orderSpelling { get; set; }
        //public string firstCharacter { get; set; }
        //public string nameSpelling { get; set; }
    }
}
