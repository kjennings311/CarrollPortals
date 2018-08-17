using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Carroll.Data.Entities.Repository;
namespace Carroll.Data.Services.Helpers
{
    public sealed class WorkflowHelper
    {

        public static bool RunNotifyWorkflow(string RecordId)
        {

            if(ConfigurationManager.AppSettings["NotifyWorkFlow"] == "true")
            {
                // run your logic here to 
                EmailMessage _message = new EmailMessage();
                _message.EmailFrom = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]);
                _message.EmailCc = Convert.ToString(ConfigurationManager.AppSettings["EmailFrom"]).Split(',');
                _message.EmailTo = null; // populate from db
                _message.Subject = string.Format(Convert.ToString(ConfigurationManager.AppSettings["NotifyEmailSubject"]), "Report Name", "Property", "Datetimecreated");
                _message.Body = ""; // format and send data.. 

               // EmailHelper.SendEmail(_message);



            }


            return true;
        }
    }
}

public static class EmailHelper
{

    public static bool SendEmail(EmailMessage Message, string RecordId, string RecordCreatedBy, string RecordCreatedByGuid)
    {
        // write your email function here..


        // Create an activity record
        IDataRepository _repo = new EntityDataRepository();
        _repo.LogActivity("Notification Email Sent", RecordCreatedBy, RecordCreatedByGuid, RecordId, "Notify Email Sent");
        _repo = null;

        return true;
    }
}

public class EmailMessage
{
    public string[] EmailTo { get; set; }
    public string[] EmailCc { get; set; }
    public string EmailFrom { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}