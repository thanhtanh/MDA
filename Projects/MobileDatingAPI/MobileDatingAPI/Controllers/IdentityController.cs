using MobileDatingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Principal;
using Microsoft.Owin.Security;
using MobileDatingAPI.Models.ViewModels;
using System.Threading.Tasks;
using MobileDatingAPI.Models.Business;


namespace MobileDatingAPI.Controllers
{
    public class IdentityController : Controller
    {

        private static EmailAdpater EmailAdapter = new EmailAdpater();
        private static UserIdentityTokenShortener TokenShortener = new UserIdentityTokenShortener();
        public static LoginTokenManager LoginTokenManager = new LoginTokenManager();

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            var result = new LogInViewModels();

            var userManager = AuthenUtils.UserManager;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return this.Json(result.ReturnError("Sai tên đăng nhập hoặc mật khẩu.", 1));
            }

            var user = userManager.FindByName(username);
            if (user == null)
            {
                return this.Json(result.ReturnError("Sai tên đăng nhập hoặc mật khẩu.", 1));
            }

            var pwCheckResult = userManager.CheckPassword(user, password);

            if (pwCheckResult)
            {
                var token = IdentityController.LoginTokenManager.GenerateLoginToken(user.Id);

                if (token == null)
                {
                    return this.Json(result.ReturnError("Đăng nhập thất bại. Vui lòng thử lại sau vài giây.", 2));
                }

                result.LoginToken = token.Token;

                // Activate user
                UserProfileBusiness.ActiveUserManger.ActivateUser(user.Id, token.Token);

                return this.Json(result);
            }
            else
            {
                return this.Json(result.ReturnError("Sai tên đăng nhập hoặc mật khẩu.", 1));
            }


        }

        [HttpPost]
        public async Task<ActionResult> RecoverPassword(string username)
        {
            var result = new RecoverPasswordViewModels();

            ApplicationUser user = null;
            var userManager = AuthenUtils.UserManager;

            if (!string.IsNullOrEmpty(username))
            {
                user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(username);
                }
            }

            if (user == null)
            {
                return this.Json(result.ReturnError("Tên đăng nhập hoặc email không tồn tại", 1));
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var shortenedToken = IdentityController.TokenShortener.Shorten(token);

            result.RecoveryEmail = Utils.MaskEmailAddress(user.Email);
            IdentityController.EmailAdapter.SendEmail(
                "Mã xác nhận thay đổi mật khẩu",
                "Mã xác nhận thay đổi mật khẩu bạn yêu cầu là: " + shortenedToken.Token,
                user.Email);

            return this.Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(string token, string username, string newPw, string confirmPw)
        {
            var result = new ResetPasswordViewModels();

            if (string.IsNullOrEmpty(token))
            {
                result.ValidationFailedFields.Add(new BaseApiValidationViewModels.FieldValidationFailure() { Name = "lblTolenStatus", Message = "Mã xác nhận không chính xác.", });

                return this.Json(result.ReturnError("Thông tin chưa đầy đủ.", 1));
            }

            if (string.IsNullOrEmpty(newPw))
            {
                result.ValidationFailedFields.Add(new BaseApiValidationViewModels.FieldValidationFailure() { Name = "lblNewPwStatus", Message = "Mật khẩu mới quá ngắn.", });

                return this.Json(result.ReturnError("Thông tin chưa đầy đủ.", 1));
            }

            if (string.IsNullOrEmpty(confirmPw) || !newPw.Equals(confirmPw))
            {
                result.ValidationFailedFields.Add(new BaseApiValidationViewModels.FieldValidationFailure() { Name = "lblConfirmPwStatus", Message = "Xác nhận mật khẩu không trùng.", });

                return this.Json(result.ReturnError("Thông tin chưa đầy đủ.", 1));
            }

            ApplicationUser user = null;
            var userManager = AuthenUtils.UserManager;

            if (!string.IsNullOrEmpty(username))
            {
                user = await userManager.FindByNameAsync(username);

                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(username);
                }
            }

            if (user == null)
            {
                return this.Json(result.ReturnError("Tên đăng nhập hoặc email không tồn tại", 2));
            }

            var originalToken = IdentityController.TokenShortener.WithdrawToken(token);
            if (originalToken == null)
            {
                result.ValidationFailedFields.Add(new BaseApiValidationViewModels.FieldValidationFailure() { Name = "lblTolenStatus", Message = "Mã xác nhận không chính xác.", });

                return this.Json(result.ReturnError("Thông tin chưa đầy đủ.", 1));
            }
            

            var resetResult = await AuthenUtils.UserManager.ResetPasswordAsync(user.Id, originalToken.OriginalToken, newPw);

            if (!resetResult.Succeeded)
            {
                return this.Json(result.ReturnError(resetResult.Errors.First(), 3));
            }

            return this.Json(result);
        }

        [HttpPost]
        public async  Task<ActionResult> Register(string username, string email, string password, string confirmPw)
        {
            var result = new BaseApiValidationViewModels();

            var userManager = AuthenUtils.UserManager;

            ApplicationUser user = new ApplicationUser()
            {
                UserName = username,
                Email = email,
                
            };

            var opResult = await userManager.CreateAsync(user, password);

            if (!opResult.Succeeded)
            {
                return this.Json(result.ReturnError(opResult.Errors.First(), 1));
            }
            else
            {
            }

            return this.Json(result);
        }

        

    }
}