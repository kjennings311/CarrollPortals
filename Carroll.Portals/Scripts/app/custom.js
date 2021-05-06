// var $BaseApiUrl = "http://localhost:1002/";
var $BaseApiUrl = "http://aspnet.carrollaccess.net:1002/";


//49786/";
//   and UserOject are global variables can be used here.

var $ismobile = false;

if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|BB|PlayBook|IEMobile|Windows Phone|Kindle|Silk|Opera Mini/i.test(navigator.userAgent))
{
    // Take the user to a different screen here.
    $ismobile = true;
}

var $reloadpageformobile = false;
var $isemailconfirmed = false;

function ShowToastNotification(message,locationreload)
{
    $(".modal-alert").html(message);
  
    if ($ismobile)
    {
        $('#mobiletoastnotification').modal('show');
        $(".toastconfirmation").show();

        if (locationreload == "2") {
            $reloadpageformobile = true;
        }
    }
    else
    {
        $('#toastnotification').modal('show');

        if (locationreload == "1" || locationreload == "2") {

            setTimeout(function () { location.reload(); }, 3000);
        }
        else
        {
            setTimeout(function () { $('#toastnotification').modal('hide'); }, 3000);          

        }
    }
}

function ReloadLocation()
{
    if ($reloadpageformobile)
        setTimeout(function () { location.reload(); }, 2000);
    else
    {
        $('#toastnotification').modal('hide');
        $(".toastconfirmation").hide();

    }
}

function SelectCurrentMenu()
{
    if($ismobile == false)
    if ($("#pageid").length > 0)
    {
        $("#" + $("#pageid").val()).addClass("active");
        // hr
        if (["empleaserider", "newhire", "payroll", "empseparation", "requisitionrequest"].includes($("#pageid").val()))
        {
            $("#hrforms").addClass("active");
        }
        //Admin
        if (["users", "usertorole", "usertoproperty", "carrollpositions", "payperiod"].includes($("#pageid").val()))
        {
            $("#admin").addClass("active");
        }

        //expense        
        if (["instructions", "expense", "mileagelog"].includes($("#pageid").val()))
        {
            $("#expensereports").addClass("active");
        }

        //Referal
        if (["referalcontact", "referalrequest"].includes($("#pageid").val()))
        {
            $("#residentrelations").addClass("active");
        }
    }
}

if (!$ismobile)
    SelectCurrentMenu();

function validateEmail(emailID)
{
    atpos = emailID.indexOf("@");
    dotpos = emailID.lastIndexOf(".");
    if (atpos < 1 || (dotpos - atpos < 2))
    {    
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

function closereviewmodal()
{
    $("#reviewmodal").modal('hide'); 
}


function submitformdata()
{
    $("#reviewmodal").modal('hide');
    $("#homeval").val('0');
    $("#toastnotificationhome").modal('hide');
    $('.dynamicForm #savechanges').click();
   
}



function BindElements() {

    $form = $('.dynamicForm');

    var processing = false;

    $('.dynamicForm #savechanges').click(function ()
    {
        if (processing == false)
        {

            if ($("#homeval").length > 0) {
                if ($("#homeval").val() == '1') {
                    $("#reviewmodal").modal({
                        backdrop: 'static',
                        keyboard: false
                    },'show');
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
            if (RecordId == '' || RecordId === undefined) formUrl = "api/form/GenerateForm/" + FormName;
            else formUrl = "api/form/GenerateEditForm?entitytype=" + FormName + "&RECORDID=" + RecordId;


            //   var formUrl = "api/form/GenerateForm/" + $('.dynamicForm #savechanges').attr("formname");

            // if form is user then check if user exists with email id or not
            processing = true;

            if ($('.dynamicForm #savechanges').attr("formname") == "user" && RecordId == '') {
              
                $.ajax({
                    type: "GET",
                    url: $BaseApiUrl + "api/user/checkifuserexists/",
                    data: "id=" + $("#UserEmail").val(),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
                    async: false,
                    success: function (data) {

                        if (data == true) {
                           
                            var _err = "<b>Please correct the following errors</b><ul>";

                            _err += "<li> User with given Email Already Exists, Please Use Another </li>";
                            $form.find($this).closest('.form-group').addClass('has-error');
                            $form.find('#failureMessage').html(_err);
                            $form.find('.failure-message').show('slow');
                            ScrollToElement($form.find('.failure-message'));
                            $(".modal-alert").html('');
                            $('#toastnotification').modal('hide');
                            setTimeout(ScrollToElement($form), 3000);

                        }
                        else {
                            return;
                        }

                    }

                });
            }


            if ($("#divprocessingbtn").length > 0) {
                $("#divprocessingbtn").show();
            }

            //*****************************************************************************
            // Let's get the original form and then load values into original form to send back to server for validation

            $.ajax({
                type: "get",
                dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + formUrl,
                beforeSend: function (xhr) {
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
                                if ($('#' + $fields[i]["fieldName"]).prop('checked') == true) {
                                    //.is(":checked")
                                    $fields[i]["fieldValue"] = true;
                                    // console.log("check field value" + true);
                                }
                                else {
                                    $fields[i]["fieldValue"] = false;
                                }
                                break;
                            case "File":
                                // console.log('before assign ' + $fields[i]["fieldValue"] + " " + imagebase64);
                                if (imagebase64 != "")
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

                -
                    $.ajax({
                        type: "POST",
                        url: $BaseApiUrl + "api/Form/CreateUpdateFormData/" + $('.dynamicForm #savechanges').attr("formname"),
                        data: JSON.stringify($data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
                        async: false,
                        statusCode:
                        {
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

                                    if ($("#divprocessingbtn").length > 0) {
                                        $("#divprocessingbtn").hide();
                                    }

                                    $('.dynamicForm #savechanges').attr('disabled', false);
                                    ShowToastNotification('Record Successfully Updated', 2);
                                    //$(".modal-alert").html('Record Successfully Updated');
                                    //$('#toastnotification').modal('show');
                                    //// go back to previous screen after 8 seconds
                                    // setTimeout(function () { location.reload(); }, 3000);
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
                                    $('.dynamicForm #savechanges').attr('disabled', false);
                                    setTimeout(ScrollToElement($form), 3000);
                                    $(".modal-alert").html('');
                                    $('#toastnotification').modal('hide');
                                    if ($("#divprocessingbtn").length > 0) {
                                        $("#divprocessingbtn").hide();
                                    }


                                    $("#homeval").val('1');

                                } catch (err) { alert(err.message); }

                            }
                        } // status code
                    });
                    /******************************update/create end************************************/

                    processing = false;
                }
            });
        }
    });
}


function LoadUserProperty()
{

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
            alert('error' + ts.exceptionMessage);
        }
    });

    return val;
}


