using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MobileDatingAPI.Models.Business
{
    public class LogInTokenBusiness : BaseBusiness
    {

        private const int TokenLength = 32;
        // Note: only upper characters are valid
        private static readonly char[] AvailableCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                                                               'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                                                               'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7',
                                                               '8', '9', '0', };
        /// <summary>
        /// Unit: minute
        /// </summary>
        private const int TokenLifeTime = 15 * 24 * 60;

        public LogInToken FindToken(string token)
        {
            return this.ApiEntities.LogInTokens
                .Where(q => q.Active && q.ExpirationTime > DateTime.Now && q.Token.Equals(token))
                .FirstOrDefault();
        }


        public LogInTokenCache GenerateLogInToken(string userID)
        {
            string token;

            do
            {
                token = Utils.GenerateRandomToken(TokenLength, AvailableCharacters);
            } while (this.ApiEntities.LogInTokens.Any(q => q.Token.Equals(token)));

            var expirationTime = DateTime.Now.AddMinutes(TokenLifeTime);

            LogInTokenCache cache = new LogInTokenCache()
            {
                Token = token,
                UserID = userID,
                ExpirationTime = expirationTime,
            };

            LogInToken entity = new LogInToken()
            {
                Token = token,
                UserId = userID,
                ExpirationTime = expirationTime,
                Active = true,
            };
            this.ApiEntities.LogInTokens.Add(entity);
            this.ApiEntities.SaveChanges();

            return cache;
        }

        public bool DeactiveToken(string token)
        {
            var entity = this.ApiEntities.LogInTokens
                .Where(q => q.Active && q.ExpirationTime > DateTime.Now && q.Token.Equals(token))
                .FirstOrDefault();

            if (entity != null)
            {
                entity.Active = false;
                this.ApiEntities.SaveChanges();

                return true;
            }

            return false;
        }

        public void CleanUp()
        {
            this.ApiEntities.CleanUpLogInTokens();
        }

    }
}