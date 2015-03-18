using MobileDatingAPI.Controllers;
using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingAPI.Models
{

    public static class Utils
    {

        public static Random random = new Random();

        public static string MaskEmailAddress(string origin)
        {
            StringBuilder result = new StringBuilder();
            bool masked = true;

            foreach (var c in origin)
            {
                if (masked && c == '@')
                {
                    masked = false;
                }

                if (masked)
                {
                    result.Append('*');
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public static string GenerateRandomToken(int tokenLength, char[] availableCharacters)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < tokenLength; i++)
            {
                result.Append(availableCharacters[random.Next(availableCharacters.Length)]);
            }

            return result.ToString();
        }

        public static string GetUserFromTokenID(this Controller controller, string token, BaseApiViewModels model)
        {
            if (string.IsNullOrEmpty(token))
            {
                if (model != null)
                {
                    model.ReturnError("Tài khoản đăng nhập không đúng hoặc đã hết hạn.", 403);
                }
                

                return null;
            }

            var tokenCache = IdentityController.LoginTokenManager.GetUserIdFromToken(token);
            if (tokenCache == null)
            {
                if (model != null)
                {
                    model.ReturnError("Tài khoản đăng nhập không đúng hoặc đã hết hạn.", 403);
                }
                
                return null;
            }

            return tokenCache.UserID;
         }

    }

}