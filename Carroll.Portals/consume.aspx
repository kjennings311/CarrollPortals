<%@ Page Language="C#" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

        // replace with an instance of the users account.
        AccountSettings accountSettings = new AccountSettings();

        OneLogin.Saml.Response samlResponse = new OneLogin.Saml.Response(accountSettings);
        samlResponse.LoadXmlFromBase64(Request.Form["SAMLResponse"]);

        if (samlResponse.IsValid())
        {
            //Response.Write("OK!");
            //Response.Write(samlResponse.GetNameID());
            //Response.Redirect("success.aspx");
            // if we are here then we have successful login.. Let's redirect to assign roles and redirect to homepage...
            // NOW THAT USER IS LOGGED IN USING ONELOGIN.. 
            // LET'S MAKE SURE USER EXISTS IN LOCAL DATABASE FOR MAPPING PURPOSES.
            Carroll.Data.Entities.Repository.EntityUserRepository _userservice = new  Carroll.Data.Entities.Repository.EntityUserRepository();
            if (_userservice.CheckIfUserExists(samlResponse.GetNameID())){
                FormsAuthentication.SetAuthCookie(samlResponse.GetNameID(), true);
                Carroll.Portals.Models.LoggedInUser.AssignRolesToUser();
                Response.Redirect("/account/loginsuccess");
            }
            else
            {
                //User does not exist in the database.. send them to access denied page..
                Response.Redirect("/accessdenied.aspx");
            }
        }
        else
        {
            Response.Write("Failed");
        }
    }
</script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>TEST
        </div>
    </form>
</body>
</html>
