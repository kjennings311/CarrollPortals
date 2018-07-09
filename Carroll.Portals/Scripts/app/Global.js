var User;
var Token;

// just an ajax function that needs to be called from each of the caller.. so a token can be retrieved before a second call.
function GetToken() {  
         $.ajax({
             type: "get",
             dataType: "json",
             url: "/getcurrentuser.ashx",
             async: false,
             success: function (data) {

                 if (JSON.stringify(data) === "relogin") location.href = "/account/signin";
                 else {
                     var userObject = data;
                     Token = userObject.Token;
                     User = userObject.SiteUser;

                     return Token;
                 }

             }
         });
    
}
      

