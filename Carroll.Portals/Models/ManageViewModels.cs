﻿using System;
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
}