$(function () {
    var dtfechaInicial = $('#dtBirthDay'); //our date input has the name "date"
    var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    dtfechaInicial.datepicker({
        format: 'dd/mm/yyyy',
        container: container,
        todayHighlight: true,
        autoclose: true
    });

    var dtAdmissionDate = $('#dtAdmissionDate'); //our date input has the name "date"
    var container1 = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    dtAdmissionDate.datepicker({
        format: 'dd/mm/yyyy',
        container: container1,
        todayHighlight: true,
        autoclose: true
    });



    var dtEntryDate = $('#dtEntryDate'); //our date input has the name "date"
    var container2 = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    dtEntryDate.datepicker({
        format: 'dd/mm/yyyy',
        container: container2,
        todayHighlight: true,
        autoclose: true
    });

    var dtRenewalDate = $('#dtRenewalDate'); //our date input has the name "date"
    var container3 = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    dtRenewalDate.datepicker({
        format: 'dd/mm/yyyy',
        container: container3,
        todayHighlight: true,
        autoclose: true
    });

    var dtLeavingDate = $('#dtLeavingDate'); //our date input has the name "date"
    var container4 = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
    dtLeavingDate.datepicker({
        format: 'dd/mm/yyyy',
        container: container4,
        todayHighlight: true,
        autoclose: true
    });


    $("#dtBirthDay").change(function () {
        $("#BirthDay").val($("#dtBirthDay").val());
    });

    $("#dtAdmissionDate").change(function () {
        $("#AdmissionDate").val($("#dtAdmissionDate").val());
    });

    $("#dtRenewalDate").change(function () {
        $("#RenewalDate").val($("#dtRenewalDate").val());
    });

    $("#dtEntryDate").change(function () {
        $("#EntryDate").val($("#dtEntryDate").val());
    });

    $("#dtLeavingDate").change(function () {
        //alert($("#dtEntryDate").val());
        $("#LeavingDate").val($("#dtLeavingDate").val());
    });

   
});
