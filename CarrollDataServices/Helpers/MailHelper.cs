using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using Carroll.Data.Services.Models;

namespace Carroll.Data.Services.Helpers
{
    public sealed class MailHelper
    {
        //public static bool SendMarketingGroupEmails(List<EmailToContact> EmailToContact, string IgnoreEmails, string CurrentUser)
        //{

        //    IFormsRepository _repository = null;
        //        //SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        //        foreach (EmailToContact _contact in EmailToContact)
        //        {
        //            // If the email is ignored then do not send to group
        //            if ((string.IsNullOrEmpty(IgnoreEmails)) || (IgnoreEmails.ToUpper().IndexOf(_contact.ContactLeadId.ToString().ToUpper()) == -1))
        //            {
        //                RecordNote _note = new RecordNote();
        //                string sNote = string.Empty;
        //                try
        //                {
        //                    if (!string.IsNullOrEmpty(_contact.Email))
        //                    {
        //                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
        //                        message.To.Add(_contact.Email);
        //                        message.Subject = System.Web.HttpUtility.UrlDecode(_contact.EmailSubject);
        //                        message.From = new System.Net.Mail.MailAddress(_contact.From);
        //                        message.Body = System.Web.HttpUtility.UrlDecode(_contact.EmailContent);
        //                        message.IsBodyHtml = true;
        //                        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("localhost");
        //                        smtp.Send(message);
        //                        message = null;
        //                        sNote = "<span style=\"color:green;font-weight:bold;\"><i class=\"fa fa-envelope\"></i>&nbsp;&nbsp;Marketing email sent.</span><br/><br/>";

        //                    }
        //                    else
        //                    {
        //                        sNote = "<span style=\"color:red;font-weight:bold;\"><i class=\"fa fa-ban\"></i>&nbsp;&nbsp;Missing email address, unable to send marketing email</span><br/><br/>";

        //                    }


        //                }
        //                catch { sNote = "<span style=\"color:red;font-weight:bold;\"><i class=\"fa fa-ban\"></i>&nbsp;&nbsp; Error sending marketing email.</span><br/><br/>"; }

        //                // Log to user notes as activity
        //                sNote += "From: " + _contact.From + "<br/>";
        //                sNote += "Subject: " + _contact.EmailSubject + "<br/>";
        //                sNote += _contact.EmailContent;
        //                _note.Note = sNote;
        //                _note.RecordId = _contact.ContactLeadId;
        //                _repository = new EntityFormsRepository();
        //                _repository.CreateRecordNotes(_note, CurrentUser);
        //            }
               
        //    }
           
        //    return false;
        //}

        
    }
}
