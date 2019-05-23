using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using Carroll.Data.Entities;
using Carroll.Data.Entities.Repository;
using Newtonsoft.Json;
//using Rotativa;

namespace Carroll.Data.Services.Helpers
{
    public sealed class WorkflowHelper
    {
        public static bool RunNotifyWorkflow(string RecordId, Char Type)
        {

            if (ConfigurationManager.AppSettings["NotifyWorkFlow"] == "true")
            {

                // run your logic here to 
                EmailMessage _message = new EmailMessage();

                _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                var _entities = new CarrollFormsEntities();
                var propid = new Guid(RecordId);

                Guid propertyid = Guid.NewGuid();

                var _proprep = new EntityPropertyRepository();

                if (Type == 'P')
                {

                    var ClaimData = (from tbl in _entities.FormPropertyDamageClaims
                                     join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                     where tbl.PDLId == propid
                                     select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    propertyid = ClaimData.tbl.PropertyId;

                    var propresult = _proprep.GetProperty(ClaimData.tbl.PropertyId.ToString());
                    var equitypartner = (from tbl in _entities.EquityPartners
                                         where tbl.EquityPartnerId == propresult.EquityPartner
                                         select tbl.PartnerName).FirstOrDefault();

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Property Damage Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate);

                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table> <br> <br> <table border='1' cellpadding='5' cellspacing='0' >";

                    _message.Body += "<tr><td style='width:20%;padding-bottom:20px;'  > <strong> Incident Location : </strong> </td> <td>" + (ClaimData.tbl.IncidentLocation == null ? "" : ClaimData.tbl.IncidentLocation) + "</td> </tr><tr> <td><strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.ToShortDateString() + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Weather Conditions : </strong> </td> <td>" + (ClaimData.tbl.WeatherConditions == null ? "" : ClaimData.tbl.WeatherConditions) + " </td></tr><tr> <td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td></tr>";

                    if (ClaimData.tbl.AuthoritiesContacted == false)
                        _message.Body += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> </tr><tr> <td><strong> Authorities Contacted : </strong> </td><td> No </td></tr>";
                    else
                        _message.Body += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> </tr><tr> <td><strong> Authorities Contacted : </strong> </td><td> Yes </td></tr>";
                    if (ClaimData.tbl.LossOfRevenues == false)
                        _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> </tr><tr> <td><strong> Loss Of Revenues : </strong> </td><td> No </td></tr>";
                    else
                        _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> </tr><tr> <td><strong> Loss Of Revenues : </strong> </td><td> Yes </td></tr>";


                    if (ClaimData.tbl.WitnessPresent == false)
                        _message.Body += "<tr><td><strong> Witness Present : </strong> </td><td> No  </td> </tr><tr> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";
                    else
                        _message.Body += "<tr><td><strong> Witness Present : </strong> </td><td> Yes  </td> </tr><tr> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";

                    _message.Body += "<tr> <td><strong> Witness Address : </strong> </td><td>" + ClaimData.tbl.WitnessAddress + "</td> </tr><tr> <td><strong> Witness Phone : </strong> </td><td> " + (ClaimData.tbl.WitnessPhone == null ? "" : ClaimData.tbl.WitnessPhone) + "</td></tr>";

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.IncidentReportedBy + " </td> </tr><tr> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td></tr><tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr></table>";
                    _message.Body += "</div></div>";
                    //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db



                    // Popute Target To Email's
                    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                    {
                        _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    }
                    else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
                    {
                        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
                    }

                    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RMEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.VPEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RVPEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager2Email);
                    }



                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }
                else if (Type == 'G')
                {

                    var ClaimData = (from tbl in _entities.FormGeneralLiabilityClaims
                                     join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                     where tbl.GLLId == propid
                                     select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    propertyid = ClaimData.tbl.PropertyId;


                    var propresult = _proprep.GetProperty(ClaimData.tbl.PropertyId.ToString());
                    var equitypartner = (from tbl in _entities.EquityPartners
                                         where tbl.EquityPartnerId == propresult.EquityPartner
                                         select tbl.PartnerName).FirstOrDefault();

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "General Liability Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table> <br> <br>  <table border='1' cellpadding='5' cellspacing='0' >";

                    _message.Body += "<tr><td style='width:20%;padding-bottom:20px;' > <strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.Value.ToShortDateString() + "</td> </tr><tr> <td><strong> Incident Location : </strong> </td><td>" + ClaimData.tbl.IncidentLocation + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td></tr><tr><td><strong> Authorities Contacted : </strong> </td> <td>" + (ClaimData.tbl.AuthoritiesContacted == true ? "Yes" : "No") + " </td></tr>";

                    _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> </tr><tr> <td><strong> Claimant Name : </strong> </td><td> " + ClaimData.tbl.ClaimantName + " </td></tr>";

                    _message.Body += "<tr><td><strong> Claimant Address : </strong> </td><td>" + ClaimData.tbl.ClaimantAddress + "</td> </tr><tr> <td><strong> Claimant Phone1 : </strong> </td><td> " + ClaimData.tbl.ClaimantPhone1 + " </td></tr>";
                    _message.Body += "<tr> <td><strong> Claimant Phone2 : </strong> </td><td> " + ClaimData.tbl.ClaimantPhone2 + " </td></tr><tr><td><strong> Any Injuries : </strong> </td><td>" + (ClaimData.tbl.AnyInjuries == true ? "Yes " : "No") + "</td></tr>";

                    _message.Body += "<tr> <td><strong> Any Injuries : </strong> </td><td> " + (ClaimData.tbl.AnyInjuries == true ? "Yes " : " No ") + " </td> </tr><tr><td><strong> Injury Description : </strong> </td><td>" + ClaimData.tbl.InjuryDescription + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Witness Present : </strong> </td><td> " + (ClaimData.tbl.WitnessPresent == true ? "Yes " : " No ") + " </td></tr><tr><td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Witness Address : </strong> </td><td> " + ClaimData.tbl.WitnessAddress + " </td></tr><tr><td><strong> Witness Phone : </strong> </td><td>" + ClaimData.tbl.WitnessPhone + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Description Of Property : </strong> </td>  <td> " + ClaimData.tbl.DescriptionOfProperty + " </td></tr><tr><td><strong> Description Of Damage : </strong> </td><td>" + ClaimData.tbl.DescriptionOfDamage + "</td></tr>";


                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> </tr><tr> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td></tr><tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr></table>";
                    //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db
                    _message.Body += "</div></div>";

                    // Popute Target To Email's


                    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                    {
                        _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    }
                    else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
                    {
                        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
                    }

                    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RMEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.VPEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RVPEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager2Email);
                    }


                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }
                else if (Type == 'M')
                {

                    var ClaimData = (from tbl in _entities.FormMoldDamageClaims
                                     join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                     where tbl.MDLId == propid
                                     select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    propertyid = ClaimData.tbl.PropertyId;

                    var propresult = _proprep.GetProperty(ClaimData.tbl.PropertyId.ToString());

                    var equitypartner = (from tbl in _entities.EquityPartners
                                         where tbl.EquityPartnerId == propresult.EquityPartner
                                         select tbl.PartnerName).FirstOrDefault();

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Mold Damage Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table>  <br> <br> <table border='1' cellpadding='5' cellspacing='0'>";

                    _message.Body += "<tr><td style='width:20%;padding-bottom:20px;' > <strong> Location : </strong> </td><td>" + ClaimData.tbl.Location + "</td> </tr><tr> <td><strong> Description : </strong> </td><td>" + ClaimData.tbl.Description + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Are Building Materials Still Wet : </strong> </td><td>" + (ClaimData.tbl.AreBuildingMaterialsStillWet == true ? "Yes" : "No") + "</td></tr><tr><td><strong> How Much Water Present : </strong> </td> <td>" + ClaimData.tbl.HowMuchWater + " </td></tr>";

                    _message.Body += "<tr><td><strong> Estimated Time Damage Present : </strong> </td><td>" + ClaimData.tbl.EstimatedTimeDamagePresent + "</td> </tr><tr> <td><strong> Planned Actions  : </strong> </td><td> " + ClaimData.tbl.PlannedActions + " </td></tr>";

                    _message.Body += "<tr><td><strong> Discovery Date : </strong> </td><td>" + ClaimData.tbl.DiscoveryDate + "</td> </tr><tr> <td><strong> Suspected Cause : </strong> </td><td> " + ClaimData.tbl.SuspectedCause + " </td></tr>";
                    _message.Body += "<tr> <td><strong> Is Standing Water Present : </strong> </td><td> " + (ClaimData.tbl.IsStandingWaterPresent == true ? "Yes" : "No") + " </td></tr><tr><td><strong> Estimated Surface Area Effected : </strong> </td><td>" + ClaimData.tbl.EstimatedSurfaceAreaAffected + "</td></tr>";

                    _message.Body += "<tr> <td><strong> Corrective Actions Taken  : </strong> </td><td> " + ClaimData.tbl.CorrectiveActionsTaken + " </td></tr><tr><td><strong> Additional Comments  : </strong> </td><td>" + ClaimData.tbl.AdditionalComments + "</td></tr>";

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> </tr><tr> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td></tr><tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr></table>";
                    // _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db
                    _message.Body += "</div></div>";
                    // Popute Target To Email's

                    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                    {
                        _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    }
                    else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
                    {
                        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
                    }

                    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RMEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.VPEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RVPEmail);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    }
                    if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager2Email);
                    }


                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }

            }
            return true;

        }


        public static dynamic SendHrWorkFlowEmail(string RecordId, string FormType, string Action)
        {

            // Check Form Type 

            if (FormType == "NewHire")
            {
                // Check Action whether Email is to Employee or Regional Manager Or HR
                if (Action == "Employee Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();

                    DynamicLink dl = new DynamicLink();
                    dl.DynamicLinkId = propertyid;
                    dl.FormType = FormType;
                    dl.OpenStatus = true;
                    dl.Action = Action;
                    dl.ReferenceId = propid;
                    dl.CreatedDate = DateTime.Now;
                    _entities.DynamicLinks.Add(dl);
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi " + NewhireDetails.EmployeeName + " </h1> <br> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <i style='font-size:9px;font-style:italic;'> **Please note that this link will expire within 48 hours </i> <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;

                         return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }



                }
                else if (Action == "Regional Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();

                    DynamicLink dl = new DynamicLink();
                    dl.DynamicLinkId = propertyid;
                    dl.FormType = FormType;
                    dl.Action = Action;
                    dl.OpenStatus = true;
                    dl.ReferenceId = propid;
                    dl.CreatedDate = DateTime.Now;
                    _entities.DynamicLinks.Add(dl);
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi " + NewhireDetails.EmployeeName + " </h1> <br> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <i style='font-size:9px;font-style:italic;'> **Please note that this link will expire within 48 hours </i> <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;

                      return  EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }



                }
                else
                {

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();

                    DynamicLink dl = new DynamicLink();
                    dl.DynamicLinkId = propertyid;
                    dl.FormType = FormType;
                    dl.Action = Action;
                    dl.OpenStatus = true;
                    dl.ReferenceId = propid;
                    dl.CreatedDate = DateTime.Now;
                    _entities.DynamicLinks.Add(dl);
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                 // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi, </h1> <br> <p> ";
                        _message.Body += " Employee New Hire Notice  for "+NewhireDetails.EmployeeName+ " has been successfully reviewed and completed. Please find attached copy. Please find the Attachment of Employee New Hire Notice <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;
                        return _message;
                       // return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else
            {
                return false;
            }

        }
        public static dynamic ReSendHrWorkFlowEmail(string RecordId, string FormType, string Action)
        {

            // Check Form Type 

            if (FormType == "NewHire")
            {
                // Check Action whether Email is to Employee or Regional Manager Or HR
                if (Action == "Employee Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();

                    DynamicLink dl = (from tbl in _entities.DynamicLinks
                                      where tbl.ReferenceId == propid && tbl.FormType == FormType && tbl.Action == Action
                                      select tbl).FirstOrDefault();

                    dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi " + NewhireDetails.EmployeeName + " </h1> <br> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <i style='font-size:9px;font-style:italic;'> **Please note that this link will expire within 48 hours </i> <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;

                        return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }



                }
                else if (Action == "Regional Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();


                    DynamicLink dl = (from tbl in _entities.DynamicLinks
                                      where tbl.ReferenceId == propid && tbl.FormType == FormType && tbl.Action == Action
                                      select tbl).FirstOrDefault();

                    dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();


                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi " + NewhireDetails.EmployeeName + " </h1> <br> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <i style='font-size:9px;font-style:italic;'> **Please note that this link will expire within 48 hours </i> <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;

                        return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }



                }
                else
                {

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();

                    //DynamicLink dl = new DynamicLink();
                    //dl.DynamicLinkId = propertyid;
                    //dl.FormType = FormType;
                    //dl.Action = Action;
                    //dl.OpenStatus = true;
                    //dl.ReferenceId = propid;
                    //dl.CreatedDate = DateTime.Now;
                    //_entities.DynamicLinks.Add(dl);
                    //_entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi, </h1> <br> <p> ";
                        _message.Body += " Employee New Hire Notice  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy. Please find the Attachment of Employee New Hire Notice <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;
                        return _message;
                        // return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else
            {

                var propid = new Guid(RecordId);

                var _entities = new CarrollFormsEntities();

                EmailMessage _message = new EmailMessage();

                _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);

                // get Employee Details i.e name and email

                if (FormType == "EmployeeLeaseRider")
                {

                    var NewhireDetails = (from tbl in _entities.EmployeeLeaseRaiders
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi, </h1> <br> <p> ";
                        _message.Body += "Employee Lease Rider   for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of Employee Lease Rider <br> <br> <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        tos.Add("shashank.trivedi@carrollorg.com");
                        _message.EmailTo = tos;
                        return _message;
                    }
                    else
                        return false;

                }
                else if (FormType == "NoticeOfEmployeeSeparation")
                {
                    var NewhireDetails = (from tbl in _entities.NoticeOfEmployeeSeperations
                                          where tbl.EmployeeSeperationId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Notice Of Employee Separation has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi, </h1> <br> <p> ";
                        _message.Body += "Notice Of Employee Separation for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of Notice Of Employee Separation <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        tos.Add("shashank.trivedi@carrollorg.com");
                        _message.EmailTo = tos;
                        return _message;



                    }
                    else
                        return false;

                }
                else if (FormType == "RequisitionRequest")
                {

                    var NewhireDetails = (from tbl in _entities.RequisitionRequests
                                          where tbl.RequisitionRequestId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = "http://localhost/Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Requisition Request has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> Hi, </h1> <br> <p> ";
                        _message.Body += "Requisition Request   for " + NewhireDetails.PropertyName + " has been successfully reviewed and completed. Please find attached copy of Requisition Request  <h5> Thank You, <br> Carroll Management Group   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com");
                        tos.Add("shashank.trivedi@carrollorg.com");
                        _message.EmailTo = tos;
                        return _message;



                    }
                    else
                        return false;

                }
                else
                    return false;

            }
        }
    }

    public static class EmailHelper
    {
        //public static EmailMessage SetWorkFlowEmails(EmailMessage emailmessage,)
        //{
        //    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

        //    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
        //    {
        //        _message.EmailTo.Add(workflowemails.InsuranceEmail);
        //    }
        //    else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
        //    {
        //        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
        //    }

        //    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
        //    {
        //        _message.EmailTo.Add(workflowemails.RMEmail);
        //    }
        //    if (!string.IsNullOrEmpty(workflowemails.VPEmail))
        //    {
        //        _message.EmailTo.Add(workflowemails.VPEmail);
        //    }
        //    if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
        //    {
        //        _message.EmailTo.Add(workflowemails.RVPEmail);
        //    }
        //    if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
        //    {
        //        _message.EmailTo.Add(workflowemails.AssetManager1Email);
        //    }
        //    if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
        //    {
        //        _message.EmailTo.Add(workflowemails.AssetManager2Email);
        //    }



        //    return emailmessage;
        //}




        public static SmtpClient SetMailServerSettings()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            NetworkCredential networkCredential = new NetworkCredential("sekhar.babu@forcitude.com", "R21221.Skr");

            smtp.Credentials = networkCredential;
            smtp.Port = 587; //587
            return smtp;

        }


        public static bool SendHrFormNotificationEmailWithAttachment(EmailMessage Message, string RecordId, string RecordCreatedBy)
        {
            // write your email function here..
            using (MailMessage mail = new MailMessage())
            {

                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                        null, MediaTypeNames.Text.Html);

                SmtpClient smtp = SetMailServerSettings();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                foreach (var item in Message.EmailTo)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Match match = regex.Match(item);
                        if (match.Success)
                        {
                            mail.To.Add(item);
                        }
                    }
                }

                mail.From = new MailAddress(Message.EmailFrom, "Carroll Organization");

                foreach (var item in Message.EmailCc)
                {
                    mail.CC.Add(new MailAddress(item));
                }

                mail.AlternateViews.Add(av1);

                mail.IsBodyHtml = true;

                //ProformaViewModel d = new ProformaViewModel();
                //d.basicdetails = db.proc_getproformabasicdetails(item.OrderFormID).FirstOrDefault();
                //d.productlist = db.proc_getproformadetails(item.OrderFormID).ToList();




                //var actionPDF = new Rotativa.ViewAsPdf("SendProformaPDF", d)//some route values)
                //{
                //    //FileName = "TestView.pdf",
                //    PageSize = Size.A4,
                //    PageOrientation = Rotativa.Options.Orientation.Portrait,
                //    PageMargins = { Left = 1, Right = 1 }
                //};


                //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

                //MemoryStream file = new MemoryStream(applicationPDFData);
                //file.Seek(0, SeekOrigin.Begin);

                //Attachment data = new Attachment(file, item.OrderFormNumber + " Invoice Details.pdf", "application/pdf");
                //attachmsg = "";
                //attachmsg += data.Name;
                //ContentDisposition disposition = data.ContentDisposition;
                //disposition.CreationDate = System.DateTime.Now;
                //disposition.ModificationDate = System.DateTime.Now;
                //disposition.DispositionType = DispositionTypeNames.Attachment;

                //mail.Attachments.Add(data);

                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
                mail.To.Clear();
                // remove this line before going production
                //  mail.To.Add("pavan.nanduri@carrollorg.com");
                mail.To.Add("sekhar.babu@forcitude.com");
                  mail.To.Add("Shashank.Trivedi@carrollorg.com");

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.Priority = MailPriority.High;

                smtp.Send(mail);


            }
            return true;
        }

        public static bool SendHrFormNotificationEmail(EmailMessage Message, string RecordId, string RecordCreatedBy)
    {
        // write your email function here..
        using (MailMessage mail = new MailMessage())
        {

            AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                    null, MediaTypeNames.Text.Html);

            SmtpClient smtp = SetMailServerSettings();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            foreach (var item in Message.EmailTo)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Match match = regex.Match(item);
                    if (match.Success)
                    {
                        mail.To.Add(item);
                    }
                }
            }

            mail.From = new MailAddress(Message.EmailFrom, "Carroll Organization");

            foreach (var item in Message.EmailCc)
            {
                mail.CC.Add(new MailAddress(item));
            }

            mail.AlternateViews.Add(av1);

            mail.IsBodyHtml = true;
            mail.Subject = Message.Subject;
            mail.Body = Message.Body;
            mail.To.Clear();
            // remove this line before going production
            //  mail.To.Add("pavan.nanduri@carrollorg.com");
            mail.To.Add("sekhar.babu@forcitude.com");
           mail.To.Add("Shashank.Trivedi@carrollorg.com");

            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.Priority = MailPriority.High;

            smtp.Send(mail);


        }
        return true;
    }

    public static bool SendEmail(EmailMessage Message, string RecordId, string RecordCreatedBy, string RecordCreatedByGuid)
        {
            // write your email function here..
            using (MailMessage mail = new MailMessage())
            {

                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Message.Body,
                        null, MediaTypeNames.Text.Html);

                SmtpClient smtp = SetMailServerSettings();
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                foreach (var item in Message.EmailTo)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Match match = regex.Match(item);
                        if (match.Success)
                        {
                            mail.To.Add(item);
                        }
                    }
                }

                mail.From = new MailAddress(Message.EmailFrom, "Carroll Organization");

                foreach (var item in Message.EmailCc)
                {
                    mail.CC.Add(new MailAddress(item));
                }

                mail.AlternateViews.Add(av1);

                mail.IsBodyHtml = true;
                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
                mail.To.Clear();
                // remove this line before going production
                //  mail.To.Add("pavan.nanduri@carrollorg.com");
                mail.To.Add("sekhar.babu@forcitude.com");
                mail.To.Add("Shashank.Trivedi@carrollorg.com"); 

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.Priority = MailPriority.High;

                smtp.Send(mail);

                // Create an activity record
                IDataRepository _repo = new EntityDataRepository();
                _repo.LogActivity("Notification Email Sent", RecordCreatedBy, RecordCreatedByGuid, RecordId, "Workflow Notification Sent");
                _repo = null;

            }
            return true;
        }
    }

    public class EmailMessage
    {
        public EmailMessage()
        {
            EmailTo = new List<string>();
        }
        public List<string> EmailTo { get; set; }
        public string[] EmailCc { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}