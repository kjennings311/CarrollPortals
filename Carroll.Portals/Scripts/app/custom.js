
 // var $BaseApiUrl = "http://localhost:1002/"; 

 var $BaseApiUrl = "http://aspnet.carrollaccess.net:1002/";

//49786/";
//   and UserOject are global variables can be used here.

function validateEmail(emailID)
{
    atpos = emailID.indexOf("@");
    dotpos = emailID.lastIndexOf(".");
    if (atpos < 1 || (dotpos - atpos < 2)) {
    
        return false;
    }
    return (true);
}

var imagebase64 = "";

function encodeImageFileAsURL(element)
{
      var filesSelected = element.files;
    if (filesSelected.length > 0)
    {
        var fileToLoad = filesSelected[0];
        var fileReader = new FileReader();
        var srcData = "";
        fileReader.onload = function (fileLoadedEvent) {
             srcData= fileLoadedEvent.target.result; // <--- data: base64
            console.log('src data' + srcData);
            var newImage = document.createElement('img');
            newImage.src = srcData;
            imagebase64 = srcData;
            document.getElementById("imgTest").innerHTML = newImage.outerHTML;
        }
        fileReader.readAsDataURL(fileToLoad);
      
        console.log("base vlaue is "+imagebase64)
    }
}

function closereviewmodal() {

    $("#reviewmodal").modal('hide'); 
}


function submitformdata()
{
    $("#reviewmodal").modal('hide'); 
    $("#homeval").val('0');
    $('.dynamicForm #savechanges').click();
   
}



function BindElements()
{

    $form = $('.dynamicForm');

    $('.dynamicForm #savechanges').click(function ()
    {
        if ($("#homeval").length > 0)
        {
            if ($("#homeval").val() == '1')
            {
                $("#reviewmodal").modal('show'); 
                return false;
            }
        }
        
        $('.dynamicForm #savechanges').attr('disabled', true);
        $('.success-message').hide();
        $('.failure-message').hide();

    //    if (!CheckFormErrors($form)) {

        var $this = $(this);

        var RecordId = $('.btnEdit').attr("itemId");
        var FormName = $('.dynamicForm #savechanges').attr("formname");
        if (RecordId == '' || RecordId=== undefined) formUrl = "api/form/GenerateForm/" + FormName;
        else formUrl = "api/form/GenerateEditForm?entitytype=" + FormName + "&RECORDID=" + RecordId;


     //   var formUrl = "api/form/GenerateForm/" + $('.dynamicForm #savechanges').attr("formname");

        // if form is user then check if user exists with email id or not

      
        if ($('.dynamicForm #savechanges').attr("formname") == "user" && RecordId == '')
        {

       
        $.ajax({
            type: "GET",
            url: $BaseApiUrl + "api/user/checkifuserexists/",
            data:"id="+$("#UserEmail").val(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
            async: false,
            success: function (data) {

                if (data == true)
                {
                    alert('success insdie');
                    var _err = "<b>Please correct the following errors</b><ul>";

                    _err += "<li> User with given Email Already Exists, Please Use Another </li>";
                    $form.find($this).closest('.form-group').addClass('has-error');
                    $form.find('#failureMessage').html(_err);
                    $form.find('.failure-message').show('slow');
                    ScrollToElement($form.find('.failure-message'));
                    setTimeout(ScrollToElement($form), 3000);

                }
                else
                {
                    return;
                }
               
            }
          
            });
        }


            //*****************************************************************************
            // Let's get the original form and then load values into original form to send back to server for validation

            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + formUrl,
                beforeSend: function (xhr)
                {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                },
                success: function (data) {
                    // var $FormElements = $formBegin;
                    var $data = data;
                  
                    var $fields = $data["formFields"];
                    for (var i = 0; i < $fields.length; i++) {
                        // alert($fields[i]["fieldLabel"]+ " " + $fields[i]["fieldType"]);
                        // Build the form elements here
                        switch ($fields[i]["fieldType"]) {
                            case "Text":
                            case "LongText":
                            case "Password":                         
                            case "Select":                           
                            case "Person":    
                                $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                break;
                            case "Check":
                                if ($('#' + $fields[i]["fieldName"]).prop('checked') == true)
                                {
                                    //.is(":checked")
                                    $fields[i]["fieldValue"] = true;
                                   // console.log("check field value" + true);
                                }
                                else
                                {
                                    $fields[i]["fieldValue"] = false;
                                }
                                break;
                            case "File":
                               // console.log('before assign ' + $fields[i]["fieldValue"] + " " + imagebase64);
                                if(imagebase64!="")
                                $fields[i]["fieldValue"] = imagebase64;

                               // console.log('after assign ' + $fields[i]["fieldValue"] + " " + imagebase64);
                                break;
                            case "Hidden":
                                //// Let's populate Created and CreatedName'
                                //switch ($fields[i]["fieldName"].toLowerCase()) {
                                
                                //    case "createdby":
                                       
                                 
                                //            $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                     
                                //                                                break;
                                //    case "createdbyname":
                                      
                                      
                                //            $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                  
                                        
                                //        break;
                                //    case "UserPhoto":
                                //        $fields[i]["fieldValue"] = imagebase64;
                                //        break;
                                //    default:
                                //        $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                //        break;
                                //}
                                $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                break;
                            default:

                                break;
                                
                        }
                        
                    }

                    
                    
                    // Let's set user id field so we can use that as a reference
                 //   $data["userId"] = User.UserId + "~" + User.FirstName + " " + User.LastName
                    //  alert(JSON.stringify($data));
                    // now that we have updated Form record.. Let's pass it back to server with all  data for validation..

                    /**************************************************************************************/
                    
                    $.ajax({
                        type: "POST",
                        url: $BaseApiUrl + "api/Form/CreateUpdateFormData/" + $('.dynamicForm #savechanges').attr("formname"),
                        data: JSON.stringify($data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                        async: false,
                        statusCode: {
                            200: function (data) {
                                // if we are here it means all good.. 
                                alert(data.responseText);
                            },
                            201: function (data) {
                                // if we are here it means all good.. 
                                if (data.responseText == "OK") {
                                    // show success message
                                    //$form.find('#successMessage').html("Record successfully updated!");
                                    //$form.find('.success-message').show('slow');
                                    //ScrollToElement($form.find('.success-message'));
                                    $('.dynamicForm #savechanges').attr('disabled', false);
                                    $(".alert").html('Record Successfully Updated !');
                                    $('#toastnotification').modal('show'); 
                                    // go back to previous screen after 8 seconds
                                   setTimeout(function () {  location.reload(); } , 3000);
                                } else {

                                    //if (originatingrecord != '') location.href = "/viewrecord/" + originatingrecord;
                                    //else location.href = "/viewrecord/" + data.responseText;
                                }
                            },

                           
                            500: function (data) {
                                alert(data.responseText);
                            },
                            400: function (XMLHttpResponse, textStatus, errorThrown) {
                                var _err = "<b>Please correct the following errors</b><ul>";
                                try {
                                    var $response = $.parseJSON(XMLHttpResponse.responseText);

                                    for (var i = 0, len = $response.length; i < len; i++) {
                                        // highlight fields in red that have errors
                                        $this = $("#" + $response[i].Key);
                                        _err += "<li>" + $response[i].Message + "</li>";
                                        $form.find($this).closest('.form-group').addClass('has-error');
                                    }
                                    $form.find('#failureMessage').html(_err);
                                    $form.find('.failure-message').show('slow');
                                    ScrollToElement($form.find('.failure-message'));
                                    setTimeout(ScrollToElement($form), 3000);
                                } catch (err) { alert(err.message); }

                            }
                        } // status code
                    });
                    /******************************update/create end************************************/

                }

            });

            //*****************************************************************************

            //switch ($('.dynamicForm #savechanges').attr("formname")) {
            //    case "property":
            //        // Create property object that we can post to the server

            //        var jsonObject = {};
            //        jsonObject["PropertyName"] = $('.dynamicForm #PropertyName').val();
            //        jsonObject["PropertyNumber"] = $('.dynamicForm #PropertyNumber').val();
            //        jsonObject["LegalName"] = $('.dynamicForm #LegalName').val();
            //        jsonObject["Units"] = $('.dynamicForm #Units').val();
            //        jsonObject["TaxId"] = $('.dynamicForm #TaxId').val();
            //        jsonObject["PropertyAddress"] = $('.dynamicForm #PropertyAddress').val();
            //        jsonObject["City"] = $('.dynamicForm #City').val();
            //        jsonObject["State"] = $('.dynamicForm #State').val();
            //        jsonObject["ZipCode"] = $('.dynamicForm #ZipCode').val();
            //        jsonObject["PhoneNumber"] = $('.dynamicForm #PhoneNumber').val();
            //        jsonObject["PropertyEmail"] = $('.dynamicForm #PropertyEmail').val();
            //        jsonObject["AcquisitionDate"] = $('.dynamicForm #AcquisitionDate').val();
            //        jsonObject["DispositionDate"] = $('.dynamicForm #DispositionDate').val();
            //        jsonObject["IsOwned"] = $('.dynamicForm #IsOwned').val();
            //        jsonObject["IsActive"] = $('.dynamicForm #IsActive').val();               
            //        jsonObject["VicePresident"] = $('.dynamicForm #VicePresident').val();
            //        jsonObject["PropertyManager"] = $('.dynamicForm #PropertyManager').val();
            //        jsonObject["RegionalManager"] = $('.dynamicForm #RegionalManager').val();
            //        jsonObject["AssetManager1"] = $('.dynamicForm #AssetManager1').val();
            //        jsonObject["AssetManager2"] = $('.dynamicForm #AssetManager2').val();
            //        jsonObject["ConstructionManager"] = $('.dynamicForm #ConstructionManager').val();
            //        jsonObject["PropertyId"] = ($('.dynamicForm #PropertyId').val() == '') ? "0" : $('.dynamicForm #PropertyId').val();
            //        jsonObject["CreatedBy"] = User.UserId;
            //        jsonObject["CreatedByName"] = User.FirstName + " " + User.LastName;
            //        jsonObject["EmailAddress"] = $('.dynamicForm #PropertyEmail').val();




            //                 $.ajax({
            //                    type: "post",
            //                    url: $BaseApiUrl + "api/Property/CUProperty",
            //                    data: JSON.stringify(jsonObject),
            //                    contentType: "application/json; charset=utf-8",
            //                    dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
            //                    async: true,                            
            //                    beforeSend: function (xhr) {
            //                        xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

            //                    },
            //                    success: function (result) {
            //                        alert("Saved changes.");
            //                        ToggleAdd('property');
            //                    },
            //                    error: function (msg) {
            //                        // if(msg.statusCode != 401) alert('Error getting info..');
            //                        alert("Error occured! Please try again later.");
            //                    }

            //        });

            //                 break;
            //    case "contact":

            //        break;
            //    default:
            //        break;


            //}


 //       }

//*********
    });
}


