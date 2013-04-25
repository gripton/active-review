$(document).ready(sizeContent);

var ContentMinHeight = parseInt($("#Top_Left").css("min-height").replace(/[^-\d\.]/g, '')) + $("#Bottom_Left").height();
    
$("#Content").css("min-height", ContentMinHeight + "px");

//Every resize of window
$(window).resize(sizeContent);

//Dynamically assign height
function sizeContent() {
    var newHeight = calculateNewContentHeight();
    $("#Content").css("height", newHeight + "px");
    
    var resizableContentHeight = calculateResizableHeight("Left");
    $("#Top_Left").css("height", resizableContentHeight+ "px");

    var resizableContentHeight = calculateResizableHeight("Right");
    $("#Top_Right").css("height", resizableContentHeight + "px");

    setScrollDisplay("Right");
    setScrollDisplay("Left");
}

function setScrollDisplay(postFix) {
    var resizableContentHeight = calculateResizableHeight(postFix);

    var element = $("#Top_" + postFix)

    if (resizableContentHeight < element[0].scrollHeight)
    {
        element.css("overflow-y", "scroll");
    }
    else
    {
        element.css("overflow-y", "hidden");
    }
}

function calculateNewContentHeight() {
    return $(window).height() - $("#Header").height() - $("#Footer").height() - 90;
}

function calculateResizableHeight(postFix) {
    var newHeight = calculateNewContentHeight();
    return newHeight - $("#Bottom_" + postFix).height();
}

function setScrollableToBottom(postFix) {
    var div = $("#Top_" + postFix);
    div[0].scrollTop = div[0].scrollHeight;
}