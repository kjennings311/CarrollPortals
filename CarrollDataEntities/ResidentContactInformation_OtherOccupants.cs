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
    
    public partial class ResidentContactInformation_OtherOccupants
    {
        public System.Guid OccupantId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
    
        public virtual ResidentContactInformation ResidentContactInformation { get; set; }
    }
}