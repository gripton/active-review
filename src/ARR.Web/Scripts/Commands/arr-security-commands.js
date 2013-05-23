function loggedInUser() {
    return $.cookie('arr_account');
}

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

