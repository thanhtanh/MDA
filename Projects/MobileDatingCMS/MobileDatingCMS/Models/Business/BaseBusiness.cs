using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingCMS.Models.Business
{
    public class BaseBusiness
    {

        public CmsEntities CmsEntities
        {
            get
            {
                if (System.Web.HttpContext.Current.Items["CmsEntities"] == null)
                    System.Web.HttpContext.Current.Items["CmsEntities"] = new CmsEntities();
                return (CmsEntities)System.Web.HttpContext.Current.Items["CmsEntities"];
            }
        }

    }
}