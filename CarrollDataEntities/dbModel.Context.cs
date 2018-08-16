﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Carroll.Data.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CarrollFormsEntities : DbContext
    {
        public CarrollFormsEntities()
            : base("name=CarrollFormsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<EquityPartner> EquityPartners { get; set; }
        public virtual DbSet<FormGeneralLiabilityClaim> FormGeneralLiabilityClaims { get; set; }
        public virtual DbSet<FormMoldDamageClaim> FormMoldDamageClaims { get; set; }
        public virtual DbSet<FormPropertyDamageClaim> FormPropertyDamageClaims { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserInProperty> UserInProperties { get; set; }
        public virtual DbSet<UserInRole> UserInRoles { get; set; }
        public virtual DbSet<SiteUser> SiteUsers { get; set; }
        public virtual DbSet<view_getallclaims> view_getallclaims { get; set; }
        public virtual DbSet<FormAttachment> FormAttachments { get; set; }
        public virtual DbSet<FormComment> FormComments { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
    
        public virtual ObjectResult<sp_GetUserProperties_Result> sp_GetUserProperties()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetUserProperties_Result>("sp_GetUserProperties");
        }
    
        public virtual ObjectResult<sp_GetUserRoles_Result> sp_GetUserRoles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetUserRoles_Result>("sp_GetUserRoles");
        }
    
        public virtual ObjectResult<proc_GetGeneralLiabilityClaims_Result> proc_GetGeneralLiabilityClaims()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_GetGeneralLiabilityClaims_Result>("proc_GetGeneralLiabilityClaims");
        }
    
        public virtual ObjectResult<proc_GetMoldDamageClaims_Result> proc_GetMoldDamageClaims()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_GetMoldDamageClaims_Result>("proc_GetMoldDamageClaims");
        }
    
        public virtual ObjectResult<proc_GetPropertyDamageClaims_Result> proc_GetPropertyDamageClaims()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_GetPropertyDamageClaims_Result>("proc_GetPropertyDamageClaims");
        }
    
        public virtual ObjectResult<SP_GetAllClaims_Result> SP_GetAllClaims(Nullable<System.Guid> userid, Nullable<System.Guid> propertyid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            var propertyidParameter = propertyid.HasValue ?
                new ObjectParameter("propertyid", propertyid) :
                new ObjectParameter("propertyid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetAllClaims_Result>("SP_GetAllClaims", useridParameter, propertyidParameter);
        }
    
        public virtual ObjectResult<spProperties_Result> spProperties()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spProperties_Result>("spProperties");
        }
    }
}
