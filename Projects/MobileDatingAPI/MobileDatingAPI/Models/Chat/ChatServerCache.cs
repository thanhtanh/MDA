using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Chat
{

    public class ChatServerCache
    {

        private Dictionary<string, Dictionary<string, string>> ChatConversations { get; set; }

        public ChatServerCache()
        {

        }

    }

}