function LoadUserProperty() {
    var val = "";

    $.ajax({
        url: $BaseApiUrl + "api/data/GetUserProperty?userid=" + $("#CreatedBy").val(),
        type: 'GET',
        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
        processData: false,
        contentType: false,
        success: function (data) {
                     $("#UserPropertyAccess").val(""+data);           
        },
        error: function (ts) {
            alert('error' + ts.errorMessage);
        }
    });
    return val;
}


function getForm(FormName, RecordId)
{
    var formUrl = "";
    var TXT_ERROR = " <div class=\"alert alert-danger alert-dismissable failure-message\" style=\"display:none\"><div id=\"failureMessage\">there was an error!</div> </div>";
    var TXT_SUCCESS = "<div class=\"alert alert-success alert-dismissable success-message\" style=\"display:none\"><div id=\"successMessage\"></div> </div>";
    var $formBegin = '<form  class="form-horizontal CustomForm">';
    var $formEnd = '</form>';
    var $line = '<div class="hr-line-dashed"></div>';
    var $textbox = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input maxlength="100"   type="text" validationformat="{1}" class="form-control {2}" id="{3}" {4} value="{5}"></div></div>';
    var $datebox = '<div class="form-group"><label class="col-sm-12 control-label">  <a  class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input maxlength="100"  type="date" validationformat="{1}" data-date-format="mm/dd/yyyy" class="form-control {2}" id="{3}" {4} value="{5}"></div></div>';
    var $longtext = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><textarea validationformat="{1}"  class="form-control {2}" id="{3}" {4} > {5} </textarea> </div></div>';
    var $passbox = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input maxlength="100" type="password" validationformat="{1}" class="form-control {2}"  id="{3}" {4} value="{5}"></div></div>';
    var $filebox = '<div class="form-group"><label class="col-sm-2 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0}  </label ><div class="col-sm-10"><input maxlength="100" type="file" validationformat="{1}" onchange="encodeImageFileAsURL(this);" class="form-control {2}" id="{3}" {4} value="{5}"></div> <div id="imgTest" style="background: black;clear: both;margin-left:30%;width:300px;"><img src="{5}" style="width:80px;height:80px;"> </div></div>';
    var $hiddenField = '<input type="hidden" id="{0}" value="{1}"/>';
    var $checkbox = ' <div class="form-group"><label class="col-sm-3 control-label"> <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{3}" > i </a> {0} </label><div class="col-sm-6"> <div class="col-md-1" > <input class="form-control" type="checkbox" style="width:18px;"  id="{1}" value="1"   {2}></div> </div></div>';
    var $person = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input type="text" validationformat="{1}" class="form-control {2}"  id="{3}" {4}></div></div>';
    var $savebuttons = '  <div class="hr-line-dashed"></div>'
        + TXT_SUCCESS + TXT_ERROR
        + '<div class="form-group" >'
        + '<div class="col-sm-4 col-sm-offset-4">'
        + '<a class="btn btn-white" href="javascript:location.reload();">Cancel</a>&nbsp;'
        + '<a id="savechanges" class="btn btn-primary btn-add" style="background:#2f4050 !important;border:none !important" href="javascript:void(0);" formname="' + FormName + '">Save changes</button>'
        + '</div></div >';
    var $select = '<div class="form-group">'
        + '<label class="col-sm-12 control-label"> <a class="tooltipwala" data-container="body" href="#" data-toggle="popover" data-trigger="hover" data-content="{3}"  > i </a> {0} </label>'
        + '<div class="col-sm-10">'
        + '<select data-placeholder="Select option" class="form-control {1}" id="{2}" >'
        + '<option value="">Select</option>'
        + '</select>'
        + '</div>'
        + '</div>';
    if (RecordId == '') formUrl = "api/form/GenerateForm/" + FormName;
    else formUrl = "api/form/GenerateEditForm?entitytype=" + FormName + "&RECORDID=" + RecordId;
        
    $.ajax({
        type: "get",
        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
        url: $BaseApiUrl + formUrl ,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

        },
        success: function (data) {
            var $FormElements = $formBegin;
            var $data = data;

            console.log(data);

            var $fields = $data["formFields"];
            for (var i = 0; i < $fields.length; i++) {
               // alert($fields[i]["fieldLabel"]+ " " + $fields[i]["fieldType"]);
                // Build the form elements here
                switch ($fields[i]["fieldType"]) {
                    case "Text":

                        var $req = $fields[i]["required"];
                        var $datamask = "";
                     

                        if ($fields[i]["fieldValidationType"] == "DateTime" && $fields[i]["fieldValue"] != null)
                        {
                            var s = $fields[i]["fieldValue"];
                            
                            var bits = s.split(/\D/);
                          //  console.log(bits);

                            var datestring = bits[1]+"/"+bits[0]+"/"+bits[2];
                           // console.log(datestring);
                         
                          //  var datestring = d.getMonth()+"/"+d.getDate() + "/"  + d.getFullYear();

                            $FormElements += format($datebox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : datestring);
                        }
                        else
                        {
                            $FormElements += format($textbox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"], ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);
                        }
                       
                        break;
                    case "LongText":

                        var $req = $fields[i]["required"];
                        var $datamask = "";

                        $FormElements += format($longtext, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"], ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);
                        break;
                    case "Password":

                        var $req = $fields[i]["required"];
                        var $datamask = "";

                        $FormElements += format($passbox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"], ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);
                        break;
                    case "File":
                        var $req = $fields[i]["required"];
                        var $datamask = "";
                    
                        $FormElements += format($filebox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"], ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);
                                              
                        break;
                    case "Check":
                      //  var $req = $fields[i]["required"];
                        var checkedtext = "";
                        var checked = ($fields[i]["fieldValue"] == null) ? false : $fields[i]["fieldValue"];
                     

                        if (checked=="True") checkedtext = "checked=checked";
                        else checkedtext = "";
                     

                        $FormElements += format($checkbox, $fields[i]["fieldLabel"], $fields[i]["fieldName"], checkedtext, ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);
                        break;
                    case "Select":
                        var $req = $fields[i]["required"];
                        var dataurl = $fields[i]["dataLoadUrl"];

                        $FormElements += format($select, $fields[i]["fieldLabel"], ($req) ? "required" : "", $fields[i]["fieldName"], ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);

                        //Let's Load select options from websevice
                      
                        if ($fields[i]["fieldName"] == "PropertyId")
                        {
                          
                            if (RecordId == '')
                            {                              
                                LoadOptionsProp($fields[i]["fieldName"], dataurl, $("#UserPropertyAccess").val());
                            }
                        }
                        else
                        {
                            //Let's Load select options from websevice
                            LoadOptions($fields[i]["fieldName"], dataurl, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"]);

                        }
                           
                        break;
                    case "Person":
                        var $req = $fields[i]["required"];
                        var personId = ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"];
                        var val = 'Value="' + personId + '"';
                        $FormElements += format($person, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required tokenInput" : "tokenInput", $fields[i]["fieldName"], val, ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);
                        break;
                    case "Hidden":
                        $FormElements += format($hiddenField, $fields[i]["fieldName"], ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"]);
                        
                }

                
              
            }
            $FormElements += $savebuttons;
            $FormElements += $formEnd;
            $(".dynamicForm").html($FormElements);
            // now that the form is loaded here.. let's bind events here
            BindElements();
            ApplyInputMask($(".dynamicForm"));
            ApplyTokenInput($(".dynamicForm"));
        }

    });
 }

// Loads choices in select, list boxes etc
function LoadOptions(fieldId, DataLoadUrl, value)
{
    var options = "";
    var selected = "";

    $.get($BaseApiUrl + DataLoadUrl, function (data) {
       
        for (var i = 0; i < data.length; i++)
        {           

            if (data[i]["key"] == value)
                selected = "selected=selected";
            options += "<option value=\"" + data[i]["key"] + "\"" + selected + ">" + data[i]["value"] + "</option>";
                selected = "";        
    
        }
        // now let's load options into select box
        $('#' + fieldId).append(options);
        
    });
}


// Loads choices in select, list boxes etc
function LoadOptionsProp(fieldId, DataLoadUrl, value) {
    var options = "";
    var selected = "";

    $.get($BaseApiUrl + DataLoadUrl, function (data) {

        for (var i = 0; i < data.length; i++)
        {
            if ($("#UserR").val() == "Administrator")
            {

                if (data[i]["key"] == value)
                    selected = "selected=selected";
                options += "<option value=\"" + data[i]["key"] + "\"" + selected + ">" + data[i]["value"] + "</option>";
                selected = "";

            }
            else
            {
                if (value.toLowerCase().indexOf(data[i]["key"]) >= 0)
           {
                    selected = "selected=selected";
                    options += "<option value=\"" + data[i]["key"] + "\"" + selected + ">" + data[i]["value"] + "</option>";
                    selected = "";
                }
                    
            }

        }
        // now let's load options into select box
        $('#' + fieldId).append(options);

    });
}

function LoadHrForm(formname) {
    $("#myModal").modal('show');
}

function ToggleAdd(formaname) {
    // these controls are in properties.aspx page

    if (!$('.RowsContatiner').is(":visible"))
    {
        if(!confirm("Form is Opened, Any Un Saved Changes will be lost, Do you want to Continue?"))
        {           
            return false;
        }

    }

    $('.RowsContatiner').toggle('slow', function () {
        $('.AddEditContainer').toggle('slow', function () {
            if ($(this).is(":visible")) {
                getForm(formaname, '');
                if ($(".form-heading").length)
                {
                    if (formaname =="FormPropertyDamageClaim")
                        $(".form-heading").html("Add Property Damage Claim");
                    else if (formaname == "FormGeneralLiabilityClaim")
                        $(".form-heading").html("Add General Liability Claim");
                    else if (formaname == "FormMoldDamageClaim")
                        $(".form-heading").html("Add Mold Damage Claim");
                    else
                        $(".form-heading").html("");

                    $("#homeval").val('1');

                }

            
                //var popOverSettings = {
                //    placement: 'right',
                //    container: 'body',
                //    html: true,
                //    selector: '[data-toggle="popover"]', //Sepcify the selector here
                //    content: function () {
                //        return $('#popover-content').html();
                //    }
                //}

                //$('body').popover(popOverSettings);
            }
        });
    });
   
}


function LoadForm(formaname)
{
    // these controls are in properties.aspx page

    // hide popup
    // open another popup, load the form, show header
    $('#myModal').modal('hide');

    getForm(formaname, '');
    if ($(".form-heading").length) {
        if (formaname == "FormPropertyDamageClaim")
            $(".form-heading").html("Add Property Damage Claim");
        else if (formaname == "FormGeneralLiabilityClaim")
            $(".form-heading").html("Add General Liability Claim");
        else if (formaname == "FormMoldDamageClaim")
            $(".form-heading").html("Add Mold Damage Claim");
        else
            $(".form-heading").html("");

        $('.claimmodal').modal('hide');

        $('.incidentformmodal').modal('show');

        //  $(".hidewhenformopen").hide();

        //  $('[data-toggle="popover"]').popover(); 

        var popOverSettings = {
            placement: 'top',
            container: 'body',
            trigger: 'hover',
            html: true,
            selector: '[data-toggle="popover"]', //Sepcify the selector here
            content: function () {
                return $('#popover-content').html() === undefined ? "" : $('#popover-content').html();
            }
        }

        $('body').popover(popOverSettings);

        $(".tooltipwala").each(function () {
            if ($(this).attr("data-content") == "") {
                $(this).hide();
            }

        });


    }





    //if ($('.AddEditContainer').is(":visible"))
    //{
    //    if (!confirm("Form is Opened, Any Un Saved Changes will be lost, Do you want to Continue?")) {
    //        return false;
    //    }
    //    else
    //    {
    //        $('#myModal').modal('hide');
    //    }
    //}
    //else {
    //    LoadUserProperty();
    //}

  

    //$('.RowsContatiner').toggle('slow', function ()
    //{
    //    $('.AddEditContainer').toggle('slow', function ()
    //    {

    //        if ($(this).is(":visible")) {
    //            getForm(formaname, '');
    //            if ($(".form-heading").length) {
    //                if (formaname == "FormPropertyDamageClaim")
    //                    $(".form-heading").html("Add Property Damage Claim");
    //                else if (formaname == "FormGeneralLiabilityClaim")
    //                    $(".form-heading").html("Add General Liability Claim");
    //                else if (formaname == "FormMoldDamageClaim")
    //                    $(".form-heading").html("Add Mold Damage Claim");
    //                else
    //                    $(".form-heading").html("");

    //                $('.claimmodal').modal('hide');

    //                $('.incidentformmodal').modal('show');

    //                $(".hidewhenformopen").hide();

    //                  $('[data-toggle="popover"]').popover(); 

    //                var popOverSettings = {
    //                    placement: 'top',
    //                    container: 'body',
    //                    trigger: 'hover',
    //                    html: true,
    //                    selector: '[data-toggle="popover"]', //Sepcify the selector here
    //                    content: function () {
    //                        return $('#popover-content').html() === undefined ? "" : $('#popover-content').html();
    //                    }
    //                }

    //                $('body').popover(popOverSettings);

    //                $(".tooltipwala").each(function ()
    //                {
    //                    if ($(this).attr("data-content") == "")
    //                    {
    //                        $(this).hide();
    //                    }
                       
    //                });


    //            }



    //        }
    //        else {

    //            $(".hidewhenformopen").show();

    //        }

    //    });
    //});

}

function ToggleEdit(formname)
{
    var recordid = $('.btnEdit').attr("itemId");
    
    // these controls are in properties.aspx page
    $('.RowsContatiner').toggle('slow', function () {
        $('.AddEditContainer').toggle('slow', function () {
            if ($(this).is(":visible")) { getForm(formname, recordid); }
        });
    });

}


// takes thisIsTest converts to this Is Test
function splitCamelCase(s) {
    return s.split(/(?=[A-Z])/).join(' ');
}

// this function let's you enable disable add delete buttons' 
function HandleRowClick(obj)
{
    var $this = jQuery(obj);

    if ($("#homecontainer").length)
        console.log(JSON.stringify(obj) + "Selected Item in Home");
   
    if (!$this.hasClass('selected'))
    {
       
    //    $this.removeClass('selected');

        $('.btnDelete').css("color", "red");
        $('.btnDelete').removeAttr("disabled");
        $('.btnDelete').attr("itemId", $this.attr("id"));
        $('.btnDelete').attr("itemType", $this.attr("itemType"));
        $('.btnEdit').css("color", "green");
        $('.btnEdit').removeAttr("disabled");
        $('.btnEdit').attr("itemId", $this.attr("id"));
        $('.btnEdit').attr("itemType", $this.attr("itemType"));
        // bind button events we can lookup item id on button attribute

        $('.btnDelete').unbind('click').bind('click', function ()
        {
            if (confirm("Are you sure you want to delete?"))
            {
                var idToDelete = $(this).attr("itemId");
                var ItemType = $(this).attr("itemType");
             
                //  make ajax call to delete the row and remove the row.
                $.ajax({
                    type: "post",
                    dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                    url: $BaseApiUrl + "api/data/deleterecord?entitytype=" + ItemType + "&recordid=" + idToDelete,
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                    },
                    success: function (data) {
                        if (data) {
                            alert("record deleted!");
                            $this.animate({ backgroundColor: "#fbc7c7" }, "fast")
                                .animate({ opacity: "hide" }, "slow");

                        }

                    }

                });

                       
            }
        });
    }
    else {
        //$('.dtprops tbody tr').removeClass('selected');
        //$this.addClass('selected');
        $('.btnDelete').css("color", "#ccc");
        $('.btnDelete').attr("disabled", "disabled");
        $('.btnDelete').removeAttr("itemId");
        $('.btnDelete').removeAttr("itemType");
        $('.btnEdit').css("color", "#ccc");
        $('.btnEdit').attr("disabled", "disabled");
        $('.btnEdit').removeAttr("itemId");
        $('.btnEdit').removeAttr("itemType");
        $('.btnDelete').unbind('click').bind('click', function () {

            return false;
        });
    }
}

function LoadFormView(data)
{
    location.href = "http://"+location.hostname+"/Home/viewclaim/?Claim=" + data;
}

function CheckNull(variable)
{
    if (variable == null)
    {
        return '';
    }
    else
        return variable;
}

// called from Contacts page
function LoadContacts() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=Contact",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["contactId"]);
                            $(row).attr('itemType', "Contact");
                            $(row).attr('onClick', 'HandleRowClick(this);');

                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },

                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }

                        ],

                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta) {

                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'

                                }
                            },

                            { "data": "firstName", "name": "firstName", "autoWidth": false },
                            { "data": "lastName", "name": "lastName", "autoWidth": false },
                            { "data": "title", "name": "title", "autoWidth": false },
                            { "data": "email", "name": "email", "autoWidth": false },
                            { "data": "phone", "name": "phone", "autoWidth": false }
                           
                        ]
                    }).columns.adjust();


                }
            });
        }
    );
}

