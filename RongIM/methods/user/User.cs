﻿using io.rong.methods.user.blacklist;
using io.rong.methods.user.block;
using io.rong.methods.user.onlineStatus;
using io.rong.models;
using io.rong.models.response;
using io.rong.models.push;
using io.rong.methods.user.tag;
using io.rong.util;
using System;
using System.Text;
using System.Web;
using io.rong.models.group;

namespace io.rong.methods.user
{
    public class User
    {
        private static readonly Encoding UTF8 = Encoding.UTF8;
        private static readonly String PATH = "user";
        private String appKey;
        private String appSecret;
        public Block block;
        public Blacklist blackList;
        public OnlineStatus onlineStatus;
        public Tag tag;
        private RongCloud rongCloud;

        public RongCloud RongCloud
        {
            get { return this.rongCloud; }
            set
            {
                this.rongCloud = value;
                block.RongCloud = value;
                blackList.RongCloud = value;
                onlineStatus.RongCloud = value;
                tag.RongCloud = value;
            }
        }


        public User(String appKey, String appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.block = new Block(appKey, appSecret);
            this.blackList = new Blacklist(appKey, appSecret);
            this.onlineStatus = new OnlineStatus(appKey, appSecret);
            this.tag = new Tag(appKey, appSecret);
        }
        /**
         * 获取 Token 方法 
         * url  "/user/getToken"
         * docs "http://rongcloud.cn/docs/server.html#getToken"
         *
         * @param user 用户信息 id,name,portrait(必传)
         *
         * @return TokenResult
         **/
        public TokenResult Register(UserModel user)
        {
            //需要校验的字段
            String message = CommonUtil.CheckFiled(user, PATH, CheckMethod.REGISTER);
            if (null != message)
            {
                return (TokenResult)RongJsonUtil.JsonStringToObj<TokenResult>(message);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("&userId=").Append(HttpUtility.UrlEncode(user.id.ToString(), UTF8));
            sb.Append("&name=").Append(HttpUtility.UrlEncode(user.name.ToString(), UTF8));
            if (!string.IsNullOrWhiteSpace(user.portrait))
            {
                sb.Append("&portraitUri=").Append(HttpUtility.UrlEncode(user.portrait.ToString(), UTF8));
            }
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }

            String result = RongHttpClient.ExecutePost(appKey, appSecret, body,
                    rongCloud.ApiHostType.Type + "/user/getToken.json", "application/x-www-form-urlencoded");

            return (TokenResult)RongJsonUtil.JsonStringToObj<TokenResult>(CommonUtil.GetResponseByCode(PATH, CheckMethod.REGISTER, result));
        }

        /**
         * 刷新用户信息方法 
         * url  "/user/refresh"
         * docs "http://www.rongcloud.cn/docs/server.html#user_refresh"
         *
         * @param user 用户信息 id name portrait(必传)
         *
         * @return ResponseResult
         **/
        public Result Update(UserModel user)
        {
            //需要校验的字段
            String message = CommonUtil.CheckFiled(user, PATH, CheckMethod.UPDATE);
            if (null != message)
            {
                return (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(message);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("&userId=").Append(HttpUtility.UrlEncode(user.id.ToString(), UTF8));

            if (user.name != null)
            {
                sb.Append("&name=").Append(HttpUtility.UrlEncode(user.name.ToString(), UTF8));
            }

            if (user.portrait != null)
            {
                sb.Append("&portraitUri=").Append(HttpUtility.UrlEncode(user.portrait.ToString(), UTF8));
            }
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body,
                    RongCloud.ApiHostType.Type + "/user/refresh.json", "application/x-www-form-urlencoded");

            return (ResponseResult)RongJsonUtil.JsonStringToObj<ResponseResult>(CommonUtil.GetResponseByCode(PATH, CheckMethod.UPDATE, result));
        }

        /**
        * 查询用户信息方法 
        * url  "/user/info"
        * docs "https://www.rongcloud.cn/docs/server.html#user_info"
        *
        * @param userId 用户 Id（必传）
        *
        * @return ResponseResult
        **/
        public UserInfoResult Get(string userId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("&userId=").Append(HttpUtility.UrlEncode(userId, UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body,
                    RongCloud.ApiHostType.Type + "/user/info.json", "application/x-www-form-urlencoded");
            var model = RongJsonUtil.JsonStringToObj<UserInfoResult>(result);
            model.id = userId;
            return model;
        }

        /// <summary>
        /// 获取用户群组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MyGroupsModel GetGroups(string userId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("&userId=").Append(HttpUtility.UrlEncode(userId, UTF8));
            String body = sb.ToString();
            if (body.IndexOf("&") == 0)
            {
                body = body.Substring(1, body.Length - 1);
            }
            String result = RongHttpClient.ExecutePost(appKey, appSecret, body,
                    RongCloud.ApiHostType.Type + "/user/group/query.json", "application/x-www-form-urlencoded");
            var model = RongJsonUtil.JsonStringToObj<MyGroupsModel>(result);
            return model;
        }
    }
}
