if (!Modernizr.inputtypes.date) {
    $(function () {
        $(".datecontrol").datepicker();
        locale = "es";
    });
}