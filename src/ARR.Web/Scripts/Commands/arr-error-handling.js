function setupErrorHandling(viewModel) {
    $.ajaxSetup({
        error: function (jqXhr, exception) {
            if (jqXhr.status === 0) {
                displayMessage('Error: Could not connect to server', true);
            }
            else if (jqXhr.status == 401) {
                displayMessage('Error: Unauthorized', true);
            }
            else if (jqXhr.status == 403) {
                var obj = JSON.parse(jqXhr.responseText);
                displayMessage('Error: Current state of Review Session does not allow this action: ' + obj.ExceptionMessage, true);
            }
            else if (jqXhr.status == 404) {
                //displayMessage('Error: Could not access resource', true);
                displayMessage('Error: Review Session identifier not found', true);
            }
            else if (jqXhr.status == 500) {
                var obj = JSON.parse(jqXhr.responseText);
                displayMessage(obj.ExceptionMessage, true);
            }
            else if (exception === 'parsererror') {
                displayMessage('Requested JSON parse failed.', true);
            }
            else if (exception === 'timeout') {
                displayMessage('Time out error.', true);
            }
            else if (exception === 'abort') {
                displayMessage('Ajax request aborted.', true);
            }
            else {
                alert('Uncaught Error.\n' + jqXhr.responseText);
            }

            // Make sure if the ajax fails, we turn off the screen mask
            if (viewModel.processingViewModel) {
                viewModel.processingViewModel.turnOffProcessing();
            }
        }
    });
};

function displayMessage(message, isError) {
    $("#Message").removeClass("alert-error");
    $("#Message").removeClass("alert-success");
    if (isError) {
        $("#Message").addClass("alert-error");
    } else {
        $("#Message").addClass("alert-success");
    }
    $("#MessageText").text(message);
    $("#Message").addClass("show");
    setTimeout(function () {
        hideMessage();
    }, 10000);
}

function hideMessage() {
    $("#Message").removeClass("show");
}

