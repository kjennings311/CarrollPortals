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
        public byte[] SignatureOfPropertyManager { get; set; }
        public string Position { get; set; }
        public byte[] SignatureOfEmployee { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }


    }

    public  class PrintViewClaim
    {
        public char Type { get; set; }
        public PrintGeneralLiabilityClaim GLC { get; set; }
        public PrintPropertyDamageClaim PDC { get; set; }
        public PrintMoldDamageClaim MDC { get; set; }
        public PrintProperty Prop { get; set; }
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
    }

    public  class PrintMoldDamageClaim
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
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }


    }

    public class PrintPayRollStatusChange
    {

        public System.Guid PayrollStatusChangeNoticeId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> ChangeEffectiveDate { get; set; }
        public string Manager { get; set; }
        public Nullable<System.DateTime> TodayDate { get; set; }
        public string Client_Location { get; set; }
        public Nullable<bool> NewHire { get; set; }
        public Nullable<bool> ReHire { get; set; }
        public Nullable<bool> Transfer { get; set; }
        public string Position { get; set; }
        public string Exempt { get; set; }
        public string NonExempt { get; set; }
        public string SsHash { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Status_FullTime_PartTime { get; set; }
        public string Wage_Salary { get; set; }
        public string Allocation { get; set; }
        public string StreetAddress { get; set; }
        public string City_State_Zip { get; set; }
        public string Phone { get; set; }
        public Nullable<double> Change_Pay_Rate_From { get; set; }
        public Nullable<double> Change_Pay_Rate_To { get; set; }
        public string Change_Property_From { get; set; }
        public string Change_Property_To { get; set; }
        public string Address_ContactInfo { get; set; }
        public string Date_Of_Suspence { get; set; }
        public Nullable<double> Suspence_Paid { get; set; }
        public Nullable<double> Suspence_UnPaid { get; set; }
        public string Leave_Absence { get; set; }
        public Nullable<double> Leave_Paid { get; set; }
        public Nullable<double> Leave_UnPaid { get; set; }
        public string Explanation { get; set; }
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    }

    public class PrintNoticeOfEmployeeSeparation
    {
        public System.Guid EmployeeSeperationId { get; set; }
        public Nullable<System.DateTime> EffectiveDateOfChange { get; set; }
        public Nullable<bool> EligibleForReHire { get; set; }
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
        public Nullable<System.Guid> CreatedUser { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.Guid> ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    }
}