function LoadPartners() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=Partner",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["equityPartnerId"]);
                            $(row).attr('itemType', "Partner");
                            $(row).attr('onClick', 'HandleRowClick(this);');

                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }

                        ],

                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            //{
                            //    render: function (data, type, row, meta) {

                            //        return '<i style="color:#1ab394" class="fa fa-edit"></i>'

                            //    }
                            //},

                            { "data": "partnerName", "name": "partnerName", "autoWidth": false },
                            { "data": "addressLine1", "name": "addressLine1", "autoWidth": false },
                            { "data": "addressLine2", "name": "addressLine2", "autoWidth": false },
                            { "data": "city", "name": "city", "autoWidth": false },
                            { "data": "state", "name": "state", "autoWidth": false },
                            { "data": "zipCode", "name": "zipCode", "autoWidth": false },                          
                            { "data": "contactId", "name": "contactId", "autoWidth": false },
                            //{
                            //    //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                            //    data: 'contactId',
                            //    render: function (data) {
                            //        if (data != null)
                            //        {
                            //            //select CONCAT('{"name":"', FirstName, ' ', LastName, '","id":"', ContactId, '"}') from dbo.Contacts where ContactId = [CarrollForms].[dbo].[Properties].VicePresident) as VicePresident
                            //            $.get($BaseApiUrl + "api/contact/GetContact?contactId=" + data, function (result)
                            //            {
                            //                console.log(result);
                            //                return '<a href="/contact?id=' + result["contactId"] + '">' + result["firstName"] + ' ' + result["lastName"] + '</a>';
                            //            });
                            //           // return data;                                       
                            //        } else return '';
                            //    }
                            //},
                            {
                                "data": "createdDate", 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            },
                            { "data": "createdByName", "name": "createdByName", "autoWidth": false }

                        ]
                    }).columns.adjust();


                }
            });
        }
    );
}
// called from properties page
function LoadProperties() {
    //$('.ibox').children('.ibox-content').toggleClass('sk-loading');

    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=Property",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                },
                statusCode: {
                    401: function () {
                        // beacuse the session with oAuth is limited to 10 minutes.. we will need to redirect to home page to reset all objects
                        //$.when(GetToken()).then(
                        //    function () {  });
                        location.href = "/";
                        // location.href = "/account/SignOut"
                    },
                    404: function () { },
                    200: function () { },
                    201: function () { },
                    202: function () { }
                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["propertyId"]);
                            $(row).attr('itemType', "property");
                            $(row).attr('onClick', 'HandleRowClick(this);');

                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },

                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }

                        ],

                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            //{
                            //    render: function (data, type, row, meta) {

                            //        //return '<i style="color:#1ab394" class="fa fa-2x fa-edit"></i>&nbsp;&nbsp;<i style="color:red" class="fa fa-2x fa-trash" onClick="alert($(row).attr(\'id\');"></i>'
                            //        return ''
                            //    }
                            //},

                            { "data": "propertyNumber", "name": "propertyNumber", "autoWidth": false },
                            { "data": "propertyName", "name": "propertyName", "autoWidth": false },
                            { "data": "legalName", "name": "legalName", "autoWidth": false },
                            { "data": "units", "name": "units", "autoWidth": false },
                            { "data": "isOwned", "name": "isOwned", "autoWidth": false },
                            { "data": "propertyAddress", "name": "propertyAddress", "autoWidth": false },
                            { "data": "city", "name": "city", "autoWidth": false },
                            { "data": "state", "name": "state", "autoWidth": false },
                            { "data": "zipCode", "name": "zipCode", "autoWidth": false },
                            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": false },
                            { "data": "emailAddress", "name": "emailAddress", "autoWidth": false },
                            { "data": "equityPartnerSiteCode", "name": "equityPartnerSiteCode", "autoWidth": false },
                            {
                               
                                data: 'equityPartner',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/equitypartner?id=' + result["id"] + '">' + result["name"] + '</a>';
                                     
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'equityPartnerContact',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },

                            //{ "data": "vicePresident", "name": "vicePresident", "autoWidth": false },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'vicePresident',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'regionalVicePresident',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'regionalManager',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'propertyManager',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'constructionManager',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'assetManager1',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'assetManager2',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },

                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'insuranceContact',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/contact?id=' + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                "data": "acquisitionDate", 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            },
                            {
                                "data": "dispositionDate", 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            },
                            {
                                "data": "createdDate", 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            },
                            { "data": "createdByName", "name": "createdByName", "autoWidth": false }


                        ]
                    }).columns.adjust();

                    //  $('.ibox').children('.ibox-content').toggleClass('sk-loading');

                }
            });
        }
    );
}

// called from Users page
function LoadUsers() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=User",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["userId"]);
                            $(row).attr('itemType', "User");
                            $(row).attr('onClick', 'HandleRowClick(this);');
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },

                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }

                        ],

                        columns: [  //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta)
                                {
                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                                }
                            },
                            { "data": "userEmail", "name": "userEmail", "autoWidth": false },
                            { "data": "firstName", "name": "firstName", "autoWidth": false },
                            { "data": "lastName", "name": "lastName", "autoWidth": false },
                            { "data": "phone", "name": "phone", "autoWidth": false },
                            {
                                "data": "userPhoto", render: function (data, type, row, meta) {
                                    return '<img src = "'+ data +'" width = "50px" height = "50px" > ';
                                }
                        , "name": "userPhoto", "autoWidth": false },
                            { "data": "isActive", "name": "isActive", "autoWidth": false },
                            { "data": "isApproved", "name": "isApproved", "autoWidth": false }]
                    }).columns.adjust();
                }
            });
        }
    );
}


