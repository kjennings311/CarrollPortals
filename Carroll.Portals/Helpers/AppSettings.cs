using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
/// <summary>
/// Summary description for AppSettings
/// </summary>
public class AppSettings
{
    public string assertionConsumerServiceUrl = string.Concat(ConfigurationManager.AppSettings["ServiceURL"], "consume.aspx");
        //"http://aspnet.carrollaccess.net:8000/Consume.aspx";
    public string issuer = "Carroll Forms";
}
