using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MobileDatingCMS.Models;
using MobileDatingCMS.Models.Business;
using MobileDatingCMS.Models.ViewModels;
using System.Diagnostics;
using System.Net;

namespace MobileDatingCMS.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AccountController : BaseController
    {

        #region Constants

        private static readonly string[] InitializingRoles = { "Admin", "Manager", "User" };

        #endregion

        #region Business

        private AccountBusiness accountBusiness = new AccountBusiness();

        #endregion

        public ActionResult Index()
        {
            var accountList = this.accountBusiness.GetAllAccountList().ToList();

            return this.View(accountList);
        }

        public ActionResult ToggleBlocking(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.accountBusiness.ToggleBlocking(id);

            return this.RedirectToAction("Index");
        }

        public ActionResult AddAccount()
        {
            return this.View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> AddAccount(AccountViewModels model)
        {
            if (!model.ValidateModel(this.ModelState))
            {
                return this.View(model);
            }

            var message = await this.accountBusiness.AddAccount(model);

            if (message == null)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                model.ErrorMessage = message;
                return this.View(model);
            }
        }

        public ActionResult EditAccount(string id)
        {
            var user = this.accountBusiness.GetUserById(id, false);
            var model = new AccountViewModels(user);

            return this.View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> EditAccount(AccountViewModels model)
        {
            if (!model.ValidateModel(this.ModelState))
            {
                return this.View(model);
            }

            var message = await this.accountBusiness.EditAccount(model);

            if (message == null)
            {
                return this.RedirectToAction("Index");
            }
            else
            {
                model.ErrorMessage = message;
                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string message = await this.accountBusiness.DeleteAccount(id);

            if (!string.IsNullOrEmpty(message))
            {
                return this.View(message);
            }

            return this.RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<ActionResult> InitializeIdentity()
        {
            #region Create missing roles

            foreach (var role in InitializingRoles)
            {
                if (!this.dc.AspNetRoles.Any(q => q.Id.Equals(role)))
                {
                    var roleEntity = new AspNetRole()
                    {
                        Id = role,
                        Name = role,
                    };

                    this.dc.AspNetRoles.Add(roleEntity);
                }
            }

            this.dc.SaveChanges();

            #endregion

            #region Create Admin account if none exist

            if (!dc.AspNetUsers.Any(q => q.UserName.Equals("admin")))
            {
                var adminUser = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "datvm@outlook.com",
                };
                var result = await AuthenUtils.UserManager.CreateAsync(adminUser, "123456aA@");
                if (result.Errors.Count() > 0)
                {
                    throw new Exception(result.Errors.First());
                }
            }

            #endregion

            #region Set Admin Role

            var adminAccount = AuthenUtils.UserManager.FindByName("admin");
            foreach (var role in InitializingRoles)
            {
                if (!AuthenUtils.UserManager.IsInRole(adminAccount.Id, role))
                {
                    AuthenUtils.UserManager.AddToRole(adminAccount.Id, role);
                }
            }

            #endregion

            return this.RedirectToAction("");
        }

    }


}