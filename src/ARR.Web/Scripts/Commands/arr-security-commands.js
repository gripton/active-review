$(function () {
    $.ajaxSetup({ 
        headers:
        {
            "Authorization": getSessionAuthorization()
        }
    });
});

function getSessionAuthorization() {
    //TODO: load the username from the cookies
    return getAuthorizationHeader("thorfio","");
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

