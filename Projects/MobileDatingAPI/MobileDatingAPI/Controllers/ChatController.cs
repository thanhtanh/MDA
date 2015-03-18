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
    public class ChatController : Controller
    {

        private ChatBusiness business = new ChatBusiness();

        [HttpPost]
        public ActionResult SendMessage(string token, string userId, string content)
        {


            return null;
        }

    }
}