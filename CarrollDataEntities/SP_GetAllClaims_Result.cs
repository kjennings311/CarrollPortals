//------------------------------------------------------------------------------
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
    
    public partial class SP_GetAllClaims_Result
    {
        public System.Guid Id { get; set; }
        public System.Guid PropertyId { get; set; }
        public string ClaimType { get; set; }
        public string PropertyName { get; set; }
        public Nullable<int> PropertyNumber { get; set; }
        public string PropertyManager { get; set; }
        public string PropertyAddress { get; set; }
        public string IncidentLocation { get; set; }
        public string IncidentDescription { get; set; }
        public Nullable<System.DateTime> IncidentDateTime { get; set; }
        public string ReportedBy { get; set; }
        public Nullable<System.DateTime> DateReported { get; set; }
        public string ReportedPhone { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
    }
}