// called from User Role page
function LoadUserRoles() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=UserInRole",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["userRoleId"]);
                            $(row).attr('itemType', "UserInRole");
                            $(row).attr('onClick', 'HandleRowClick(this);');
                            console.log($(row));
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },

                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }

                        ],

                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta) {
                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                                }
                            },
                            { "data": "userEmail", "name": "userEmail", "autoWidth": false },
                            { "data": "userName", "name": "userName", "autoWidth": false },
                            { "data": "roleName", "name": "roleName", "autoWidth": false }
                         ]
                    }).columns.adjust();
                }
            });
        }
    );
}



// called from User Role page
function LoadUserProperties() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=UserInProperty",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["userInPropertyId"]);
                            $(row).attr('itemType', "UserInProperty");
                            $(row).attr('onClick', 'HandleRowClick(this);');
                            console.log($(row));
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }
                        ],

                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta) {
                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                                }
                            },
                            { "data": "userEmail", "name": "userEmail", "autoWidth": false },
                            { "data": "userName", "name": "userName", "autoWidth": false },
                            { "data": "propertyName", "name": "propertyName", "autoWidth": false }
                        ]
                    }).columns.adjust();
                }
            });
        }
    );
}



// called from User Role page
function LoadFormPropertyDamageClaims() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=FormPropertyDamageClaim",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["pdlId"]);
                            $(row).attr('itemType', "FormPropertyDamageClaim");
                            $(row).attr('onClick', 'HandleRowClick(this);');
                            console.log($(row));
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }
                        ],
                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta) {
                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                                }
                            },
                            { "data": "propertyName", "name": "propertyName", "autoWidth": false },
                            { "data": "incidentDateTime", "name": "incidentDateTime", "autoWidth": false },
                            { "data": "weatherConditions", "name": "weatherConditions", "autoWidth": false },
                            { "data": "incidentLocation", "name": "incidentLocation", "autoWidth": false },
                            { "data": "descriptionOfProperty", "name": "descriptionOfProperty", "autoWidth": false },
                            { "data": "incidentDescription", "name": "incidentDescription", "autoWidth": false },
                            { "data": "estimateOfDamage", "name": "estimateOfDamage", "autoWidth": false },
                            { "data": "authoritiesContacted", "name": "authoritiesContacted", "autoWidth": false },
                            { "data": "contactPerson", "name": "contactPerson", "autoWidth": false },
                            { "data": "reportNumber", "name": "reportNumber", "autoWidth": false },
                            { "data": "lossOfRevenues", "name": "lossOfRevenues", "autoWidth": false },
                            { "data": "witnessPresent", "name": "witnessPresent", "autoWidth": false },
                            { "data": "witnessName", "name": "witnessName", "autoWidth": false },
                            { "data": "witnessPhone", "name": "witnessPhone", "autoWidth": false },
                            { "data": "witnessAddress", "name": "witnessAddress", "autoWidth": false },
                            { "data": "incidentReportedBy", "name": "incidentReportedBy", "autoWidth": false },
                            { "data": "reportedPhone", "name": "reportedPhone", "autoWidth": false },
                            { "data": "dateReported", "name": "dateReported", "autoWidth": false },
                            { "data": "createdDate", "name": "createdDate", "autoWidth": false },
                            { "data": "createdByName", "name": "createdByName", "autoWidth": false }
                        ]
                    }).columns.adjust();
                }
            });
        }
    );
}


// called from User Role page
function LoadGeneralLiabilityClaims() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=FormGeneralLiabilityClaim",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["gllId"]);
                            $(row).attr('itemType', "FormGeneralLiabilityClaim");
                            $(row).attr('onClick', 'HandleRowClick(this);');
                            console.log($(row));
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }
                        ],
                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta) {
                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                                }
                            },
                            { "data": "propertyName", "name": "propertyName", "autoWidth": false },
                            { "data": "incidentDateTime", "name": "incidentDateTime", "autoWidth": false },                         
                            { "data": "incidentLocation", "name": "incidentLocation", "autoWidth": false },
                            { "data": "incidentDescription", "name": "incidentDescription", "autoWidth": false },
                            { "data": "authoritiesContacted", "name": "authoritiesContacted", "autoWidth": false },
                            { "data": "contactPerson", "name": "contactPerson", "autoWidth": false },
                            { "data": "claimantName", "name": "claimantName", "autoWidth": false },                            
                            { "data": "claimantAddress", "name": "claimantAddress", "autoWidth": false },
                            { "data": "claimantPhone1", "name": "claimantPhone1", "autoWidth": false },
                            { "data": "claimantPhone2", "name": "claimantPhone2", "autoWidth": false },
                            { "data": "anyInjuries", "name": "anyInjuries", "autoWidth": false },
                            { "data": "injuryDescription", "name": "injuryDescription", "autoWidth": false },
                            { "data": "witnessPresent", "name": "witnessPresent", "autoWidth": false },
                            { "data": "witnessName", "name": "witnessName", "autoWidth": false },
                            { "data": "witnessPhone", "name": "witnessPhone", "autoWidth": false },
                            { "data": "witnessAddress", "name": "witnessAddress", "autoWidth": false },
                            { "data": "descriptionOfProperty", "name": "descriptionOfProperty", "autoWidth": false },
                            { "data": "descriptionOfDamage", "name": "descriptionOfDamage", "autoWidth": false },
                            { "data": "reportedBy", "name": "reportedBy", "autoWidth": false },
                              { "data": "reportedPhone", "name": "reportedPhone", "autoWidth": false },                     
                            { "data": "dateReported", "name": "dateReported", "autoWidth": false },
                            { "data": "createdDate", "name": "createdDate", "autoWidth": false },
                            { "data": "createdByName", "name": "createdByName", "autoWidth": false }
                        ]
                    }).columns.adjust();
                }
            });
        }
    );
}



// called from User Role page
function LoadMoldDamageClaims() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecords?entitytype=FormMoldDamageClaim",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["mdlId"]);
                            $(row).attr('itemType', "FormMoldDamageClaim");
                            $(row).attr('onClick', 'HandleRowClick(this);');
                            console.log($(row));
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }
                        ],
                        columns: [
                            //{
                            //    "targets": -1,
                            //    "data": null,
                            //    "sortable": false,
                            //    "defaultContent": "<button>Click!</button>"
                            //},
                            {
                                render: function (data, type, row, meta) {
                                    return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                                }
                            },
                            { "data": "propertyName", "name": "propertyName", "autoWidth": false },
                            { "data": "discoveryDate", "name": "discoveryDate", "autoWidth": false },
                            { "data": "location", "name": "location", "autoWidth": false },
                            { "data": "description", "name": "description", "autoWidth": false },
                            { "data": "suspectedCause", "name": "suspectedCause", "autoWidth": false },
                            { "data": "areBuildingMaterialsStillWet", "name": "areBuildingMaterialsStillWet", "autoWidth": false },
                            { "data": "isStandingWaterPresent", "name": "isStandingWaterPresent", "autoWidth": false },
                            { "data": "howMuchWater", "name": "howMuchWater", "autoWidth": false },
                            { "data": "estimatedSurfaceAreaAffected", "name": "estimatedSurfaceAreaAffected", "autoWidth": false },
                            { "data": "estimatedTimeDamagePresent", "name": "estimatedTimeDamagePresent", "autoWidth": false },
                            { "data": "correctiveActionsTaken", "name": "correctiveActionsTaken", "autoWidth": false },
                            { "data": "plannedActions", "name": "plannedActions", "autoWidth": false },
                            { "data": "additionalComments", "name": "additionalComments", "autoWidth": false },                          
                            { "data": "reportedBy", "name": "reportedBy", "autoWidth": false },
                            { "data": "reportedPhone", "name": "reportedPhone", "autoWidth": false },
                            { "data": "dateReported", "name": "dateReported", "autoWidth": false },
                            { "data": "createdDate", "name": "createdDate", "autoWidth": false },
                            { "data": "createdByName", "name": "createdByName", "autoWidth": false }
                        ]
                    }).columns.adjust();
                }
            });
        }
    );
}



function ApplyInputMask(container) {
    //https://github.com/RobinHerbots/jquery.inputmask

    $inputs = container.find('.form-control');
    $inputs.each(function () {
        $this = $(this);

        if ($this.attr('validationformat') !== undefined) {
           
            switch ($this.attr('validationformat')) {

                case "PhoneWithExt":
                    $this.inputmask("mask", { "mask": "999-999-9999 [x99999]", showTooltip: true });
                    var elem = $this.wrap("<div class=\"input-group\"></div>")
                    $this.parent().append("<div class=\"input-group-addon\"><i class=\"fa fa-phone\"></i></div>");
                    break;
                case "Phone":
                    $this.inputmask("mask", { "mask": "999-999-9999", showTooltip: true });
                    var elem = $this.wrap("<div class=\"input-group\"></div>")
                    $this.parent().append("<div class=\"input-group-addon\"><i class=\"fa fa-phone\"></i></div>");
                    break;
                case "Social Security Number":
                    $this.inputmask("999-99-9999", { placeholder: " ", clearMaskOnLostFocus: true });
                    break;
                case "Date":
                    // $this.inputmask("mask", { "mask": "mm/dd/yyyy", showTooltip: true });
                    // $this.datepicker();
                    //http://trentrichardson.com/examples/timepicker/
                    $this.datepicker({});
                    var elem = $this.wrap("<div class=\"input-group\"></div>")
                    $this.parent().append("<div class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></div>");
                    break;
                case "DateTime":
                    // $this.inputmask("mask", { "mask": "mm/dd/yyyy", showTooltip: true });
                    //$this.datepicker({});
                    //http://trentrichardson.com/examples/timepicker/
                    //https://github.com/uxsolutions/bootstrap-datepicker
                  //  $this.datetimepicker();
                    $this.datepicker({});
                    var elem = $this.wrap("<div class=\"input-group\"></div>")
                    $this.parent().append("<div class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></div>");
                    break;
                case "Number":
                    $this.inputmask('Regex', { regex: "[0-9 ]{1,50}" });
                    break;
                case "Email":
                    var elem = $this.wrap("<div class=\"input-group\"></div>")
                    $this.parent().append("<div class=\"input-group-addon\"><i class=\"fa fa-envelope\"></i></div>");
                    break;
                //case "Contact":

                //    break;
                case "None":
                    break;
                default: break;

            }
          //  $(":input").inputmask();
        }
    });
}

