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
    
    public partial class PayrollRejectionHistory
    {
        public System.Guid HistoryID { get; set; }
        public Nullable<System.Guid> PayRollId { get; set; }
        public string Name { get; set; }
        public string RejectionDesc { get; set; }
        public Nullable<System.DateTime> ClientDateTime { get; set; }
        public Nullable<System.Guid> RejectedUser { get; set; }
    }
}
