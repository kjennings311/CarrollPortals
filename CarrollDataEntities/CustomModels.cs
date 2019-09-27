using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carroll.Data.Entities
{
    
    public enum EntityType
    {
        Property=0,
        Contact=1,
        Partner=2,
        User=3,
        Role=4,
        UserInRole = 5,
        UserInProperty = 6,
        FormPropertyDamageClaim=7,
        FormMoldDamageClaim=8,
        FormGeneralLiabilityClaim=9,
        AllClaims=10,
        PayPeriods=11,
        CarrollPositions=12

    }

    public enum DFieldType
    {
        IsText=0,
        IsDate=1,
        IsPerson=2,
        IsPic=3
    }

     public class Config
    {
        public string PkName { get; set; }
        public string EtType { get; set; }
        public List<DtableConfigArray> Columns { get; set; }
        public dynamic Rows { get; set; }
    }

    public class DtableConfigArray
    {
        public string name { get; set; }
        public string label { get; set; }
        public DFieldType type { get; set; }
        public string href { get; set; }
    }

    public class ClaimDetails
    {
        public dynamic Claim { get; set; }
        public dynamic Comments { get; set; }
        public dynamic Attchments { get; set; }
        public dynamic Activity { get; set; }
    }

    public class PrintClaimComments
    {
        public System.Guid CommentId { get; set; }
        public string Comment { get; set; }
        public short RefFormType { get; set; }
        public System.Guid RefFormID { get; set; }
        public System.DateTime CommentDate { get; set; }
        public string CommentByName { get; set; }
        public System.Guid CommentBy { get; set; }
    }



    public class PrintClaimAttachments
    {
        public System.Guid AttachmentId { get; set; }
        public System.Guid RefId { get; set; }
        public short RefFormType { get; set; }
        public string At_Name { get; set; }
        public string At_FileName { get; set; }
        public string UploadedByName { get; set; }
        public System.Guid UploadedBy { get; set; }
        public System.DateTime UploadedDate { get; set; }
    }


    public class PrintClaimActivity
    {
        public System.Guid ActivityId { get; set; }
        public System.Guid RecordId { get; set; }
        public string ActivityDescription { get; set; }
        public Nullable<System.DateTime> ActivityDate { get; set; }
        public string ActivityStatus { get; set; }
        public string ActivityByName { get; set; }
        public Nullable<System.Guid> ActivityBy { get; set; }
    }


    public class PrintViewClaim
    {
        public char Type { get; set; }
        public PrintGeneralLiabilityClaim GLC { get; set; }
        public PrintPropertyDamageClaim PDC { get; set; }
        public PrintMoldDamageClaim MDC { get; set; }
        public PrintProperty Prop { get; set; }
    }

    public class ExportClaim
    {
        public char Type { get; set; }
        public dynamic GLC { get; set; }
        public dynamic PDC { get; set; }
        public dynamic MDC { get; set; }
        public proc_getpropertydetails_Result Prop { get; set; }
        public List<FormComment> PrintComments { get; set; }
        public List<FormAttachment> PrintAttachments { get; set; }
        public List<Activity> PrintClaimActivity { get; set; }
    }


    public class PrintProperty
    {
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
        public string InsuranceContact { get; set; }
        public string EquityPartnerContact { get; set; }
        public string EquityPartner { get; set; }
        public string VicePresident { get; set; }
        public string RegionalVicePresident { get; set; }
        public string PropertyManager { get; set; }
        public string AssetManager2 { get; set; }
        public string AssetManager1 { get; set; }
        public string RegionalManager { get; set; }
        public string ConstructionManager { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string ClaimNumber { get; set; }
        public string IncidentTime { get; set; }
        public string ResidentName { get; set; }
        public string ResidentContactInformation { get; set; }
        public string PoliceReportNumber { get; set; }

    }
    public class PrintGeneralLiabilityClaim
    {

        public System.Guid GLLId { get; set; }
        public System.Guid PropertyId { get; set; }
        public Nullable<System.DateTime> IncidentDateTime { get; set; }
        public string IncidentLocation { get; set; }
        public string IncidentDescription { get; set; }
        public Nullable<bool> AuthoritiesContacted { get; set; }
        public string ContactPerson { get; set; }
        public string ClaimantName { get; set; }
        public string ClaimantAddress { get; set; }
        public string ClaimantPhone1 { get; set; }
        public string ClaimantPhone2 { get; set; }
        public Nullable<bool> AnyInjuries { get; set; }
        public string InjuryDescription { get; set; }
        public Nullable<bool> WitnessPresent { get; set; }
        public string WitnessName { get; set; }
        public string WitnessPhone { get; set; }
        public string WitnessAddress { get; set; }
        public string DescriptionOfProperty { get; set; }
        public string DescriptionOfDamage { get; set; }
        public string ReportedBy { get; set; }
        public Nullable<System.DateTime> DateReported { get; set; }
        public string ReportedPhone { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<bool> NotifySecurityOfficer { get; set; }
        public string ClaimNumber { get; set; }
        public string ResidentName { get; set; }
        public string ResidentContactInformation { get; set; }
        public string PoliceReportNumber { get; set; }
    }

    public class PrintMoldDamageClaim
    {

        public System.Guid MDLId { get; set; }
        public System.Guid PropertyId { get; set; }
        public Nullable<System.DateTime> DiscoveryDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string SuspectedCause { get; set; }
        public Nullable<bool> AreBuildingMaterialsStillWet { get; set; }
        public Nullable<bool> IsStandingWaterPresent { get; set; }
        public string HowMuchWater { get; set; }
        public string EstimatedSurfaceAreaAffected { get; set; }
        public string EstimatedTimeDamagePresent { get; set; }
        public string CorrectiveActionsTaken { get; set; }
        public string PlannedActions { get; set; }
        public string AdditionalComments { get; set; }
        public string ReportedBy { get; set; }
        public Nullable<System.DateTime> DateReported { get; set; }
        public string ReportedPhone { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string ClaimNumber { get; set; }
        public Nullable<bool> ApartmentOccupied { get; set; }
        public string ResidentsAffected { get; set; }
        public Nullable<bool> ResidentsRelocating { get; set; }
        public string ResidentContactInformation { get; set; }
        public string ResidentName { get; set; }
    }

    public class PrintPropertyDamageClaim
    {
        public System.Guid PDLId { get; set; }
        public System.Guid PropertyId { get; set; }
        public System.DateTime IncidentDateTime { get; set; }
        public string WeatherConditions { get; set; }
        public string IncidentLocation { get; set; }
        public string DescriptionOfProperty { get; set; }
        public string IncidentDescription { get; set; }
        public string EstimateOfDamage { get; set; }
        public Nullable<bool> AuthoritiesContacted { get; set; }
        public string ContactPerson { get; set; }
        public string ReportNumber { get; set; }
        public Nullable<bool> LossOfRevenues { get; set; }
        public Nullable<bool> WitnessPresent { get; set; }
        public string WitnessName { get; set; }
        public string WitnessPhone { get; set; }
        public string WitnessAddress { get; set; }
        public string IncidentReportedBy { get; set; }
        public string ReportedPhone { get; set; }
        public Nullable<System.DateTime> DateReported { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public string ClaimNumber { get; set; }
        public string IncidentTime { get; set; }
        public string ResidentName { get; set; }
        public string ResidentContactInformation { get; set; }
        public string PoliceReportNumber { get; set; }
    }

    //public class ExportClaim
    //{
    //    public char Type { get; set; }
    //    public PrintGeneralLiabilityClaim GLC { get; set; }
    //    public PrintPropertyDamageClaim PDC { get; set; }
    //    public PrintMoldDamageClaim MDC { get; set; }
    //    public PrintProperty Prop { get; set; }
    //    public List<PrintClaimComments> PrintComments { get; set; }
    //    public List<PrintClaimAttachments> PrintAttachments { get; set; }
    //    public List<PrintClaimActivity> PrintClaimActivity { get; set; }
    //}


    public class PrintClaimDetails
    {
        public dynamic ClaimDetails { get; set; }
        public dynamic PropertyDetails { get; set; }
    }

    public class RecordUpdateResult
    {
        public string RecordId { get; set; }
        public bool Succeded { get; set; }
    }

    public class EmailParams
    {
        public EmailParams()
        {
            logo = "carroll.jpg";
            signature = "<p> Pavan Nanduri <br>  Vice President, Information Systems <br> Carroll Organization <br> pavan.nanduri@carrollorg.com <br> 3340 Peachtree Rd NE - Suite 2250 <br> Atlanta, GA  30326 <br> Office: 404 - 812 - 8298<br> Cell: 770 - 508 - 5050 <br> Fax: 404 - 806 - 4312 <br> www.CarrollOrganization.com <br> <br>";
            fromemail = "sekhar.babu@forcitude.com";
            Company = "Carroll Organisation";
            mailfooterhtml = "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
            mailstart = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <table >  ";
            
        }

        public string logo { get; set; }
        public string signature { get; set; }
        public string fromemail { get; set; }
        public string Company { get; set; }
        public string mailfooterhtml { get; set; }
        public string mailbody { get; set; }
        public string mailstart { get; set; } 
           }

}
