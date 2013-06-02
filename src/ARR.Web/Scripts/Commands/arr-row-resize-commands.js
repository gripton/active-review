var ContentMinHeight = parseInt($("#TopRow").css("min-height").replace(/[^-\d\.]/g, '')) + $("#Bottom_Left").height();
$("#Content").css("min-height", ContentMinHeight + "px");

//Every resize of window
$(window).resize(sizeContent);

//Dynamically assign height
function sizeContent() {
    var newHeight = calculateTopRowHeight();
    //alert(newHeight);
    $("#TopRow").css("height", newHeight + "px");
    setScrollDisplay("#ScrollArea");
}

function setScrollDisplay(id) {
    $("#ScrollArea").css("height", calculateScrollHeight() + "px");
    var element = $(id);
    if (element[0] !== undefined) {
        element.tinyscrollbar();
        element.css("overflow-y", "hidden");
        element.tinyscrollbar_update();
    }
}

function calculatePadding(id) {
    return $(id).outerHeight(true) - $(id).height();
}

/*Calculates what the height of the content should be. */
function calculateTopRowHeight() {
    return $(window).height() - $("#Header").outerHeight(true) - $("#BottomRow").outerHeight(true) - calculatePadding("#TopRow");
}

function calculateScrollHeight() {
    return  $("#TopRow").height() - $("#Title").outerHeight(true);
}


function setScrollableToBottom() {
    var div = $("#TopRow");
    div[0].scrollTop = div[0].scrollHeight;
}