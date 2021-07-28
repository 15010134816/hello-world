using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace io.rong.models
{
    public class Result
    {
        /// <summary>
        /// 返回码，200 为正常
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }

        public Result(int code, String msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public Result()
        {

        }
        /**
         * 设置code
         *
         */
        public void setCode(int code)
        {
            this.code = code;
        }

        /**
         * 获取code
         *
         * @return int
         */
        public int getCode()
        {
            return code;
        }

        /**
         * 获取msg
         *
         * @return String
         */
        public String getMsg()
        {
            return this.msg;
        }
        /**
         * 设置msg
         *
         */
        public void setMsg(String msg)
        {
            this.msg = msg;
        }
    }
}