//input token
function ApplyTokenInput(container) {
    // Let's  get contacts array before we proceed.. 
    var contacts = "";
    $.get($BaseApiUrl + "api/data/GetRecords?entitytype=Contact", function (data) {
        contacts = data;
        // now that we have contacts array.. Let's load token inputs..

        container.find('.tokenInput').each(function () {
          //  $(this).tokenInput("clear");
            var val = $(this).val();
            
            $(this).tokenInput($BaseApiUrl + "api/Data/GetTokenInputUser", {
                theme: "facebook",
                method: "get",
                contentType: "json",
                preventDuplicates: true,
                crossDomain: false,
                tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" },
                tokenDelimiter: "|",
                minChars: 2,
                tokenValue: "id",
                tokenLimit: 1,
                noResultsText: "No Results",
                searchingText: "Searching for records...",
                onDelete: function (item) {
                    //alert("Deleted " + item.name);
                    //$(this).val('');
                    // WORK AROUND TO RESET VALUE OF THE TOKEN INPUT
                    $('#' + $(this).attr("id")).val('');
                },
                onAdd: function (item) {
                    // work around to fix delete and immediately add an item
                    $('#' + $(this).attr("id")).val(item.id);
                }
            }).css("width", "100%");
            $(this).prev().prepend("<i style=\"margin-top:6px;margin-right:10px;\" class=\"fa fa-user pull-right\"></i>");

            //populate data here
            for (var i=0; i < contacts.length - 1; i++) {
               
                if (contacts[i]["contactId"] == val) {
                    var cotactname = contacts[i]["firstName"] + " " + contacts[i]["lastName"] + "<br/>" + contacts[i]["email"];
                    $(this).tokenInput("add", { id: "" + val + "", name: "" + cotactname + "" });
                    break;
                }
                
            }
            

        });
       
    });
  
   

    //// also apply tokeniputuser for user selection fields
    //$(".tokenInputUser").tokenInput("/api/user/GetTokenInputUser", {
    //    //onAdd: function (item) {
    //    //    alert("Added " + item.id);
    //    //},
    //    //onDelete: function (item) {
    //    //    alert("Deleted " + item.name);
    //    //},
    //    theme: "facebook",
    //    method: "get",
    //    tokenLimit: 1,
    //    preventDuplicates: true,
    //    tokenFormatter: function (item) { return "<li><p>" + item.name + "</p></li>" }

    //});
  
}

// gENERIC HELPER
var format = function (str, col) {
    col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

    return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
        if (m == "{{") { return "{"; }
        if (m == "}}") { return "}"; }
        return col[n];
    });
};

function CheckFormErrors($form) {

    var $inputs = $form.find('.has-error');
    $inputs.each(function () {
        $this = $(this);
        $(this).removeClass('has-error');
    });
    $inputs = $form.find('.required');
    var hasErrors = false;

    $inputs.each(function () {
        $this = $(this);

        if ($this.is(".required") && (!$this.val())) { $(this).parent().addClass('has-error'); $(this).parent().addClass('has-error'); hasErrors = true; }
        else { $(this).parent().removeClass('has-error'); $(this).parent().removeClass('has-error'); }
    });

    return hasErrors;
}

function ClearForm($form) {
    $form.each(function () {
        this.reset();
    });
    $('.success-message').hide();
    $('.failure-message').hide();
}

function ScrollToElement($elem) {
    $('html, body').animate({
        scrollTop: $elem.offset().top
    }, 1500);
}


function ConfigDatatable(Form) {
  
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getrecordswithconfig?entitytype=" + Form,
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {

                    var configdata =data;
                  
                    var columnlist1 = [];
                    columnlist1.push({
                        render: function (data, type, row, meta) {
                            return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                        }
                    });
                    var tablehead = "<tr><th></th>";
                    for (var i = 0; i < configdata.columns.length; i++) {
                        tablehead += "<th>" + configdata.columns[i].label+" </th>";
                        if (configdata.columns[i].type == "0")
                            columnlist1.push({ data: configdata.columns[i].name, name: configdata.columns[i].name, autoWidth: false });

                        else if (configdata.columns[i].type == "3") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row, meta) {
                                    return '<img src = "' + data + '" width = "50px" height = "50px" > ';
                                }
                                , "name": configdata.columns[i].name, "autoWidth": false
                            });
                        }
                        else if (configdata.columns[i].type == "1") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            });
                        }
                        else if (configdata.columns[i].type == "2") {

                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="' + configdata.columns[i].href + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                                , "name": configdata.columns[i].name
                            });

                        }
                    }

                    tablehead += "</tr>";
                   
                    $(".dtprops").html("<thead> " + tablehead + "</thead><tbody> </tbody> <tfoot> "+tablehead+"</tfoot>");
                    


                    var datatableVariable = $('.dtprops').DataTable({
                        data: configdata.rows,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here      
                                                      
                            $(row).attr('id', data[configdata.pkName]);
                            $(row).attr('itemType', configdata.etType);
                            $(row).attr('onClick', 'HandleRowClick(this);');
                            console.log($(row));
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: true,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }],
                        columns: columnlist1

                    });

                }
            });
        }
    );
}

function GetAllClaims(Type) {


    if ($.fn.DataTable.isDataTable('.dtprops')) {
        $(".dtprops").DataTable().destroy();
        $('.dtprops tbody').empty();
    }
       

    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",
                headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getallclaims?Type="+Type+"&userid="+ $("#CreatedBy").val()+"&propertyid=null",
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {

                    var configdata = data;

                    var columnlist1 = [];
                    columnlist1.push({
                        render: function (data, type, row, meta) {
                            return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                        }
                    });
                    var tablehead = "<tr><th></th>";
                    for (var i = 0; i < configdata.columns.length; i++) {
                        tablehead += "<th>" + configdata.columns[i].label + " </th>";
                        if (configdata.columns[i].type == "0")
                            columnlist1.push({ data: configdata.columns[i].name, name: configdata.columns[i].name, autoWidth: false });

                        else if (configdata.columns[i].type == "3") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row, meta) {
                                    return '<img src = "' + data + '" width = "50px" height = "50px" > ';
                                }
                                , "name": configdata.columns[i].name, "autoWidth": false
                            });
                        }
                        else if (configdata.columns[i].type == "1") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            });
                        }
                        else if (configdata.columns[i].type == "2") {

                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="' + configdata.columns[i].href + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                                , "name": configdata.columns[i].name
                            });

                        }
                    }

                    tablehead += "</tr>";

                    $(".dtprops").html("<thead> " + tablehead + "</thead><tbody> </tbody> <tfoot> " + tablehead + "</tfoot>");



                    var datatableVariable = $('.dtprops').DataTable({
                        data: configdata.rows,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data)
                        {
                            // do anything row wise here      

                            $(row).attr('id', data[configdata.pkName]);
                            $(row).attr('itemType', configdata.etType);
                            var rowdata = JSON.parse(JSON.stringify(data));
                            if (rowdata.claimType == "General Liability")
                                $(row).attr('onClick', "LoadFormView('" + rowdata.id +"g');");
                            else if (rowdata.claimType == "Mold Damage")
                                $(row).attr('onClick', "LoadFormView('" + rowdata.id + "m');");
                            else if (rowdata.claimType == "PropertyDamage")
                                $(row).attr('onClick', "LoadFormView('" + rowdata.id + "p');");                          
                           
                        },
                        "order": [[2, 'asc']],
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }],
                        columns: columnlist1

                    });

                }
            });
        }
    );
}

function GetAllMileageForms(formtype) {

    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/GetAllMileageForms?FormType=" + formtype + "&userid=" + $("#CreatedBy").val(),
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {

                    var configdata = data;

                    //if (formtype == "New Hire Notice")
                    //{
                    //    $(".ibox-title").html("<h5> All New Hire Notices </h5>");

                    //}
                    //else if (formtype == "EmployeeSeparation")
                    //{
                    //    $(".ibox-title").html("<h5> All Notices Of Employee Separation  </h5>");

                    //}
                    //else if (formtype == "PayRollChange")
                    //{
                    //    $(".ibox-title").html("<h5> All Carroll Payroll Status Change Notices  </h5>");
                    //}
                    //else if (formtype == "LeaseRider")
                    //{
                    //    $(".ibox-title").html("<h5> All Employee Lease Rider Forms  </h5>");
                    //}

                    if ($.fn.DataTable.isDataTable('.dtprops')) {
                        $('.dtprops').DataTable().clear().destroy();
                    }

                    var columnlist1 = [];
                    columnlist1.push({
                        render: function (data, type, row, meta) {
                            return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                        }
                    });
                    var tablehead = "<tr><th></th>";
                    for (var i = 0; i < configdata.columns.length; i++) {
                        tablehead += "<th>" + configdata.columns[i].label + " </th>";
                        if (configdata.columns[i].type == "0")
                            columnlist1.push({ data: configdata.columns[i].name, name: configdata.columns[i].name, autoWidth: false });

                        else if (configdata.columns[i].type == "3") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row, meta) {
                                    return '<img src = "' + data + '" width = "50px" height = "50px" > ';
                                }
                                , "name": configdata.columns[i].name, "autoWidth": false
                            });
                        }
                        else if (configdata.columns[i].type == "1") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            });
                        }
                        else if (configdata.columns[i].type == "2") {

                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="' + configdata.columns[i].href + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                                , "name": configdata.columns[i].name
                            });

                        }
                    }

                    tablehead += "</tr>";

                    $(".dtprops").html("<thead> " + tablehead + "</thead><tbody> </tbody> <tfoot> " + tablehead + "</tfoot>");


                    var datatableVariable = $('.dtprops').DataTable({
                        data: configdata.rows,
                        processing: true,
                        scrollY: '80vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here      

                            $(row).attr('id', data[configdata.pkName]);
                            $(row).attr('itemType', configdata.etType);
                            //var rowdata = JSON.parse(JSON.stringify(data));
                            //if (rowdata.claimType == "General Liability")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "g');");
                            //else if (rowdata.claimType == "Mold Damage")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "m');");
                            //else if (rowdata.claimType == "PropertyDamage")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "p');");

                        },
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }],
                        columns: columnlist1

                    });

                }
            });
        }
    );
}

