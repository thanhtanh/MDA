using MobileDatingCMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MobileDatingCMS.Models.Business
{

    public class AccountBusiness : BaseBusiness
    {

        public IEnumerable<AccountViewModels> GetAccountList()
        {
            return this.CmsEntities.AspNetUsers
                .Where(q => !q.LockoutEnabled)
                .Select(q => new AccountViewModels()
                {
                    ID = q.Id,
                    Username = q.UserName,
                    Email = q.Email,
                    FullName = q.FullName,
                    Active = !q.LockoutEnabled,
                });
        }

        public IEnumerable<AccountViewModels> GetAllAccountList()
        {
            return this.CmsEntities.AspNetUsers
                .Select(q => new AccountViewModels()
                {
                    ID = q.Id,
                    Username = q.UserName,
                    Email = q.Email,
                    FullName = q.FullName,
                    Active = !q.LockoutEnabled,
                });
        }

        public bool ToggleBlocking(string id)
        {
            var entity = this.CmsEntities.AspNetUsers.Where(q => q.Id.Equals(id)).FirstOrDefault();

            if (entity == null) { return false; }

            entity.LockoutEnabled = !entity.LockoutEnabled;
            this.CmsEntities.SaveChanges();

            return true;
        }

        public async Task<string> AddAccount(AccountViewModels model)
        {
            try
            {

                #region Create Application User

                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.Username,
                        Email = model.Email,
                    };

                    var result = await AuthenUtils.UserManager.CreateAsync(user, model.Password);
                    if (result.Errors.Count() > 0)
                    {
                        return result.Errors.First();
                    }
                }


                #endregion

                #region Assign Role

                {
                    var user = await AuthenUtils.UserManager.FindByNameAsync(model.Username);

                    var result = await AuthenUtils.UserManager.AddToRoleAsync(user.Id, "User");
                    if (result.Errors.Count() > 0)
                    {
                        return result.Errors.First();
                    }
                }

                #endregion

                #region Create Profile

                {
                    var user = this.CmsEntities.AspNetUsers.Where(q => q.UserName.Equals(model.Username)).First();

                    user.FullName = model.FullName;

                    UserProfile profile = new UserProfile()
                    {
                        UserID = user.Id,
                    };

                    this.CmsEntities.UserProfiles.Add(profile);

                    this.CmsEntities.SaveChanges();
                }

                #endregion

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> EditAccount(AccountViewModels model)
        {
            try
            {
                #region Change Email

                {
                    var result = await AuthenUtils.UserManager.SetEmailAsync(model.ID, model.Email);
                    if (result.Errors.Count() > 0)
                    {
                        return result.Errors.First();
                    }
                }

                #endregion

                #region Change Name

                {
                    var user = this.GetUserById(model.ID, false);

                    user.FullName = model.FullName;

                    this.CmsEntities.SaveChanges();
                }

                #endregion

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> DeleteAccount(string id)
        {
            try
            {
                ApplicationUser user = await AuthenUtils.UserManager.FindByIdAsync(id);

                var result = await AuthenUtils.UserManager.DeleteAsync(user);
                if (result.Errors.Count() > 0)
                {
                    return result.Errors.First();
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public AspNetUser GetUserById(string id, bool requiredActive = true)
        {
            if (requiredActive)
            {
                return this.CmsEntities.AspNetUsers.Where(q => !q.LockoutEnabled && q.Id.Equals(id)).FirstOrDefault();
            }
            else
            {
                return this.CmsEntities.AspNetUsers.Where(q => q.Id.Equals(id)).FirstOrDefault();
            }

        }

    }

}