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
namespace Carroll.Data.Services.Helpers
{
    public sealed class WorkflowHelper
    {

        public static bool RunNotifyWorkflow(string RecordId, Char Type)
        {

            if(ConfigurationManager.AppSettings["NotifyWorkFlow"] == "true")
            {
                // run your logic here to 
                EmailMessage _message = new EmailMessage();
                _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');

                var _entities = new CarrollFormsEntities();
                var propid = new Guid(RecordId);

                Guid propertyid = Guid.NewGuid();

                if (Type == 'P')
                {

                    var ClaimData = (from tbl in _entities.FormPropertyDamageClaims
                                     join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                     where tbl.PDLId == propid
                                     select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    propertyid = ClaimData.tbl.PropertyId;


                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Property Damage Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <table>";

                    _message.Body += "<tr><td> <strong> Incident Location : </strong> </td><td>" + (ClaimData.tbl.IncidentLocation == null ? "" : ClaimData.tbl.IncidentLocation) + "</td> <td><strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.ToShortDateString() + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Weather Conditions : </strong> </td> <td>" + (ClaimData.tbl.WeatherConditions == null ? "" : ClaimData.tbl.WeatherConditions) + " </td><td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td></tr>";
                    if (ClaimData.tbl.AuthoritiesContacted == false)
                        _message.Body += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> <td><strong> Authorities Contacted : </strong> </td><td> No </td></tr>";
                    else
                        _message.Body += "<tr><td><strong> Estimate Of Damage : </strong> </td><td> " + (ClaimData.tbl.EstimateOfDamage == null ? "" : ClaimData.tbl.EstimateOfDamage) + "</td> <td><strong> Authorities Contacted : </strong> </td><td> Yes </td></tr>";
                    if (ClaimData.tbl.LossOfRevenues == false)
                        _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> <td><strong> Loss Of Revenues : </strong> </td><td > No </td></tr>";
                    else
                        _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> <td><strong> Loss Of Revenues : </strong> </td><td > Yes </td></tr>";


                    if (ClaimData.tbl.WitnessPresent == false)
                        _message.Body += "<tr><td><strong> Witness Present : </strong> </td><td> No  </td> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";
                    else
                        _message.Body += "<tr><td>><strong> Witness Present : </strong> </td><td> Yes  </td> <td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr> ";

                    _message.Body += "<tr> <td><strong> Witness Address : </strong> </td><td>" + ClaimData.tbl.WitnessAddress + "</td> <td><strong> Witness Phone : </strong> </td><td> " + (ClaimData.tbl.WitnessPhone == null ? "" : ClaimData.tbl.WitnessPhone) + "</td></tr>";

                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.IncidentReportedBy + " </td> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr></table>";
                    _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    _message.EmailTo = null; // populate from db
                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Report Name", "Property", "Datetimecreated");



                    // Popute Target To Email's


                    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                        _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    else
                        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);

                    _message.EmailTo.Add(workflowemails.RMEmail);
                    _message.EmailTo.Add(workflowemails.VPEmail);
                    _message.EmailTo.Add(workflowemails.RVPEmail);
                    _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    _message.EmailTo.Add(workflowemails.AssetManager2Email);

                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }
                else if (Type == 'G')
                {

                    var ClaimData = (from tbl in _entities.FormGeneralLiabilityClaims
                                     join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                     where tbl.GLLId == propid
                                     select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    propertyid = ClaimData.tbl.PropertyId;

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "General Liability Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <table>";

                    _message.Body += "<tr><td> <strong> Incident Date : </strong> </td><td>" + ClaimData.tbl.IncidentDateTime.Value.ToShortDateString() + "</td> <td><strong> Incident Location : </strong> </td><td>" + ClaimData.tbl.IncidentLocation + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Incident Description : </strong> </td><td>" + ClaimData.tbl.IncidentDescription + "</td><td><strong> Authorities Contacted : </strong> </td> <td>" + (ClaimData.tbl.AuthoritiesContacted == true ? "Yes" : "No") + " </td></tr>";
               
                        _message.Body += "<tr><td><strong> Contact Person : </strong> </td><td>" + (ClaimData.tbl.ContactPerson == null ? "" : ClaimData.tbl.ContactPerson) + "</td> <td><strong> Claimant Name : </strong> </td><td> "+ClaimData.tbl.ClaimantName+" </td></tr>";

                    _message.Body += "<tr><td><strong> Claimant Address : </strong> </td><td>" + ClaimData.tbl.ClaimantAddress  + "</td> <td><strong> Claimant Phone1 : </strong> </td><td> " + ClaimData.tbl.ClaimantPhone1 + " </td></tr>";
                    _message.Body += "<tr> <td><strong> Claimant Phone2 : </strong> </td><td> " + ClaimData.tbl.ClaimantPhone2 + " </td><td><strong> Any Injuries : </strong> </td><td>" + (ClaimData.tbl.AnyInjuries==true ? "Yes ":"No" ) + "</td></tr>";

                    _message.Body += "<tr> <td><strong> Any Injuries : </strong> </td><td> " +  (ClaimData.tbl.AnyInjuries == true ? "Yes ": " No ")  + " </td><td><strong> Injury Description : </strong> </td><td>" + ClaimData.tbl.InjuryDescription + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Witness Present : </strong> </td><td> " + (ClaimData.tbl.WitnessPresent == true ? "Yes " : " No ") + " </td><td><strong> Witness Name : </strong> </td><td>" + ClaimData.tbl.WitnessName + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Witness Address : </strong> </td><td> " + ClaimData.tbl.WitnessAddress + " </td><td><strong> Witness Phone : </strong> </td><td>" + ClaimData.tbl.WitnessPhone + "</td></tr>";
                    _message.Body += "<tr> <td><strong> Description Of Property : </strong> </td><td> " + ClaimData.tbl.DescriptionOfProperty + " </td><td><strong> Description Of Damage : </strong> </td><td>" + ClaimData.tbl.DescriptionOfDamage + "</td></tr>";


                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr></table>";
                    _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    _message.EmailTo = null; // populate from db
                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Report Name", "Property", "Datetimecreated");



                    // Popute Target To Email's


                    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                        _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    else
                        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);

                    _message.EmailTo.Add(workflowemails.RMEmail);
                    _message.EmailTo.Add(workflowemails.VPEmail);
                    _message.EmailTo.Add(workflowemails.RVPEmail);
                    _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    _message.EmailTo.Add(workflowemails.AssetManager2Email);

                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }
                else if (Type == 'M')
                {

                    var ClaimData = (from tbl in _entities.FormMoldDamageClaims
                                     join tblprop in _entities.Properties on tbl.PropertyId equals tblprop.PropertyId
                                     where tbl.MDLId == propid
                                     select new { tbl, tblprop.PropertyName }).FirstOrDefault();

                    propertyid = ClaimData.tbl.PropertyId;

                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "General Liability Claim", ClaimData.PropertyName, ClaimData.tbl.CreatedDate);
                    _message.Body = "<div style=\" padding: 30px; background:#b9b7b7;\"> <div style=\"background-color:white; padding:30px;\"> <table>";

                    _message.Body += "<tr><td> <strong> Location : </strong> </td><td>" + ClaimData.tbl.Location + "</td> <td><strong> Description : </strong> </td><td>" + ClaimData.tbl.Description + "</td> </tr>";

                    _message.Body += "<tr><td><strong> Are Building Materials Still Wet : </strong> </td><td>" + (ClaimData.tbl.AreBuildingMaterialsStillWet == true ? "Yes" : "No") + "</td><td><strong> How Much Water Present : </strong> </td> <td>" + ClaimData.tbl.HowMuchWater + " </td></tr>";

                    _message.Body += "<tr><td><strong> Estimated Time Damage Present : </strong> </td><td>" + ClaimData.tbl.EstimatedTimeDamagePresent + "</td> <td><strong> Planned Actions  : </strong> </td><td> " + ClaimData.tbl.PlannedActions + " </td></tr>";

                    _message.Body += "<tr><td><strong> Discovery Date : </strong> </td><td>" + ClaimData.tbl.DiscoveryDate + "</td> <td><strong> Suspected Cause : </strong> </td><td> " + ClaimData.tbl.SuspectedCause + " </td></tr>";
                    _message.Body += "<tr> <td><strong> Is Standing Water Present : </strong> </td><td> " + (ClaimData.tbl.IsStandingWaterPresent == true ? "Yes" : "No" ) + " </td><td><strong> Estimated Surface Area Effected : </strong> </td><td>" + ClaimData.tbl.EstimatedSurfaceAreaAffected  + "</td></tr>";

                    _message.Body += "<tr> <td><strong> Corrective Actions Taken  : </strong> </td><td> " + ClaimData.tbl.CorrectiveActionsTaken + " </td><td><strong> Additional Comments  : </strong> </td><td>" + ClaimData.tbl.AdditionalComments + "</td></tr>";
                  
                    _message.Body += "<tr><td><strong> Reported By : </strong> </td><td> " + ClaimData.tbl.ReportedBy + " </td> <td><strong> Reported Phone : </strong> </td><td>" + ClaimData.tbl.ReportedPhone + "</td></tr>";
                    _message.Body += "<tr><td><strong> Date Reported : </strong> </td><td> " + (ClaimData.tbl.DateReported == null ? "" : ClaimData.tbl.DateReported.Value.ToShortDateString()) + "</td><td><strong> Created By : </strong> </td><td>" + ClaimData.tbl.CreatedByName + "</td></tr>";

                    _message.Body += "<tr><td><strong> Created Date : </strong> </td><td>" + ClaimData.tbl.CreatedDate + "</td></tr></table>";
                    _message.Body += Convert.ToString(ConfigurationManager.AppSettings["EmailSignature"]) + "<div style=\"width:100%; \"> <img src=\"https://drive.google.com/uc?id=1PqI8SyVh9XZh_5Zzo1pr-l-KF1OIh5OQ\" style=\"height:100px;width:90%;padding:10px; \"> </div></div></div>";
                    _message.EmailTo = null; // populate from db
                    _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Report Name", "Property", "Datetimecreated");
                    

                    // Popute Target To Email's
                    
                    var workflowemails = _entities.proc_getworkflowemails(propertyid).FirstOrDefault();

                    if (!string.IsNullOrEmpty(workflowemails.InsuranceEmail))
                        _message.EmailTo.Add(workflowemails.InsuranceEmail);
                    else
                        _message.EmailTo.Add(workflowemails.EquityPartnerEmail);

                    _message.EmailTo.Add(workflowemails.RMEmail);
                    _message.EmailTo.Add(workflowemails.VPEmail);
                    _message.EmailTo.Add(workflowemails.RVPEmail);
                    _message.EmailTo.Add(workflowemails.AssetManager1Email);
                    _message.EmailTo.Add(workflowemails.AssetManager2Email);

                    EmailHelper.SendEmail(_message, RecordId, ClaimData.tbl.CreatedByName, ClaimData.tbl.CreatedBy.ToString());
                }





            }


