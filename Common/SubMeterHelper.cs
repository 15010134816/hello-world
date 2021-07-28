using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SubMeterHelper
    {
        private static SubMeterHelper _instance = null;
        private static readonly object lockobj = new object();
        private SubMeterHelper()
        {

        }

        public static SubMeterHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new SubMeterHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        public int GetDtIndex(object s)
        {
            var i = Math.Abs(s.GetHashCode());
            var userDtCount = ReadConfig.UserInfoDtCount;
            return i % userDtCount;
        }
    }
}
