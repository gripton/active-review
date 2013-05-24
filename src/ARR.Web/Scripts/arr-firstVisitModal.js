$(document).ready(function () {
    if ($.cookie("arr_firstVisit") == null) {
        $("#modalIntro").modal('show');
        $.cookie('arr_firstVisit', true, { expires: 9999 });
    }
});
