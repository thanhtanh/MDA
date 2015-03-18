using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingAPI.Models
{
    public static class ControllerExtension
    {

        public static bool ValidateNullParams(this  Controller controller, params string[] parameters)
        {
            foreach (var param in parameters)
            {
                if (string.IsNullOrEmpty(param))
                {
                    return false;
                }
            }

            return true;
        }

        public static ActionResult ReturnBadRequest(this Controller controller)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

    }
}