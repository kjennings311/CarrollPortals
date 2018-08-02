//var $BaseApiUrl = "http://localhost:1002/"; 

 var $BaseApiUrl = "http://aspnet.carrollaccess.net:1002/";

function LoadAllForms()
{
    // clear output and display loading spinner...
   // $('.allforms').html('<img src=\"/img/ajax-loader.gif\" alt=\"Loading...\" class=\"ajax-loader\"/>').fadeIn('slow');;



    $.ajax({
        type: "GET",
        url: $BaseApiUrl + "api/mongoservice/GetAllForms",       
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data)
        {
          
            $(".allforms").html('');

            $.each(data, function (index, value)
            {               
                $(".allforms").append('<tr><td> ' + value[1].value + ' </td><td> ' + value[4].value.substring(0, 10) + ' </td><td> ' + value[3].value + ' </td><td style="width:20%;" ><a href="javascript:void(0);" class="form-edit btn btn-small btn-primary" data-form="' + value[0].value + '" ><i class="fa fa-edit"> </i> </a> <a href="return false;" class="del-form btn btn-small btn-danger" data-form="' + value[0].value +'" ><i class="fa  fa-trash"> </i> </a> </td> </tr>');
            });
            //for (var i in data) {
            //    $(".allforms").append('<tr formname="' + data[i].formName + '" formnameplural="' + data[i].formNamePlural + '" formid="' + data[i].id + '"><td> ' + data[i].formName + ' </td><td> ' + data[i].createdBy + ' </td><td> ' + data[i].createdDate.substring(0, 10) + ' </td><td style="width:20%;" ><a href="javascript:void(0);" class="form-edit btn btn-small btn-primary edit" data-form="' + data[i].id + '" ><i class="fa fa-edit"> </i> </a> <a href="return false;" class="del-form btn btn-small btn-danger delete" data-form="' + data[i].id + '" ><i class="fa  fa-trash"> </i> </a> </td> </tr>');
               
            //}
           // CreateFormsRows(data);

        }
    });
}

function ClearForm($form) {
    $form.each(function () {
        this.reset();
    });
    $('.success-message').hide();
    $('.failure-message').hide();
}

//function CreateFormsRows(data) {
//    for (var i in data) {
//        $(".allforms").append('<tr formname="' + data[i].formName + '" formnameplural="' + data[i].formNamePlural + '" formid="' + data[i].id + '"><td> ' + data[i].formName + ' </td><td> ' + data[i].createdBy + ' </td><td> ' + data[i].createdDate.substring(0, 10) + ' </td><td style="width:20%;" ><a href="javascript:void(0);" class="form-edit btn btn-small btn-primary edit" data-form="' + data[i].id + '" ><i class="fa fa-edit"> </i> </a> <a href="return false;" class="del-form btn btn-small btn-danger delete" data-form="' + data[i].id + '" ><i class="fa  fa-trash"> </i> </a> </td> </tr>');

//    }
//}

function CreateEditForm()
{
   
    $form = $('#frmForm');
    $form.find('.success-message').hide();
    $form.find('.failure-message').hide();

    var hasErrors = CheckFormErrors($form);
    if (!hasErrors) {
        //var jsonObject = {};

        //jsonObject["id"] = "";//($('#frmForm #FormId').val() == -1) ? "" : $('#frmForm #FormId').val();
        //jsonObject["formName"] = $('#frmForm #FormName').val();
        //jsonObject["formNamePlural"] = $('#frmForm #FormNamePlural').val();
        //jsonObject["createdBy"] = "";
        //jsonObject["createdDate"] = "";
        //jsonObject["modifiedBy"] = "";
        //jsonObject["modifiedDate"] = "";
        var jsonArray = [];
        var jsonObject = {};
        jsonObject["name"] = "FormName";
        jsonObject["value"] = $('#frmForm #FormName').val();
        jsonArray.push(jsonObject);
        var jsonObject1 = {};
        jsonObject1["name"] = "FormNamePlural";
        jsonObject1["value"] = $('#frmForm #FormNamePlural').val();
        jsonArray.push(jsonObject1);

        //jsonArray.push('{ "name": "FormName", "value": "' + $('#frmForm #FormName').val()  +  ' " }');
        //jsonArray.push('{ "name": "FormNamePlural", "value": "' + $('#frmForm #FormNamePlural').val() + '" }');
       
        

        $.ajax({
            type: "POST",
            url: $BaseApiUrl + "api/mongoservice/CEFORM",
            data: JSON.stringify(jsonArray),
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            dataType: "json",
            async: false,
            statusCode: {
                200: function (data) {
                    // if we are here it means all good.. 
                    alert(data.responseText);
                },
                201: function (data) {
                    // if we are here it means all good.. 
                   // if (data.responseText == "OK") {
                        ClearForm($form);
                        $('#frmForm .successMessage').html("Form created/updated");
                        $form.find('.success-message').show('slow');
                        $(".allforms").html('');

                        $.each(data, function (index, value) {
                            $(".allforms").append('<tr><td> ' + value[1].value + ' </td><td> ' + value[4].value.substring(0, 10) + ' </td><td> ' + value[3].value + ' </td><td style="width:20%;" ><a href="return false;" class="form-edit btn btn-small btn-primary" data-form="' + value[0].value + '" ><i class="fa fa-edit"> </i> </a> <a href="return false;" class="form-del btn btn-small btn-danger" data-form="' + value[0].value + '" ><i class="fa  fa-trash"> </i> </a> </td> </tr>');
                        });
                       // CreateFormsRows(data);
                  //  }
                },

                success: function (data) {



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
                            $this.parent().addClass('has-error');
                        }
                        $('#frmForm .failureMessage').html(_err + "</ul>");
                        $form.find('.failure-message').show('slow');



                    } catch (err) { alert(err.message); }

                }
            } // status code
        });
    }
    return false;
}


$(document).ready(function () {

    $(document).on('click', '.form-edit', function (e) {  });
    
    $(document).on('click', '.form-del', function (e) { });
});