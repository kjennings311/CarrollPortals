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
    using System.Collections.Generic;
    
    public partial class MileageLogHeader
    {
        public System.Guid MonthlyMileageLogId { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeAddress { get; set; }
        public string ReportedMonthMileage { get; set; }
        public Nullable<double> TotalNumberOfMiles { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public string Signature { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
        public Nullable<System.Guid> PropertyId { get; set; }
        public string PropertyName { get; set; }
    }
}