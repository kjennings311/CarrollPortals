<%@ Page Language="C#" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["UseOneLogin"] == "true")
        {
            AccountSettings accountSettings = new AccountSettings();

            OneLogin.Saml.AuthRequest req = new OneLogin.Saml.AuthRequest(new AppSettings(), accountSettings);

            Response.Redirect(accountSettings.idp_sso_target_url + "?SAMLRequest=" + Server.UrlEncode(req.GetRequest(OneLogin.Saml.AuthRequest.AuthRequestFormat.Base64)));
        }
        else Response.Redirect("/");

    }
    </script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>Redirecting to SSO..
        </div>
    </form>
</body>
</html>