function getForm(FormName, RecordId)
{

    var formUrl = "";
    var TXT_ERROR = " <div class=\"alert col-md-10 alert-danger alert-dismissable failure-message\" style=\"display:none\"><div id=\"failureMessage\">there was an error!</div> </div>";
    var TXT_SUCCESS = "<div class=\"alert col-md-10 alert-success alert-dismissable success-message\" style=\"display:none\"><div id=\"successMessage\"></div> </div>";
    var $formBegin = '<form  class="form-horizontal CustomForm">';
    var $formEnd = '</form>';
    var $line = '<div class="hr-line-dashed"></div>';
    var $textbox = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input maxlength="100" data-climit="{7}"  type="text" validationformat="{1}" class="form-control {2}" id="{3}" {4} value="{5}"><span id="cnt{3}" > </span></div></div>';
    var $datebox = '<div class="form-group"><label class="col-sm-12 control-label">  <a  class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input maxlength="100"  type="date" validationformat="{1}"  class="form-control {2} hasDatepicker" id="{3}" {4} value="{5}"></div></div>';
    var $longtext = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><textarea validationformat="{1}" data-climit="{7}"  class="form-control {2} " id="{3}" {4} >{5}</textarea> <span id="cnt{3}" > </span> </div></div>';
    var $passbox = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input maxlength="100" type="password" validationformat="{1}" class="form-control {2}"  id="{3}" {4} value="{5}"></div></div>';
    var $filebox = '<div class="form-group"><label class="col-sm-2 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0}  </label ><div class="col-sm-10"><input maxlength="100" type="file" validationformat="{1}" onchange="encodeImageFileAsURL(this);" class="form-control {2}" id="{3}" {4} value="{5}"></div> <div id="imgTest" style="background: black;clear: both;margin-left:30%;width:300px;"><img src="{5}" style="width:80px;height:80px;"> </div></div>';
    var $hiddenField = '<input type="hidden" id="{0}" value="{1}"/>';
    var $checkbox = ' <div class="form-group"><label class="col-sm-6 col-xs-9 control-label"> <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{3}" > i </a> {0} </label><div class="col-sm-6 col-xs-3"> <div class="col-md-1 col-xs-12" > <input class="form-control" type="checkbox" style="width:18px;"  id="{1}" value="1"   {2}></div> </div></div>';
    var $person = '<div class="form-group"><label class="col-sm-12 control-label">  <a class="tooltipwala" data-container="body"  href="#" data-toggle="popover" data-trigger="hover" data-content="{6}" > i </a> {0} </label ><div class="col-sm-10"><input type="text" data-climit="{7}" validationformat="{1}" class="form-control {2}"  id="{3}" {4}><span id="cnt{3}" > </span></div></div>';
    var $savebuttons = '<div class="hr-line-dashed"></div>'
        + TXT_SUCCESS + TXT_ERROR
        + '<div class="form-group" >'
        + '<div class="col-sm-3 col-md-4 col-md-offset-3 col-lg-4 col-lg-offset-3 col-sm-offset-3">'       
        + '<a id="savechanges" class="btn btn-primary col-sm-offset-3  col-xs-offset-3 btn-add" style="background:#2f4050 !important; margin-bottom:10px; border:none !important" href="javascript:void(0);" formname="' + FormName + '">Save changes</button>'
        + '<a class="btn btn-white col-xs-offset-3 ppcancel"  href="javascript:location.reload();">Cancel</a>&nbsp;'
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
        dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
        url: $BaseApiUrl + formUrl,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

        },
        success: function (data) {
            var $FormElements = $formBegin;
            var $data = data;

            console.log(data);

            var $fields = $data["formFields"];
            console.log($fields);
            for (var i = 0; i < $fields.length; i++) {
                // alert($fields[i]["fieldLabel"]+ " " + $fields[i]["fieldType"]);
                // Build the form elements here
                switch ($fields[i]["fieldType"]) {
                    case "Text":

                        var $req = $fields[i]["required"];
                        var $datamask = "";


                        if ($fields[i]["fieldValidationType"] == "DateTime") {
                          //  alert($fields[i]["fieldValue"] + " inde datetime");

                            if ($fields[i]["fieldValue"] == null)
                            {
                               
                             //   $fields[i]["fieldValue"] = "";
                               
                                $FormElements += format($datebox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, "", ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"]);

                            }
                            else
                            {
                              
                                var s = $fields[i]["fieldValue"];

                                var bits = s.split(/\D/);
                                //  console.log(bits);

                               

                               // var datestring = bits[1] + "/" + bits[0] + "/" + bits[2];

                                
                                if ( bits[1].length == 1)
                                    bits[1] = "0" + bits[1];
                                if (bits[0].length == 1)
                                    bits[0] = "0" + bits[0];

                                var datestring = bits[2] + "-" + bits[0] + "-" + bits[1];

                              //  console.log(datestring);

                                //  var datestring = d.getMonth()+"/"+d.getDate() + "/"  + d.getFullYear();

                                $FormElements += format($datebox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : datestring, ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"], ($fields[i]["charLimit"] == null) ? "" : $fields[i]["charLimit"]);

                            }
                           

                        }
                        else
                        {
                            $FormElements += format($textbox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"], ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"], ($fields[i]["charLimit"] == null) ? "" : $fields[i]["charLimit"]);
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
                        $FormElements += format($person, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required tokenInput" : "tokenInput", $fields[i]["fieldName"], val, ($fields[i]["popOverText"] == null) ? "" : $fields[i]["popOverText"], ($fields[i]["charLimit"] == null) ? "" : $fields[i]["charLimit"]);
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

            if ($("#DateReported").length > 0)
            {
                document.getElementById("DateReported").valueAsDate = new Date();
                $("#DateReported").prop("disabled", true);
            }
            if ($("#PoliceReportNumber").length > 0) {
                
                $("#PoliceReportNumber").closest('.form-group').hide();
            }

    //         $('input[type="date"]').datepicker({
    //    format: "mm/dd/yyyy",
    //   "setDate": new Date()
    //});

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

    var propertyarray = value.split('se');

    $.get($BaseApiUrl + DataLoadUrl, function (data) {

        for (var i = 0; i < data.length; i++)
        {
            if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" )
            {

                if (data[i]["key"] == value)
                    selected = "selected=selected";
                options += "<option value=\"" + data[i]["key"] + "\"" + selected + ">" + data[i]["value"] + "</option>";
                selected = "";

            }
            else
            {
           //     if (propertyarray.includes(data[i]["key"]))
           //{
                    selected = "selected=selected";
                    options += "<option value=\"" + data[i]["key"] + "\"" + selected + ">" + data[i]["value"] + "</option>";
                    selected = "";
                //}
                    
            }

        }
        // now let's load options into select box
        $('#' + fieldId).append(options);

    });
}


function ApplyDateMask()
{
    console.log('calling date mask');
   
    //$('input[type="date"]').datepicker({
    //    format: "mm/dd/yyyy",
    //    "setDate": new Date()
    //});

    $('.date-mask').datepicker({
        format: "mm/dd/yyyy"
    }).datepicker("setDate", 'now');
}


function LoadHrForm(formname)
{
    $("#myModal").modal({
        backdrop: 'static',
        keyboard: false
    }, 'show');

    $(document).on('keyup', 'textarea', function () {
        var max = 1000;
        var len = $(this).val().length;
        if (len >= max) {
            $('#cnt' + $(this).attr('id')).html('<span style="color:red"> you have reached the limit </span>');
            var str = $(this).val();
            $(this).val(str.substring(1, 1000));

        } else {
            var char = max - len;
            $('#cnt' + $(this).attr('id')).html('<span style="color:green">' + char + ' characters left</span>');
        }

    });

    setTimeout(function () { ApplyDateMask();  }, 2000);
    
   
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
                    if ($ismobile)
                    {
                        if (formaname == "FormPropertyDamageClaim")
                            $(".form-heading").html("Property Damage Claim");
                        else if (formaname == "FormGeneralLiabilityClaim")
                            $(".form-heading").html("General Liability Claim");
                        else if (formaname == "FormMoldDamageClaim")
                            $(".form-heading").html("AMG (ASSUMED MICROBIAL GROWTH)");
                        else
                            $(".form-heading").html("");

                    }
                    else
                    {

                    if (formaname =="FormPropertyDamageClaim")
                        $(".form-heading").html("Property Damage Claim");
                    else if (formaname == "FormGeneralLiabilityClaim")
                        $(".form-heading").html("General Liability Claim");
                    else if (formaname == "FormMoldDamageClaim")
                        $(".form-heading").html("AMG (ASSUMED MICROBIAL GROWTH)");
                    else
                        $(".form-heading").html("");
                    }
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
            $(".form-heading").html("Property Damage Claim");
        else if (formaname == "FormGeneralLiabilityClaim")
            $(".form-heading").html("General Liability Claim");
        else if (formaname == "FormMoldDamageClaim")
            $(".form-heading").html("AMG (Assumed Microbial Growth) Claim");
        else
            $(".form-heading").html("");

        $('.claimmodal').modal('hide');

        $('.incidentformmodal').modal({
            backdrop: 'static',
            keyboard: false
        },'show');

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

        $('.btnDelete').css("color", "#1ab394");
        $('.btnDelete').removeAttr("disabled");
        $('.btnDelete').attr("itemId", $this.attr("id"));
        $('.btnDelete').attr("itemType", $this.attr("itemType"));
        $('.btnEdit').css("color", "#1ab394");
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
function HandleRowClickr(obj) {
    var $this = jQuery(obj);

    if ($("#homecontainer").length)
        console.log(JSON.stringify(obj) + "Selected Item in Home");

    if (!$this.hasClass('selected')) {

        //    $this.removeClass('selected');

        $('.btnDelete').css("color", "white");
        $('.btnDelete').removeAttr("disabled");
        $('.btnDelete').attr("itemId", $this.attr("id"));
        $('.btnDelete').attr("itemType", $this.attr("itemType"));
        $('.btnEdit').css("color", "white");
        $('.btnEdit').show();
        $('.btnEdit').removeAttr("disabled");

        $('.btnEdit').attr("itemId", $this.attr("id"));
        $('.btnEdit').attr("itemType", $this.attr("itemType"));
        // bind button events we can lookup item id on button attribute

        $('.btnDelete').unbind('click').bind('click', function () {
            if (confirm("Are you sure you want to delete?")) {
                var idToDelete = $(this).attr("itemId");
                var ItemType = $(this).attr("itemType");

                //  make ajax call to delete the row and remove the row.
                $.ajax({
                    type: "post",
                    dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
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
        $('.btnEdit').hide();
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
   // location.href = "http://"+location.hostname+"/Home/viewclaim/?Claim=" + data;
    // changed by Pavan 08/19/2019 @12:21pm est
    if ($("#UserR").val() != "Corporate")
    location.href = "/Home/viewclaim/?Claim=" + data;
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

function openmobilepopup(body)
{
    body = body.replace('null', '');
    
    $("#mobilepopupbody").html(body);

    $(".table td").each(function () {
        $(this).text().replace("null", "");
        $(this).text().replace("General Liability", "GL");
        $(this).text().replace("Mold Damage", "ML");
        $(this).text().replace("PropertyDamage", "PM");
    });

    $("#mobilepopup").modal({
        backdrop: 'static',
        keyboard: false
    },'show');
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
                success: function (data)
                {
                    if ($.fn.DataTable.isDataTable('.dtprops')) {
                        $('.dtprops').DataTable().destroy();
                    }

                    $('.dtprops tbody').empty();

                 //   alert("is mobile" + $ismobile);

                    var doms = '<"html5buttons"B>lTfgitp';
                    if ($ismobile)
                        doms = '<"top"i>rt<"bottom"flp><"clear">';

                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "pagingType": "full",
                        "rowCallback": function (row, data) {
                            // do anything row wise here
                            $(row).attr('id', data["contactId"]);
                            $(row).attr('itemType', "Contact");

                            if (!$ismobile)
                                $(row).attr('onClick', 'HandleRowClick(this);');
                            else
                            {
                                var html = "<tr><td> First Name :  </td> <td> " + data["firstName"] + " </td> </tr><tr> <td> Last Name </td> <td>" + data["lastName"] + " </td></tr><tr> <td> Email:  </td> <td>" + data["email"] + " </td> </tr><tr><td> Title :  </td> <td> " + data["title"] + " </td> </tr><tr><td> Phone :  </td> <td>" + data["phone"] +" </td> </tr> ";
                                html=html.replace("null", "");
                                $(row).attr('onClick', 'openmobilepopup("' + html + '");');
                            }
                            //console.log(data);

                        },
                        "order": [[2, 'asc']],
                        dom: doms, //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
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

                    setTimeout(function () {

                        CheckToLoadContact();

                    }, 1000);


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
                success: function (data)
                {
                    if ($.fn.DataTable.isDataTable('.dtprops'))
                    {
                        $('.dtprops').DataTable().destroy();
                    }

                    $('.dtprops tbody').empty();

                    var doms = '<"html5buttons"B>lTfgitp';
                    if ($ismobile)
                        doms = '<"top"i>rt<"bottom"flp><"clear">';

                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "pagingType": "full",
                        "rowCallback": function (row, data)
                        {
                            // do anything row wise here
                            $(row).attr('id', data["equityPartnerId"]);
                            $(row).attr('itemType', "Partner");
                            if (!$ismobile)
                                $(row).attr('onClick', 'HandleRowClick(this);');
                            else {
                                var html = "<tr><td> Partner Name :  </td> <td> " + data["partnerName"] + " </td> </tr><tr> <td> Address Line1 : </td> <td>" + data["addressLine1"] + " </td></tr><tr> <td> Address Line2 :  </td> <td>" + data["addressLine2"] + " </td> </tr><tr><td> City :  </td> <td> " + data["city"] + " </td> </tr><tr><td> State :  </td> <td>" + data["state"] + " </td> </tr> <tr><td> Contact Person :  </td> <td>" + data["contactPerson"] + " </td> </tr> ";
                                html = html.replace("null", "");
                                $(row).attr('onClick', 'openmobilepopup("' + html + '");');
                            }                          

                        },
                        "order": [[2, 'asc']],
                        dom: doms, //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
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
                            { "data": "contactPerson", "name": "contactPerson", "autoWidth": false },
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

                    setTimeout(function () {

                        CheckToLoadContact();

                    }, 1000);
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

                    if ($.fn.DataTable.isDataTable('.dtprops')) {
                        $('.dtprops').DataTable().destroy();
                    }

                    $('.dtprops tbody').empty();
                    var doms = '<"html5buttons"B>lTfgitp';
                    if ($ismobile)
                        doms = '<"top"i>rt<"bottom"flp><"clear">';
                    var datatableVariable = $('.dtprops').DataTable({
                        data: data,
                        processing: true,
                        "scrollX": true,
                        "pagingType": "full",
                        "rowCallback": function (row, data)
                        {
                            // do anything row wise here
                            $(row).attr('id', data["propertyId"]);
                            $(row).attr('itemType', "property");
                            if (!$ismobile)
                                $(row).attr('onClick', 'HandleRowClick(this);');
                            else
                            {
                                var html = "<tr><td> Property # :  </td> <td> " + data["propertyNumber"] + " </td> </tr><tr> <td> Property Name : </td> <td>" + data["propertyName"] + " </td></tr><tr> <td> Legal Name :  </td> <td>" + data["legalName"] + " </td> </tr><tr><td> Units :  </td> <td> " + data["units"] + " </td> </tr><tr><td> Owned :  </td> <td>" + data["isOwned"] + " </td> </tr> ";

                                html = html + "<tr><td> Address :  </td> <td> " + data["propertyAddress"] + " </td> </tr><tr> <td> City : </td> <td>" + data["city"] + " </td></tr><tr> <td> State :  </td> <td>" + data["state"] + " </td> </tr><tr><td> Zip :  </td> <td> " + data["zipCode"] + " </td> </tr><tr><td> Phone :  </td> <td>" + data["phoneNumber"] + " </td> </tr> <tr><td> PM Email :  </td> <td>" + data["emailAddress"] + " </td> </tr> ";

                                //eq
                                if (data.equityPartner != null)
                                {
                                    html = html + "<tr><td> Equity Partner : </td><td>" + data.equityPartner.substring(9, data.equityPartner.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Equity Partner : </td><td> </td> </tr>";
                                }


                                //vice prese
                                if (data.vicePresident != null) {
                                    html = html + "<tr><td> Vice President : </td><td>" + data.vicePresident.substring(9, data.vicePresident.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Vice President : </td><td> </td> </tr>";
                                }


                                //vice prese
                                if (data.regionalVicePresident != null) {
                                    html = html + "<tr><td> Regional Vice President  </td><td>" + data.regionalVicePresident.substring(9, data.regionalVicePresident.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Regional Vice President  </td><td> </td> </tr>";
                                }

                                //RM
                                if (data.regionalManager != null) {
                                    html = html + "<tr><td> Regional Manager : </td><td>" + data.regionalManager.substring(9, data.regionalManager.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Regional Manager : </td><td> </td> </tr>";
                                }


                                //RM
                                if (data.propertyManager != null) {
                                    html = html + "<tr><td> Property Manager : </td><td>" + data.propertyManager.substring(9, data.propertyManager.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Property Manager : </td><td> </td> </tr>";
                                }

                                //RM
                                if (data.constructionManager != null) {
                                    html = html + "<tr><td> Construction Manager : </td><td>" + data.constructionManager.substring(9, data.constructionManager.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Construction Manager : </td><td> </td> </tr>";
                                }


                                //RM
                                if (data.assetManager1 != null) {
                                    html = html + "<tr><td> Asset Manager : </td><td>" + data.assetManager1.substring(9, data.assetManager1.indexOf('","')) + " </td> </tr>";
                                }
                                else {
                                    html = html + "<tr><td> Asset Manager : </td><td> </td> </tr>";
                                }

                              //  html = html + "<tr><td> TakeOver Date: </td><td> "+data[""]+" </td> </tr>";


                                html = html.replace("null", "");
                                $(row).attr('onClick', 'openmobilepopup("' + html + '");');
                            }                                                   

                        },
                        "order": [[2, 'asc']],
                        dom: doms, //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
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
                                        return '<a target="_blank" href="/data/partners?name=' + result["name"] + '">' + result["name"] + '</a>';
                                     
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
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'regionalVicePresident',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'regionalManager',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'propertyManager',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'constructionManager',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'assetManager1',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'assetManager2',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
                                    } else return '';
                                }
                            },

                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'insuranceContact',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a target="_blank" href="/data/contacts?name=' + result["name"] + '">' + result["name"] + '</a>';
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
                            { "data": "isActive", "name": "isActive", "autoWidth": false },
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
                   // $this.datepicker({});
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

function CheckToLoadContact()
{
    var filter = getParameterByName("name");
    //alert(filter);
    if (filter != null) {
        $("#DataTables_Table_0_filter input").val(filter);
        $('.dtprops').DataTable().search(filter).draw();
    }
}

function FilterClaims(filter)
{
    $("#DataTables_Table_0_filter input").val(filter);
    $('.dtprops').DataTable().search(filter).draw();
    $(".pr,.ge,.mo").removeClass('active');

    if (filter == "Mold")
    {
        $(".mo").addClass('active');
    }
    else if (filter == "General")
    {
        $(".ge").addClass('active');
    }
    else if (filter == "Property")
    {
        $(".pr").addClass('active');
    }
}

$(document).on("change", "#DataTables_Table_0_filter input",function () {

    if ($("#DataTables_Table_0_filter input").val() == "") {
        $(".pr,.ge,.mo").removeClass('active');
    }
});

var intcount = 0;

function GetAllClaims(Type,chk) {


    if ($.fn.DataTable.isDataTable('.dtprops')) {

        console.log($(".dtprops").DataTable());
        if ($(".dtprops").DataTable() != undefined)
        {
            $(".dtprops").DataTable().clear().destroy();
            $('.dtprops tbody').empty();
        }
    }

    var orderby = 0;
    if (chk)
    {
        orderby = 1;
    }




    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",
                headers: { 'Access-Control-Allow-Origin': true },
                url: $BaseApiUrl + "api/data/getallclaims?Type="+Type+"&orderby="+orderby+"&userid="+ $("#CreatedBy").val()+"&propertyid=null",
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

                    var doms = '<"html5buttons"B>lTfgitp';
                    if ($ismobile)
                        doms = '<"top"lfi>rt<"bottom"p><"clear">';
                    
                    var sort = [[6, 'asc']];

                    if (chk)
                    {
                        sort = [[7, 'asc']];
                    }
                  
                  


                    var datatableVariable = $('.dtprops').on('init.dt', function ()
                    {
                        console.log('Table initialisation complete: ' + new Date().getTime());
                        $("#filtercheck1").remove();

                        if (chk)
                            $("#DataTables_Table_0_filter label").append(' <div id="filtercheck1" style="text-align:right;margin-right:3%;"> <input type="checkbox" value="1" checked id="filterval1" /> <span id="filterspan1" style="display:block;float: right;padding: 1px 0px 0px 5px;"> Show Updated Claims </span>  </div>');
                        else
                            $("#DataTables_Table_0_filter label").append(' <div id="filtercheck1" style="text-align:right;margin-right:3%;"> <input type="checkbox" value="1"  id="filterval1" /> <span id="filterspan1" style="display:block;float: right;padding: 1px 0px 0px 5px;"> Show Updated Claims </span>  </div>');


                    }).DataTable({
                        data: configdata.rows,
                        processing: true,
                        scrollY: '50vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "pagingType": "full",
                        "rowCallback": function (row, data)
                        {
                            // do anything row wise here      

                            $(row).attr('id', data[configdata.pkName]);
                            $(row).attr('itemType', configdata.etType);
                            var rowdata = JSON.parse(JSON.stringify(data));
                            console.log(rowdata.claimType.trim());
                            if (rowdata.claimType.toLowerCase().trim() == "general")
                                $(row).attr('onClick', "LoadFormView('" + rowdata.id +"g');");
                            else if (rowdata.claimType.toLowerCase().trim() == "amg")
                                $(row).attr('onClick', "LoadFormView('" + rowdata.id + "m');");
                            else if (rowdata.claimType.toLowerCase().trim() == "property")
                                $(row).attr('onClick', "LoadFormView('" + rowdata.id + "p');");                          
                           
                        },                       
                        dom: doms, //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
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
                        columns: columnlist1,
                        columnDefs: [{
                            "targets": [4,7,6],
                            "type": 'date',
                        }]

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
                        select: false,     // enable single row selection
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
                url: $BaseApiUrl + "api/data/GetAllHrForms?FormType=" + formtype + "&userid="+ $("#CreatedBy").val(),
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
                    var selct = false;

                    if (formtype == "Requisition Request") 
                    {
                        selct = 'single';
                    }

                    var datatableVariable = $('.dtprops').DataTable({
                        data: configdata.rows,
                        processing: true,
                        scrollY: '80vh',
                        scrollCollapse: true,
                        "scrollX": true,
                        "aaSorting": [[1, "desc"]],
                        "rowCallback": function (row, data) {
                            // do anything row wise here     
                          //  var d = JSON.parse(data);
                          //  alert(row + d);
                          ////  console.log(row + " " + JSON.parse(data));
                          //  $(row).attr('id', row.printOption);
                          //  $(row).attr('itemType', configdata.etType);
                            $(row).attr('onClick', 'HandleRowClickr(this);');

                            //var rowdata = JSON.parse(JSON.stringify(data));
                            //if (rowdata.claimType == "General Liability")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "g');");
                            //else if (rowdata.claimType == "Mold Damage")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "m');");
                            //else if (rowdata.claimType == "PropertyDamage")
                            //    $(row).attr('onClick', "LoadFormView('" + rowdata.id + "p');");

                        },                       
                        dom: '<"html5buttons"B>lTfgitp', //dom: 'Bfrtip',        // element order: NEEDS BUTTON CONTAINER (B) ****
                        select: selct,     // enable single row selection
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
            alert('error' + ts.exceptionMessage);
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

function LoadAllProperties() {


    // now let's load options into select box
   // $('#' + fieldId).append(options);
    var st = $("#UserPropertyAccess").val();
    var propertyarray = Array();

    if (st.includes("se")) {
        propertyarray = $("#UserPropertyAccess").val().split('se');
        000002
    }
    else
        propertyarray.push(st);


    var options1 = "<option value='' > Select Property </option>";

    if ($("#pageidname").val() == "requisitionrequest" || $("#pageidname").val() == "LeaseRider")
        var options1 = "<option value='' > Select Property </option> <option value='Corporate' > Corporate </option> ";

    $.get($BaseApiUrl + "api/user/GetPropertiesForSelect", function (data)
    {
        var defaultsel = $("#location").attr('seloption');

        for (var i = 0; i < data.length; i++)
        {
            if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" || $("#UserR").val() == "HR" ) {

                if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
                    options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                else
                    options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                selected = "";

            }
            else
            {
                if (propertyarray.includes(data[i]["key"]))
                {                   
                    options1 += "<option value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
              }
            }

            //if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" || $("#UserR").val() == "HR") {

            //    if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
            //        options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
            //    else
            //        options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
            //    selected = "";
            //}
            //else {

            //}

        }
        // now let's load options into select box
        $('#location').html(options1);
        if ($("#property1").length > 0) {
            $('#property1').html(options1);
        }
    });


}


function LoadHrPositionsByType(t)
{
    var options = "";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPositionsByType?Type=" + t, function (data) {
        
        options = "<option value='' > Select  </option>";
        for (var i = 0; i < data.length; i++) {
            
            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";
        }
        
        $('#position').html(options);
    });
    
        // now let's load options into select box
     

    }



function LoadHrPositionsByTypeForJobTitle(t) {
    var options = "";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPositionsByType?Type=" + t, function (data) {

        options = "<option value='' > Select  </option>";
        for (var i = 0; i < data.length; i++) {

            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";
        }

        $('#jobtitle').html(options);
    });



    var st = $("#UserPropertyAccess").val();
    var propertyarray = Array();

    if (st.includes("se")) {
        propertyarray = $("#UserPropertyAccess").val().split('se');
    }
    else
        propertyarray.push(st);


    var options1 = "<option value='' > Select Property </option>";
    if ($("#pageidname").val() == "requisitionrequest" || $("#pageidname").val() == "LeaseRider")
        var options1 = "<option value='' > Select Property </option> <option value='Corporate' > Corporate </option> ";


    $.get($BaseApiUrl + "api/user/GetPropertiesForSelect", function (data)
    {
       
        var defaultsel = $("#location").attr('seloption');

        console.log(defaultsel);

        for (var i = 0; i < data.length; i++)
        {

          
            if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" || $("#UserR").val() == "HR") {

                if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
                    options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                else
                    options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                selected = "";

            }
            else {
                if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"]) {
                    options1 += "<option selected value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                }
                else
                    if (propertyarray.includes(data[i]["key"])) {
                        options1 += "<option value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                    }
            }

            //if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
            //    options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
            //else
            //    options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
            selected = "";

        }
        // now let's load options into select box
        $('#location').html(options1);

        if ($("#property1").length > 0) {

            $('#property1').html(options);
        }
 
        // now let's load options into select box

    });


}


function LoadPropertiesForLiveAt() {
    
    
    $.get($BaseApiUrl + "api/user/GetPropertiesForSelect", function (data)
    {

        var liveat = "<option value='' > Select Property </option>";
        for (var i = 0; i < data.length; i++) {

            liveat += "<option value =\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
        }
       

        if ($("#liveatlocation").length > 0) {
            $('#liveatlocation').html(liveat);
        }
        
    });
}


function LoadHrPositions()
{
    var options = "";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPositions", function (data) {
       options = "<option value='' > Select  </option>";
        for (var i = 0; i < data.length; i++)
        {
            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";
        }
        // now let's load options into select box
        if ($("#position").length > 0)
        $('#position').html(options);

        if ($("#totitle").length > 0)
        {
            $('#totitle').html(options);
        }
        if ($("#fromtitle").length > 0)
        {
            $('#fromtitle').html(options);
        }
    });

    var st = $("#UserPropertyAccess").val();
    var propertyarray = Array();

    if (st.includes("se")) {
        propertyarray = $("#UserPropertyAccess").val().split('se');
    }
    else
        propertyarray.push(st);


    var options1 = "<option value='' > Select Property </option>";
    if ($("#pageidname").val() == "requisitionrequest" || $("#pageidname").val() == "LeaseRider")
        var options1 = "<option value='' > Select Property </option> <option value='Corporate' > Corporate </option> ";


    $.get($BaseApiUrl + "api/user/GetPropertiesForSelect", function (data)
    {
        var defaultsel = $("#location").attr('seloption');

        console.log(defaultsel);

        for (var i = 0; i < data.length; i++)
        {
            if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" || $("#UserR").val() == "HR") {

                if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
                    options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                else
                    options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                selected = "";

            }
            else {
                if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"]  ) {
                    options1 += "<option selected value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                }
                else
                    if (propertyarray.includes(data[i]["key"])) {
                        options1 += "<option value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                    }
            }

            //if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
            //    options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
            //else
            //    options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
           selected = "";

        }
        // now let's load options into select box
        $('#location').html(options1);
        if ($("#property1").length > 0) {

            $('#property1').html(options);
        }
    });


}

function LoadPropertiesForSelect(iskey,control)
{

    var st = $("#UserPropertyAccess").val();
    var propertyarray = Array();

    if (st.includes("se")) {
        propertyarray = $("#UserPropertyAccess").val().split('se');
    }
    else
        propertyarray.push(st);

    var options1 = "<option value='' > Select Property </option>";
    if ($("#pageidname").val() == "requisitionrequest" || $("#pageidname").val() == "LeaseRider")
        var options1 = "<option value='' > Select Property </option> <option value='Corporate' > Corporate </option> ";


    $.get($BaseApiUrl + "api/user/GetPropertiesForSelect", function (data) {
        var defaultsel = $("#"+control).attr('seloption');

        if (iskey == true)
        {
            for (var i = 0; i < data.length; i++)
            {

                if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" || $("#UserR").val() == "HR") {

                    if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
                        options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                    else
                        options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                    selected = "";

                }
                else
                {
                    

                    if (propertyarray.includes(data[i]["key"])) {
                        options1 += "<option value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                    }
                }


                //if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
                //    options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                //else
                //    options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                selected = "";
            }
        }
        else
        {
            for (var i = 0; i < data.length; i++)
            {
                if ($("#UserR").val() == "Administrator" || $("#UserR").val() == "Management" || $("#UserR").val() == "HR") {

                    if (defaultsel == data[i]["key"] || defaultsel == data[i]["value"])
                        options1 += "<option selected value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                    else
                        options1 += "<option value=\"" + data[i]["key"] + "\">" + data[i]["value"] + "</option>";
                    selected = "";

                }
                else {

                   // options1 += "<option value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                                       
                    if (propertyarray.includes(data[i]["key"])) {
                        options1 += "<option value=\"" + data[i]["key"] + "\" >" + data[i]["value"] + "</option>";
                    }
                }

                //if (defaultsel == data[i]["value"])
                //    options1 += "<option selected value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
                //else
                //    options1 += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
                selected = "";

            }
        }

     
        // now let's load options into select box
        if ($("#" + control).length > 0)
        {
            $("#" + control).html(options1);
        }
    //    if ($("#location").length > 0) {

    //        $('#location').append(options1);

    //    }

    //if ($("#property1").length > 0) {
    //        $('#property1').append(options);
    //    }

    //    if ($("#propertyname").length > 0) {
    //        $('#propertyname').append(options);
    //    }
    });
}


function LoadPositions() {
    var options = "";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPositions", function (data) {
        options = "<option value='' > Select  </option>";
        for (var i = 0; i < data.length; i++) {
            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";
        }
        // now let's load options into select box
        $('#position').append(options);

        if ($("#totitle").length > 0) {
            $('#totitle').html(options);
        }
        if ($("#fromtitle").length > 0) {
            $('#fromtitle').html(options);
        }

        if ($("#jobtitle").length > 0) {
            $('#jobtitle').html(options);
        }
    });
    
}


function LoadPayRoles() {

    var options = "<option> SELECT </option>";

    $.get($BaseApiUrl + "api/Data/GetAllCarrollPayPerilds", function (data) {

        for (var i = 0; i < data.length; i++) {
            options += "<option value=\"" + data[i]["value"] + "\">" + data[i]["value"] + "</option>";
            selected = "";

        }
        // now let's load options into select box
        $('#beginpayperiod').html(options);

    });
}

function ReturnUSDate(datestring) {

    return (datestring.substring(5, 7) + '/' + datestring.substring(8, 10) + '/' + datestring.substring(0, 4));
}

function LoadClaim()
{

    var claim = getParameterByName("Claim");
    var Type = claim[claim.length - 1];
    claim = claim.substr(0, claim.length - 1);

    $("#claim").val(claim);
    if(Type=="m")
        $("#type").val("3");
    else if (Type == "p")
        $("#type").val("1");
    else if (Type == "g")
        $("#type").val("2");

    var hrv = $("#exportclaim").attr('href');
   
    hrv = hrv.replace("num", claim);
  
    $("#exportclaim").attr('href', hrv + Type);


    var hrv = $("#printclaim").attr('href');

    hrv = hrv.replace("num", claim);

    $("#printclaim").attr('href', hrv + Type);

       
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
                                $("#heading").html("Property Damage Claim - " + ClaimData.claim.tbl.claimNumber);
                                $("#property").html(ClaimData.claim.propertyName);
                                claimbody += ' <table class="table">';
                                claimbody += '<tr><td style="width:30%;"> Weather Conditions:</td><td>' + CheckNull(ClaimData.claim.tbl.weatherConditions) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Incident Date:</td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.incidentDateTime)) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Incident Time:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentTime) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Incident Location:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentLocation) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">  Resident Name :</td><td>' + ClaimData.claim.tbl.residentName + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">  Resident Contact Information :</td><td>' + ClaimData.claim.tbl.residentContactInformation + '</td></tr>';
                             
                                claimbody += '<tr><td style="width:30%;">Incident Description:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentDescription) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Estimate of Damage:</td><td>' + CheckNull(ClaimData.claim.tbl.estimateOfDamage) + '</td></tr>';

                                if (ClaimData.claim.tbl.authoritiesContacted == false)
                                    claimbody += '<tr><td style="width:30%;">Authorities Contacted:</td><td>No</td></tr>';
                                else
                                {
                                    claimbody += '<tr><td style="width:30%;">Authorities Contacted:</td><td>Yes</td></tr>'; 
                                    claimbody += '<tr><td style="width:30%;"> Police Report Number </td><td>' + CheckNull(ClaimData.claim.tbl.policeReportNumber) + '</td></tr>'; 
                                }
                                                                 
                                claimbody += '<tr><td style="width:30%;"> Authority Contact Person:</td><td>' + CheckNull(ClaimData.claim.tbl.contactPerson) + '</td></tr>';  
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
                                claimbody += '<tr><td style="width:30%;">Witness Phone (Alternate): </td><td>' + CheckNull(ClaimData.claim.tbl.reportedPhone) + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Reported By:</td><td>' + CheckNull(ClaimData.claim.tbl.incidentReportedBy) + '</td></tr>';
                             
                                claimbody += '<tr><td style="width:30%;">Reported Date:</td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.dateReported).substring(0, 10)) + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Created By:</td><td>' + CheckNull(ClaimData.claim.tbl.createdByName) + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;">Created Date:</td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10)) + ' ' + CheckNull(ClaimData.claim.tbl.createdDate).substring(11, 19) + '</td></tr>';
                                claimbody += '</table>'

                                $("#claimbody").html(claimbody);

                            }
                            else if (Type == "m") {
                                $("#heading").html("AMG (Assumed Microbial Growth) " + ClaimData.claim.tbl.claimNumber);
                                $("#property").html(ClaimData.claim.propertyName);

                            
                               // $('.claimDesc').html(ClaimData.claim.tbl.description);
                                claimbody += ' <table class="table ">';
                                claimbody += '<tr><td  style="width:30%;"> Discovery Date : </td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.discoveryDate).substring(0, 10)) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Location :</td><td>' + ClaimData.claim.tbl.location + '</td></tr>';        
                                
                                claimbody += '<tr><td style="width:30%;"> Apartment Occupied :</td><td>' + (ClaimData.claim.tbl.apartmentOccupied == true ? "Yes ":"No")  + '</td></tr>';                               
                                 claimbody += '<tr><td style="width:30%;"> Residents Affected :</td><td>' + ClaimData.claim.tbl.residentsAffected + '</td></tr>';                               
                                claimbody += '<tr><td style="width:30%;">   Resident Name :</td><td>' + ClaimData.claim.tbl.residentName + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">Resident Contact Information :</td><td>' + ClaimData.claim.tbl.residentContactInformation + '</td></tr>';                              
                                claimbody += '<tr><td style="width:30%;"> Resident Relocating : </td><td>' + (ClaimData.claim.tbl.residentsRelocating == true ? "Yes":"No")  + '</td></tr>';                               

                                claimbody += '<tr><td  style="width:30%;"> Description :</td><td>' + ClaimData.claim.tbl.description + '</td></tr>';  
                                claimbody += '<tr><td  style="width:30%;"> Suspected Cause : </td><td>' + ClaimData.claim.tbl.suspectedCause + '</td></tr>';   
                                console.log('building still wet'+ClaimData.claim.tbl.areBuildingMaterialsStillWet )
                                if (ClaimData.claim.tbl.areBuildingMaterialsStillWet == false ) {
                                    claimbody += '<tr><td  style="width:30%;">Are Building Materials Still Wet: </td><td> No </td></tr>';
                                    
                                }

                                else {
                                    claimbody += '<tr><td  style="width:30%;">Are Building Materials Still Wet: </td><td> Yes </td></tr>';
                                   
                                }
                                if (ClaimData.claim.tbl.isStandingWaterPresent == false)
                                    claimbody += '<tr><td  style="width:30%;"> Is Standing Water Present : </td><td>  No' + '</td></tr>';
                                else
                                    claimbody += '<tr><td  style="width:30%;"> Is Standing Water Present : </td><td> Yes </td></tr>';

                                claimbody += '<tr><td  style="width:30%;"> How Much Water Present :</td><td>' + ClaimData.claim.tbl.howMuchWater + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;">Estimated Surface Area Affected : </td><td>' + ClaimData.claim.tbl.estimatedSurfaceAreaAffected + '</td></tr>'; 
                                claimbody += '<tr><td  style="width:30%;"> Estimated Time Damage Present: </td><td>' + ClaimData.claim.tbl.estimatedTimeDamagePresent + '</td></tr>';                                  
                                claimbody += '<tr><td  style="width:30%;"> Corrective Actions Taken : </td><td> ' + ClaimData.claim.tbl.correctiveActionsTaken + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Planned Actions : </td><td>' + ClaimData.claim.tbl.plannedActions + '</td></tr>';
                                                          
                                                            
                            
                                                          
                                //if (ClaimData.claim.tbl.correctiveActionsTaken== false)
                                //    claimbody += '<tr><td> Corrective Actions Taken : </td><td> No </td></tr>';
                                //else
                            
                                claimbody += '<tr><td  style="width:30%;"> Additional Comments : </td><td>' + ClaimData.claim.tbl.additionalComments + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Reported By : </td><td>' + ClaimData.claim.tbl.reportedBy + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Reported Date : </td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.dateReported).substring(0, 10)) + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Reported Phone : </td><td>' + ClaimData.claim.tbl.reportedPhone + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Created By :</td><td>' + ClaimData.claim.tbl.createdByName + '</td></tr>';
                                claimbody += '<tr><td  style="width:30%;"> Created Date:</td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10)) + ' ' + CheckNull(ClaimData.claim.tbl.createdDate).substring(11, 19) + '</td></tr>';
                                claimbody += '</table>'

                                $("#claimbody").html(claimbody);
                            }
                            else if (Type == "g")
                            {
                                $("#heading").html("General Liability Claim - " + ClaimData.claim.tbl.claimNumber);
                                $("#property").html(ClaimData.claim.propertyName);
                                claimbody += ' <table class="table">';

                                claimbody += '<tr><td style="width:30%;"> Incident Date:</td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.incidentDateTime).substring(0, 10)) + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Incident Time:</td><td>' + ClaimData.claim.tbl.incidentTime + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;"> Incident Location:</td><td>' + ClaimData.claim.tbl.incidentLocation + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;"> Incident Description:</td><td>' + ClaimData.claim.tbl.incidentDescription + '</td></tr>';  
                               
                                if (ClaimData.claim.tbl.authoritiesContacted == false)
                                    claimbody += '<tr><td>Authorities Contacted: </td><td> No </td></tr>';
                                else {
                                    claimbody += '<tr><td>Authorities Contacted: </td><td> Yes </td></tr>';
                                    claimbody += '<tr><td style="width:30%;"> Police Report Number </td><td>' + CheckNull(ClaimData.claim.tbl.policeReportNumber) + '</td></tr>'; 
                                }
                                    
                                claimbody += '<tr><td style="width:30%;"> Authority Contact Person :</td><td>' + ClaimData.claim.tbl.contactPerson + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">   Resident Name :</td><td>' + ClaimData.claim.tbl.residentName + '</td></tr>';
                                claimbody += '<tr><td style="width:30%;">  Resident Contact Information :</td><td>' + ClaimData.claim.tbl.residentContactInformation + '</td></tr>';

                               
                                
                                claimbody += '<tr><td style="width:30%;"> Claimant Name:</td><td>' + ClaimData.claim.tbl.claimantName + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Claimant Address:</td><td>' + ClaimData.claim.tbl.claimantAddress + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Claimant Primary Phone :</td><td>' + ClaimData.claim.tbl.claimantPhone1 + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;"> Claimant Secondary Phone :</td><td>' + ClaimData.claim.tbl.claimantPhone2 + '</td></tr>'; 
                              
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
                                claimbody += '<tr><td style="width:30%;">Witness Phone (Alternate) :</td><td>' + ClaimData.claim.tbl.reportedPhone + '</td></tr>'; 

                              //  claimbody += '<tr><td style="width:30%;">Description Of Property:</td><td>' + ClaimData.claim.tbl.descriptionOfProperty + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Description Of Damage:</td><td>' + ClaimData.claim.tbl.descriptionOfDamage + '</td></tr>'; 
                              
                                claimbody += '<tr><td style="width:30%;">Reported By:</td><td>' + ClaimData.claim.tbl.reportedBy + '</td></tr>';                                 
                             
                                if (ClaimData.claim.tbl.notifySecurityOfficer == true)
                                    claimbody += '<tr><td style="width:30%;">Notify Security Officer :</td><td> Yes </td></tr>';
                                else
                                    claimbody += '<tr><td style="width:30%;">Notify Security Officer :</td><td> No </td></tr>'; 

                                claimbody += '<tr><td style="width:30%;">Date Reported:</td><td>' + ReturnUSDate((ClaimData.claim.tbl.dateReported).substring(0, 10)) + '</td></tr>';  

                                claimbody += '<tr><td style="width:30%;">Created By:</td><td>' + ClaimData.claim.tbl.createdByName + '</td></tr>'; 
                                claimbody += '<tr><td style="width:30%;">Created Date:</td><td>' + ReturnUSDate(CheckNull(ClaimData.claim.tbl.createdDate).substring(0, 10)) + ' ' + CheckNull(ClaimData.claim.tbl.createdDate).substring(11, 19)+'</td></tr>'; 
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
                                $("#attachmentbody").append('<tr><td style="float:left" ><a href="' + $BaseApiUrl + '/UploadedFiles/' + value.at_FileName + '" target="_blank" >' + value.at_Name + ' </a></td><td style="width:20%;" >' + value.uploadedDate.substring(0, 10) + ' </td> </td><td style="width:20%;" >' + value.uploadedByName + ' </td> </tr>');
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
                                    
                                    $details = "<li class='font-bold'>Address:</li><li>" + $prop.propertyAddress + "</li>";
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

                                    var details = "";
                                    if ($prop.assetManager1 != null) {
                                        var $assetManager = JSON.parse($prop.assetManager1);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Asset Managers:</li><li>" + $assetManager.name + "</li>";
                                    } else {
                                        details += "<li>&nbsp;</li><li class='font-bold'>Asset Managers:</li>";
                                    }


                                    if ($prop.assetManager2 != null) {
                                        var $assetManager2 = JSON.parse($prop.assetManager2);
                                        details += "<li>" + $assetManager2.name + "</li>";
                                    } 

                                    if ($prop.vicePresident != null) {
                                        var $val = JSON.parse($prop.vicePresident);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Vice President:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.regionalVicePresident != null) {
                                        var $val = JSON.parse($prop.regionalVicePresident);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Regional Vice President:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.constructionManager != null) {
                                        var $val = JSON.parse($prop.constructionManager);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Construction Manager:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.regionalManager != null) {
                                        var $val = JSON.parse($prop.regionalManager);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Regional Manager:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.propertyManager != null)
                                    {
                                        var $val = JSON.parse($prop.propertyManager);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Property Manager:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($prop.insuranceContact != null)
                                    {
                                        var $val = JSON.parse($prop.insuranceContact);
                                        details += "<li>&nbsp;</li><li class='font-bold'>Insurance Contact:</li><li>" + $val.name + "</li>";
                                    } 

                                    if ($ismobile)
                                    {
                                        $('.PropDetails').html($details);
                                        $('.PropDetails1').html(details);

                                        $('.propertyName').html($prop.propertyName);

                                    }
                                    else
                                    {

                                        $('.PropDetails').html($details+details);
                                        $('.propertyName').html($prop.propertyName);
                                    }

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
            alert('error' + ts.exceptionMessage);
        }
    });

}

$(document).ready(function () {

    $("#btnAddComment").click(function () {

        if ($("#txtcomment").val() == "") {
            alert("Please Write Comment to Proceed");
            $("#txtcomment").focus();
        }
        else {
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
                dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
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

                    setTimeout(function () {
                        $("#btnAddComment").html('Send');
                        $("#btnAddComment").attr('disabled', false);
                        $("#btnAddComment").fadeIn();
                        $("#txtcomment").val('');

                        $("#txtcomment").attr('disabled', false);
                    }, 1000);
                },
                error: function (ts) {
                    alert('error' + ts.exceptionMessage);
                }
            });
        }


    });

    //$("#printclaim").click(function (e) {
       
       
    //    e.preventDefault();
    //    var claim = getParameterByName("Claim");
    //    var Type = claim[claim.length - 1];
    //    claim = claim.substr(0, claim.length - 1);

    //    if (claim != "") {
    //        var printWindow = window.open("/Home/PrintClaim/?claim=" + claim + "&Type=" + Type, 'Claim Details', 'left=20, top=20, width=1200, height=auto, toolbar=0, resizable=1,scrollbars=no');

    //        printWindow.addEventListener('load', function () {
    //            setTimeout(function () {
    //             //   printWindow.print();
    //              // printWindow.close(); Notification.success({ message: "PDF Downloaded .....", delay: 3000 });
    //            }, 1000);
    //        }, true);
    //    }
    //});

    //$("#exportclaim").click(function (e) {
    //    console.log('iam epxort ');
    //    e.preventDefault();
    //    var claim = getParameterByName("Claim");
    //    var Type = claim[claim.length - 1];
    //    claim = claim.substr(0, claim.length - 1);

    //    if (claim != "") {
    //        var printWindow = window.open("/Home/ExportClaim/?claim=" + claim + "&Type=" + Type, 'Claim Details', 'left=20, top=20, width=1200, height=auto, toolbar=0, resizable=1,scrollbars=no');

    //        printWindow.addEventListener('load', function () {
    //            setTimeout(function () {
    //              //  printWindow.print();
    //                //printWindow.close(); Notification.success({ message: "PDF Downloaded .....", delay: 3000 });
    //            }, 1000);
    //        }, true);
    //    }
    //});

    $(".allow_decimal").on("input", function (evt) {
        var self = $(this);
        self.val(self.val().replace(/[^0-9\.]/g, ''));
        if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
            evt.preventDefault();
        }
    });

    $("#btnUpload").click(function () {

        if ($("#logo").val() == "") {
            alert("Please Upload an Attachment to Proceed");
            $("#logo").focus();
        }
        else
        {
         //   var ext = $('#logo').val().split('.').pop().toLowerCase();

            var fi = document.getElementById('logo');

            // VALIDATE OR CHECK IF ANY FILE IS SELECTED.
            if (fi.files.length > 0) {

               // RUN A LOOP TO CHECK EACH SELECTED FILE.
                for (var i = 0; i <= fi.files.length - 1; i++)
                {
                    var ext = fi.files.item(i).name.split('.').pop().toLowerCase();      // THE NAME OF THE FILE.

                    if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'docx', 'xls', 'xlsx', 'pdf', 'zip', 'mp4', 'mkv', 'doc', 'flv', 'avi', 'mov', 'mpg', 'wmv', '3gp']) == -1)
                    {
                        alert('Invalid File Type! ' + fi.files.item(i).name + " is not supported                          (Allowed File Types are 'gif', 'png', 'jpg', 'jpeg', 'docx', 'xls', 'xlsx', 'pdf', 'zip', 'mp4', 'mkv', 'doc', 'flv', 'avi', 'mov', 'mpg', 'wmv', '3gp' )");
                        return false;
                    }

                    if (fi.files.item(i).size / 1000000 > 25)
                    {
                        alert(fi.files.item(i).name + " Exceeds the allowed size (25MB)");
                        return false;

                    }

                }
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

            var fi = document.getElementById('logo');
            for (var i = 0; i <= fi.files.length - 1; i++)
            {
                var ext = fi.files.item(i).name.split('.').pop().toLowerCase();      // THE NAME OF THE FILE.

                if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg', 'docx', 'xls', 'xlsx', 'pdf', 'zip', 'mp4', 'mkv', 'doc', 'flv', 'avi', 'mov', 'mpg', 'wmv', '3gp']) != -1)
                {
                    
                        if (fi.files.item(i).size / 1000000 <= 25)
                        {
                            insertForm.append(fi.files[i].name, fi.files[i]);
                        }
                    }
                   
               
            }

            $.ajax({
                url: $BaseApiUrl + "api/data/InsertAttachment",
                type: 'POST',
                dataType: "json", headers: { 'Access-Control-Allow-Origin': true },
                processData: false,
                contentType: false,
                data: insertForm,
                success: function (data) {
                    // To-do code if ajax request is successful

                    $("#attachmentbody").html('');

                    $.each(data.attachments, function (index, value) {
                        $("#attachmentbody").append('<tr><td><a href="' + $BaseApiUrl + '/UploadedFiles/' + value.at_FileName + '" target="_blank" >' + value.at_Name + ' </a></td><td style="width:20%;" >' + value.uploadedDate.substring(0, 10) + ' </td><td>' + value.uploadedByName + ' </td> </tr>');
                    });

                    $("#activitybody").html('');

                    $.each(data.activity, function (index, value) {
                        $("#activitybody").append('<tr><td style="float:left" >' + value.activityDescription + ' </td><td style="width:20%;" >' + value.activityDate + ' </td> <td> ' + value.activityStatus + '</td> <td>' + value.activityByName + ' </td></tr>');
                    });

                    setTimeout(function ()
                    {
                        $("#btnUpload").html('Upload');
                        $("#btnUpload").attr('disabled', false);
                        $("#btnUpload").fadeIn();
                        $("#logo").val('');

                        $("#logo").attr('disabled', false);
                    }, 1000);

                },
                error: function (ts) {
                    alert('error' + ts.exceptionMessage);
                }
            });
        }
    });

    $(document).on('keyup', 'textarea', function () {
        var max = 1000;
        var len = $(this).val().length;
        if (len >= max) {
            $('#cnt' + $(this).attr('id')).html('<span style="color:red"> you have reached the limit </span>');
            var str = $(this).val();
            $(this).val(str.substring(1, 1000));

        } else {
            var char = max - len;
            $('#cnt' + $(this).attr('id')).html('<span style="color:green">' + char + ' characters left</span>');
        }

    });


    function Loadtextlength()
    {

        $(document).on('keyup', 'textarea', function () {
            var max = 1000;
            var len = $(this).val().length;
            if (len >= max) {
                $('#cnt' + $(this).attr('id')).html('<span style="color:red"> you have reached the limit </span>');
                var str = $(this).val();
                $(this).val(str.substring(1, 1000));

            } else {
                var char = max - len;
                $('#cnt' + $(this).attr('id')).html('<span style="color:green">' + char + ' characters left</span>');
            }

        });
    }

    $(document).on('change', "#AuthoritiesContacted", function ()
    {
        if ($(this).is(":checked"))
        {
            if ($("#PoliceReportNumber").length > 0)
            {
                $("#PoliceReportNumber").closest('.form-group').show();
            }
        }
        else
        {
           $("#PoliceReportNumber").closest('.form-group').hide();
        }
    });


    // Activity Log

    $(document).on('click', ".viewlog", function () {
        var currrowId = $(this).closest('tr').children('td:eq(1)').text();
        var formtype = $(this).attr('data-formtype');
        var refid = $(this).attr('data-ref');

        //$("#printactivity").attr("data-ID", refid);
        //$("#printactivity").attr("data-formtype", formtype);
        $("#printactivity").attr("href", "/Hr/ExportActivity/?Id=" + refid + "&FormType=" + formtype + "&rid=" + currrowId);
        
       
        //$("#printactivity").attr("data-rowid ", currrowId);
        $(".titlemodal").html("Activity - " + currrowId);

        var insertForm = new FormData();
        insertForm.append('FormType', formtype);
        //  insertForm.append('action', action);
        insertForm.append('RecordId', refid);

        if ($("#divprocessingbtn").length > 0) {
            $("#divprocessingbtn").show();
        }


        $.ajax({
            url: $BaseApiUrl + "api/data/GetHrFormLog",
            type: 'POST',
            dataType: "JSON",
            processData: false,
            contentType: false,
            data: insertForm,
            success: function (data) {
                // To-do code if ajax request is successful

                $(".alert").html('');
                if ($("#divprocessingbtn").length > 0) {
                    $("#divprocessingbtn").hide();
                }

                if ($.fn.DataTable.isDataTable('#viewlog')) {
                    $('#viewlog').DataTable().destroy();
                }

                if ($.fn.DataTable.isDataTable('#signaturelog')) {
                    $('#signaturelog').DataTable().destroy();
                }


                $("#hractivitybody").html('');
              

                $.each(data.log, function (index, value) {
                    $("#hractivitybody").append('<tr><td style="float:left" >' + value.activitySubject + ' </td> <td> ' + value.activityDate.substring(0, 19) + '</td> <td style="float:left">' + value.activityByName + ' </td></tr>');
                });

                $("#hrsignmetadata").html('');
             
                $.each(data.metadata, function (index, value) {
                    $("#hrsignmetadata").append('<tr><td style="float:left" >' + value.action.replace("Email", "Signature") + ' </td> <td > ' + value.ip + '</td> <td> ' + value.browserinfo + '</td> <td>' + value.datetime.substring(0, 19) + ' </td></tr>');
                });


                $('#activitymodal').modal({
                    backdrop: 'static',
                    keyboard: false
                },'show');

                
                $('#viewlog').DataTable({
                    dom: 'frtip',
                    "aaSorting": [[1, "desc"]],
                    "aoColumnDefs": [
                        { 'bSortable': false, 'aTargets': [0] }
                    ]
                });

              
                $('#signaturelog').DataTable({
                    dom: 'frtip',
                    "aaSorting": [[3, "desc"]],
                    "aoColumnDefs": [
                        { 'bSortable': false, 'aTargets': [1] }
                    ]
                });
              
                if ((formtype == "NewHire" || formtype == "PayRoll") && data.rejection !="" && data.rejection != null)
                {
                    $(".rejectionblock").show();
                    $("#rjmetadata").html('');
                    if ($.fn.DataTable.isDataTable('#rejectionlog')) {
                        $('#rejectionlog').DataTable().destroy();
                    }
                    $("#rjmetadata").html('');
                    console.log(data.rejection);
                    $.each(data.rejection, function (index, value) {
                       
                        $("#rjmetadata").append('<tr><td style="float:left" >' + value.name + '</td> <td > ' + value.rejectionDesc + '</td> <td>' + value.datetime + ' </td></tr>');
                    });
                                      
                    $('#rejectionlog').DataTable({
                        dom: 'frt',
                        "aaSorting": [[3, "desc"]],
                        "aoColumnDefs": [
                            { 'bSortable': false, 'aTargets': [1] }
                        ]
                    });
                }
                else
                    $(".rejectionblock").hide();

            },
            error: function (ts) {
                alert('error' + ts.exceptionMessage);
            }
        });
    });


    $("#printactivity12").click(function (e) {

        e.preventDefault();
        var claim = $("#printactivity").attr('data-ID');
        var type = $("#printactivity").attr('data-formtype');
        var rid = $("#printactivity").attr('data-rowid');

        if (claim != "") {
            var printWindow = window.open("/Hr/ExportActivity/?Id="+claim+"&FormType=" + type+"rid="+rid, 'Acitivity Details', 'left=20, top=20, width=1200, height=auto, toolbar=0, resizable=1,scrollbars=no');

            printWindow.addEventListener('load', function () {
                setTimeout(function () {
                    printWindow.close();
                    //printWindow.close(); Notification.success({ message: "PDF Downloaded .....", delay: 3000 });
                }, 1000);
            }, true);
        }
    });


});
