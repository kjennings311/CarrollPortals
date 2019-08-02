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
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<RequisitionRequest> RequisitionRequests { get; set; }
        public virtual DbSet<EmployeeLeaseRaider> EmployeeLeaseRaiders { get; set; }
        public virtual DbSet<EquityPartnerContact> EquityPartnerContacts { get; set; }
        public virtual DbSet<ExpenseReimbursementDetail> ExpenseReimbursementDetails { get; set; }
        public virtual DbSet<ExpenseReimbursementHeader> ExpenseReimbursementHeaders { get; set; }
        public virtual DbSet<MileageLogDetail> MileageLogDetails { get; set; }
        public virtual DbSet<MileageLogHeader> MileageLogHeaders { get; set; }
        public virtual DbSet<CarrollPayPeriod> CarrollPayPeriods { get; set; }
        public virtual DbSet<CarrollPosition> CarrollPositions { get; set; }
        public virtual DbSet<PayrollStatusChangeNotice> PayrollStatusChangeNotices { get; set; }
        public virtual DbSet<EmployeeNewHireNotice> EmployeeNewHireNotices { get; set; }
        public virtual DbSet<NoticeOfEmployeeSeperation> NoticeOfEmployeeSeperations { get; set; }
        public virtual DbSet<ResidentReferalSheet> ResidentReferalSheets { get; set; }
        public virtual DbSet<ResidentContactInformation> ResidentContactInformations { get; set; }
        public virtual DbSet<ResidentContactInformation_OtherOccupants> ResidentContactInformation_OtherOccupants { get; set; }
        public virtual DbSet<ResidentContactInformation_Residents> ResidentContactInformation_Residents { get; set; }
        public virtual DbSet<ResidentContactInformation_Vehicles> ResidentContactInformation_Vehicles { get; set; }
        public virtual DbSet<DynamicLink> DynamicLinks { get; set; }
        public virtual DbSet<ActivityLogHrForm> ActivityLogHrForms { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
    
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
    
        public virtual int SP_GetAllClaims(Nullable<System.Guid> userid, Nullable<System.Guid> propertyid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            var propertyidParameter = propertyid.HasValue ?
                new ObjectParameter("propertyid", propertyid) :
                new ObjectParameter("propertyid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_GetAllClaims", useridParameter, propertyidParameter);
        }
    
        public virtual ObjectResult<spProperties_Result> spProperties()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spProperties_Result>("spProperties");
        }
    
        public virtual ObjectResult<proc_getworkflowemails_Result> proc_getworkflowemails(Nullable<System.Guid> propid)
        {
            var propidParameter = propid.HasValue ?
                new ObjectParameter("propid", propid) :
                new ObjectParameter("propid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getworkflowemails_Result>("proc_getworkflowemails", propidParameter);
        }
    
        public virtual ObjectResult<SP_GetAllClaims1_Result> SP_GetAllClaims1(Nullable<System.Guid> userid, Nullable<System.Guid> propertyid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            var propertyidParameter = propertyid.HasValue ?
                new ObjectParameter("propertyid", propertyid) :
                new ObjectParameter("propertyid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetAllClaims1_Result>("SP_GetAllClaims1", useridParameter, propertyidParameter);
        }
    
        public virtual ObjectResult<proc_getpropertydetails_Result> proc_getpropertydetails(Nullable<System.Guid> propid)
        {
            var propidParameter = propid.HasValue ?
                new ObjectParameter("propid", propid) :
                new ObjectParameter("propid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getpropertydetails_Result>("proc_getpropertydetails", propidParameter);
        }
    
        public virtual ObjectResult<proc_getallpayrollstatuschange_Result> proc_getallpayrollstatuschange()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallpayrollstatuschange_Result>("proc_getallpayrollstatuschange");
        }
    
        public virtual ObjectResult<proc_getallrequisitionrequests_Result> proc_getallrequisitionrequests()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallrequisitionrequests_Result>("proc_getallrequisitionrequests");
        }
    
        public virtual ObjectResult<proc_getallmonthlyexpensedetails_Result> proc_getallmonthlyexpensedetails(Nullable<System.Guid> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallmonthlyexpensedetails_Result>("proc_getallmonthlyexpensedetails", useridParameter);
        }
    
        public virtual ObjectResult<proc_getallexpensemileagelogs_Result> proc_getallexpensemileagelogs(Nullable<System.Guid> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallexpensemileagelogs_Result>("proc_getallexpensemileagelogs", useridParameter);
        }
    
        public virtual ObjectResult<proc_getallcarrollpayperiods_Result> proc_getallcarrollpayperiods()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallcarrollpayperiods_Result>("proc_getallcarrollpayperiods");
        }
    
        public virtual ObjectResult<proc_getallcarrollpositions_Result> proc_getallcarrollpositions()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallcarrollpositions_Result>("proc_getallcarrollpositions");
        }
    
        public virtual int proc_getallemployeenewhirenotice()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_getallemployeenewhirenotice");
        }
    
        public virtual ObjectResult<string> proc_getpayperiodsforcurrentyear()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("proc_getpayperiodsforcurrentyear");
        }
    
        public virtual ObjectResult<proc_getallresidentreferalsheets_Result> proc_getallresidentreferalsheets()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallresidentreferalsheets_Result>("proc_getallresidentreferalsheets");
        }
    
        public virtual ObjectResult<proc_getallresidentreferals_Result> proc_getallresidentreferals(Nullable<System.Guid> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallresidentreferals_Result>("proc_getallresidentreferals", useridParameter);
        }
    
        public virtual ObjectResult<Proc_getallresidentcontacts_Result> Proc_getallresidentcontacts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_getallresidentcontacts_Result>("Proc_getallresidentcontacts");
        }
    
        public virtual ObjectResult<proc_getallemployeenewhirenotice1_Result1> proc_getallemployeenewhirenotice1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallemployeenewhirenotice1_Result1>("proc_getallemployeenewhirenotice1");
        }
    
        public virtual ObjectResult<proc_getallemployeenewhirenoticenew_Result> proc_getallemployeenewhirenoticenew()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallemployeenewhirenoticenew_Result>("proc_getallemployeenewhirenoticenew");
        }
    
        public virtual ObjectResult<proc_getallemployeeleaseriders_Result> proc_getallemployeeleaseriders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallemployeeleaseriders_Result>("proc_getallemployeeleaseriders");
        }
    
        public virtual ObjectResult<proc_getallnoticeofemployeeseparation_Result> proc_getallnoticeofemployeeseparation()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallnoticeofemployeeseparation_Result>("proc_getallnoticeofemployeeseparation");
        }
    
        public virtual ObjectResult<proc_getcontactsforexcel_Result1> proc_getcontactsforexcel()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getcontactsforexcel_Result1>("proc_getcontactsforexcel");
        }
    
        public virtual ObjectResult<proc_getequitypartnersforexcel_Result1> proc_getequitypartnersforexcel()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getequitypartnersforexcel_Result1>("proc_getequitypartnersforexcel");
        }
    
        public virtual ObjectResult<proc_getpropertydetailsforupdate_Result> proc_getpropertydetailsforupdate(Nullable<System.Guid> propertyid)
        {
            var propertyidParameter = propertyid.HasValue ?
                new ObjectParameter("propertyid", propertyid) :
                new ObjectParameter("propertyid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getpropertydetailsforupdate_Result>("proc_getpropertydetailsforupdate", propertyidParameter);
        }
    
        public virtual ObjectResult<proc_getpropertiesforexcel_Result2> proc_getpropertiesforexcel()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getpropertiesforexcel_Result2>("proc_getpropertiesforexcel");
        }
    
        public virtual ObjectResult<proc_getpropertiesforexcelupdate_Result> proc_getpropertiesforexcelupdate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getpropertiesforexcelupdate_Result>("proc_getpropertiesforexcelupdate");
        }
    
        public virtual ObjectResult<proc_getequitypartners_Result> proc_getequitypartners()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getequitypartners_Result>("proc_getequitypartners");
        }
    
        public virtual int proc_gethrformsactivity(string formtype, Nullable<System.Guid> refid)
        {
            var formtypeParameter = formtype != null ?
                new ObjectParameter("formtype", formtype) :
                new ObjectParameter("formtype", typeof(string));
    
            var refidParameter = refid.HasValue ?
                new ObjectParameter("refid", refid) :
                new ObjectParameter("refid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_gethrformsactivity", formtypeParameter, refidParameter);
        }
    
        public virtual ObjectResult<proc_gethrformsactivity1_Result> proc_gethrformsactivity1(string formtype, Nullable<System.Guid> refid)
        {
            var formtypeParameter = formtype != null ?
                new ObjectParameter("formtype", formtype) :
                new ObjectParameter("formtype", typeof(string));
    
            var refidParameter = refid.HasValue ?
                new ObjectParameter("refid", refid) :
                new ObjectParameter("refid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_gethrformsactivity1_Result>("proc_gethrformsactivity1", formtypeParameter, refidParameter);
        }
    
        public virtual ObjectResult<proc_getallhrformsactivity_Result> proc_getallhrformsactivity(string formtype, Nullable<System.Guid> refid)
        {
            var formtypeParameter = formtype != null ?
                new ObjectParameter("formtype", formtype) :
                new ObjectParameter("formtype", typeof(string));
    
            var refidParameter = refid.HasValue ?
                new ObjectParameter("refid", refid) :
                new ObjectParameter("refid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_getallhrformsactivity_Result>("proc_getallhrformsactivity", formtypeParameter, refidParameter);
        }
    
        public virtual ObjectResult<SP_GetAllClaimsnew_Result> SP_GetAllClaimsnew(Nullable<System.Guid> userid, Nullable<System.Guid> propertyid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(System.Guid));
    
            var propertyidParameter = propertyid.HasValue ?
                new ObjectParameter("propertyid", propertyid) :
                new ObjectParameter("propertyid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_GetAllClaimsnew_Result>("SP_GetAllClaimsnew", useridParameter, propertyidParameter);
        }
    }
}
