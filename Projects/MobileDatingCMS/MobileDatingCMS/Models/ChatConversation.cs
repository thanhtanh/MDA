//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileDatingCMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChatConversation
    {
        public ChatConversation()
        {
            this.ChatItems = new HashSet<ChatItem>();
        }
    
        public int ID { get; set; }
        public string UserID { get; set; }
        public string TargetUserID { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        public virtual ICollection<ChatItem> ChatItems { get; set; }
    }
}
