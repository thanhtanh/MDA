using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileDatingCMS.Models.ViewModels
{

    public class AccountViewModels : BaseViewModels
    {

        [Required]
        public new string ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public AccountViewModels() { }

        public AccountViewModels(AspNetUser baseInfo) : this()
        {
            this.ID = baseInfo.Id;
            this.Username = baseInfo.UserName;
            this.FullName = baseInfo.FullName;
            this.Email = baseInfo.Email;
            this.Active = !baseInfo.LockoutEnabled;
        }


    }

}