function GetAllHRFORMs(formtype) {

    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/GetAllHrForms?FormType=" + formtype,
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                success: function (data) {

                    var configdata = data;

                    //if (formtype == "New Hire Notice")
                    //{
                    //    $(".ibox-title").html("<h5> All New Hire Notices </h5>");

                    //}
                    //else if (formtype == "EmployeeSeparation")
                    //{
                    //    $(".ibox-title").html("<h5> All Notices Of Employee Separation  </h5>");

                    //}
                    //else if (formtype == "PayRollChange")
                    //{
                    //    $(".ibox-title").html("<h5> All Carroll Payroll Status Change Notices  </h5>");
                    //}
                    //else if (formtype == "LeaseRider")
                    //{
                    //    $(".ibox-title").html("<h5> All Employee Lease Rider Forms  </h5>");
                    //}

                    if ($.fn.DataTable.isDataTable('.dtprops')) {
                        $('.dtprops').DataTable().clear().destroy();
                    }

                    var columnlist1 = [];
                    columnlist1.push({
                        render: function (data, type, row, meta) {
                            return ''//'<i style="color:#1ab394" class="fa fa-edit"></i>'
                        }
                    });
                    var tablehead = "<tr><th></th>";
                    for (var i = 0; i < configdata.columns.length; i++) {
                        tablehead += "<th>" + configdata.columns[i].label + " </th>";
                        if (configdata.columns[i].type == "0")
                            columnlist1.push({ data: configdata.columns[i].name, name: configdata.columns[i].name, autoWidth: false });

                        else if (configdata.columns[i].type == "3") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row, meta) {
                                    return '<img src = "' + data + '" width = "50px" height = "50px" > ';
                                }
                                , "name": configdata.columns[i].name, "autoWidth": false
                            });
                        }
                        else if (configdata.columns[i].type == "1") {
                            columnlist1.push({
                                "data": configdata.columns[i].name, 'render': function (date) {
                                    if (date == null) return '';
                                    var date = new Date(date);
                                    var month = date.getMonth() + 1;
                                    return month + "/" + date.getDate() + "/" + date.getFullYear();
                                }
                            });
                        }
                        else if (configdata.columns[i].type == "2") {

                            columnlist1.push({
                                "data": configdata.columns[i].name, render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="' + configdata.columns[i].href + result["id"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                                , "name": configdata.columns[i].name
                            });

                        }
                    }

                    tablehead += "</tr>";

                    $(".dtprops").html("<thead> " + tablehead + "</thead><tbody> </tbody> <tfoot> " + tablehead + "</tfoot>");


                    var datatableVariable = $('.dtprops').DataTable({
                        data: configdata.rows,
                        processing: true,
                        scrollY: '80vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "rowCallback": function (row, data) {
                            // do anything row wise here      

                            $(row).attr('id', data[configdata.pkName]);
                            $(row).attr('itemType', configdata.etType);
                            //var rowdata = JSON.parse(JSON.stringify(data));
                            //if (rowdata.claimType == "General Liability")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "g');");
                            //else if (rowdata.claimType == "Mold Damage")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "m');");
                            //else if (rowdata.claimType == "PropertyDamage")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "p');");

                        },                       
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: 'single',     // enable single row selection
                        responsive: false,     // enable responsiveness
                        altEditor: false,      // Enable altEditor ****
                        buttons: [
                            //    {
                            //    text: 'Add',
                            //    name: 'add',     // DO NOT change name
                            //    action: function (e, dt, node, config) {
                            //        ToggleAdd();

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row http://kingkode.com/free-datatables-editor-alternative/
                            //    text: 'Edit',
                            //    name: 'edit',       // DO NOT change name
                            //    action: function (e, dt, node, config) {

                            //    }
                            //},
                            //{
                            //    extend: 'selected', // Bind to Selected row
                            //    text: 'Delete',
                            //    name: 'delete'      // DO NOT change name
                            //},
                            { extend: 'copy' },
                            { extend: 'csv' },
                            { extend: 'excel' },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'LEGAL' },
                            {
                                extend: 'print',
                                customize: function (win) {
                                    $(win.document.body).addClass('white-bg');
                                    $(win.document.body).css('font-size', '10px');

                                    $(win.document.body).find('table')
                                        .addClass('compact')
                                        .css('font-size', 'inherit');
                                }
                            }],
                        columns: columnlist1

                    });

                }
            });
        }
    );
}

function LoadHrFormsCount()
{
    $.ajax({
        url: $BaseApiUrl + "api/data/GetHrFormCount",
        type: 'GET',
        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
        },
        success: function (data) {

            // To-do code if ajax request is successful
            $(".leasecount").html(data.leaseCount);
            $(".payrollcount").html(data.payRollCount);
            $(".noticecount").html(data.separationCount);
            $(".hirecount").html(data.hireCount);
            $(".claimcount").show();

        },
        error: function (ts) {
            alert('error' + ts.errorMessage);
        }
    });

}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
var claimbody = "";
var $details = "";


function LoadHrPositions() {

    var options = "";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPositions", function (data) {

        for (var i = 0; i < data.length; i++) {
            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";

        }
        // now let's load options into select box
        $('#position').append(options);

    });
}


function LoadPayRoles() {

    var options = "";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPayPerilds", function (data) {

        for (var i = 0; i < data.length; i++) {
            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";

        }
        // now let's load options into select box
        $('#beginpayperiod').append(options);

    });
}


