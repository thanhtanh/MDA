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
    
    public partial class LogInToken
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public System.DateTime ExpirationTime { get; set; }
        public bool Active { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