            return true;
        }
    }
}

public static class EmailHelper
{
    public static SmtpClient SetMailServerSettings()
    {
        SmtpClient smtp = new SmtpClient();
        smtp.Host = "smtp.gmail.com"; // smtp.Host = "smtp.gmail.com";
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = true;
        NetworkCredential networkCredential = new NetworkCredential("sekhar.babu@forcitude.com", "R21221.Skr");

        smtp.Credentials = networkCredential;
        smtp.Port = 587; //587
        return smtp;

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

            mail.From= new MailAddress(Message.EmailFrom,"Carroll Organisation");
            foreach (var item in Message.EmailCc)
            {
                mail.CC.Add( new MailAddress(item));
            }
            mail.AlternateViews.Add(av1);

            mail.IsBodyHtml = true;
            mail.Subject = Message.Subject;
            mail.Body = Message.Body;
            mail.To.Clear();
            // remove this line before going production
            mail.To.Add("sekhar.babu@forcitude.com");
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.Priority = MailPriority.High;

            smtp.Send(mail);
            
            // Create an activity record
            IDataRepository _repo = new EntityDataRepository();
            _repo.LogActivity("Notification Email Sent", RecordCreatedBy, RecordCreatedByGuid, RecordId, "Notify Email Sent");
            _repo = null;

        }
        return true;
    }
}

public class EmailMessage
{
    public List<string> EmailTo { get; set; }
    public string[] EmailCc { get; set; }
    public string EmailFrom { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}