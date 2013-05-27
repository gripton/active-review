function loggedInUser() {
    return $.cookie('arr_account');
}

/* Sets up the standard ajax header that we will send to the service */
$(function () {
    $.ajaxSetup({ 
        headers:
        {
            "Authorization": getSessionAuthorization()
        }
    });
});

function getSessionAuthorization() {
    return getAuthorizationHeader(loggedInUser(), "");
}

/* Currently we pass the header bas64 - future security enhancements
   Would likely have this kind of logic moved server side with some sort
   of claims provder */
function getAuthorizationHeader(username, password) {
    "use strict";
    var authType;

    if (password == "") {
        authType = $.base64.encode(username);
    }
    else {
        var up = $.base64.encode(username + ":" + password);
        authType = "Basic " + up;
    };
    return authType;
};

