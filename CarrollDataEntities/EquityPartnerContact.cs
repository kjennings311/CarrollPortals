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
    
    public partial class EquityPartnerContact
    {
        public System.Guid RowId { get; set; }
        public Nullable<System.Guid> EquityPartnerId { get; set; }
        public Nullable<System.Guid> ContactId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}
