using MobileDatingAPI.Models.Business;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models
{

    public class LoginTokenManager
    {

        protected ConcurrentDictionary<string, LogInTokenCache> TokenCache { get; set; }

        public LoginTokenManager()
        {
            this.TokenCache = new ConcurrentDictionary<string, LogInTokenCache>();
        }

        public LogInTokenCache GetUserIdFromToken(string token)
        {
            LogInTokenCache item = null;

            if (this.TokenCache.ContainsKey(token))
            {
                item = this.TokenCache[token];

                if (item.ExpirationTime < DateTime.Now)
                {
                    this.TokenCache.TryRemove(item.Token, out item);
                    return null;
                }
            }
            else
            {
                LogInTokenBusiness tokenBusiness = new LogInTokenBusiness();

                var entity = tokenBusiness.FindToken(token);
                if (entity != null && entity.Token != null)
                {
                    item = new LogInTokenCache()
                    {
                        Token = entity.Token,
                        UserID = entity.UserId,
                        ExpirationTime = entity.ExpirationTime,
                    };

                    this.TokenCache.TryAdd(item.Token, item);
                }
            }

            return item;
        }

        public LogInTokenCache GenerateLoginToken(string userID)
        {
            var business = new LogInTokenBusiness();

            var token = business.GenerateLogInToken(userID);
            this.TokenCache.TryAdd(token.Token, token);

            return token;
        }

        public void DestroyToken(string token)
        {
            var business = new LogInTokenBusiness();

            business.DeactiveToken(token);

            if (this.TokenCache.ContainsKey(token))
            {
                LogInTokenCache temp;
                this.TokenCache.TryRemove(token, out temp);
            }
        }

    }

    public class LogInTokenCache
    {

        public string Token { get; set; }
        public string UserID { get; set; }
        public DateTime ExpirationTime { get; set; }

    }

}