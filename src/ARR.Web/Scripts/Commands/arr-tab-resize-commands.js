$(document).ready(sizeContent);

var ContentMinHeight = parseInt($("#Top_Left").css("min-height").replace(/[^-\d\.]/g, '')) + $("#Bottom_Left").height();
$("#Content").css("min-height", ContentMinHeight + "px");

//Every resize of window
$(window).resize(sizeContent);

//Dynamically assign height
function sizeContent() {
    var newHeight = calculateNewContentHeight();
    $("#Content").css("height", newHeight + "px");

    $("#TabContent").css("height", newHeight -110 + "px");
    var resizableContentHeight = calculateResizableHeight("Left");
    $("#Top_Left").css("height", resizableContentHeight + "px");
    setScrollDisplay("Left");

    if ($("#Top_Right").val() != null) {
        var resizableContentHeight = calculateResizableHeight("Right");
        $("#Top_Right").css("height", resizableContentHeight + "px");
        setScrollDisplay("Right");
    }
}

function setScrollDisplay(postFix) {
    var resizableContentHeight = calculateResizableHeight(postFix);
    var element = $("#Top_" + postFix);

    if (element[0] !== undefined) {
        if (resizableContentHeight < element[0].scrollHeight) {
            element.css("overflow-y", "scroll");
        }
        else {
            element.css("overflow-y", "hidden");
        }
    }
}

function calculateNewContentHeight() {
    return $(window).height() - $("#Header").outerHeight() - $("#Footer").outerHeight();
}

function calculateResizableHeight(postFix) {
    var newHeight = calculateNewContentHeight();
    return newHeight - $("#Bottom_" + postFix).outerHeight() - 125;
}

function setScrollableToBottom(postFix) {
    var div = $("#Top_" + postFix);
    div[0].scrollTop = div[0].scrollHeight;
}