function LoadClaim()
{

    var claim = getParameterByName("Claim");
    var Type = claim[claim.length - 1];
    claim = claim.substr(0, claim.length - 1)

    $("#claim").val(claim);
    if(Type=="m")
        $("#type").val("3");
    else if (Type == "p")
        $("#type").val("1");
    else if (Type == "g")
        $("#type").val("2");

   
              $.ajax({
                type: "get",
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/GetClaimDetails?claim=" + claim + "&Type=" + Type,
                async: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                },
                statusCode: 
                {
                    200: function (data) {
                        if (data != null) {

                            var ClaimData = JSON.parse(JSON.stringify(data));

                            $("#claimnumber").html(ClaimData.claim.tbl.claimNumber);

                            if (Type == "p")
                            {
                                $("#heading").html("Property Damage Claim");
                                $("#property").html(ClaimData.claim.propertyName);
                                claimbody += ' <table class="table">';
                                claimbody += '<tr><td style="width:30%;">Weather Conditions:</td><td>' + CheckNull(ClaimData.claim.tbl.weatherConditions) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Incident Date:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentDateTime) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Incident Location:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentLocation) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Incident Description:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentDescription) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Estimate of Damage:</td><td>' + CheckNull(ClaimData.claim.tbl.estimateOfDamage) + '</td></tr>';

                                if (ClaimData.claim.tbl.authoritiesContacted == false)
                                    claimbody += '<tr><td style="width:30%;">Authorities Contacted:</td><td>No</td></tr>';
                                else
                                    claimbody += '<tr><td style="width:30%;">Authorities Contacted:</td><td>Yes</td></tr>'; 


                                claimbody += '<tr><td style="width:30%;">Contact Person:</td><td>' + CheckNull(ClaimData.claim.tbl.contactPerson) + '</td></tr>';  
                                if (ClaimData.claim.tbl.lossOfRevenues == false)
                                    claimbody += '<tr><td style="width:30%;"> Loss Of Revenues:</td><td>No</td></tr>';

                                else
                                    claimbody += '<tr><td style="width:30%;"> Loss Of Revenues:</td><td>Yes</td></tr>';   

                                if (ClaimData.claim.tbl.witnessPresent == false)
                                    claimbody += '<tr><td style="width:30%;"> Witness Present:</td><td>No</td></tr>';
                                else
                                    claimbody += '<tr><td style="width:30%;"> Witness Present:</td><td>Yes</td></tr>';   
                                claimbody += '<tr><td style="width:30%;">Witness Name:</td><td>' + CheckNull(ClaimData.claim.tbl.witnessName) + '</td></tr>';    
                                claimbody += '<tr><td style="width:30%;">Witness Address:</td><td>' + CheckNull(ClaimData.claim.tbl.witnessAddress) + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Witness Phone:</td><td>' + CheckNull(ClaimData.claim.tbl.witnessPhone) + '</td></tr>';    
                                claimbody += '<tr><td style="width:30%;">Reported By:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentReportedBy) + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Reported Phone:</td><td>' + CheckNull(ClaimData.claim.tbl.reportedPhone) + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Reported Date:</td><td>' + CheckNull(ClaimData.claim.tbl.dateReported).substring(0, 10) + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Created By:</td><td>' + CheckNull(ClaimData.claim.tbl.createdByName) + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;">Created Date:</td><td>' + CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10) + '</td></tr>';
                                claimbody += '</table>'

                                $("#claimbody").html(claimbody);

                            }
                            else if (Type == "m") {
                                $("#heading").html("Mold Damage Claim");
                                $("#property").html(ClaimData.claim.propertyName);

                            
                               // $('.claimDesc').html(ClaimData.claim.tbl.description);
                                claimbody += ' <table class="table ">';
                                claimbody += '<tr><td style="width:30%;"> Location :</td><td>' + ClaimData.claim.tbl.location + '</td></tr>';                               
                                claimbody += '<tr><td  style="width:30%;"> Description :</td><td>' + ClaimData.claim.tbl.description + '</td></tr>';  
                                console.log('building still wet'+ClaimData.claim.tbl.areBuildingMaterialsStillWet )
                                if (ClaimData.claim.tbl.areBuildingMaterialsStillWet == false ) {
                                    claimbody += '<tr><td  style="width:30%;">Are Building Materials Still Wet: </td><td> No </td></tr>';
                                    console.log('buidling still wet coiming false')
                                }

                                else {
                                    claimbody += '<tr><td  style="width:30%;">Are Building Materials Still Wet: </td><td> Yes </td></tr>';
                                    console.log('building still wet true');
                                }
                                   

                                claimbody += '<tr><td  style="width:30%;"> How Much Water Present :</td><td>' + ClaimData.claim.tbl.howMuchWater + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Estimated Time Damage Present: </td><td>' + ClaimData.claim.tbl.estimatedTimeDamagePresent + '</td></tr>';                                  
                                claimbody += '<tr><td  style="width:30%;"> Planned Actions : </td><td>' + ClaimData.claim.tbl.plannedActions + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Discovery Date : </td><td>' + CheckNull(ClaimData.claim.tbl.discoveryDate).substring(0, 10) + '</td></tr>';                                 
                                claimbody += '<tr><td  style="width:30%;"> Suspected Cause : </td><td>' + ClaimData.claim.tbl.suspectedCause + '</td></tr>';                                  
                                if (ClaimData.claim.tbl.isStandingWaterPresent == false)
                                    claimbody += '<tr><td  style="width:30%;"> Is Standing Water Present : </td><td>  No' + '</td></tr>';
                                else
                                    claimbody += '<tr><td  style="width:30%;"> Is Standing Water Present : </td><td> Yes </td></tr>';
                                claimbody += '<tr><td  style="width:30%;">Estimated Surface Area Affected : </td><td>' + ClaimData.claim.tbl.estimatedSurfaceAreaAffected + '</td></tr>';                                  
                                //if (ClaimData.claim.tbl.correctiveActionsTaken== false)
                                //    claimbody += '<tr><td> Corrective Actions Taken : </td><td> No </td></tr>';
                                //else
                                claimbody += '<tr><td  style="width:30%;"> Corrective Actions Taken : </td><td> ' + ClaimData.claim.tbl.correctiveActionsTaken+'</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Additional Comments : </td><td>' + ClaimData.claim.tbl.additionalComments + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Reported By : </td><td>' + ClaimData.claim.tbl.reportedBy + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Reported Date : </td><td>' + CheckNull(ClaimData.claim.tbl.dateReported).substring(0, 10) + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Reported Phone : </td><td>' + ClaimData.claim.tbl.reportedPhone + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Created By :</td><td>' + ClaimData.claim.tbl.createdByName + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Created Date:</td><td>' + CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10) + '</td></tr>';
                                claimbody += '</table>'

                                $("#claimbody").html(claimbody);
                            }
                            else if (Type == "g") {
                                $("#heading").html("General Liability Claim");
                                $("#property").html(ClaimData.claim.propertyName);
                                claimbody += ' <table class="table">';

                                claimbody += '<tr><td style="width:30%;"> Incident Date:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentDateTime).substring(0, 10) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Incident Location:</td><td>' + ClaimData.claim.tbl.incidentLocation + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;"> Incident Description:</td><td>' + ClaimData.claim.tbl.incidentDescription + '</td></tr>';  
                               
                                if (ClaimData.claim.tbl.authoritiesContacted == false)
                                    claimbody += '<tr><td>Authorities Contacted: </td><td> No </td></tr>';
                                else
                                    claimbody += '<tr><td>Authorities Contacted: </td><td> Yes </td></tr>';
                                claimbody += '<tr><td style="width:30%;"> Contact Person :</td><td>' + ClaimData.claim.tbl.contactPerson + '</td></tr>';
                                
                                claimbody += '<tr><td style="width:30%;"> Claimant Name:</td><td>' + ClaimData.claim.tbl.claimantName + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Claimant Address:</td><td>' + ClaimData.claim.tbl.claimantAddress + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Claimant Phone1:</td><td>' + ClaimData.claim.tbl.claimantPhone1 + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Claimant Phone2:</td><td>' + ClaimData.claim.tbl.claimantPhone2 + '</td></tr>'; 
                              
                                if (ClaimData.claim.tbl.anyInjuries == false)
                                    claimbody += '<tr><td> Any Injuries: </td><td> No </td></tr>';
                                else
                                    claimbody += '<tr><td> Any Injuries: </td><td> Yes </td></tr>';
                                claimbody += '<tr><td style="width:30%;">Injury Description :</td><td>' + ClaimData.claim.tbl.injuryDescription + '</td></tr>'; 
                               
                                if (ClaimData.claim.tbl.witnessPresent == false)
                                    claimbody += '<tr><td>Witness Present: </td><td> No </td></tr>';
                                else
                                    claimbody += '<tr><td>Witness Present: </td><td> Yes </td></tr>';

                                claimbody += '<tr><td style="width:30%;">Witness Name:</td><td>' + ClaimData.claim.tbl.witnessName + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Witness Address:</td><td>' + ClaimData.claim.tbl.witnessAddress + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Witness Phone:</td><td>' + ClaimData.claim.tbl.witnessPhone + '</td></tr>';

                                claimbody += '<tr><td style="width:30%;">Description Of Property:</td><td>' + ClaimData.claim.tbl.descriptionOfProperty + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Description Of Damage:</td><td>' + ClaimData.claim.tbl.descriptionOfDamage + '</td></tr>'; 
                              
                                claimbody += '<tr><td style="width:30%;">Reported By:</td><td>' + ClaimData.claim.tbl.reportedBy + '</td></tr>';                                 
                                claimbody += '<tr><td style="width:30%;">Reported Phone:</td><td>' + ClaimData.claim.tbl.reportedPhone + '</td></tr>'; 
                                if (ClaimData.claim.tbl.notifySecurityOfficer == true)
                                    claimbody += '<tr><td style="width:30%;">Notify Security Officer :</td><td> Yes </td></tr>';
                                else
                                    claimbody += '<tr><td style="width:30%;">Notify Security Officer :</td><td> No </td></tr>'; 

                                claimbody += '<tr><td style="width:30%;">Date Reported:</td><td>' + (ClaimData.claim.tbl.dateReported).substring(0, 10) + '</td></tr>';  

                                claimbody += '<tr><td style="width:30%;">Created By:</td><td>' + ClaimData.claim.tbl.createdByName + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Created Date:</td><td>' + CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10) + '</td></tr>'; 
                                claimbody += '</table>'
                             
                                $("#claimbody").html(claimbody);

                            }

                            $("#comm-att").show();
                            $(".feed-activity-list").html('');
                           // $("#commentbody").html();
                            $.each(ClaimData.comments, function (index, value) {
                               // $("#commentbody").append('<tr><td> ' + value.comment + ' </td><td style="width:20%;" >' + value.commentDate.substring(0, 10) + ' </td> </tr>');
                               
                                 $(".feed-activity-list").append('<div class=\"feed-element\" ><div class=\"media-body\">Posted by:  <strong>' + value.commentByName + '</strong><br><small class=\"text-muted\">' + value.commentDate + '</small>');
                                $(".feed-activity-list").append('<div class=\"well\">' + value.comment + '</div></div></div>');
                            });


                            $("#attachmentbody").html('');

                            $.each(ClaimData.attchments, function (index, value) {
                                $("#attachmentbody").append('<tr><td style="float:left" ><a href="' + $BaseApiUrl + '/UploadedFiles/' + value.at_FileName + '" target="_blank" >' + value.at_Name + ' </a></td><td style="width:20%;" >' + value.uploadedDate.substring(0, 10) + ' </td> </tr>');
                            });
                            
                            $("#activitybody").html('');

                            $.each(ClaimData.activity, function (index, value) {
                                $("#activitybody").append('<tr><td style="float:left" >' + value.activityDescription + ' </td><td style="width:20%;" >' + value.activityDate + ' </td> <td> ' + value.activityStatus + '</td> <td>' + value.activityByName + ' </td></tr>');
                            });

                            // NOW THAT WE ARE HERE.. Let's load property details in the page.. 

                            //**************************************************************
                            $.ajax({
                                type: "get",
                                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                                url: $BaseApiUrl + "api/property/GetProperty/" + ClaimData.claim.tbl.propertyId,
                                async: false,
                                //beforeSend: function (xhr) {
                                //    xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

                                //},
                                statusCode: {
                                    401: function () {
                                        // beacuse the session with oAuth is limited to 10 minutes.. we will need to redirect to home page to reset all objects
                                        //$.when(GetToken()).then(
                                        //    function () {  });
                                        location.href = "/";
                                        // location.href = "/account/SignOut"
                                    },
                                    404: function () { },
                                    200: function () { },
                                    201: function () { },
                                    202: function () { }
                                },
                                success: function (data) {
                                    
                                    var $prop = data[0];
                                    
                                     $details = "<li>Address:</li><li>" + $prop.propertyAddress + "</li>";
                                    $details += "<li>" + $prop.city + ", " + $prop.state + " " + $prop.zipCode + "</li>";
                                    $details += "<li>" + $prop.phoneNumber + "</li>";
                                    $details += "<li>Units: " + $prop.units + "</li>";
                                    $details += "<li>Yardi Code: " + $prop.propertyNumber + "</li>";
                                   

                                    $details += "<li>&nbsp;</li><li class='font-bold'>Legal:</li><li>" + $prop.legalName + "</li>";
                                    $details += "<li>Tax Id: " + $prop.taxId + "</li>";

                                    if ($prop.equityPartner != null) {
                                        var $eqPartner = JSON.parse($prop.equityPartner);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Partner:</li><li>" + $eqPartner.name + "</li>";
                                    } else {
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Partner:</li>"
                                    }
                                    $details += "<li>Partner Site Code: " + ($prop.equityPartnerSiteCode == null) ? '' : $prop.equityPartnerSiteCode + "</li>";

                                    if ($prop.equityPartnerContact != null) {
                                        var $val = JSON.parse($prop.equityPartnerContact);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Equity Partner Contact:</li><li>" + $val.name + "</li>";
                                    } 
                                    
                                    if ($prop.assetManager1 != null) {
                                        var $assetManager = JSON.parse($prop.assetManager1);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Asset Managers:</li><li>" + $assetManager.name + "</li>";
                                    } else {
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Asset Managers:</li>";
                                    }


                                    if ($prop.assetManager2 != null) {
                                        var $assetManager2 = JSON.parse($prop.assetManager2);
                                        $details += "<li>" + $assetManager2.name + "</li>";
                                    } 

                                    if ($prop.vicePresident != null) {
                                        var $val = JSON.parse($prop.vicePresident);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Vice President:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.regionalVicePresident != null) {
                                        var $val = JSON.parse($prop.regionalVicePresident);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Regional Vice President:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.constructionManager != null) {
                                        var $val = JSON.parse($prop.constructionManager);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Construction Manager:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.regionalManager != null) {
                                        var $val = JSON.parse($prop.regionalManager);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Regional Manager:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.propertyManager != null) {
                                        var $val = JSON.parse($prop.propertyManager);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Property Manager:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.insuranceContact != null) {
                                        var $val = JSON.parse($prop.insuranceContact);
                                        $details += "<li>&nbsp;</li><li class='font-bold'>Insurance Contact:</li><li>" + $val.name + "</li>";
                                    } 
                                    $('.PropDetails').html($details);
                                    $('.propertyName').html($prop.propertyName);
                                }
                            });

                            //****************************************************************

                        }
                        else
                        {
                            alert("No Claim Found");
                        }
                    }, 500: function (data) {
                        alert(data.responseText);
                    }
                }
            });
}

//function LoadClaim() {
//    var claim = getParameterByName("Claim");
//    var Type = claim[claim.length - 1];
//    claim = claim.substr(0, claim.length - 1)

//    $("#claim").val(claim);
//    if (Type == "m")
//        $("#type").val("3");
//    else if (Type == "p")
//        $("#type").val("1");
//    else if (Type == "g")
//        $("#type").val("2");


//    $.ajax({
//        type: "get",
//        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
//        url: $BaseApiUrl + "api/data/GetClaimDetails?claim=" + claim + "&Type=" + Type,
//        async: false,
//        beforeSend: function (xhr) {
//            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
//        },
//        statusCode: {
//            200: function (data) {
//                if (data != null) {

//                    var ClaimData = JSON.parse(JSON.stringify(data));
//                    var claimbody = "";

//                    if (Type == "p") {
//                        $("#heading").html("Property Damage Claim");
//                        $("#property").html("<strong> Property : </strong> " + ClaimData.claim.propertyName);

//                        claimbody += '<div class="col-lg-6"> <div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Incident Location : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.incidentLocation)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Weather Conditions : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.weatherConditions)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Estimate Of Damage : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.estimateOfDamage)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Contact Person : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.contactPerson)
//                            + '</span></div></div>';
//                        if (ClaimData.claim.tbl.witnessPresent == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Phone : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.witnessPhone)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Reported By: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.incidentReportedBy
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Reported Phone: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.reportedPhone
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Created Date : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10)
//                            + '</span></div></div></div>';


//                        claimbody += '<div class="col-lg-6"><div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Incident Date : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.incidentDateTime).substring(0, 10)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Incident Description : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.incidentDescription
//                            + '</span></div></div>';
//                        if (ClaimData.claim.tbl.authoritiesContacted == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Authorities Contacted : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Authorities Contacted : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';
//                        if (ClaimData.claim.tbl.lossOfRevenues == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Loss Of Revenues : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Loss Of Revenues : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Name : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.witnessName
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Address : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.witnessAddress
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Date Reported : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.dateReported).substring(0, 10)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Created By : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.createdByName
//                            + '</span></div></div></div>';

