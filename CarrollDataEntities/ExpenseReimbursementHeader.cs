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
    
    public partial class ExpenseReimbursementHeader
    {
        public System.Guid ExpenseReimbursementId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> SubmittionDate { get; set; }
        public string Address { get; set; }
        public string Col1YardiCode { get; set; }
        public string Col1GlCode { get; set; }
        public string Col2YardiCode { get; set; }
        public string Col2GlCode { get; set; }
        public string Col3YardiCode { get; set; }
        public string Col3GlCode { get; set; }
        public string Col4YardiCode { get; set; }
        public string Col4GlCode { get; set; }
        public string Col5YardiCode { get; set; }
        public string Col5GlCode { get; set; }
        public string Col6YardiCode { get; set; }
        public string Col6GlCode { get; set; }
        public string Col7YardiCode { get; set; }
        public string Col7GlCode { get; set; }
        public Nullable<double> TotalExpenses { get; set; }
        public Nullable<double> LessAnyCorrections { get; set; }
        public Nullable<double> BalanceDue { get; set; }
        public string SubmittedBySignature { get; set; }
        public Nullable<System.DateTime> SubmittedDate { get; set; }
        public string ApprovedSignature { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifedDatetime { get; set; }
        public Nullable<System.Guid> PropertyId { get; set; }
        public string PropertyName { get; set; }
        public Nullable<double> line1total { get; set; }
        public Nullable<double> line2total { get; set; }
        public Nullable<double> line3total { get; set; }
        public Nullable<double> line4total { get; set; }
        public Nullable<double> line5total { get; set; }
        public Nullable<double> line6total { get; set; }
        public Nullable<double> line7total { get; set; }
    }
}
