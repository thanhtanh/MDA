using MobileDatingAPI.Models;
using MobileDatingAPI.Models.Business;
using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingAPI.Controllers
{
    public class HomeController : Controller
    {

        private HomeBusiness homeBusiness = new HomeBusiness();

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult GetPlainInfoLists(string token)
        {
            var result = new PlainInfoListViewModels();

            var user = this.GetUserFromTokenID(token, result);
            if (user == null) { return this.Json(result); }

            result = this.homeBusiness.GetPlainInfoLists(result);

            return this.Json(result);
        }

    }
}