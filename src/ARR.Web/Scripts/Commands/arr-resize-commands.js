$(document).ready(sizeContent);

var ContentMinHeight = parseInt($("#Top_Left").css("min-height").replace(/[^-\d\.]/g, '')) + $("#Bottom_Left").height();
$("#Content").css("min-height", ContentMinHeight + "px");

//Every resize of window
$(window).resize(sizeContent);

//Dynamically assign height
function sizeContent() {
    var newHeight = calculateNewContentHeight() + 20;
    $("#Content").css("height", newHeight + "px");

    var resizableContentHeight = calculateResizableHeight("Left") - calculatePadding("#Top_Left");
    $("#Top_Left").css("height", resizableContentHeight + "px");
    setScrollDisplay("Left");

    if ($("#Top_Right").val() != null) {
        resizableContentHeight = calculateResizableHeight("Right");
        $("#Top_Right").css("height", resizableContentHeight + "px") - calculatePadding("#Top_Right");
        setScrollDisplay("Right");
    }
}

function setScrollDisplay(postFix) {
    var resizableContentHeight = calculateResizableHeight(postFix);
    var element = $("#Top_" + postFix);
    if (element[0] !== undefined) {
        element.tinyscrollbar();
        if (resizableContentHeight < element[0].scrollHeight) {
            element.css("overflow-y", "scroll");
        }
        else {
            element.css("overflow-y", "hidden");
        }
        element.tinyscrollbar_update();
    }
}

function calculatePadding(id) {
    return $(id).outerHeight() - $(id).height();
}

/*Calculates what the height of the content should be. */
function calculateNewContentHeight() {
    return $(window).height() - $("#Header").outerHeight() - $("#Footer").outerHeight();
}

function calculateResizableHeight(postFix) {
    var newHeight = calculateNewContentHeight();
    return newHeight - $("#Bottom_" + postFix).outerHeight() - calculatePadding("#Content") - $("#Title").outerHeight() - calculatePadding($("#Bottom_" + postFix)) - 26;
}

function setScrollableToBottom(postFix) {
    var div = $("#Top_" + postFix);
    div[0].scrollTop = div[0].scrollHeight;
}