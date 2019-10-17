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

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Property Damage Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy"));

                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\">  <h1> " + propresult.PropertyName + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table> <br> <br> <table border='1' cellpadding='5' cellspacing='0' >";

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
                    //}
                    //if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.RVPEmail);
                    //}

                    if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
                    {
                        _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    }
                    //if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
                    //{
                    //    _message.EmailTo.Add(workflowemails.AssetManager2Email);
                    //}

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

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "General Liability Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy"));
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table> <br> <br>  <table border='1' cellpadding='5' cellspacing='0' >";

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
                    //if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.RVPEmail);
                //}
                if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
                {
                    _message.EmailTo.Add(workflowemails.AssetManager1Email);
                }
                //if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
                //{
                //    _message.EmailTo.Add(workflowemails.AssetManager2Email);
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
                        //}
                        //if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                        //{
                        //    _message.EmailTo.Add(workflowemails.RVPEmail);
                        //}

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

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Mold Damage Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate.Value.ToString("MM/dd/yyyy"));
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h1> " + propresult.PropertyName + " </h1> <table  border='1' cellpadding='5' cellspacing='0'> <tr> <td style='width:20%;'> <strong> Address :  </strong> </td> <td> " + propresult.PropertyAddress + ", " + propresult.City + ", " + propresult.State + " " + propresult.ZipCode + " </td> </tr><tr><td><strong>  Phone :</strong> </td> <td>" + propresult.PhoneNumber + " </td> </tr> <tr><td><strong>  Units :</strong> </td> <td>" + propresult.Units + " </td> </tr><tr><td><strong>  Yardi Code :</strong> </td> <td>" + propresult.PropertyNumber + " </td> </tr><tr><td><strong>  Legal :</strong> </td> <td>" + propresult.LegalName + " </td> </tr> <tr><td><strong>  Tax ID :</strong> </td> <td>" + propresult.TaxId + " </td> </tr> <tr><td><strong>  Partner :</strong> </td> <td>" + equitypartner + " </td> </tr>    </table>  <br> <br> <table border='1' cellpadding='5' cellspacing='0'>";
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

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> </tr><tr> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
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
                    //if (!string.IsNullOrEmpty(workflowemails.RVPEmail))
                    //{
                    //    _message.EmailTo.Add(workflowemails.RVPEmail);
             //   }
                if (!string.IsNullOrEmpty(workflowemails.AssetManager1Email))
                {
                    _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    }
                    //if (!string.IsNullOrEmpty(workflowemails.AssetManager2Email))
                    //{
                    //    _message.EmailTo.Add(workflowemails.AssetManager2Email);
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

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+ "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += "Please review this form for accuracy. If any questions, contact hiring manager. You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                        _message.EmailTo = tos;

                        if(EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
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
                                          select new { tbl }  ).FirstOrDefault();


                    if (NewhireDetails != null)
                    {
                        // subject and body

                    //    var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        var link =  "http://aspnet.carrollaccess.net/Outlink/Open?link=" + dl.DynamicLinkId;


                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5><p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                       
                        tos.Add("iamregionalmanager@carrollmg.com");
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                      //  tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
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
                                          select new { tbl, siteu.FirstName,siteu.LastName, tbl.iscorporate } ).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                     //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <p> ";
                        _message.Body += " Employee New Hire Notice  for "+NewhireDetails.tbl.EmployeeName+ " has been successfully reviewed and completed. Please find attached copy of form.<br> <br> <br><br>  ";
                        _message.Body += "<br>  Thank You, <br> CARROLL   </div></div>";


                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
                      //  tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;
                        InsertHrLog(FormType, propid.ToString(), "HR Email sent", "Hr Email is sent For Employee New Hire Notice on" + DateTime.Now,"System" );

                        return _message;
                       // return EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString());

                    }
                    else
                    {
                        return false;
                    }

                }

            }
            else if(FormType =="LeaseRider")
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

                    var NewhireDetails = (from tbl in _entities.EmployeeLeaseRaiders
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br><br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
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
                        _message.Body += " Employee Lease Rider  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find the attach copy of form <br> <br> <br>  <h5> ";
                        _message.Body += "<br> Thank You, <br> CARROLL   </div></div>";


                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                       tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
                        //  tos.Add("sekhar.babu@forcitude.com");
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
                    _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices
                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Payroll Status Change Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br>  <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                         tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
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
                                          join siteu in _entities.SiteUsers on tbl.CreatedUser equals siteu.CreatedBy
                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select new {siteu.FirstName,siteu.LastName,tbl.EmployeeName }).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body
                        //    var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Payroll Status Change has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"><p> ";
                        _message.Body += "Payroll Status Change Notice  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find the attached Copy of form <br> <br> <br>  <h5> ";
                       
                        _message.Body += "<br> Thank You, <br> CARROLL   </div></div>";

                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
                        //  tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;

                        InsertHrLog(FormType, propid.ToString(), "HR Email sent ", "HR Email is sent For Payroll Status Change on" + DateTime.Now, "System");

                        return _message;
                    }
                    else
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


            var rid = (from tbl in _entities.Properties
                       where tbl.PropertyId == pid
                       select tbl.RegionalManager).FirstOrDefault();

           
            string mgr = "";
            var res = (from tbl in _entities.proc_getallcontactsincludinghighroles()
                   where tbl.ContactId == rid
                   select tbl).FirstOrDefault();

            if (res != null)
            {
                mgr = res.FirstName+" "+res.LastName;

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
            _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

            // get Employee Details i.e name and email

            var NewhireDetails = (from tbl in _entities.EmployeeNewHireNotices
                                  join tjoin in _entities.SiteUsers  on tbl.RejectedBy equals tjoin.UserId
                                  where tbl.EmployeeHireNoticeId == propid
                                  select new { tbl.EmployeeName,tbl.EmployeeHireNoticeId,tbl.SequenceNumber,tbl.Position,tbl.RejectedReason, tjoin.FirstName,tjoin.LastName,tbl.CreatedUser,tbl.RejectedDateTime } ).FirstOrDefault();
            if (NewhireDetails != null)
            {
                // subject and body

                var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Hr/EmployeeNewHireNotice?resubmit=" + propid;
                _message.Subject = "Employee New Hire Notice has been rejected";
                _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hello </h5> <p> ";
                _message.Body += " ID : "+NewhireDetails.SequenceNumber+ "  <br> Name of the person : " + NewhireDetails.EmployeeName + "  <br>Position : " + NewhireDetails.Position + "  <br>Rejection notes : " + NewhireDetails.RejectedReason + "  <br>Rejection Date Time : " + NewhireDetails.RejectedDateTime.Value.ToString("MM/dd/yyyy")+ " "+ NewhireDetails.RejectedDateTime.Value.ToShortTimeString() + "  <br>  </p>  <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                List<string> tos = new List<string>();
                 tos.Add("Shashank.Trivedi@carrollorg.com");
                tos.Add("iamnewemployee@carrollmg.com");
                tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
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

        public static dynamic ReSendHrWorkFlowEmail(string RecordId, string FormType, string Action,string UserId)
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

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br>  <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
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

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee New Hire Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                         tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamregionalmanager@carrollmg.com");
                       // tos.Add("sekhar.babu@forcitude.com");
                        _message.EmailTo = tos;

                        
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
                        _message.Body += " Employee New Hire Notice  for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form.  <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                         tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
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
                                      select tbl).FirstOrDefault();

                    dl.OpenStatus = true;
                    dl.ModifiedDate = DateTime.Now;
                    _entities.SaveChanges();

                    // Send Mail to Employee Email with Subject and Link to dyamic Page

                    EmailMessage _message = new EmailMessage();

                    _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                    _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                    // get Employee Details i.e name and email

                    var NewhireDetails = (from tbl in _entities.PayrollStatusChangeNotices
                                          where tbl.PayrollStatusChangeNoticeId == propid
                                          select tbl).FirstOrDefault();

                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Payroll Status Change Notice needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                        _message.EmailTo = tos;

                       
                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email Resent ", "Employee Email is resent for Payroll Status Change Notice on" + DateTime.Now,"System");

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

                    var NewhireDetails = (from tbl in _entities.EmployeeLeaseRaiders
                                          where tbl.EmployeeLeaseRiderId == propid
                                          select tbl).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider needs your Review";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + NewhireDetails.EmployeeName + " </h5> <p> ";
                        _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br>  <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                         tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamnewemployee@carrollmg.com");
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                        if (!string.IsNullOrEmpty(NewhireDetails.EmployeeEmail))
                            tos.Add(NewhireDetails.EmployeeEmail);
                        _message.EmailTo = tos;

                       
                        if (EmailHelper.SendHrFormNotificationEmail(_message, propertyid.ToString(), NewhireDetails.CreatedUser.ToString()))
                        {
                            WorkflowHelper.InsertHrLog(FormType, propid.ToString(), "Employee Email Resent ", "Employee Email is resent for Employee Lease Rider on" + DateTime.Now, "System");

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
                                          select new { tbl.EmployeeName,siteu.FirstName,siteu.LastName } ).FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Employee Lease Rider has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi, </h5><p> ";
                        _message.Body += "Employee Lease Rider   for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                       tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
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
                                          select new { tbl.EmployeeName, siteu.FirstName, siteu.LastName }) .FirstOrDefault();
                    if (NewhireDetails != null)
                    {
                        // subject and body

                        //   var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"])+"Outlink/Open?link=" + dl.DynamicLinkId;
                        _message.Subject = "Notice Of Employee Separation has been successfully completed";
                        _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi, </h5><p> ";
                        _message.Body += "A new \"Notice Of Employee Separation\" for " + NewhireDetails.EmployeeName + " has been successfully reviewed and completed. Please find attached copy of form </p>";
                        _message.Body += "<br> Thank You, <br> CARROLL   </div></div>";

                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                        tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
                        _message.EmailTo = tos;

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


                        _message.Body += "<br> Thank You, <br> CARROLL   </div></div>";
                        List<string> tos = new List<string>();
                        tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                     tos.Add("Shashank.Trivedi@carrollorg.com");
                        tos.Add("iamhr@carrollmg.com ");
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
                        _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                        // get Employee Details i.e name and email

                      //subject and body

                            var link = Convert.ToString(ConfigurationManager.AppSettings["TestUrl"]) + "Outlink/Open?link=" + dl.DynamicLinkId;
                            _message.Subject = "Remainder : Employee New Hire Notice needs your Review";
                            _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <h5> Hi " + item.EmployeeName + " </h5> <p> ";
                            _message.Body += " You are receiving this email because there is a document pending your review and signature. Please click on the link to access : <a href='" + link + "'> " + link + " </a> </p> <br> <br> <h5> Thank You, <br> CARROLL   </div></div>";

                           List<string> tos = new List<string>();
                            tos.Add("sekhar.babu@forcitude.com"); tos.Add("sukumar.gandhi@forcitude.com");
                          tos.Add("Shashank.Trivedi@carrollorg.com");
                            tos.Add("iamregionalmanager@carrollmg.com");

                            // tos.Add("sekhar.babu@forcitude.com");
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
                NetworkCredential networkCredential = new NetworkCredential("sekhar.babu@forcitude.com", "R21221.Skr");
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
              //  mail.To.Clear();
                // remove this line before going production
                //  mail.To.Add("pavan.nanduri@carrollorg.com");
               mail.To.Add("sekhar.babu@forcitude.com");
             mail.To.Add("Shashank.Trivedi@carrollorg.com"); mail.To.Add("sukumar.gandhi@forcitude.com");

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

            mail.From = new MailAddress("Shashank.Trivedi@carrollorg.com", "Carroll Organization");
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
            mail.To.Clear();
            // remove this line before going production
            //  mail.To.Add("pavan.nanduri@carrollorg.com");
            mail.To.Add("sekhar.babu@forcitude.com");
        mail.To.Add("Shashank.Trivedi@carrollorg.com"); mail.To.Add("sukumar.gandhi@forcitude.com");

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
                //  mail.To.Add("pavan.nanduri@carrollorg.com");

              //  mail.To.Add("sekhar.babu@forcitude.com");
              //  mail.To.Add("sukumar.gandhi@forcitude.com");
              //  mail.To.Add("Shashank.Trivedi@carrollorg.com");

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
        public string[] EmailBcc { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}