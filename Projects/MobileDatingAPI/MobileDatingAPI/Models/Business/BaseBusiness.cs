using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Business
{
    public class BaseBusiness
    {

        public MobileDatingApiEntities ApiEntities
        {
            get
            {
                if (System.Web.HttpContext.Current.Items["MobileDatingApiEntities"] == null)
                    System.Web.HttpContext.Current.Items["MobileDatingApiEntities"] = new MobileDatingApiEntities();
                return (MobileDatingApiEntities)System.Web.HttpContext.Current.Items["MobileDatingApiEntities"];
            }
        }

    }
}