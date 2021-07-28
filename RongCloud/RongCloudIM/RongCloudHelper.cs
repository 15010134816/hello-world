using Common;
using io.rong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RongCloud
{
    public class RongCloudHelper
    {
        private static readonly string appKey = ReadConfig.AppKey_RongCloud;
        private static readonly string appSecret = ReadConfig.AppSecret_RongCloud;

        public static io.rong.RongCloud RongCloudInstance { get; } = io.rong.RongCloud.GetInstance(appKey, appSecret);
    }
}