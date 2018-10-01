using Carroll.Portals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Carroll.Portals
{
    public partial class ValidateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // We are using this page as a dummy page to refresh login cookie..SOMEHOW WHEN COOKIE IS SET IN THE CONTROLLER ROLES ARE NOT
            // VISIBLE TO OTHER CLASSES UNTIL A SUBSEQUENT CALL.
            LoggedInUser.AssignRolesToUser();
            Response.Redirect("/home/index");
        }
    }
}