//                        $("#claimbody").html(claimbody);

//                    }
//                    else if (Type == "m") {
//                        $("#heading").html("Mold Damage Claim");
//                        $("#property").html("<strong> Property : </strong> " + ClaimData.claim.propertyName);

//                        claimbody += '<div class="col-lg-6"> <div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Location : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.location
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Description : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.description
//                            + '</span></div></div>';
//                        if (ClaimData.claim.tbl.areBuildingMaterialsStillWet == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Are Building Materials Still Wet: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Are Building Materials Still Wet: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> How Much Water Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.howMuchWater
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Estimated Time Damage Present: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.estimatedTimeDamagePresent
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Planned Actions : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.plannedActions
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Reported By: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.reportedBy
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Reported Phone: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.reportedPhone
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Created Date : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10)
//                            + '</span></div></div></div>';


//                        claimbody += '<div class="col-lg-6"><div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Discovery Date : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.discoveryDate).substring(0, 10)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Suspected Cause : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.suspectedCause
//                            + '</span></div></div>';
//                        if (ClaimData.claim.tbl.isStandingWaterPresent == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Is Standing Water Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Is Standing Water Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Estimated Surface Area Affected : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.estimatedSurfaceAreaAffected
//                            + '</span></div></div>';
//                        if (ClaimData.claim.tbl.correctiveActionsTaken == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Corrective Actions Taken : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Corrective Actions Taken : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Additional Comments : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.additionalComments
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Date Reported : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + (ClaimData.claim.tbl.dateReported).substring(0, 10)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Created By : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.createdByName
//                            + '</span></div></div></div>';
//                        $("#claimbody").html(claimbody);
//                    }
//                    else if (Type == "g") {
//                        $("#heading").html("General Liability Claim");
//                        $("#property").html("<strong> Property : </strong> " + ClaimData.claim.propertyName);

//                        claimbody += '<div class="col-lg-6"> <div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Incident Location : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.incidentLocation
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Incident Description : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.incidentDescription
//                            + '</span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Contact Person : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.contactPerson
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Claimant Address : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.claimantAddress
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Claimant Phone2 : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.claimantPhone2
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Injury Description : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.injuryDescription
//                            + '</span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Name </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.witnessName
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Address </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.witnessAddress
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Description Of Damage </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.descriptionOfDamage
//                            + '</span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Reported By: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.reportedBy
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Reported Phone: </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.reportedPhone
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Created Date : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10)
//                            + '</span></div></div></div>';


//                        claimbody += '<div class="col-lg-6"><div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Incident Date : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + CheckNull(ClaimData.claim.tbl.incidentDateTime).substring(0, 10)
//                            + '</span></div></div>';

//                        if (ClaimData.claim.tbl.authoritiesContacted == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Authorities Contacted : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Authorities Contacted : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Claimant Name : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.claimantName
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Claimant Phone1 : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.claimantPhone1
//                            + '</span></div></div>';
//                        if (ClaimData.claim.tbl.anyInjuries == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Any Injuries : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Any Injuries : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';
//                        if (ClaimData.claim.tbl.witnessPresent == 'false')
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> No </span></div></div>';
//                        else
//                            claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Present : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1"> Yes </span></div></div>';

//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Witness Phone : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.witnessPhone
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Description Of Property : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.descriptionOfProperty
//                            + '</span></div></div>';


//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Date Reported : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + (ClaimData.claim.tbl.dateReported).substring(0, 10)
//                            + '</span></div></div>';
//                        claimbody += '<div class="row mb-0"><div class="col-sm-4 text-sm-right"><label> Created By : </label> </div><div class="col-sm-8 text-sm-left"><span class="mb-1">' + ClaimData.claim.tbl.createdByName
//                            + '</span></div></div></div>';
//                        $("#claimbody").html(claimbody);

//                    }

//                    $("#comm-att").show();

//                    $("#commentbody").html();
//                    $.each(ClaimData.comments, function (index, value) {
//                        $("#commentbody").append('<tr><td> ' + value.comment + ' </td><td style="width:20%;" >' + value.commentDate.substring(0, 10) + ' </td> </tr>');
//                    });


//                    $("#attachmentbody").html('');

//                    $.each(ClaimData.attchments, function (index, value) {
//                        $("#attachmentbody").append('<tr><td><a href="' + $BaseApiUrl + '/UploadedFiles/' + value.at_FileName + '" target="_blank" >' + value.at_Name + ' </a></td><td style="width:20%;" >' + value.uploadedDate.substring(0, 10) + ' </td> </tr>');
//                    });

//                }
//                else {
//                    alert("No Claim Found");
//                }
//            }, 500: function (data) {
//                alert(data.responseText);
//            }
//        }
//    });
//}

function LoadUserClaims()
{
    $.ajax({
        url: $BaseApiUrl + "api/data/GetUserClaimCount?userid="+$("#CreatedBy").val(),
        type: 'GET',
        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
        async: false,       
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
        },
        success: function (data)
        {

            // To-do code if ajax request is successful
            $(".propcount").html(data.propertyCount);
            $(".liabcount").html(data.liabilityCount);
            $(".damagecount").html(data.damageCount);
            $(".claimcount").show();
          
        },
        error: function (ts) {
            alert('error' + ts.errorMessage);
        }
    });

}

$(document).ready(function ()
{

    $("#btnAddComment").click(function () {

        if ($("#txtcomment").val() == "") {
            alert("Please Write Comment to Proceed");
            $("#txtcomment").focus();
        }
        else
        {
            $("#btnAddComment").attr('disabled', true);
            $("#btnAddComment").html('<p style="color:white"> Sending .... </p>');
            $("#btnAddComment").fadeTo(0.28);
            $("#txtcomment").attr('disabled', true);

            var data = {};
            data["RefFormId"] = $("#claim").val();
            data["Comment"] = $("#txtcomment").val();
            data["RefFormType"] = $("#type").val();
            data["CommentBy"] = $("#CreatedBy").val();
            data["CommentByName"] = $("#CreatedByName").val();
           
               
                    $.ajax({
                        url: $BaseApiUrl + "api/data/InsertComment",
                        type: 'POST',
                        dataType: "json",headers: { 'Access-Control-Allow-Origin': true },                      
                        async: false,
                        data: data,
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);
                        },
                        success: function (data) {

                            // To-do code if ajax request is successful
                          //  $("#commentbody").html('');
                            $(".feed-activity-list").html('');
                            
                            $.each(data.comments, function (index, value) {
                                //$("#commentbody").append('<tr><td> ' + value.comment + ' </td><td style="width:20%;" >' + value.commentDate.substring(0, 10)+' </td> </tr>');
                              
                                $(".feed-activity-list").append('<div class=\"feed-element\"><div class=\"media-body\">Posted by:  <strong>' + value.commentByName + '</strong><br><small class=\"text-muted\">' + value.commentDate + '</small>');
                                $(".feed-activity-list").append('<div class=\"well\">' + value.comment + '</div></div></div><hr/>');
                            });

                            $("#activitybody").html('');

                            $.each(data.activity, function (index, value) {
                                $("#activitybody").append('<tr><td style="float:left" >' + value.activityDescription + ' </td><td style="width:20%;" >' + value.activityDate + ' </td> <td> ' + value.activityStatus + '</td> <td>' + value.activityByName + ' </td></tr>');
                            });

                            setTimeout(function ()
                            {
                                $("#btnAddComment").html('Send');
                                $("#btnAddComment").attr('disabled', false);
                                $("#btnAddComment").fadeIn();
                                $("#txtcomment").val('');
                               
                                $("#txtcomment").attr('disabled', false);
                            }, 1000);
                        },
                        error: function (ts) {
                            alert('error' + ts.errorMessage);
                        }
                    });
                }
           
    
    });

    $("#printclaim").click(function (e)
    {

        e.preventDefault();
        var claim = getParameterByName("Claim");
        var Type = claim[claim.length - 1];
        claim = claim.substr(0, claim.length - 1);

        if (claim != "")
        {
            var printWindow = window.open("/Home/PrintClaim/?claim=" + claim+"&Type="+Type, 'Claim Details', 'left=20, top=20, width=1200, height=auto, toolbar=0, resizable=1,scrollbars=no');

            printWindow.addEventListener('load', function () {
                setTimeout(function () {
                    printWindow.print();
                    //printWindow.close(); Notification.success({ message: "PDF Downloaded .....", delay: 3000 });
                }, 1000);
            }, true);
        }
    });


    $("#btnUpload").click(function ()
    {
        

        if ($("#logo").val() == "")
        {
            alert("Please Upload an Attachment to Proceed");
            $("#logo").focus();
        }      
        else
        {
            var ext = $('#my_file_field').val().split('.').pop().toLowerCase();

            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'docx', 'xls', 'xlsx', 'pdf', 'zip', 'mp4', 'mkv', 'doc', 'flv','avi','mov','mpg','wmv','3gp']) == -1)
            {
                alert('invalid File Upload!');
                return;
            }

            $("#btnUpload").attr('disabled', true);
            $("#btnUpload").html('<p style="color:white"> Uploading .... </p>');
            $("#btnUpload").fadeTo(0.28);
            $("#logo").attr('disabled', true);

            var insertForm = new FormData();
            insertForm.append('RefId', $("#claim").val());
            insertForm.append('At_FileName', $("#logo").val());
            insertForm.append('RefFormType', $("#type").val());
            insertForm.append('UploadedByName', $("#CreatedByName").val());
            insertForm.append('UploadedBy', $("#CreatedBy").val());
            insertForm.append('file', document.getElementById('logo').files[0]);

            $.ajax({
                url: $BaseApiUrl + "api/data/InsertAttachment",
                type: 'POST',
                dataType: "json",headers: { 'Access-Control-Allow-Origin': true },
                processData: false,
                contentType: false,
                data: insertForm,
                success: function (data)
                {
                    // To-do code if ajax request is successful

                    $("#attachmentbody").html('');

                    $.each(data.attachments, function (index, value)
                    {
                        $("#attachmentbody").append('<tr><td><a href="' + $BaseApiUrl + '/UploadedFiles/' + value.at_FileName + '" target="_blank" >' + value.at_Name + ' </a></td><td style="width:20%;" >' + value.uploadedDate.substring(0, 10) + ' </td> </tr>');
                    });

                    $("#activitybody").html('');

                    $.each(data.activity, function (index, value) {
                        $("#activitybody").append('<tr><td style="float:left" >' + value.activityDescription + ' </td><td style="width:20%;" >' + value.activityDate + ' </td> <td> ' + value.activityStatus + '</td> <td>' + value.activityByName + ' </td></tr>');
                    });

                    setTimeout(function () {
                        $("#btnUpload").html('Upload');
                        $("#btnUpload").attr('disabled', false);
                        $("#btnUpload").fadeIn();
                        $("#logo").val('');

                        $("#logo").attr('disabled', false);
                    }, 1000);

                },
                error: function (ts) {
                    alert('error' + ts.errorMessage);
                }
            });
        }
    });

    // HR Forms

   

});
