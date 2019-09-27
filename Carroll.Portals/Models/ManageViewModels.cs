using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Carroll.Portals.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class PrintEmployeeLeaseRider
    {
        public System.Guid EmployeeLeaseRiderId { get; set; }
        public decimal ApartmentMarketRentalValue { get; set; }
        public string EmployeeName { get; set; }
        public System.DateTime Date { get; set; }
        public string Community { get; set; }
        public decimal EmployeeMonthlyRent { get; set; }
        public string RentalPaymentResidencyAt { get; set; }
        public string PropertyManager { get; set; }
        public string Position { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
        public string SignatureOfPropertyManager { get; set; }
        public string SignatureOfEmployee { get; set; }
        public Nullable<System.DateTime> PositionDate { get; set; }
        public string EmployeeEmail { get; set; }
        public Nullable<System.DateTime> EmployeeSignedDateTime { get; set; }
        public string SequenceNumber { get; set; }
        public Nullable<System.DateTime> PMDate { get; set; }
        public PrintActivity printActivity { get; set; }
    }

    public class PrintRequisitionRequest
    {
        public System.Guid RequisitionRequestId { get; set; }
        public string PropertyName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string RequestorName { get; set; }
        public string RequestorPosition { get; set; }
        public string PositionCombined { get; set; }
        public string PositionOther { get; set; }
        public string Type { get; set; }
        public string Post { get; set; }
        public Nullable<bool> ChkNewPosition { get; set; }
        public Nullable<bool> ChkReplacementPosition { get; set; }
        public string ReplacingPerson { get; set; }
        public Nullable<bool> ChkCarrollCareersIndeed { get; set; }
        public Nullable<bool> ChkApartmentAssociation { get; set; }
        public string PostOther { get; set; }
        public string SpecailInstructions { get; set; }
        public string RequistionNumber { get; set; }
        public Nullable<System.DateTime> DatePosted { get; set; }
        public string Notes { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public Nullable<bool> ChkOtherPosition { get; set; }
        public Nullable<bool> ChkOtherPost { get; set; }
        public string SequenceNumber { get; set; }
        public PrintActivity printActivity { get; set; }
    }

    public  class PrintClaimComments
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
        public PrintGeneralLiabilityClaim GLC { get; set; }
        public PrintPropertyDamageClaim PDC { get; set; }
        public PrintMoldDamageClaim MDC { get; set; }
        public PrintProperty Prop { get; set; }
        public List<PrintClaimComments> PrintComments { get; set; }
        public List<PrintClaimAttachments> PrintAttachments { get; set; }
        public List<PrintClaimActivity> PrintClaimActivity { get; set; }
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

        public string IncidentTime { get; set; }
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

    public class PrintEmployeeNewHireNotice
    {
        public System.Guid EmployeeHireNoticeId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string EmployeeSocialSecuirtyNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Manager { get; set; }
        public string Location { get; set; }
        public string Position_Exempt { get; set; }
        public string Position_NonExempt { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string Wage_Salary { get; set; }
        public string Allocation { get; set; }
        public string kitordered { get; set; }
        public Nullable<System.DateTime> boardingcallscheduled { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string esignature { get; set; }
        public Nullable<System.DateTime> edate { get; set; }
        public string msignature { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public string rpmsignature { get; set; }
        public Nullable<System.DateTime> rpmdate { get; set; }
        public string La_Property1 { get; set; }
        public Nullable<double> La_Property1_Per { get; set; }
        public string La_Property2 { get; set; }
        public Nullable<double> La_Property2_Per { get; set; }
        public string Sal_Time { get; set; }
        public Nullable<System.Guid> RegionaManager { get; set; }
        public Nullable<System.DateTime> PmSignedDateTime { get; set; }
        public Nullable<System.DateTime> EmployeeSignedDateTime { get; set; }
        public Nullable<System.DateTime> RegionalManagerSignedDateTime { get; set; }
        public Nullable<bool> iscorporate { get; set; }
        public string SequenceNumber { get; set; }
        public string La_Property3 { get; set; }
        public Nullable<double> La_Property3_Per { get; set; }
        public Nullable<bool> IsResumitted { get; set; }
        public Nullable<System.Guid> ResubmittedBy { get; set; }
        public Nullable<System.DateTime> ResubmittedDateTime { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<System.Guid> RejectedBy { get; set; }
        public Nullable<System.DateTime> RejectedDateTime { get; set; }
        public string AdditionalText { get; set; }
        public string RejectedReason { get; set; }
        public  PrintActivity printActivity { get; set; }

    }

    public class ListActivity
    {
        public string ActivitySubject { get; set; }
        public string ActivityDate { get; set; }
        public string ActivityByName { get; set; }
    }

    public class ListSigns
    {
        public string browserinfo { get; set; }
        public string ip { get; set; }
        public string datetime { get; set; }
        public string Action { get; set; }

    }

    public class ListRejectReason
    {
        //public string RejectedReason { get; set; }
        //public string FirstName { get; set; }
        //public DateTime? RejectedDateTime { get; set; }
        //public string LastName { get; set; }

        public string name { get; set; }
        public string rejectionDesc { get; set; }
        public string datetime { get; set; }


    }
    public class PrintActivity
    {
        public List<ListActivity> log { get; set; }
        public List<ListSigns> metadata { get; set; }
        public List<ListRejectReason> rejection { get; set; }
    }

    public class PrintResidentReferralSheet
        {
        public System.Guid ResidentReferalId { get; set; }
        public string PropertyCode { get; set; }
        public string PropertyName { get; set; }
        public string ResidentCode { get; set; }
        public string ResidentName { get; set; }
        public string Notes { get; set; }
        public string AgriPropertyName { get; set; }
        public string AgriResidentName { get; set; }
        public string ReferredResident { get; set; }
        public string UnitNumber { get; set; }
        public Nullable<double> ReferalBonus { get; set; }
        public string ReferingResident { get; set; }
        public Nullable<System.DateTime> ResidentDate { get; set; }
        public string PropertyManager { get; set; }
        public Nullable<System.DateTime> PropertyManagerDate { get; set; }
        public string Acc_Received { get; set; }
        public string Acc_CreditApplied { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }

    public class ResidentContactInformation
    {
        public System.Guid Contactid { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public string PropertyName { get; set; }
        public string ReturnEmail { get; set; }
        public string Fax1 { get; set; }
        public string Fax11 { get; set; }
        public string Fax2 { get; set; }
        public string Fax22 { get; set; }
        public string InsuranceDeclaration { get; set; }
        public string Em_name { get; set; }
        public string Em_Address { get; set; }
        public string Em_Phone { get; set; }
        public string Em_Relation { get; set; }
        public string ResidentSingature1 { get; set; }
        public Nullable<System.DateTime> ResidentSignDate1 { get; set; }
        public string ResidentSingature2 { get; set; }
        public Nullable<System.DateTime> ResidentSignDate2 { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }

    public class ResidentReferralResidents
    {
        public System.Guid ResidentId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Home_Work { get; set; }
        public string Home_Work_Phone { get; set; }
        public string CurrentEmployer { get; set; }
        public string Position { get; set; }

    }

    public class ResidentReferralOthers
    {
        public System.Guid OccupantId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
    }

    public class ResidentReferralVehicles
    {
        public System.Guid VehicleId { get; set; }
        public Nullable<System.Guid> ResidentContactInformationId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string LicensePlatState { get; set; }
    }



    public class PrintResidentContact
    {
        public ResidentContactInformation contact { get; set; }
        public List<ResidentReferralResidents> rrs { get; set; }
        public List<ResidentReferralOthers> ros { get; set; }
        public List<ResidentReferralVehicles> rvs { get; set; }
    }

    public class PrintPayRollStatusChange
    {
        public System.Guid PayrollStatusChangeNoticeId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> ChangeEffectiveDate { get; set; }
        public string TypeOfChange { get; set; }
        public Nullable<double> FromPropNum { get; set; }
        public string FromPropName { get; set; }
        public string FromManager { get; set; }
        public Nullable<double> ToPropNum { get; set; }
        public string ToPropName { get; set; }
        public string ToManager { get; set; }
        public Nullable<double> PayChangeTo { get; set; }
        public Nullable<double> PayChangeFrom { get; set; }
        public Nullable<bool> PayChangeFullTime { get; set; }
        public Nullable<bool> PayChangePartTime { get; set; }
        public Nullable<bool> PayChangeHourly { get; set; }
        public Nullable<bool> PayChangeSalary { get; set; }
        public string BeginPayPeriod { get; set; }
        public string La_Property1 { get; set; }
        public Nullable<double> La_Property1_Per { get; set; }
        public string La_Property2 { get; set; }
        public Nullable<double> La_Property2_Per { get; set; }
        public Nullable<bool> Chk_CarAmount { get; set; }
        public Nullable<double> CarAmountPerMonth { get; set; }
        public Nullable<bool> Chk_PhoneAmount { get; set; }
        public Nullable<double> PhoneAmountPerMonth { get; set; }
        public Nullable<bool> FmlaYes { get; set; }
        public Nullable<bool> FmlaNo { get; set; }
        public Nullable<bool> EnrolledBenefitsYes { get; set; }
        public Nullable<bool> EnrolledBenefitNo { get; set; }
        public string Leave_Purpose { get; set; }
        public Nullable<System.DateTime> Leave_End { get; set; }
        public Nullable<System.DateTime> Leave_Begin { get; set; }
        public Nullable<double> Pto_Balance { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string ESignature { get; set; }
        public Nullable<System.DateTime> EDate { get; set; }
        public string MSignature { get; set; }
        public Nullable<System.DateTime> MDate { get; set; }
        public Nullable<bool> ShowPropertyChange { get; set; }
        public Nullable<bool> ShowPayChange { get; set; }
        public Nullable<bool> ShowDivisionOfLabor { get; set; }
        public Nullable<bool> ShowAllowances { get; set; }
        public Nullable<bool> ShowLeaves { get; set; }
        public Nullable<bool> ShowNotes { get; set; }
        public string FromPosition { get; set; }
        public string FromStatus { get; set; }
        public string FromWageSalary { get; set; }
        public Nullable<double> FromRate { get; set; }
        public string ToPosition { get; set; }
        public string ToStatus { get; set; }
        public string ToWageSalary { get; set; }
        public Nullable<double> ToRate { get; set; }
        public string SequenceNumber { get; set; }
        public string Leave_Purpose_Other { get; set; }
        public string FromTitle { get; set; }
        public string ToTitle { get; set; }
        public Nullable<System.Guid> Property { get; set; }
        public string EmployeeEmail { get; set; }
        public string La_Property3 { get; set; }
        public Nullable<double> La_Property3_Per { get; set; }
        public Nullable<System.DateTime> PmSignedDateTime { get; set; }
        public Nullable<System.DateTime> EmployeeSignedDateTime { get; set; }
        public PrintActivity printActivity { get; set; }
        public Nullable<bool> IsCorporate { get; set; }
    }

    public class PrintNoticeOfEmployeeSeparation
    {
        public System.Guid EmployeeSeperationId { get; set; }
        public Nullable<System.DateTime> EffectiveDateOfChange { get; set; }
        public string EligibleForReHire { get; set; }
        public string PropertyName { get; set; }
        public string PropertyNumber { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitle { get; set; }
        public string Policty_Voilated { get; set; }
        public string AdditionalRemarks { get; set; }
        public string DocumentationAvailable { get; set; }
        public string WarningGiven_Dates { get; set; }
        public Nullable<bool> EquipmentKeysReturned { get; set; }
        public Nullable<bool> C2WeeeksNoticeGiven { get; set; }
        public Nullable<bool> VacationPaidOut { get; set; }
        public Nullable<double> VacationBalance { get; set; }
        public string Notes_Comments { get; set; }
        public string DischargedText { get; set; }
        public string QuitText { get; set; }
        public string LackOfWork { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string SSignature { get; set; }
        public Nullable<System.DateTime> SDate { get; set; }
        public string SMSignature { get; set; }
        public Nullable<System.DateTime> SMDate { get; set; }
        public string HRMSignature { get; set; }
        public Nullable<System.DateTime> HRMDate { get; set; }
        public string Reason { get; set; }
        public Nullable<bool> C2WeeksCompleted { get; set; }
        public string SequenceNumber { get; set; }
        public PrintActivity printActivity { get; set; }
        public Nullable<bool> IsCoporate { get; set; }
        public string location { get; set; }
    }

    public class PrintMileageLogHeader
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

    public class PrintMileageLogDetails
    {
        public System.Guid MileageLogDetailsId { get; set; }
        public Nullable<System.Guid> MileageLogId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string BillToProperty { get; set; }
        public string Origin_Destination { get; set; }
        public string Purpose { get; set; }
        public Nullable<double> NumberOfMiles { get; set; }
    }


    public class PrintMileageLog
    {
        public PrintMileageLogHeader header { get; set; }
        public List<PrintMileageLogDetails> details { get; set; }
    }


    public class PrintExpenseHeader
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

    public class PrintExpenseDetails
    {
        public System.Guid ExpenseReimbursementDetailId { get; set; }
        public System.Guid ExpenseReimbursementId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string ShortDescription { get; set; }
        public Nullable<double> Col1Expense { get; set; }
        public Nullable<double> Col2Epense { get; set; }
        public Nullable<double> Col3Expense { get; set; }
        public Nullable<double> Col4Expense { get; set; }
        public Nullable<double> Col5Expense { get; set; }
        public Nullable<double> Col6Expense { get; set; }
        public Nullable<double> Col7Expense { get; set; }
    }

    public class PrintExpenseForm
    {
        public PrintExpenseHeader header { get; set; }
        public List<PrintExpenseDetails> details { get; set; }

    }

    public partial class proc_getcontactsforexcel_Result
    {
        public System.Guid Contactid { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public partial class proc_getequitypartnersforexcel_Result
    {
        public System.Guid EquityPartnerId { get; set; }
        public string PartnerName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ContactPerson { get; set; }
    }

    public partial class proc_getpropertiesforexcel_Result
    {
        public System.Guid PropertyId { get; set; }
        public string VP { get; set; }
        public string VP_Phone { get; set; }
        public string RVP { get; set; }
        public string RVP_Phone { get; set; }
        public string RM { get; set; }
        public string RM_Phone { get; set; }
        public Nullable<int> PropertyNumber { get; set; }
        public Nullable<int> Units { get; set; }
        public string PropertyName { get; set; }
        public string EquityPartner { get; set; }
        public string AssetManager { get; set; }
        public string ConstructionManager { get; set; }
        public string MarketingSpecialist { get; set; }
        public string PropertyManager { get; set; }
        public string PhoneNumber { get; set; }
        public string PropertyAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public string LegalName { get; set; }
        public string Purchase_TookOver { get; set; }
        public Nullable<System.DateTime> TakeOverDate { get; set; }
        public Nullable<System.DateTime> RefinancedDate { get; set; }
        public string TaxId { get; set; }
    }

}