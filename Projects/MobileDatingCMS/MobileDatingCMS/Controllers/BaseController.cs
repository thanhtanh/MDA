using MobileDatingCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingCMS.Controllers
{
    public abstract class BaseController : Controller
    {

        protected CmsEntities dc = new CmsEntities();

    }
}