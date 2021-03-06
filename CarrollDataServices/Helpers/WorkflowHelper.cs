using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
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
      
        public static bool RunNotifyWorkflow(string RecordId, Char Type,string UserId)
        {

            if (ConfigurationManager.AppSettings["NotifyWorkFlow"] == "true")
            {

                // run your logic here to 
                EmailMessage _message = new EmailMessage();

                _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["AdditionalEmails"]).Split(',');
                _message.EmailBcc = Convert.ToString(ConfigurationManager.AppSettings["BCCEmails"]).Split(',');

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

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Property Damage Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy"),ClaimData.tbl.ClaimNumber);

                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\">  <h1> " + propresult.PropertyName + " - "+ ClaimData.tbl.ClaimNumber + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table> <br> <br> <table border='1' cellpadding='5' cellspacing='0' >";

                    _message.Body += "<tr><td><strong> Weather Conditions : </strong> </td> <td>" + (ClaimData.tbl.WeatherConditions == null ? "" : ClaimData.tbl.WeatherConditions) + " </td></tr>";

                    _message.Body += "<tr><td><strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.ToString("MM/dd/yyyy") + "</td> </tr>";
                    _message.Body += "<tr> <td><strong> Incident Time : </strong> </td><td>" + ClaimData.tbl.IncidentTime + "</td></tr><tr><td style='width:20%;padding-bottom:20px;'  > <strong> Incident Location : </strong> </td> <td>" + (ClaimData.tbl.IncidentLocation == null ? "" : ClaimData.tbl.IncidentLocation) + "</td> </tr> <tr> <td><strong>  Resident Name : </strong> </td><td>" + ClaimData.tbl.ResidentName + "</td> </tr>";
                    _message.Body += "<tr> <td><strong> Resident Contact Information : </strong> </td><td>" + ClaimData.tbl.ResidentContactInformation + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td></tr>";
                    
                    _message.Body += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> </tr>";

                    if (ClaimData.tbl.AuthoritiesContacted == true)
                    {
                        _message.Body += "<tr> <td><strong> Authorities Contacted : </strong> </td><td> Yes </td></tr><tr> <td><strong> Police Report Number : </strong> </td><td>" + ClaimData.tbl.PoliceReportNumber + "</td></tr>";
                    }
                    else
                    {
                        _message.Body += "<tr> <td><strong> Authorities Contacted : </strong> </td><td> No </td></tr>";
                    }

                    if (ClaimData.tbl.LossOfRevenues == false)
                        _message.Body += "<tr><td><strong> Authority Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> </tr><tr> <td><strong> Loss Of Revenues : </strong> </td><td> No </td></tr>";
                    else
                        _message.Body += "<tr><td><strong> Authority Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> </tr><tr> <td><strong> Loss Of Revenues : </strong> </td><td> Yes </td></tr>";


                    if (ClaimData.tbl.WitnessPresent == false)
                        _message.Body += "<tr><td><strong> Witness Present : </strong> </td><td> No  </td> </tr><tr> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";
                    else
                        _message.Body += "<tr><td><strong> Witness Present : </strong> </td><td> Yes  </td> </tr><tr> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";

                    _message.Body += "<tr> <td><strong> Witness Phone : </strong> </td><td> " + (ClaimData.tbl.WitnessPhone == null ? "" : ClaimData.tbl.WitnessPhone) + "</td></tr><tr> <td><strong> Witness Phone (Alternate) : </strong> </td><td> " + (ClaimData.tbl.ReportedPhone == null ? "" : ClaimData.tbl.ReportedPhone) + "</td></tr><tr> <td><strong> Witness Address : </strong> </td><td>" + ClaimData.tbl.WitnessAddress + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.IncidentReportedBy + " </td> </tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToString("MM/dd/yyyy")) + "</td></tr><tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";
                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy") + " "+ ClaimData.tbl.CreatedDate.Value.ToShortTimeString() + "</td></tr></table>";
                    _message.Body += "</div></div>";
                    //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db



                    // Popute Target To Email's
                   var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();
                    var uid =new Guid(UserId);

                    var userdetails = (from tbl in _entities.SiteUsers
                                       where tbl.UserId == uid
                                       select tbl.UserEmail).FirstOrDefault();

                    if (userdetails != null)
                    {
                        _message.EmailTo.Add(userdetails);
                       
                    }
                    //_message.EmailTo.Add("Laura.Patterson@carrollorg.com");
                    //_message.EmailTo.Add("brian.mckay@rhodesra.com");
                    //_message.EmailTo.Add("Bruce.Federspiel@rhodesra.com");
                    //_message.EmailTo.Add("David.Perez@carrollorg.com");
                    //_message.EmailTo.Add("james.flanagan@rhodesra.com");
                    //_message.EmailTo.Add("Mike.Davis@rhodesra.com");
                    //_message.EmailTo.Add("Ryan.Cranford@rhodesra.com");
                    //_message.EmailTo.Add("Scott.Gilpatrick@carrollmg.com");


                    //if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    //}
                    //else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
                    //}

                    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RMEmail);
                    }

                    if (!string.IsNullOrEmpty(workflowemails.PropertyMgrEmail))
                    {
                        _message.EmailTo.Add(workflowemails.PropertyMgrEmail);
                    }
                    //if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.VPEmail);
               // }
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

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "General Liability Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy"),ClaimData.tbl.ClaimNumber);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " "+ ClaimData.tbl.ClaimNumber + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table> <br> <br>  <table border='1' cellpadding='5' cellspacing='0' >";

                    _message.Body += "<tr><td style='width:20%;padding-bottom:20px;' > <strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.Value.ToString("MM/dd/yyyy") + "</td> </tr><tr><td><strong> Incident Time : </strong> </td><td>" + ClaimData.tbl.IncidentTime + "</td></tr><tr> <td><strong> Incident Location : </strong> </td><td>" + ClaimData.tbl.IncidentLocation + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td></tr><tr><td><strong> Authorities Contacted : </strong> </td> <td>" + (ClaimData.tbl.AuthoritiesContacted == true ? "Yes" : "No") + " </td></tr>";
                    _message.Body += "<tr> <td><strong> Police Report Number : </strong> </td><td>" + ClaimData.tbl.PoliceReportNumber + "</td></tr>";
                   
                    _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> </tr>";
                    _message.Body += "<tr> <td><strong>  Resident Name : </strong> </td><td>" + ClaimData.tbl.ResidentName + "</td> </tr><tr> <td><strong> Resident Contact Information  : </strong> </td><td>" + ClaimData.tbl.ResidentContactInformation + "</td> </tr>";

                    _message.Body += "<tr> <td><strong> Claimant Name : </strong> </td><td> " + ClaimData.tbl.ClaimantName + " </td></tr><tr><td><strong> Claimant Address : </strong> </td><td>" + ClaimData.tbl.ClaimantAddress + "</td> </tr><tr> <td><strong> Claimant Primary Phone : </strong> </td><td> " + ClaimData.tbl.ClaimantPhone1 + " </td></tr>";
                    _message.Body += "<tr> <td><strong> Claimant Secondary Phone : </strong> </td><td> " + ClaimData.tbl.ClaimantPhone2 + " </td></tr><tr><td><strong> Any Injuries : </strong> </td><td>" + (ClaimData.tbl.AnyInjuries == true ? "Yes " : "No") + "</td></tr>";

                    _message.Body += "<tr><td><strong> Injury Description : </strong> </td><td>" + ClaimData.tbl.InjuryDescription + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Witness Present : </strong> </td><td> " + (ClaimData.tbl.WitnessPresent == true ? "Yes " : " No ") + " </td></tr><tr><td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr>";
                    _message.Body += "<tr><td><strong> Witness Phone : </strong> </td><td>" + ClaimData.tbl.WitnessPhone + "</td></tr><tr> <td><strong> Witness Phone (Alternate) : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Witness Address : </strong> </td><td> " + ClaimData.tbl.WitnessAddress + " </td></tr><tr><td><strong> Description Of Damage : </strong> </td><td>" + ClaimData.tbl.DescriptionOfDamage + "</td></tr>";

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> </tr>";
                    _message.Body += "<tr><td><strong> Notify Security Officer : </strong> </td><td> " + (ClaimData.tbl.NotifySecurityOfficer == true ? "Yes " : " No ") + " </td> </tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToString("MM/dd/yyyy")) + "</td></tr><tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy") + " " + ClaimData.tbl.CreatedDate.Value.ToShortTimeString() + "</td></tr></table>";

                    //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db
                    _message.Body += "</div></div>";

                    // Popute Target To Email's


                     var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    //_message.EmailTo.Add("Laura.Patterson@carrollorg.com");
                    //_message.EmailTo.Add("brian.mckay@rhodesra.com");
                    //_message.EmailTo.Add("Bruce.Federspiel@rhodesra.com");
                    //_message.EmailTo.Add("David.Perez@carrollorg.com");
                    //_message.EmailTo.Add("james.flanagan@rhodesra.com");
                    //_message.EmailTo.Add("Mike.Davis@rhodesra.com");
                    //_message.EmailTo.Add("Ryan.Cranford@rhodesra.com");
                    //_message.EmailTo.Add("Scott.Gilpatrick@carrollmg.com");


                    //if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    //}
                    //else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
                    //}

                    //if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.RMEmail);
                    //}
                    //if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.VPEmail);
                    //}
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

                    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RMEmail);
                    }

                    if (!string.IsNullOrEmpty(workflowemails.PropertyMgrEmail))
                    {
                        _message.EmailTo.Add(workflowemails.PropertyMgrEmail);
                    }
                    //if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.VPEmail);
                    //}
                    if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RVPEmail);
                    }

                    var uid = new Guid(UserId);

                    var userdetails = (from tbl in _entities.SiteUsers
                                       where tbl.UserId == uid
                                       select tbl.UserEmail).FirstOrDefault();

                    if (userdetails != null)
                    {
                        _message.EmailTo.Add(userdetails);
                    }
                   // _message.EmailTo.Add("Laura.Patterson@carrollorg.com");

                   // _message.EmailTo.Add("Laura.Patterson@carrollorg.com");
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

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "AMG (Assumed Microbial Growth)", ClaimData.PropertyName, ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy"), ClaimData.tbl.ClaimNumber);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " "+ ClaimData.tbl.ClaimNumber + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table>  <br> <br> <table border='1' cellpadding='5' cellspacing='0'>";
                    _message.Body += "<tr><td style='width:20%;padding-bottom:20px;' > <strong> Discovery Date : </strong> </td><td>" + ClaimData.tbl.DiscoveryDate.Value.ToString("MM/dd/yyyy") + "</td> </tr>";
                    _message.Body += "<tr><td style='width:20%;padding-bottom:20px;' > <strong> Location : </strong> </td><td>" + ClaimData.tbl.Location + "</td> </tr>";

                    _message.Body += "<tr> <td><strong> Apartment Occupied : </strong> </td><td>" + (ClaimData.tbl.ApartmentOccupied == true ? "Yes " : " No ") + "</td> </tr>";
                    _message.Body += "<tr> <td><strong> Residents Affected : </strong> </td><td>" + ClaimData.tbl.ResidentsAffected + "</td> </tr>";
                    _message.Body += "<tr> <td><strong>  Resident Name : </strong> </td><td>" + ClaimData.tbl.ResidentName + "</td> </tr>";
                    _message.Body += "<tr> <td><strong> Resident Contact Information : </strong> </td><td>" + ClaimData.tbl.ResidentContactInformation + "</td> </tr>";
                    _message.Body += "<tr> <td><strong> Resident Relocating : </strong> </td><td>" + (ClaimData.tbl.ResidentsRelocating == true ? "Yes" : "No" ) + "</td> </tr>";

                    _message.Body += "<tr> <td><strong> Description : </strong> </td><td>" + ClaimData.tbl.Description + "</td> </tr>";
                    _message.Body += "<tr> <td><strong> Suspected Cause : </strong> </td><td> " + ClaimData.tbl.SuspectedCause + " </td></tr>";
                    _message.Body += "<tr><td><strong> Are Building Materials Still Wet : </strong> </td><td>" + (ClaimData.tbl.AreBuildingMaterialsStillWet == true ? "Yes" : "No") + "</td></tr><tr> <td><strong> Is Standing Water Present : </strong> </td><td> " + (ClaimData.tbl.IsStandingWaterPresent == true ? "Yes" : "No") + " </td></tr><tr><td><strong> How Much Water Present : </strong> </td> <td>" + ClaimData.tbl.HowMuchWater + " </td></tr>";

                    _message.Body += "<tr><td><strong> Estimated Surface Area Effected : </strong> </td><td>" + ClaimData.tbl.EstimatedSurfaceAreaAffected + "</td></tr>";
                    _message.Body += "<tr><td><strong> Estimated Time Damage Present : </strong> </td><td>" + ClaimData.tbl.EstimatedTimeDamagePresent + "</td> </tr><tr> <td><strong> Corrective Actions Taken  : </strong> </td><td> " + ClaimData.tbl.CorrectiveActionsTaken + " </td></tr><tr> <td><strong> Planned Actions  : </strong> </td><td> " + ClaimData.tbl.PlannedActions + " </td></tr>";

                    _message.Body += "<tr><td><strong> Additional Comments  : </strong> </td><td>" + ClaimData.tbl.AdditionalComments + "</td></tr>";

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> </tr><tr> <td><strong> Contact Phone Number : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToString("MM/dd/yyyy")) + "</td></tr><tr><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy") + " " + ClaimData.tbl.CreatedDate.Value.ToShortTimeString() + "</td></tr></table>";
                    // _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db
                    _message.Body += "</div></div>";
                    // Popute Target To Email's

                      var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    //_message.EmailTo.Add("Laura.Patterson@carrollorg.com");
                    //_message.EmailTo.Add("brian.mckay@rhodesra.com");
                    //_message.EmailTo.Add("Bruce.Federspiel@rhodesra.com");
                    //_message.EmailTo.Add("David.Perez@carrollorg.com");
                    //_message.EmailTo.Add("james.flanagan@rhodesra.com");
                    //_message.EmailTo.Add("Mike.Davis@rhodesra.com");
                    //_message.EmailTo.Add("Ryan.Cranford@rhodesra.com");
                    //_message.EmailTo.Add("Scott.Gilpatrick@carrollmg.com");


                    //if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    //}
                    //else if (!string.IsNullOrEmpty(workflowemails.EquityPartnerEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.EquityPartnerEmail);
                    //}

                    //if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.RMEmail);
                    //}
                    //if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.VPEmail);
                    //}
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

                    if (!string.IsNullOrEmpty(workflowemails.RMEmail))
                    {
                        _message.EmailTo.Add(workflowemails.RMEmail);
                    }

                    // MOLDNotification@ARIUMLiving.com
                    _message.EmailTo.Add("MOLDNotification@ARIUMLiving.com");

                    if (!string.IsNullOrEmpty(workflowemails.PropertyMgrEmail))
                {
                    _message.EmailTo.Add(workflowemails.PropertyMgrEmail);
                }
                //if (!string.IsNullOrEmpty(workflowemails.VPEmail))
                //{
                //    _message.EmailTo.Add(workflowemails.VPEmail);
                //}
                //if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                //{
                //    _message.EmailTo.Add(workflowemails.RVPEmail);
                //}
                //_message.EmailTo.Add("Laura.Patterson@carrollorg.com");

                var uid = new Guid(UserId);

                    var userdetails = (from tbl in _entities.SiteUsers
                                       where tbl.UserId == uid
                                       select tbl.UserEmail).FirstOrDefault();

                    if (userdetails != null)
                    {
                        _message.EmailTo.Add(userdetails);

                    }
                  
                 //   _message.EmailTo.Add("Laura.Patterson@carrollorg.com");


                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }

            }
            return true;

        }

        public static   bool SendClaimUpdatesLastweek()
        {
            EmailMessage _message = new EmailMessage();

            _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
            //_message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["AdditionalEmails"]).Split(',');
            //_message.EmailBcc = Convert.ToString(ConfigurationManager.AppSettings["BCCEmails"]).Split(',');

            var _entities = new CarrollFormsEntities();

            string bdy = "";
            _message.Subject = "Weekly Claim Update Summary " ;

            bdy = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\">  <h2> The following claims have been updated between "+DateTime.Today.AddDays(-7).ToShortDateString()+" to "+DateTime.Now.ToShortDateString()+" </h2>" +
                "<table  border='1' cellpadding='5' cellspacing='0'>  ";
            bdy += "<thead> <tr><th> ID </th><th> Property Name  </th><th> Type  </th><th> Incident Date </th><th> Resident Name </th> <th> Submitted Date </th><th> Updated Date </th> <th> Type of Update </th>  <tr> </thead>";


            // PM

            // get all pms with 

            var allpms = (from tbl in _entities.SiteUsers
                          join ur in _entities.UserInRoles on tbl.UserId equals ur.UserId
                          join r in _entities.Roles on ur.RoleId equals r.RoleId
                          where tbl.IsActive == true && r.RoleName == "Property"
                          select new { tbl.FirstName, tbl.LastName, tbl.UserEmail,tbl.UserId }).ToList();

            foreach (var item1 in allpms)
            {
                if(item1.UserEmail.ToString() == "pm.av@carrollmg.com" || item1.UserEmail.ToString() == "iampropertymanager@carrollmg.com")
                {
                    
                var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(item1.UserId,false).ToList();
                string Body = "<tbody> ";

                if(re.Count > 0)
                foreach (var item in re)
                {

                    Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                }
                else
                {
                    Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                }

                Body += " </tbody> </table>";
                _message.Body = bdy+Body+ "</div></div>";
                    //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db

                 //   _message.EmailTo.Add(item1.UserEmail);
                     _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
                    _message.EmailTo.Add("sekhar.babu@forcitude.com");
                    EmailHelper.SendEmailUpdate(_message);

                }

            }


            // RM


            var allrms = (from tbl in _entities.SiteUsers
                          join ur in _entities.UserInRoles on tbl.UserId equals ur.UserId
                          join r in _entities.Roles on ur.RoleId equals r.RoleId
                          where tbl.IsActive == true && r.RoleName == "Regional"
                          select new { tbl.FirstName, tbl.LastName, tbl.UserEmail, tbl.UserId }).ToList();

            foreach (var item1 in allrms)
            {
               
                    var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(item1.UserId, false).ToList();
                    string Body = "<tbody> ";

                    if (re.Count > 0)
                        foreach (var item in re)
                        {

                            Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                        }
                    else
                    {
                        Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                    }

                    Body += " </tbody> </table>";
                    _message.Body = bdy + Body + "</div></div>";
                //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                // populate from db

              //  _message.EmailTo.Add(item1.UserEmail);
                 _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
                _message.EmailTo.Add("sekhar.babu@forcitude.com");
                EmailHelper.SendEmailUpdate(_message);

                

            }


            // RVP
            var allrvms = (from tbl in _entities.SiteUsers
                          join ur in _entities.UserInRoles on tbl.UserId equals ur.UserId
                          join r in _entities.Roles on ur.RoleId equals r.RoleId
                          where tbl.IsActive == true && r.RoleName == "RVP"
                          select new { tbl.FirstName, tbl.LastName, tbl.UserEmail, tbl.UserId }).ToList();

            foreach (var item1 in allrvms)
            {

                var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(item1.UserId, false).ToList();
                string Body = "<tbody> ";

                if (re.Count > 0)
                    foreach (var item in re)
                    {

                        Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                    }
                else
                {
                    Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                }

                Body += " </tbody> </table>";
                _message.Body = bdy + Body + "</div></div>";
                //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                // populate from db

                //   _message.EmailTo.Add(item1.UserEmail);
                // _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");               
                _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
                _message.EmailTo.Add("sekhar.babu@forcitude.com");
                EmailHelper.SendEmailUpdate(_message);



            }



            // VP

            var allrvpms = (from tbl in _entities.SiteUsers
                           join ur in _entities.UserInRoles on tbl.UserId equals ur.UserId
                           join r in _entities.Roles on ur.RoleId equals r.RoleId
                           where tbl.IsActive == true && r.RoleName == "VP"
                           select new { tbl.FirstName, tbl.LastName, tbl.UserEmail, tbl.UserId }).ToList();

            foreach (var item1 in allrvpms)
            {

                var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(item1.UserId, false).ToList();
                string Body = "<tbody> ";

                if (re.Count > 0)
                    foreach (var item in re)
                    {

                        Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                    }
                else
                {
                    Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                }

                Body += " </tbody> </table>";
                _message.Body = bdy + Body + "</div></div>";
                //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                // populate from db

               // _message.EmailTo.Add(item1.UserEmail);
                // _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");               
                _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
                _message.EmailTo.Add("sekhar.babu@forcitude.com");
                EmailHelper.SendEmailUpdate(_message);



            }




            //A1

            var allrams = (from tbl in _entities.SiteUsers
                           join ur in _entities.UserInRoles on tbl.UserId equals ur.UserId
                           join r in _entities.Roles on ur.RoleId equals r.RoleId
                           where tbl.IsActive == true && r.RoleName == "Asset Manager"
                           select new { tbl.FirstName, tbl.LastName, tbl.UserEmail, tbl.UserId }).ToList();

            foreach (var item1 in allrams)
            {

                var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(item1.UserId, false).ToList();
                string Body = "<tbody> ";

                if (re.Count > 0)
                    foreach (var item in re)
                    {

                        Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                    }
                else
                {
                    Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                }

                Body += " </tbody> </table>";
                _message.Body = bdy + Body + "</div></div>";
                //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                // populate from db

               // _message.EmailTo.Add(item1.UserEmail);
                // _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");               
                _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
                _message.EmailTo.Add("sekhar.babu@forcitude.com");
                EmailHelper.SendEmailUpdate(_message);



            }


            // A2


            var allramss = (from tbl in _entities.SiteUsers
                           join ur in _entities.UserInRoles on tbl.UserId equals ur.UserId
                           join r in _entities.Roles on ur.RoleId equals r.RoleId
                           where tbl.IsActive == true && r.RoleName == "Asset Manager"
                           select new { tbl.FirstName, tbl.LastName, tbl.UserEmail, tbl.UserId }).ToList();

            foreach (var item1 in allramss)
            {

                var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(item1.UserId, true).ToList();
                string Body = "<tbody> ";

                if (re.Count > 0)
                    foreach (var item in re)
                    {

                        Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                    }
                else
                {
                    Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                }

                Body += " </tbody> </table>";
                _message.Body = bdy + Body + "</div></div>";
                //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                // populate from db
               
              //  _message.EmailTo.Add(item1.UserEmail);
                // _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");               
                _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
                _message.EmailTo.Add("sekhar.babu@forcitude.com");

                EmailHelper.SendEmailUpdate(_message);

            }






            //var _res = _entities.proc_GetLastWeekClaimUpdates().ToList();
            //_message.Body += "<tbody> ";
            //foreach (var item in _res)
            //{

            //    _message.Body += "<tr><td> "+item.ClaimNumber+ " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString()+ "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>"+item.UpdateType+" </td>  <tr> </thead>";

            //}

            //_message.Body += " </tbody> </table>";
            //_message.Body += "</div></div>";
            ////   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
            //// populate from db

            //    _message.EmailTo.Add("sekhar.babu@forcitude.com");
            //_message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");
            //_message.EmailTo.Add("sukumar.gandhi@forcitude.com");
            //_message.EmailTo.Add("iampropertymanager@carrollmg.com");

            //EmailHelper.SendEmailUpdate(_message);

            // sending for each property manager
            //loop through each call sp send mail 
            // subject and dates same
            // add body with property details and send
            // instead of calling them 

            return true;
        }

        public static bool SendWeeklySummary(string email, Guid Userid, bool isasset2)
        {

            EmailMessage _message = new EmailMessage();

            _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
            //_message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["AdditionalEmails"]).Split(',');
            //_message.EmailBcc = Convert.ToString(ConfigurationManager.AppSettings["BCCEmails"]).Split(',');

            var _entities = new CarrollFormsEntities();

            string bdy = "";
            _message.Subject = "Weekly Claim Update Summary ";

            bdy = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\">  <h2> The following claims have been updated between " + DateTime.Today.AddDays(-7).ToShortDateString() + " to " + DateTime.Now.ToShortDateString() + " </h2>" +
                "<table  border='1' cellpadding='5' cellspacing='0'>  ";
            bdy += "<thead> <tr><th> ID </th><th> Property Name  </th><th> Type  </th><th> Incident Date </th><th> Resident Name </th> <th> Submitted Date </th><th> Updated Date </th> <th> Type of Update </th>  <tr> </thead>";


            // PM

            // get all pms with 

          
                    var re = _entities.proc_GetLastWeekClaimUpdatesrolewise(Userid, isasset2).ToList();
                    string Body = "<tbody> ";

                    if (re.Count > 0)
                        foreach (var item in re)
                        {

                            Body += "<tr><td> " + item.ClaimNumber + " </td><td> " + item.PropertyName + "  </td><td> " + item.ClaimType + "  </td><td> " + item.IncidentDateTime.Value.ToShortDateString() + "  </td><td> " + item.ResidentName + " </td><td> " + item.CreatedDate.Value.ToShortDateString() + " </td><td> " + item.Updateddate.Value.ToShortDateString() + " </td> <td>" + item.UpdateType + " </td>  </tr>";

                        }
                    else
                    {
                        Body += "<tr><td colspan='8'>  No Results Found  </td>  <tr> ";

                    }

                    Body += " </tbody> </table>";
                    _message.Body = bdy + Body + "</div></div>";
                    //   _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    // populate from db

                   // _message.EmailTo.Add("sekhar.babu@forcitude.com");
                     _message.EmailTo.Add("Shashank.Trivedi@carrollorg.com");               

                    EmailHelper.SendEmailUpdate(_message);

             

            return true;
        }

        public static dynamic UpdatePmBrowserInfo(string RecordId, string FormType, string Action, string browser, string ipaddress)
        {

            var propid = new Guid(RecordId);

            Guid propertyid = Guid.NewGuid();

            var _entities = new CarrollFormsEntities();

            DynamicLink dl1 = new DynamicLink();
            dl1.DynamicLinkId = propertyid;
            dl1.FormType = FormType;
            dl1.OpenStatus = true;
            dl1.Action = Action;
            dl1.ReferenceId = propid;
            dl1.CreatedDate = DateTime.Now;
            dl1.BrowserInformation = browser;
            dl1.Clientdatetime = DateTime.Now;
            dl1.IpAddress = ipaddress;
            _entities.DynamicLinks.Add(dl1);
            _entities.SaveChanges();
            return true;

        }

        public static dynamic SendHrWorkFlowEmail(string RecordId, string FormType, string Action,string UserId)
        {

            string BrowserNotes = "<p> Browser Settings </p><ul style=\"list-style-type:disc;margin-left:30px;\"><li> <a href=\"https://support.google.com/chrome/answer/95472?co=GENIE.Platform%3DDesktop&hl=en\">Google Chrome </a> </li><li> <a href=\"https://support.microsoft.com/en-in/help/4026392/microsoft-edge-block-pop-ups\">Microsoft Edge </a> </li> <li> <a href=\"https://support.microsoft.com/en-us/help/17479/windows-internet-explorer-11-change-security-privacy-settings\"> Internet Explorer </a> </li> <li><a href=\"https://support.mozilla.org/en-US/kb/pop-blocker-settings-exceptions-troubleshooting\">Mozilla Firefox </a> </li><li>  <a href=\"https://support.apple.com/en-us/HT203987\"> Safari </a> </li> </ul>";


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
                    //  _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";

                        List<string> tos = new List<string>();

                        if (!string.IsNullOrEmpty(NewhireDetails.EmailAddress))
                            tos.Add(NewhireDetails.EmailAddress);
                        _message.EmailTo = tos;

                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email sent", "Employee Email sent for New Hire Notice on" + DateTime.Now, "System");

                            return true;
                        }
                        else
                        {
                            return false;
                        }

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
                    //_message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select new { tbl }).FirstOrDefault();


                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        //  var link =  "http://forms.carrollaccess.net/Outlink/Open?link=" + dl.DynamicLinkId;


                        _message.Subject = "Employee New Hire Notice needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5><p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";
                        List<string> tos = new List<string>();
                        // check if corporate then createduser email

                        if (NewhireDetails.tbl.iscorporate == true)
                        {
                            var getemail = (from tbl in _entities.SiteUsers
                                            where tbl.UserId == NewhireDetails.tbl.CreatedUser
                                            select tbl.UserEmail).FirstOrDefault();
                            if (getemail != null)
                            {
                                if (getemail != "")
                                {
                                    tos.Add(getemail);
                                }

                            }
                        }
                        else
                        {
                            var pid = new Guid(NewhireDetails.tbl.Location);

                            var getemail = (from tbl in _entities.SiteUsers
                                            join tblprop in _entities.Properties on tbl.managementcontact equals tblprop.RegionalManager
                                            where tblprop.PropertyId == pid
                                            select tbl).FirstOrDefault();
                            if (getemail != null)
                            {
                                if (getemail.UserEmail != "")
                                {
                                    tos.Add(getemail.UserEmail);
                                }

                                _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello " + getemail.FirstName + " " + getemail.LastName + " </h5><p> ";
                                _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";


                            }

                        }

                        // else if property then regional manager of(location selected property id)


                        _message.EmailTo = tos;

                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.tbl.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Regional Email sent", "Regional Email sent for New Hire Notice on" + DateTime.Now, "System");

                            return true;
                        }
                        else
                        {
                            return false;
                        }


                    }
                    else
                    {
                        return false;
                    }

                }

                else if (Action == "Service Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);


                    // Send Mail to ServiceDesk with Subject  and body in table format

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    //_message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');
                    var _entities = new CarrollFormsEntities();
                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select new { tbl }).FirstOrDefault();


                    if (NewhireDetails != null)
                    {
                        // subject and body
                        string corp = "CARROLL Corporate";

                        if (NewhireDetails.tbl.iscorporate == false)
                        {
                            var pid = new Guid(NewhireDetails.tbl.Location);

                            var getprop = (from tbl in _entities.Properties
                                           where tbl.PropertyId == pid
                                           select tbl).FirstOrDefault();
                            if (getprop != null)
                            {
                                if (getprop.PropertyName != "")
                                {
                                    corp = getprop.PropertyName;
                                }
                            }
                        }



                        _message.Subject = "Employee New Hire Notice for "+corp+" Succcessfully Submitted - " + NewhireDetails.tbl.SequenceNumber;
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5><p> ";

                        List<string> tos = new List<string>();
                        // check if corporate then createduser email
                    //  tos.Add("sekhar.babu@forcitude.com");
                       //  tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add(Convert.ToString(ConfigurationManager.AppSettings["ServiceDeskEmail"]));

                        //  var pid = new Guid(NewhireDetails.tbl.Location);

                     

                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1>  Employee New Hire Notice  - " + NewhireDetails.tbl.SequenceNumber + " </h1> <p> ";
                        //  _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";


                        _message.Body += " <table style='border:1px solid; '  border='1' cellpadding='20' cellspacing='0'  ><tbody><tr><td> <b> Employee Name   </b> </td><td> " + NewhireDetails.tbl.EmployeeName + "</td> </tr><tr><td> <b> Start Date   </b> </td> <td> " + NewhireDetails.tbl.StartDate.Value.ToShortDateString() + " </td> </tr>" +
                            "<tr><td><b> Location    </b>  </td><td> " + corp + "</td></tr><tr><td><b> Manager    </b>  </td><td> " + NewhireDetails.tbl.Manager + "</td></tr><tr><td> <b> Position   </b> </td> <td> " + NewhireDetails.tbl.Position + " </td> </tr>";

                        


                            if (!string.IsNullOrEmpty(NewhireDetails.tbl.La_Property1))
                        {
                            string other = "";


                            if (!string.IsNullOrEmpty(NewhireDetails.tbl.La_Property2))
                            {
                                other += "<br>" + NewhireDetails.tbl.La_Property2 + "(" + NewhireDetails.tbl.La_Property2_Per + ")";
                            }

                            if (!string.IsNullOrEmpty(NewhireDetails.tbl.La_Property3))
                            {
                                other += "<br>" + NewhireDetails.tbl.La_Property3 + "(" + NewhireDetails.tbl.La_Property3_Per + ")";
                            }

                            _message.Body += "<tr><td> <b> Labor Allocation    </b>  </td><td> " + NewhireDetails.tbl.La_Property1 + "(" + NewhireDetails.tbl.La_Property1_Per + ")" + other + "</td></tr>";
                        }



                        _message.Body += " </tbody></table>  <br>  <p> Thank you, <br> CARROLL </p>  </div></div>";


                        // else if property then regional manager of(location selected property id)


                        _message.EmailTo = tos;

                        return _message;


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

                    //string br1 = "", ip1 = "", da1 = "", br2 = "", ip2 = "", d2="",ip3="",br3="",d3="";

                    //var dldetails = (from tbl in _entities.DynamicLinks
                    //                 where tbl.FormType == "NewHire" && tbl.Action == "Employee Email" && tbl.ReferenceId == propid
                    //                 orderby tbl.CreatedDate ascending
                    //                 select new { browserinfo=tbl.BrowserInformation,ip=tbl.IpAddress,datetime=tbl.Clientdatetime }).FirstOrDefault();
                    //if(dldetails!=null)
                    //{
                    //    br1 = dldetails.browserinfo;
                    //    ip1 = dldetails.ip;
                    //    da1 = dldetails.datetime.ToString();
                    //}
                    //var dldetails1 = (from tbl in _entities.DynamicLinks
                    //                 where tbl.FormType == "NewHire" && tbl.Action == "Regional Email" && tbl.ReferenceId == propid
                    //                  orderby tbl.CreatedDate ascending
                    //                  select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime }).FirstOrDefault();

                    //if (dldetails1 != null)
                    //{
                    //    br2 = dldetails1.browserinfo;
                    //    ip2 = dldetails1.ip;
                    //    d2 = dldetails1.datetime.ToString();
                    //}

                    //var dldetails2 = (from tbl in _entities.DynamicLinks
                    //                  where tbl.FormType == "NewHire" && tbl.Action == "PM Email" && tbl.ReferenceId == propid
                    //                  select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime }).FirstOrDefault();

                    //if (dldetails2 != null)
                    //{
                    //    br3 = dldetails2.browserinfo;
                    //    ip3 = dldetails2.ip;
                    //    d3 = dldetails2.datetime.ToString();
                    //}
                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          join siteu in _entities.SiteUsers on tbl.CreatedUser equals siteu.UserId
                                          where tbl.EmployeeHireNoticeId == propid
                                          select new { tbl, siteu.FirstName, siteu.LastName, tbl.iscorporate }).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <p> ";
                        _message.Body += " Employee New Hire Notice  for " + NewhireDetails.tbl.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form.<br> <br> <br><br>  ";
                        _message.Body += "<br>  <h5> Thank you, <br> CARROLL</h5>  </div></div>";


                        List<string> tos = new List<string>();
                        tos.Add(ConfigurationManager.AppSettings["HrEmail"]);
                        var email = (from tbl in _entities.SiteUsers
                                     where tbl.UserId == NewhireDetails.tbl.CreatedUser
                                     select tbl.UserEmail).FirstOrDefault();
                        if (!string.IsNullOrEmpty(email))
                            tos.Add(email);
                        _message.EmailTo = tos;
                        InsertHrLog(FormType, propid.ToString(), "HR Email sent", "Hr Email is sent For Employee New Hire Notice on" + DateTime.Now, "System");

                        // send service desk email 



                        return _message;
                        // return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else if (FormType == "LeaseRider")
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
                    //  _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeLeaseRaiders
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";

                        List<string> tos = new List<string>();

                        if (!string.IsNullOrEmpty(NewhireDetails.EmployeeEmail))
                            tos.Add(NewhireDetails.EmployeeEmail);
                        _message.EmailTo = tos;

                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email sent ", "Employee Email sent for Employee Lease Rider on" + DateTime.Now, "System");

                            return true;
                        }
                        else
                        {
                            return false;
                        }
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


                    //string br = "";
                    //string ip = "";
                    //string dat = "";
                    //var dldetails = (from tbl in _entities.DynamicLinks
                    //                 where tbl.FormType == "LeaseRider" && tbl.Action == "Employee Email" && tbl.ReferenceId == propid
                    //                 orderby tbl.CreatedDate ascending
                    //                 select tbl).FirstOrDefault();

                    //if(dldetails!= null)
                    //{
                    //    br = dldetails.BrowserInformation;
                    //    ip = dldetails.IpAddress;
                    //    dat = dldetails.Clientdatetime.ToString();
                    //}


                    //var dldetails2 = (from tbl in _entities.DynamicLinks
                    //                  where tbl.FormType == "LeaseRider" && tbl.Action == "PM Email" && tbl.ReferenceId == propid
                    //                  select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime }).FirstOrDefault();
                    //string br3 = "";
                    //string ip3 = "";
                    //string d3 = "";

                    //if (dldetails2 != null)
                    //{
                    //    br3 = dldetails2.browserinfo;
                    //    ip3 = dldetails2.ip;
                    //    d3 = dldetails2.datetime.ToString();
                    //}

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeLeaseRaiders
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //    var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <p> ";
                        _message.Body += " Employee Lease Rider  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find the attached copy of form <br> <br> <br>  <h5> ";
                        _message.Body += "<br> Thank you, <br> CARROLL  </h5> </div></div>";


                        List<string> tos = new List<string>();
                        tos.Add(ConfigurationManager.AppSettings["HrEmail"]);
                        var email = (from tbl in _entities.SiteUsers
                                     where tbl.UserId == NewhireDetails.CreatedUser
                                     select tbl.UserEmail).FirstOrDefault();
                        if (!string.IsNullOrEmpty(email))
                            tos.Add(email);

                        _message.EmailTo = tos;

                        InsertHrLog(FormType, propid.ToString(), "HR Email sent ", "HR Email is sent For Employee Lease Rider on" + DateTime.Now, "System");

                        return _message;
                    }
                    else
                        return false;
                }

            }
            else if (FormType == "PayRoll")
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
                    // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices
                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Payroll Status Change Notice needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";
                        List<string> tos = new List<string>();

                        if (!string.IsNullOrEmpty(NewhireDetails.EmployeeEmail))
                            tos.Add(NewhireDetails.EmployeeEmail);
                        _message.EmailTo = tos;


                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email sent ", "Employee Email sent for Payroll Status Change Notice on" + DateTime.Now, "System");

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }



                }
                else if (Action == "Service Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);


                    // Send Mail to ServiceDesk with Subject  and body in table format

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    //_message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');
                    var _entities = new CarrollFormsEntities();
                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices
                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select new { tbl }).FirstOrDefault();


                    if (NewhireDetails != null)
                    {
                        // subject and body
                        string corp = "CARROLL Corporate";

                        if (NewhireDetails.tbl.IsCorporate == false)
                        {
                            var pid = NewhireDetails.tbl.Property;

                            var getprop = (from tbl in _entities.Properties
                                           where tbl.PropertyId == pid
                                           select tbl).FirstOrDefault();
                            if (getprop != null)
                            {
                                if (getprop.PropertyName != "")
                                {
                                    corp = getprop.PropertyName;
                                }
                            }
                        }


                        _message.Subject = "Payroll Status Change Notice for "+corp+" Succcessfully Submitted - " + NewhireDetails.tbl.SequenceNumber;
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5><p> ";

                        List<string> tos = new List<string>();
                        // check if corporate then createduser email
                     // tos.Add("sekhar.babu@forcitude.com");
                     //    tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add(Convert.ToString(ConfigurationManager.AppSettings["ServiceDeskEmail"]));

                        //  var pid = new Guid(NewhireDetails.tbl.Location);

                     


                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1>  Payroll Status Change Notice - " + NewhireDetails.tbl.SequenceNumber + " </h1> <p> ";
                        //  _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";


                        _message.Body += " <table style='border:1px solid;' border='1' cellpadding='20' cellspacing='0' ><tbody> <tr><td colspan='2' style='text-align:center;font-weight:bold;background: #012f4c;color: white;' > Employee Details </td> </tr> <tr><td> <b> Employee Name   </b>  </td><td> " + NewhireDetails.tbl.EmployeeName + "</td></tr><tr><td> <b> Employee Email   </b> </td> <td> " + NewhireDetails.tbl.EmployeeEmail + " </td> </tr>" +
                            "<tr><td> <b> Location   </b>  </td><td> " + corp + "</td>  </tr>";

                        if (NewhireDetails.tbl.ShowPropertyChange == true)
                        {
                            _message.Body += "<tr><td colspan='2' style='text-align:center;font-weight:bold;background: #012f4c;color: white;'  > Property Change </td> </tr> ";

                            _message.Body += "<tr><td>  <b> Change Effective Date    </b> </td><td> " + NewhireDetails.tbl.ChangeEffectiveDate.Value.ToShortDateString() + "</td> </tr>";
                            _message.Body += "<tr><td> <b> From Property#     </b> </td><td> " + NewhireDetails.tbl.FromPropNum + "</td></tr><tr> <td> <b> Name    </b> </td><td> " + NewhireDetails.tbl.FromPropName + "</td></tr><tr> <td> <b> Manager    </b>  </td><td> " + NewhireDetails.tbl.FromManager + "</td> </tr>";
                            _message.Body += "<tr><td>  <b>To Property#     </b> </td><td> " + NewhireDetails.tbl.ToPropNum + "</td></tr><tr> <td> <b> Name     </b> </td><td> " + NewhireDetails.tbl.ToPropName + "</td> </tr><tr><td> <b> Manager    </b>  </td><td> " + NewhireDetails.tbl.ToManager + "</td> </tr>";
                        }

                        if (NewhireDetails.tbl.ShowPayChange == true)
                        {

                            _message.Body += "<tr><td colspan='2' style='text-align:center;font-weight:bold;background: #012f4c;color: white;' > Pay / Position Change </td> </tr> ";
                            _message.Body += "<tr><td> <b> From Title      </b> </td><td> " + NewhireDetails.tbl.FromTitle + "</td></tr><tr> <td>  <b> To Title     </b>  </td><td> " + NewhireDetails.tbl.ToTitle + "</td>  </tr>";
                            _message.Body += "<tr><td> <b> Begin Pay Period    </b> </td><td> " + NewhireDetails.tbl.BeginPayPeriod + "</td> </tr>";
                        }
                        if (NewhireDetails.tbl.ShowDivisionOfLabor == true)
                            if (!string.IsNullOrEmpty(NewhireDetails.tbl.La_Property1))
                            {
                                string other = "";


                                if (!string.IsNullOrEmpty(NewhireDetails.tbl.La_Property2))
                                {
                                    other += "<br>" + NewhireDetails.tbl.La_Property2 + "(" + NewhireDetails.tbl.La_Property2_Per + ")";
                                }

                                if (!string.IsNullOrEmpty(NewhireDetails.tbl.La_Property3))
                                {
                                    other += "<br>" + NewhireDetails.tbl.La_Property3 + "(" + NewhireDetails.tbl.La_Property3_Per + ")";
                                }
                                _message.Body += "<tr><td colspan='2' style='text-align:center;font-weight:bold;background: #012f4c;color: white;' > Labor Allocation  </td> </tr> ";
                                _message.Body += "<tr><td> <b> Labor Allocation   </b> </td><td> " + NewhireDetails.tbl.La_Property1 + "(" + NewhireDetails.tbl.La_Property1_Per + ")" + other + "</td> </tr>";
                            }

                        if (NewhireDetails.tbl.ShowLeaves == true)
                        {
                            _message.Body += "<tr><td colspan='2' style='text-align:center;font-weight:bold;background: #012f4c;color: white;' > Leave Of Absence  </td> </tr> ";

                            _message.Body += "<tr><td> <b> Leave Begin Date     </b> </td><td> " + NewhireDetails.tbl.Leave_Begin.Value.ToShortDateString() + "</td></tr><tr> <td> <b> Leave End     </b>  </td><td> " + NewhireDetails.tbl.Leave_End.Value.ToShortDateString() + "</td> </tr>";
                        }


                        _message.Body += " </tbody></table>  <br>  <p> Thank you, <br> CARROLL </p>  </div></div>";






                        // else if property then regional manager of(location selected property id)


                        _message.EmailTo = tos;

                        return _message;


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


                    //string br = "";
                    //string ip = "";
                    //string dat = "";
                    //var dldetails = (from tbl in _entities.DynamicLinks
                    //                 where tbl.FormType == "PayRoll" && tbl.Action == "Employee Email" && tbl.ReferenceId == propid
                    //                 orderby tbl.CreatedDate ascending
                    //                 select tbl).FirstOrDefault();

                    //if (dldetails != null)
                    //{
                    //    br = dldetails.BrowserInformation;
                    //    ip = dldetails.IpAddress;
                    //    dat = dldetails.Clientdatetime.ToString();
                    //}


                    //var dldetails2 = (from tbl in _entities.DynamicLinks
                    //                  where tbl.FormType == "PayRoll" && tbl.Action == "PM Email" && tbl.ReferenceId == propid
                    //                  select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime }).FirstOrDefault();
                    //string br3 = "";
                    //string ip3 = "";
                    //string d3 = "";

                    //if (dldetails2 != null)
                    //{
                    //    br3 = dldetails2.browserinfo;
                    //    ip3 = dldetails2.ip;
                    //    d3 = dldetails2.datetime.ToString();
                    //}
                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices

                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select new { tbl.EmployeeName, tbl.CreatedUser }).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body
                        //    var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Payroll Status Change has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"><p> ";
                        _message.Body += "Payroll Status Change Notice  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find the attached Copy of form <br> <br> <br>  <h5> ";

                        _message.Body += "<br> Thank you, <br> CARROLL </h5>   </div></div>";

                        List<string> tos = new List<string>();

                        tos.Add(ConfigurationManager.AppSettings["HrEmail"]);
                        var email = (from tbl in _entities.SiteUsers
                                     where tbl.UserId == NewhireDetails.CreatedUser
                                     select tbl.UserEmail).FirstOrDefault();
                        if (!string.IsNullOrEmpty(email))
                            tos.Add(email);

                        _message.EmailTo = tos;

                        InsertHrLog(FormType, propid.ToString(), "HR Email sent ", "HR Email is sent For Payroll Status Change on" + DateTime.Now, "System");

                        return _message;
                    }
                    else
                        return false;
                }

            }
            else if (FormType == "Seperation")
            {
                if (Action == "Service Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);


                    // Send Mail to ServiceDesk with Subject  and body in table format

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    //_message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');
                    var _entities = new CarrollFormsEntities();
                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.NoticeOfEmployeeSeperations
                                          where tbl.EmployeeSeperationId == propid
                                          select new { tbl }).FirstOrDefault();


                    if (NewhireDetails != null)
                    {
                        // subject and body

                        string corp = "CARROLL Corporate";

                        if (NewhireDetails.tbl.IsCoporate == false)
                        {
                            var pid = new Guid(NewhireDetails.tbl.location);

                            var getprop = (from tbl in _entities.Properties
                                           where tbl.PropertyId == pid
                                           select tbl).FirstOrDefault();
                            if (getprop != null)
                            {
                                if (getprop.PropertyName != "")
                                {
                                    corp = getprop.PropertyName;
                                }
                            }
                        }


                        _message.Subject = "Notice of Employee Separation for "+corp+" Succcessfully Submitted - " + NewhireDetails.tbl.SequenceNumber;
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5><p> ";

                        List<string> tos = new List<string>();
                   //     // check if corporate then createduser email
                    //   tos.Add("sekhar.babu@forcitude.com");
                   //      tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add(Convert.ToString(ConfigurationManager.AppSettings["ServiceDeskEmail"]));

                        //  var pid = new Guid(NewhireDetails.tbl.Location);

                    


                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1>  Notice of Employee Separation  - " + NewhireDetails.tbl.SequenceNumber + " </h1> <p> ";
                        //  _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";


                        _message.Body += " <table border='1' cellpadding='20' cellspacing='0' ><tbody><tr><td> <b> Employee Name  </b> </td><td> " + NewhireDetails.tbl.EmployeeName + "</td></tr><tr><td> <b> Effective Date of Change   </b> </td> <td> " + NewhireDetails.tbl.EffectiveDateOfChange.Value.ToShortDateString() + " </td> </tr>" +
                            "<tr><td> <b> Location    </b> </td><td> " + corp + "</td>  </tr>";

                        if (NewhireDetails.tbl.IsCoporate == false)
                        {
                            _message.Body += "<tr><td> <b> Property Number     </b> </td><td> " + NewhireDetails.tbl.PropertyNumber + "</td>  </tr>";
                            

                        }



                            _message.Body += "<tr><td> <b> Job Title     </b>  </td><td> " + NewhireDetails.tbl.JobTitle + "</td> </tr>";


                        _message.Body += " </tbody></table>  <br>  <p> Thank you, <br> CARROLL </p>  </div></div>";






                        // else if property then regional manager of(location selected property id)


                        _message.EmailTo = tos;

                        return _message;


                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public static string GetMgrNamefromnewhire(string RecordId)
        {
            var propid = new Guid(RecordId);
            var _entities = new CarrollFormsEntities();
           
            var regionalmgr = (from tbl in  _entities.EmployeeNewHireNotices 
                               where tbl.EmployeeHireNoticeId == propid
                               select tbl.Location).FirstOrDefault();
            if(regionalmgr!= null)
            {

            }
            var pid = new Guid(regionalmgr);


            // regional manager
            var stid = "";

            var rid = (from tbl in _entities.Properties
                       where tbl.PropertyId == pid
                       select tbl.RegionalManager).FirstOrDefault();
            if (rid != null)
                stid = rid.ToString();
            else
            {
                // get rvp 

                var rid1 = (from tbl in _entities.Properties
                           where tbl.PropertyId == pid
                           select tbl.RegionalVicePresident).FirstOrDefault();
                if (rid1 != null)
                    stid = rid1.ToString();
                else
                {
                    // get rvp 

                    var rid11 = (from tbl in _entities.Properties
                                where tbl.PropertyId == pid
                                select tbl.VicePresident).FirstOrDefault();
                    if (rid11 != null)
                        stid = rid11.ToString();


                }

            }
           
            string mgr = "";
            var res = (from tbl in _entities.SiteUsers
                   where tbl.managementcontact == rid
                   select tbl).FirstOrDefault();

            if (res != null)
            {
                mgr =char.ToUpper(res.FirstName[0])+ res.FirstName.Substring(1).ToLower()+" "+char.ToUpper(res.LastName[0])+res.LastName.Substring(1).ToLower();

            }
            return mgr;
        }

        public static dynamic SendNewHireRejectionEmail(string RecordId,string User)
        {
            var propid = new Guid(RecordId);
            var _entities = new CarrollFormsEntities();
            

            // Send Mail to Employee Email with Subject and Link to dyamic Page

            EmailMessage _message = new EmailMessage();

            _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
           // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

            // get Employee Details i.e name and email

            var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                  join tjoin in _entities.SiteUsers  on tbl.RejectedBy equals tjoin.UserId
                                  where tbl.EmployeeHireNoticeId == propid
                                  select new { tbl.EmployeeName,tbl.EmployeeHireNoticeId,tbl.SequenceNumber,tbl.Position,tbl.RejectedReason, tjoin.FirstName,tjoin.LastName,tbl.CreatedUser,tbl.RejectedDateTime } ).FirstOrDefault();
            if (NewhireDetails != null)
            {
                // subject and body

                var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Hr/EmployeeNewHireNotice?resubmit=" + propid;
                _message.Subject = "Employee New Hire Notice has been rejected by "+NewhireDetails.FirstName +" "+NewhireDetails.LastName ;
                _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5> <p> ";
                _message.Body += " ID : "+NewhireDetails.SequenceNumber+ "  <br> Name : " + NewhireDetails.EmployeeName + "  <br>Position : " + NewhireDetails.Position + "  <br>Rejection notes : " + NewhireDetails.RejectedReason + "  <br>Rejection Date Time : " + NewhireDetails.RejectedDateTime.Value.ToString("MM/dd/yyyy")+ " "+ NewhireDetails.RejectedDateTime.Value.ToShortTimeString() + "  <br>  </p>  <br> <br> <h5> Thank you, <br> CARROLL </h5>   </div></div>";
                List<string> tos = new List<string>();
                 tos.Add(ConfigurationManager.AppSettings["HrEmail"]);

                // get created user email

                var email = (from tbl in _entities.SiteUsers
                             where tbl.UserId == NewhireDetails.CreatedUser
                             select tbl.UserEmail).FirstOrDefault();
                if (!string.IsNullOrEmpty(email))
                    tos.Add(email);
                
                _message.EmailTo = tos;
               
                

                if (EmailHelper.SendHrFormNotificationEmail(_message, propid.ToString(), NewhireDetails.CreatedUser.ToString()))
                {
                    WorkflowHelper.InsertHrLog("NewHire", propid.ToString(), "New Hire Notice has been rejected by \" "+User+" \"", "New Hire Notice has been Rejected on" + DateTime.Now, User);
                    var list = _entities.Database.ExecuteSqlCommand("update DynamicLinks set OpenStatus=0 where DynamicLinkId='" + propid.ToString() + "'");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }


        public static dynamic SendPayrollRejectionEmail(string RecordId, string User)
        {
            var propid = new Guid(RecordId);
            var _entities = new CarrollFormsEntities();


            // Send Mail to Employee Email with Subject and Link to dyamic Page

            EmailMessage _message = new EmailMessage();

            _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
            // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

            // get Employee Details i.e name and email

            var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices
                                  join tjoin in _entities.SiteUsers on tbl.RejectedBy equals tjoin.UserId
                                  where tbl.PayrollStatusChangeNoticeId == propid
                                  select new { tbl.EmployeeName, tbl.PayrollStatusChangeNoticeId, tbl.SequenceNumber,   tbl.RejectedReason, tjoin.FirstName, tjoin.LastName, tbl.CreatedUser, tbl.RejectedDateTime }).FirstOrDefault();
            if (NewhireDetails != null)
            {
                // subject and body

                var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Hr/Payrollstatuschangenotice?resubmit=" + propid;
                _message.Subject = "PayRoll Status Change has been rejected by " + NewhireDetails.FirstName + " " + NewhireDetails.LastName;
                _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5> <p> ";
                _message.Body += " ID : " + NewhireDetails.SequenceNumber + "  <br> Name : " + NewhireDetails.EmployeeName + "  <br>Rejection notes : " + NewhireDetails.RejectedReason + "  <br>Rejection Date Time : " + NewhireDetails.RejectedDateTime.Value.ToString("MM/dd/yyyy") + " " + NewhireDetails.RejectedDateTime.Value.ToShortTimeString() + "  <br>  </p>  <br> <br> <h5> Thank you, <br> CARROLL </h5>   </div></div>";
                List<string> tos = new List<string>();
                tos.Add(ConfigurationManager.AppSettings["HrEmail"]);

                // get created user email

                var email = (from tbl in _entities.SiteUsers
                             where tbl.UserId == NewhireDetails.CreatedUser
                             select tbl.UserEmail).FirstOrDefault();
                if (!string.IsNullOrEmpty(email))
                   // tos.Add(email);

                _message.EmailTo = tos;



                if (EmailHelper.SendHrFormNotificationEmail(_message, propid.ToString(), NewhireDetails.CreatedUser.ToString()))
                {
                    WorkflowHelper.InsertHrLog("PayRoll", propid.ToString(), "PayRoll Status Change has been rejected by \" " + User + " \"", "PayRoll Status Change has been Rejected on" + DateTime.Now, User);
                    var list = _entities.Database.ExecuteSqlCommand("update DynamicLinks set OpenStatus=0 where DynamicLinkId='" + propid.ToString() + "'");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public static dynamic ReSendHrWorkFlowEmail(string RecordId, string FormType, string Action,string UserId)
        {

            string BrowserNotes = "<p> Browser Settings </p><ul style=\"list-style-type:disc;margin-left:30px;\"><li> <a href=\"https://support.google.com/chrome/answer/95472?co=GENIE.Platform%3DDesktop&hl=en\">Google Chrome </a> </li><li> <a href=\"https://support.microsoft.com/en-in/help/4026392/microsoft-edge-block-pop-ups\">Microsoft Edge </a> </li> <li> <a href=\"https://support.microsoft.com/en-us/help/17479/windows-internet-explorer-11-change-security-privacy-settings\"> Internet Explorer </a> </li> <li><a href=\"https://support.mozilla.org/en-US/kb/pop-blocker-settings-exceptions-troubleshooting\">Mozilla Firefox </a> </li><li>  <a href=\"https://support.apple.com/en-us/HT203987\"> Safari </a> </li> </ul>";

            // Check Form Type 

            // Check if it is open 
            //var propid1 = new Guid(RecordId);

            //Guid propertyid1 = Guid.NewGuid();

            //var _entities123 = new CarrollFormsEntities();

            //DynamicLink dl1 = (from tbl in _entities123.DynamicLinks
            //                   where tbl.ReferenceId == propid && tbl.FormType == FormType && tbl.Action == Action
            //                   select tbl).FirstOrDefault();
            //if (dl1 == null)
            //{
            //    return "Record Not found";
            //}
            //else
            //{
            //    if (dl1.OpenStatus == false)
            //    {
            //        return "Employee Signature is already submitted, Please refresh the page";
            //    }
            //}


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
                                      orderby tbl.CreatedDate descending
                                      select tbl).FirstOrDefault();
                    if (dl.OpenStatus == false)
                    {
                        return "Employee Signature is already submitted, Please refresh the page";
                    }
                       dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                 //   _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();

                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";
                        List<string> tos = new List<string>();
                      
                        if (!string.IsNullOrEmpty(NewhireDetails.EmailAddress))
                            tos.Add(NewhireDetails.EmailAddress);
                        _message.EmailTo = tos;

                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog("NewHire", propid.ToString(), "Email RESENT to Employee ", "Employee Email is resent for New Hire Notice on" + DateTime.Now, UserId);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
                                      orderby tbl.CreatedDate descending
                                      select tbl).FirstOrDefault();
                    if (dl.OpenStatus == false)
                    {
                        return "Regional Signature is already submitted, Please refresh the page";
                    }
                    dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();


                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                  // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                          where tbl.EmployeeHireNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        List<string> tos = new List<string>();

                        string str = "";

                        if (NewhireDetails.iscorporate == true)
                        {
                            var getemail = (from tbl in _entities.SiteUsers
                                            where tbl.UserId == NewhireDetails.CreatedUser
                                            select tbl.UserEmail).FirstOrDefault();
                            if (getemail != null)
                            {
                                if (getemail != "")
                                {
                                    tos.Add(getemail);
                                }

                            }
                        }
                        else
                        {
                            var pid = new Guid(NewhireDetails.Location);

                            var getemail = (from tbl in _entities.SiteUsers
                                            join tblprop in _entities.Properties on tbl.managementcontact equals tblprop.RegionalManager
                                            where tblprop.PropertyId == pid 
                                            select tbl).FirstOrDefault();
                            if (getemail != null)
                            {
                                if (getemail.UserEmail != "")
                                {
                                    tos.Add(getemail.UserEmail);
                                }

                                str = getemail.FirstName + " " + getemail.LastName;

                            }

                        }

                        //tos.Add("sekhar.babu@forcitude.com");
                        //tos.Add("carrollforms@carrollorg.com");
                        _message.EmailTo = tos;


                        // subject and body
                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
// var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello " +str + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";





                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog("NewHire", propid.ToString(), "Email RESENT to Regional", "Regional Email is resent for New Hire Notice on" + DateTime.Now, UserId);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
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

                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi, </h5> <p> ";
                        _message.Body += " Employee New Hire Notice  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form.  <br> <br> <h5> Thank you, <br> CARROLL </h5>    </div></div>";
                        List<string> tos = new List<string>();
                       tos.Add(ConfigurationManager.AppSettings["HrEmail"]);
                        var email = (from tbl in _entities.SiteUsers
                                     where tbl.UserId == NewhireDetails.CreatedUser
                                     select tbl.UserEmail).FirstOrDefault();
                        if (!string.IsNullOrEmpty(email))
                            tos.Add(email);
                        _message.EmailTo = tos;
                      
                       InsertHrLog(FormType, propid.ToString(), "HR Email sent", "Hr Email is sent For New Hire Notice on" + DateTime.Now, "System");
                        
                        return _message;
                        // return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }

                }

            }
            if (FormType == "PayRoll")
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
                                      orderby tbl.CreatedDate descending
                                      select tbl).FirstOrDefault();

                    if (dl.OpenStatus == false)
                    {
                       
                        return "Employee Signature is already submitted, Please refresh the page";
                    }


                    dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                   // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices
                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select tbl).FirstOrDefault();

                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Payroll Status Change Notice needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";
                        List<string> tos = new List<string>();
                        if(NewhireDetails.EmployeeEmail != "")
                       tos.Add(NewhireDetails.EmployeeEmail);
                        _message.EmailTo = tos;

                       
                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email Resent ", "Employee Email is resent for Payroll Status Change Notice on" + DateTime.Now,UserId);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;

            }
            if (FormType == "LeaseRider")
            {
                // Check Action whether Email is to Employee or Regional Manager Or HR
                if (Action == "Employee Email")
                {
                    // Create a Dynamic Link to this form With Open Status 

                    var propid = new Guid(RecordId);

                    Guid propertyid = Guid.NewGuid();

                    var _entities = new CarrollFormsEntities();

                    //DynamicLink dl = new DynamicLink();
                    //dl.DynamicLinkId = propertyid;
                    //dl.FormType = FormType;
                    //dl.OpenStatus = true;
                    //dl.Action = Action;
                    //dl.ReferenceId = propid;
                    //dl.CreatedDate = DateTime.Now;
                    //_entities.DynamicLinks.Add(dl);
                    //_entities.SaveChanges();


                    DynamicLink dl = (from tbl in _entities.DynamicLinks
                                      where tbl.ReferenceId == propid && tbl.FormType == FormType && tbl.Action == Action
                                      orderby tbl.CreatedDate descending
                                      select tbl).FirstOrDefault();

                    if (dl.OpenStatus == false)
                    {
                        return "Employee Signature is already submitted, Please refresh the page";
                    }


                    dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();
                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                   // _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.EmployeeLeaseRaiders
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider needs your review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";
                        List<string> tos = new List<string>();
                     
                        if (!string.IsNullOrEmpty(NewhireDetails.EmployeeEmail))
                            tos.Add(NewhireDetails.EmployeeEmail);
                        _message.EmailTo = tos;

                       
                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email Resent ", "Employee Email is resent for Employee Lease Rider on" + DateTime.Now, UserId);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                    return false;

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
                                          join siteu in _entities.SiteUsers on tbl.CreatedUser equals siteu.UserId
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select new { tbl.EmployeeName,siteu.FirstName,siteu.LastName,tbl.CreatedUser } ).FirstOrDefault();

                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi, </h5><p> ";
                        _message.Body += "Employee Lease Rider   for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form <br> <br> <h5> Thank you, <br> CARROLL  </h5>  </div></div>";
                        List<string> tos = new List<string>();
                    
                       tos.Add(ConfigurationManager.AppSettings["HrEmail"]);
                        var email = (from tbl in _entities.SiteUsers
                                     where tbl.UserId == NewhireDetails.CreatedUser
                                     select tbl.UserEmail).FirstOrDefault();
                        if (!string.IsNullOrEmpty(email))
                            tos.Add(email);
                        _message.EmailTo = tos;
                      
                        InsertHrLog("LeaseRider", propid.ToString(), "HR Email sent", " Hr Email is sent for Employee Lease Rider" + DateTime.Now, "System" );

                        return _message;
                    }
                    else
                        return false;

                }
                else if (FormType == "NoticeOfEmployeeSeparation")
                {

                    var dldetails2 = (from tbl in _entities.DynamicLinks
                                      where tbl.FormType == "NoticeOfEmployeeSeparation" && tbl.Action == "PM Email" && tbl.ReferenceId == propid
                                      select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime }).FirstOrDefault();
                    string br3 = "";
                    string ip3 = "";
                    string d3 = "";

                    if (dldetails2 != null)
                    {
                        br3 = dldetails2.browserinfo;
                        ip3 = dldetails2.ip;
                        d3 = dldetails2.datetime.ToString();
                    }

                    var NewhireDetails = (from tbl in _entities.NoticeOfEmployeeSeperations
                                          join siteu in _entities.SiteUsers on tbl.CreatedUser equals siteu.UserId
                                          where tbl.EmployeeSeperationId == propid
                                          select new { tbl.EmployeeName, siteu.FirstName, siteu.LastName,tbl.CreatedUser }) .FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Notice Of Employee Separation has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi, </h5><p> ";
                        _message.Body += "A new \"Notice Of Employee Separation\" for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form </p>";
                        _message.Body += "<br><h5> Thank you, <br> CARROLL </h5>  </div></div>";

                        List<string> tos = new List<string>();
                    
                       tos.Add(ConfigurationManager.AppSettings["HrEmail"]);
                        _message.EmailTo = tos;
                        var email = (from tbl in _entities.SiteUsers
                                     where tbl.UserId == NewhireDetails.CreatedUser
                                     select tbl.UserEmail).FirstOrDefault();
                        if (!string.IsNullOrEmpty(email))
                            tos.Add(email);

                        InsertHrLog("NoticeOfEmployeeSeparation", propid.ToString(), "HR Email sent", " Hr Email is sent for Notice of Employee Separation" + DateTime.Now, "System");

                        return _message;
                    }
                    else
                        return false;

                }
                else if (FormType == "RequisitionRequest")
                {

                    var dldetails2 = (from tbl in _entities.DynamicLinks
                                      where tbl.FormType == "RequisitionRequest" && tbl.Action == "PM Email" && tbl.ReferenceId == propid
                                      select new { browserinfo = tbl.BrowserInformation, ip = tbl.IpAddress, datetime = tbl.Clientdatetime }).FirstOrDefault();
                    string br3 = "";
                    string ip3 = "";
                    string d3 = "";

                    if (dldetails2 != null)
                    {
                        br3 = dldetails2.browserinfo;
                        ip3 = dldetails2.ip;
                        d3 = dldetails2.datetime.ToString();
                    }



                    var NewhireDetails = (from tbl in _entities.RequisitionRequests
                                          join siteu in _entities.SiteUsers on tbl.CreatedUser equals siteu.UserId
                                          where tbl.RequisitionRequestId == propid
                                          select new { tbl.PropertyName, siteu.FirstName, siteu.LastName }).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body
                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Requisition Request has been Submitted";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi, </h5><p> ";
                        _message.Body += "A new \"requisition request\"   for " + NewhireDetails.PropertyName + " has been submitted. Please find attached copy of the form </p>";


                        _message.Body += "<br> <h5>  Thank you, <br> CARROLL </h5>   </div></div>";
                        List<string> tos = new List<string>();
                     
                        //tos.Add(ConfigurationManager.AppSettings["RecuritingEmail"]);
                        _message.EmailTo = tos;

                        InsertHrLog("RequisitionRequest", propid.ToString(), "HR Email sent ", " Hr Email is sent for Requisition Request on" + DateTime.Now, "System");

                        return _message;
                    }
                    else
                        return false;

                }
                else
                    return false;

            }
        }


        public static dynamic DailyRemainderToRPMForNewHireNotice()
        {
            string BrowserNotes = "<p> Browser Settings </p><ul style=\"list-style-type:disc;margin-left:30px;\"><li> <a href=\"https://support.google.com/chrome/answer/95472?co=GENIE.Platform%3DDesktop&hl=en\">Google Chrome </a> </li><li> <a href=\"https://support.microsoft.com/en-in/help/4026392/microsoft-edge-block-pop-ups\">Microsoft Edge </a> </li> <li> <a href=\"https://support.microsoft.com/en-us/help/17479/windows-internet-explorer-11-change-security-privacy-settings\"> Internet Explorer </a> </li> <li><a href=\"https://support.mozilla.org/en-US/kb/pop-blocker-settings-exceptions-troubleshooting\">Mozilla Firefox </a> </li><li>  <a href=\"https://support.apple.com/en-us/HT203987\"> Safari </a> </li> </ul>";

            using (var _entities = new CarrollFormsEntities())
            {

                var results = (from tbl in _entities.EmployeeNewHireNotices
                               where tbl.RegionalManagerSignedDateTime == null && tbl.EmployeeSignedDateTime != null
                               select tbl).ToList();

                foreach (var item in results)
                {
                    
                    // Check Form Type 
                    // Check Action whether Email is to Employee or Regional Manager Or HR

                    var dl = (from tbl in _entities.DynamicLinks
                                  where tbl.FormType == "NewHire" && tbl.Action == "Regional Email"
                                  select tbl).FirstOrDefault();
                    
                        // Send Mail to Employee Email with Subject and Link to dyamic Page

                        EmailMessage _message = new EmailMessage();

                        _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                      //  _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                        // get Employee Details i.e name and email

                      //subject and body

                            var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                            _message.Subject = "Remainder : Employee New Hire Notice needs your review";
                            _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + item.EmployeeName + " </h5> <p> ";
                    _message.Body += " You are receiving this email because there is a document pending your review and signature. <br><br>  Please click <a href='" + link + "'> HERE </a> to access and review the form for accuracy. Please note that the document will open in a new pop-up window so check your browser’s pop-up settings (some helpful links below) <br><br> If you have any questions, feel free to reach out to CARROLL team. </p> <br>  <p> Thank you, <br> CARROLL </p> <br> <br>  " + BrowserNotes + " </div></div>";

                    List<string> tos = new List<string>();
                           if(!string.IsNullOrEmpty(item.EmailAddress))
                            tos.Add(item.EmailAddress);                          
                            _message.EmailTo = tos;
                   
                        WorkflowHelper.InsertHrLog("NewHire", dl.ReferenceId.ToString(), "Remainder Email to Employee sent ", "Remainder Employee Email is sent for Employee Lease Rider on" + DateTime.Now, "Remainder by Server");

                    return EmailHelper.SendHrFormNotificationEmail(_message, dl.ReferenceId.ToString(), item.CreatedUser.ToString());

                    }

                return true;

                }
            }

        public static bool InsertHrLog(string FormType, string RecordId, string ActivitySubject, string ActivityDesc, string UserGuid)
        {
            IDataRepository _repo = new EntityDataRepository();
            _repo.HrLogActivity(FormType, RecordId, ActivitySubject, ActivityDesc, UserGuid);
            return true;
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

            if (ConfigurationManager.AppSettings["islive"] == "true")
            {

                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                smtp.Host = section.Network.Host; //"smtp.gmail.com"; // smtp.Host = "smtp.gmail.com";
                                                  // smtp.Host = "smtp.office365.com"; // smtp.Host = "smtp.gmail.com";
                smtp.Port = section.Network.Port;
                smtp.EnableSsl = section.Network.EnableSsl;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                //    smtp.TargetName = "STARTTLS/smtp.office365.com";
                //  NetworkCredential networkCredential = new NetworkCredential("iamnewemployee@carrollmg.com", "Carroll123!");
                if ((!string.IsNullOrEmpty(section.Network.UserName)) && (!string.IsNullOrEmpty(section.Network.Password)))
                {
                    NetworkCredential networkCredential = new NetworkCredential(section.Network.UserName, section.Network.Password);
                }


                // smtp.Credentials = networkCredential;

                // smtp.Port = 587; //587
            }
            else
            {
                smtp.Host = "smtp.gmail.com";                                              
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;   
                NetworkCredential networkCredential = new NetworkCredential("sekhar.babu@forcitude.com", "R21221.skr");
                smtp.Credentials = networkCredential;
                smtp.Port = 587; 
            }
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

                //foreach (var item in Message.EmailCc)
                //{
                //    mail.CC.Add(new MailAddress(item));
                //}

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
              //  mail.To.Clear();
                // remove this line before going production
            

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
                if(Message.EmailCc != null)
                {
                    foreach (var item in Message.EmailCc)
                    {
                        mail.CC.Add(new MailAddress(item));
                    }
                }
            

            mail.AlternateViews.Add(av1);

            mail.IsBodyHtml = true;
            mail.Subject = Message.Subject;
            mail.Body = Message.Body;
          //  mail.To.Clear();
            // remove this line before going production
           
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
                if(Message.EmailCc != null)
                foreach (var item in Message.EmailCc)
                {
                    mail.CC.Add(new MailAddress(item));
                }

                if (Message.EmailBcc != null)
                    foreach (var item in Message.EmailBcc)
                {
                    mail.Bcc.Add(new MailAddress(item));
                }

                mail.AlternateViews.Add(av1);

                mail.IsBodyHtml = true;
                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
              //  mail.To.Clear();
                // remove this line before going production
              

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.Priority = MailPriority.High;

                smtp.Send(mail);

                // Create an activity record
                IDataRepository _repo = new EntityDataRepository();
                _repo.LogActivity("Notification Email Sent", "System", RecordCreatedByGuid, RecordId, "Workflow Notification Sent");
                _repo = null;

            }
            return true;
        }


        public static bool SendEmailUpdate(EmailMessage Message)
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
                if (Message.EmailCc != null)
                    foreach (var item in Message.EmailCc)
                    {
                        mail.CC.Add(new MailAddress(item));
                    }

                if (Message.EmailBcc != null)
                    foreach (var item in Message.EmailBcc)
                    {
                        mail.Bcc.Add(new MailAddress(item));
                    }

                mail.AlternateViews.Add(av1);

                mail.IsBodyHtml = true;
                mail.Subject = Message.Subject;
                mail.Body = Message.Body;
                //  mail.To.Clear();
                // remove this line before going production


                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.Priority = MailPriority.High;

                smtp.Send(mail);

                // Create an activity record
                //IDataRepository _repo = new EntityDataRepository();
                //_repo.LogActivity("Notification Email Sent", RecordCreatedBy, RecordCreatedByGuid, RecordId, "Workflow Notification Sent");
                //_repo = null;

            }
            return true;
        }


        public static string FirstCharToUpper(string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
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
        public string[] EmailBcc { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}