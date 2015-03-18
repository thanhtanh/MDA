using MobileDatingCMS.Models;
using MobileDatingCMS.Models.Business;
using MobileDatingCMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingCMS.Controllers
{

    [Authorize]
    public class HobbyController : Controller
    {

        protected HobbyBusiness hobbyBusiness = new HobbyBusiness();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int? id)
        {
            var hobbies = this.hobbyBusiness.GetHobbies(id);

            var result = new List<HobbyTreeItemViewModels>();
            foreach (var hobby in hobbies)
            {
                result.Add(new HobbyTreeItemViewModels(hobby));
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddHobby()
        {
            return this.View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult AddHobby(HobbyViewModels model)
        {
            if (!model.ValidateModel(this.ModelState))
            {
                return this.View(model);
            }

            this.hobbyBusiness.AddHobby(model);

            return this.RedirectToAction("Index");
        }

    }
}