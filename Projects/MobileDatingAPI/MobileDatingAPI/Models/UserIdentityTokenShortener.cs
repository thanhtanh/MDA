using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MobileDatingAPI.Models
{

    public class UserIdentityTokenShortener
    {
        
        #region Constants

        // Note: only upper characters are valid
        private static readonly char[] AvailableCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                                                               'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                                                               'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7',
                                                               '8', '9', '0', };
        private const int TokenLength = 5;

        /// <summary>
        /// Unit: minute
        /// </summary>
        private const int TokenLifetime = 60;

        #endregion

        private Dictionary<string, UserIdentityTokenShorteningInfo> tokenDictionary = new Dictionary<string, UserIdentityTokenShorteningInfo>();

        public UserIdentityTokenShortener() { }

        public UserIdentityTokenShorteningInfo Shorten(string originalToken)
        {
            string token;

            do
            {
                token = UserIdentityTokenShortener.GenerateToken();
            } while (this.tokenDictionary.ContainsKey(token));

            UserIdentityTokenShorteningInfo result = new UserIdentityTokenShorteningInfo()
            {
                Token = token,
                OriginalToken = originalToken,
                ExpirationTime = DateTime.Now.AddMinutes(TokenLifetime),
            };

            this.tokenDictionary.Add(token, result);

            return result;
        }

        public UserIdentityTokenShorteningInfo WithdrawToken(string token)
        {
            token = token.ToUpper();

            if (this.tokenDictionary.ContainsKey(token))
            {
                var result = this.tokenDictionary[token];

                if (result.ExpirationTime < DateTime.Now) { return null; }

                return result;
            }
            else
            {
                return null;
            }
        }

        public static string GenerateToken()
        {
            return Utils.GenerateRandomToken(TokenLength, AvailableCharacters);
        }

    }

    public class UserIdentityTokenShorteningInfo
    {
        public string Token { get; set; }
        public string OriginalToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }

}