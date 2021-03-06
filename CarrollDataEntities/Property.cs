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
    
    public partial class Property
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Property()
        {
            this.UserInProperties = new HashSet<UserInProperty>();
        }
    
        public System.Guid PropertyId { get; set; }
        public string PropertyName { get; set; }
        public Nullable<int> PropertyNumber { get; set; }
        public string LegalName { get; set; }
        public Nullable<int> Units { get; set; }
        public string TaxId { get; set; }
        public string PropertyAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<System.DateTime> AcquisitionDate { get; set; }
        public Nullable<System.DateTime> DispositionDate { get; set; }
        public bool IsOwned { get; set; }
        public bool IsActive { get; set; }
        public string EquityPartnerSiteCode { get; set; }
        public Nullable<System.Guid> EquityPartner { get; set; }
        public Nullable<System.Guid> EquityPartnerContact { get; set; }
        public Nullable<System.Guid> InsuranceContact { get; set; }
        public Nullable<System.Guid> VicePresident { get; set; }
        public Nullable<System.Guid> RegionalVicePresident { get; set; }
        public Nullable<System.Guid> PropertyManager { get; set; }
        public Nullable<System.Guid> AssetManager1 { get; set; }
        public Nullable<System.Guid> AssetManager2 { get; set; }
        public Nullable<System.Guid> RegionalManager { get; set; }
        public Nullable<System.Guid> ConstructionManager { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.Guid> MarketingSpecialist { get; set; }
        public string Purchase_TookOver { get; set; }
        public Nullable<System.DateTime> RefinancedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInProperty> UserInProperties { get; set; }
    }
}
