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
    
    public partial class Location
    {
        public Location()
        {
            this.Location1 = new HashSet<Location>();
            this.UserLocationHistories = new HashSet<UserLocationHistory>();
        }
    
        public int ID { get; set; }
        public int ParentLocationID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<Location> Location1 { get; set; }
        public virtual Location Location2 { get; set; }
        public virtual ICollection<UserLocationHistory> UserLocationHistories { get; set; }
    }
}