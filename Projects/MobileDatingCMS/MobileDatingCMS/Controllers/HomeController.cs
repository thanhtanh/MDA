using Microsoft.AspNet.Identity.Owin;
using MobileDatingCMS.Models;
using MobileDatingCMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingCMS.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password, string returnUrl)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var loginResult = await AuthenUtils.SignInManager.PasswordSignInAsync(username, password, false, shouldLockout: false);

            var result = new LoginResultViewModels();

            switch (loginResult)
            {
                case SignInStatus.Success:
                    result.Succeeded = true;
                    result.ReturnUrl = this.RedirectUrlToLocal(returnUrl);
                    break;
                case SignInStatus.LockedOut:
                    result.Succeeded = false;
                    result.Error = "Tài khoản này đã bị khóa.";
                    break;
                case SignInStatus.Failure:
                default:
                    result.Succeeded = false;
                    result.Error = "Đăng nhập thất bại.";

                    break;
            }

            return this.Json(result);
        }

        public ActionResult LogOut()
        {
            AuthenUtils.AuthenticationManager.SignOut();

            return this.RedirectToAction("Index", "Home");
        }

        #region Helper Methods

        private string RedirectUrlToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }
            return this.Url.Action("Index", "Home");
        }

        #endregion


    }
}