﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileDatingAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MobileDatingApiEntities : DbContext
    {
        public MobileDatingApiEntities()
            : base("name=MobileDatingApiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<FriendList> FriendLists { get; set; }
        public virtual DbSet<FriendListRequest> FriendListRequests { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Hobby> Hobbies { get; set; }
        public virtual DbSet<InterestedIn> InterestedIns { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LogInToken> LogInTokens { get; set; }
        public virtual DbSet<NotificationCategory> NotificationCategories { get; set; }
        public virtual DbSet<RelationshipStatu> RelationshipStatus { get; set; }
        public virtual DbSet<ReligiousView> ReligiousViews { get; set; }
        public virtual DbSet<UserHobbyMapping> UserHobbyMappings { get; set; }
        public virtual DbSet<UserLocationHistory> UserLocationHistories { get; set; }
        public virtual DbSet<UserMatching> UserMatchings { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        public virtual DbSet<UserPhoto> UserPhotoes { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserStatus> UserStatuses { get; set; }
    
        public virtual int CleanUpLogInTokens()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CleanUpLogInTokens");
        }
    }
}
