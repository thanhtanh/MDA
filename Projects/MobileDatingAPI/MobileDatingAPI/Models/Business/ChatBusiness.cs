using MobileDatingAPI.Models.Chat;
using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Business
{
    public class ChatBusiness : BaseBusiness
    {

        public static readonly ChatServerCache ChatServerCache = new ChatServerCache();

    }
}