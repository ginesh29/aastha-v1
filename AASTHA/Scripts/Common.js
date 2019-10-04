//**********Exclude Validation**************
$.validator.setDefaults({ ignore: '' });
$.validator.unobtrusive.parse($("form"));
//**********JQ Grid**************
var JQ_sopt_int = JSON.parse('{"sopt": ["eq","ne","le","lt","gt","ge"]}');
var JQ_sopt_string = JSON.parse('{"sopt": ["cn","eq"]}');
var JQ_sopt_bool = JSON.parse('{"sopt": ["eq","ne"], "value": ":All;True:Yes;False:No"}');
var JQ_sopt_status = JSON.parse('{"sopt": ["eq","ne"], "value": ":All;Active:Active;InActive:InActive"}');
var JQ_sopt_date = JSON.parse('{"sopt": ["eq","ge","le"]}');

var JQ_DateFormat = 'm/d/Y';

function ReloadGrid(grid) {
    $("#" + grid).trigger('reloadGrid');
}
//**********JQ Grid**************

function SuccessMessage(data) {
    if (data == null || data.length <= 0)
        return;

    $('body').showMessage({
        'thisMessage': [data],
        className: 'success',
        displayNavigation: false,
        location: 'top',
        autoClose: true
    });
}

function ErrorMessage(data) {
    if (data == null || data.length <= 0)
        return;

    $('body').showMessage({
        'thisMessage': [data],
        className: 'fail',
        displayNavigation: false,
        location: 'top',
        useEsc: false,
        autoClose: true
    });
}
function displaysuccessmsg(data) {
    $("#successmsg").text(data);
    $("#errormsg").hide();
    $("#successmsg").show().delay(2000).hide("200000");
}

function displayerrormsg(data) {
    $("#errormsg").text(data);
    $("#successmsg").hide();
    $("#errormsg").show().delay(2000).hide("200000");
}

function blockui() {
    $.blockUI({ message: $('#loader') });
}
function unblockui() {
    $.unblockUI();
}
function blockui_div() {
    $('div.blockui_div').block({
        message: '<h3>Please Wait...</h3>'
    });
}
function unblockui_div() {
    $('div.blockui_div').unblock();
}
function blockui_form() {
    $('div.blockui_form').block({
        message: '<h5>Please Wait...</h5>'
    });
}
function unblockui_form() {
    $('div.blockui_div').unblock();
}
function blockui_grid() {
    $('div.blockui_grid').block({
        message: '<h3>Please Wait...</h3>'
    });
}
function blockui_leftgrid() {
    $('div.blockui_leftgrid').block({
        message: '<h3>Please Wait...</h3>'
    });
}
function unblockui_leftgrid() {
    $('div.blockui_leftgrid').unblock();
}
function blockui_rightgrid() {
    $('div.blockui_rightgrid').block({
        message: '<h3>Please Wait...</h3>'
    });
}
function unblockui_rightgrid() {
    $('div.blockui_rightgrid').unblock();
}
function OnSuccess() {
    $("input[type='text']").val("");
    $("select").val("");
}
function unblockui_grid() {
    $('div.blockui_grid').unblock();
}
function blockui_modal() {
    $('div.blockui_div modal').block({
        message: $('#loader_div')
    });
}
function unblockui_modal() {
    $('div.blockui_div modal').unblock();
}

$(document).ready(function () {
    $("a").click(function () {
        var href = $(this).attr('href');
        if (href *= "#" || href == null) {
            blockui();
        }
        else {
            unblockui();
        }
    });
    $(".Capitalize").keyup(function () {
        var caps = $(this).val().charAt(0).toUpperCase() + $(this).val().slice(1);
        $(this).val(caps);
    })
});
window.onbeforeunload = function () {
    blockui();
}
//$(document).on('submit', 'form', function () {
//    blockui();
//});
function resetValidation() {
    //Removes validation from input-fields
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');

    //Removes validation summary
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');

}
function resetform() {
    $('form').each(function () {
        this.reset();
    });
}
function resetselect2() {
    $('#Patient_Id').select2("val", "");
}
function resetselectboxit() {
    $("select.selectboxit").selectBoxIt().selectBoxIt('selectOption', 0);
}
function resetmultiselect() {
    $("select.multiselect").multiselect("deselectAll", false);
    $("select.multiselect").multiselect('updateButtonText');
}

function savemsg(type) {
    setTimeout(function () {
        var opts = {
            "closeButton": true,
            "debug": false,
            "positionClas": "toast-top-right",
            "toastClass": "green",
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr.success("Record Has Been Saved Successfully", type, opts);
    }, 1000);
}

function deletemsg(type) {
    setTimeout(function () {
        var opts = {
            "closeButton": true,
            "debug": false,
            "positionClas": "toast-top-right",
            "toastClass": "red",
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr.success("Record Has Been Deleted Successfully", type, opts);
    }, 1000);
}

function datemask() {
    $(".datepicker").inputmask("d/m/y", { "placeholder": "dd/mm/yyyy" });
}
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};