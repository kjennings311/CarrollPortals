var $BaseApiUrl = "http://localhost:1002/"; 
// var $BaseApiUrl = "http://aspnet.carrollaccess.net:1002/";
//49786/";
//Token and UserOject are global variables can be used here.
$(document).ready(function () {
    
  
});

var imagebase64 = "";

function encodeImageFileAsURL(element) {

      var filesSelected = element.files;
    if (filesSelected.length > 0) {
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

function BindElements() {
    $form = $('.dynamicForm');
    $('.dynamicForm #savechanges').click(function () {
        $('.success-message').hide();
        $('.failure-message').hide();
    //    if (!CheckFormErrors($form)) {
            var $this = $(this);
        var formUrl = "api/form/GenerateForm/" + $('.dynamicForm #savechanges').attr("formname");

        // if form is user then check if user exists with email id or not

        alert($('.dynamicForm #savechanges').attr("formname"));

        if ($('.dynamicForm #savechanges').attr("formname") == "user") {

       
        $.ajax({
            type: "GET",
            url: $BaseApiUrl + "api/user/checkifuserexists/",
            data:"id="+$("#UserEmail").val(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                alert('success');
              
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
                dataType: "json",
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
                            case "Password":                         
                            case "Select":                           
                            case "Person":    
                                $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                break;
                            case "Check":
                                if ($('#' + $fields[i]["fieldName"]).prop('checked'))
                                {
                                    $fields[i]["fieldValue"] = true;
                                    console.log("check field value" + true);
                                }
                                else
                                {
                                    $fields[i]["fieldValue"] = false;
                                }
                                break;
                            case "File":
                               // console.log('before assign ' + $fields[i]["fieldValue"] + " " + imagebase64);
                                $fields[i]["fieldValue"] = imagebase64;

                               // console.log('after assign ' + $fields[i]["fieldValue"] + " " + imagebase64);
                                break;
                            case "Hidden":
                                // Let's populate Created and CreatedName'
                                switch ($fields[i]["fieldName"].toLowerCase()) {
                                
                                    case "createdby":
                                        // if there is already a value read it and load if empty load current user
                                        if ($('#' + $fields[i]["fieldName"]).val() != "") {
                                            $fields[i]["fieldValue"] = User.UserId;
                                        } else {
                                            $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                        }
                                        
                                        break;
                                    case "createdbyname":
                                        if ($('#' + $fields[i]["fieldName"]).val() != "") {
                                            $fields[i]["fieldValue"] = User.FirstName + " " + User.LastName;
                                        } else {
                                            $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                        }
                                        
                                        break;
                                    case "UserPhoto":
                                        $fields[i]["fieldValue"] = imagebase64;
                                    default:
                                        $fields[i]["fieldValue"] = $('#' + $fields[i]["fieldName"]).val();
                                        break;
                                }
                                break;
                            default:

                                break;
                                
                        }
                        
                    }
                    
                    // Let's set user id field so we can use that as a reference
                    //$data["userId"] = User.UserId + "~" + User.FirstName + " " + User.LastName
                    //  alert(JSON.stringify($data));
                    // now that we have updated Form record.. Let's pass it back to server with all  data for validation..

                    /**************************************************************************************/
                    //
                    $.ajax({
                        type: "POST",
                        url: $BaseApiUrl + "api/Form/CreateUpdateFormData/" + $('.dynamicForm #savechanges').attr("formname"),
                        data: JSON.stringify($data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
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
                                    $form.find('#successMessage').html("Record successfully updated!");
                                    $form.find('.success-message').show('slow');
                                    ScrollToElement($form.find('.success-message'));
                                    // go back to previous screen after 5 seconds
                                  //  setTimeout(location.reload(), 5000);
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
            //                    dataType: "json",
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

function getForm(FormName, RecordId)
{
    var formUrl = "";
    var TXT_ERROR = " <div class=\"alert alert-danger alert-dismissable failure-message\" style=\"display:none\"><div id=\"failureMessage\">there was an error!</div> </div>";
    var TXT_SUCCESS = "<div class=\"alert alert-success alert-dismissable success-message\" style=\"display:none\"><div id=\"successMessage\"></div> </div>";
    var $formBegin = '<form  class="form-horizontal CustomForm">';
    var $formEnd = '</form>';
    var $line = '<div class="hr-line-dashed"></div>';
    var $textbox = '<div class="form-group"><label class="col-sm-2 control-label"> {0}</label ><div class="col-sm-10"><input maxlength="100" type="text" validationformat="{1}" class="form-control {2}" id="{3}" {4} value="{5}"></div></div>';
    var $passbox = '<div class="form-group"><label class="col-sm-2 control-label"> {0}</label ><div class="col-sm-10"><input maxlength="100" type="password" validationformat="{1}" class="form-control {2}" id="{3}" {4} value="{5}"></div></div>';
    var $filebox = '<div class="form-group"><label class="col-sm-2 control-label"> {0}</label ><div class="col-sm-10"><input maxlength="100" type="file" validationformat="{1}" onchange="encodeImageFileAsURL(this);" class="form-control {2}" id="{3}" {4} value="{5}"></div> <div id="imgTest" style="background: black;clear: both;margin-left:30%;width:300px;"> </div></div>';
    var $hiddenField = '<input type="hidden" id="{0}" value="{1}"/>';
    var $checkbox = ' <div class="form-group"><label class="col-sm-2 control-label">{0}</label><div class="col-sm-10"><div class="i-checks"><label> <input class="form-control" type="checkbox" id="{1}" value="1"  {2}> <i></i> {0} </label></div></div></div>';
    var $person = '<div class="form-group"><label class="col-sm-2 control-label"> {0}</label ><div class="col-sm-10"><input type="text" validationformat="{1}" class="form-control {2}" id="{3}" {4}></div></div>';
    var $savebuttons = '  <div class="hr-line-dashed"></div>'
        + TXT_SUCCESS + TXT_ERROR
        + '<div class="form-group" >'
        + '<div class="col-sm-4 col-sm-offset-2">'
        + '<a class="btn btn-white" href="javascript:location.reload();">Cancel</a>&nbsp;'
        + '<a id="savechanges" class="btn btn-primary btn-add" href="javascript:void(0);" formname="' + FormName + '">Save changes</button>'
        + '</div></div >';
    var $select = '<div class="form-group">'
        + '<label class="col-sm-2 control-label">{0}</label>'
        + '<div class="col-sm-10">'
        + '<select data-placeholder="Select option" class="form-control {1}" tabindex="2" id="{2}" >'
        + '<option value="">Select</option>'
        + '</select>'
        + '</div>'
        + '</div>';
    if (RecordId == '') formUrl = "api/form/GenerateForm/" + FormName;
    else formUrl = "api/form/GenerateEditForm?entitytype=" + FormName + "&RECORDID=" + RecordId;
        
    $.ajax({
        type: "get",
        dataType: "json",
        url: $BaseApiUrl + formUrl ,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + Token);

        },
        success: function (data) {
            var $FormElements = $formBegin;
            var $data = data;

            var $fields = $data["formFields"];
            for (var i = 0; i < $fields.length; i++) {
               // alert($fields[i]["fieldLabel"]+ " " + $fields[i]["fieldType"]);
                // Build the form elements here
                switch ($fields[i]["fieldType"]) {
                    case "Text":

                        var $req = $fields[i]["required"];            
                        var $datamask = "";
                       
                        $FormElements += format($textbox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"]);
                        break;
                    case "Password":

                        var $req = $fields[i]["required"];
                        var $datamask = "";

                        $FormElements += format($passbox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"]);
                        break;
                    case "File":

                        var $req = $fields[i]["required"];
                        var $datamask = "";

                        $FormElements += format($filebox, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required" : "", $fields[i]["fieldName"], $datamask, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"]);
                        break;
                    case "Check":
                      //  var $req = $fields[i]["required"];
                        var checkedtext = "";
                        var checked = ($fields[i]["fieldValue"] == null) ? false : $fields[i]["fieldValue"];
                        if (checked) checkedtext = "checked=checked";
                        else checkedtext = "";
                        $FormElements += format($checkbox, $fields[i]["fieldLabel"], $fields[i]["fieldName"], checkedtext);
                        

                        break;
                    case "Select":
                        var $req = $fields[i]["required"];
                        var dataurl = $fields[i]["dataLoadUrl"];

                        $FormElements += format($select, $fields[i]["fieldLabel"], ($req) ? "required" : "", $fields[i]["fieldName"]);
                        //Let's Load select options from websevice
                        LoadOptions($fields[i]["fieldName"],dataurl, ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"]);
                        break;
                    case "Person":
                        var $req = $fields[i]["required"];
                        var personId = ($fields[i]["fieldValue"] == null) ? "" : $fields[i]["fieldValue"];
                        var val = 'Value="' + personId + '"';
                        $FormElements += format($person, $fields[i]["fieldLabel"], $fields[i]["fieldValidationType"], ($req) ? "required tokenInput" : "tokenInput", $fields[i]["fieldName"], val);
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
function LoadOptions(fieldId, DataLoadUrl, value) {
    var options = "";
    var selected = "";
    console.log(data);
    $.get($BaseApiUrl + DataLoadUrl, function (data) {
        for (var i = 0; i < data.length - 1; i++){
            if (data[i]["key"] == value) selected = "selected=selected";
            options += "<option value=\"" + data[i]["key"] + "\"" + selected + ">" + data[i]["value"] + "</option>";
            selected = "";
    
        }
        // now let's load options into select box
        $('#' + fieldId).append(options);
    });
}

function ToggleAdd(formaname) {
    // these controls are in properties.aspx page
    $('.RowsContatiner').toggle('slow', function () {
        $('.AddEditContainer').toggle('slow', function () {
            if ($(this).is(":visible")) { getForm(formaname,''); }
        });
    });
   
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

function LoadData(formName) {
   
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",
                url: $BaseApiUrl + "api/data/getrecords?entitytype=" + formName,
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
                   
                   // var tbody = $("<tbody />"), tr;
                   //// var $data = data;
                   // $.each(data, function (_, obj) {
                   //     tr = $("<tr />");
                   //     $.each(obj, function (_, text) {
                   //         tr.append("<td>" + text + "</td>")
                   //     });
                   //     tr.appendTo(tbody);
                   // });
                   // tbody.appendTo(".dtData");

                   
                   
                    var my_columns = [];

                    $.each(data[0], function (key, value) {
                        var my_item = {};
                        my_item.data = key;
                        my_item.title = splitCamelCase(key).toUpperCase();
                        my_columns.push(my_item);
                    });

                   

                   
                    //alert(JSON.stringify(my_columns));
                    $('.dtData').DataTable({
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
                        "columns": my_columns
                    }).columns.adjust();

                    //  $('.ibox').children('.ibox-content').toggleClass('sk-loading');

                }
            });
        }
    );
}
// takes thisIsTest converts to this Is Test
function splitCamelCase(s) {
    return s.split(/(?=[A-Z])/).join(' ');
}

// this function let's you enable disable add delete buttons'
function HandleRowClick(obj) {
    var $this = jQuery(obj);
 
    if (!$this.hasClass('selected')) {
    //    $this.removeClass('selected');
        $('.btnDelete').css("color", "red");
        $('.btnDelete').removeAttr("disabled");
        $('.btnDelete').attr("itemId", $this.attr("id"));
        $('.btnDelete').attr("itemType", $this.attr("itemType"));
        $('.btnEdit').css("color", "green");
        $('.btnEdit').removeAttr("disabled");
        $('.btnEdit').attr("itemId", $this.attr("id"));
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
                    dataType: "json",
                    url: $BaseApiUrl + "api/data/deleterecord/entitytype=" + ItemType + "&recordid=" + idToDelete,
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
        $('.btnEdit').css("color", "#ccc");
        $('.btnEdit').attr("disabled", "disabled");
        $('.btnEdit').removeAttr("itemId");
        $('.btnDelete').unbind('click').bind('click', function () {

            return false;
        });
    }
}


// called from Contacts page
function LoadContacts() {
    $.when(GetToken()).then(
        function () {
            $.ajax({
                type: "get",
                dataType: "json",
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
                dataType: "json",
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
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'contactId',
                                render: function (data) {
                                    if (data != null) {
                                        //select CONCAT('{"name":"', FirstName, ' ', LastName, '","id":"', ContactId, '"}') from dbo.Contacts where ContactId = [CarrollForms].[dbo].[Properties].VicePresident) as VicePresident
                                        $.get($BaseApiUrl + "api/contact/GetContact?contactId=" + data, function (result) {
                                            return '<a href="/contact?id=' + result["contactId"] + '">' + result["firstName"] + ' ' + result["lastName"] + '</a>';
                                        });
                                        return data;
                                       
                                    } else return '';
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
                dataType: "json",
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
                            {
                                //{"name":"Scott Gilpatrick","id":"71ED1F0C-EE49-4FE5-B379-859CAD723DA2"}
                                data: 'equityPartner',
                                render: function (data, type, row) {
                                    if (data != null) {
                                        var result = JSON.parse(data);
                                        return '<a href="/equitypartner?id=' + result["id"] + '">' + result["name"] + '</a>';
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
                dataType: "json",
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

                        columns: [
                            //{
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
                dataType: "json",
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
                            { "data": "roleName", "name": "lastName", "autoWidth": false }
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
            $(":input").inputmask();
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