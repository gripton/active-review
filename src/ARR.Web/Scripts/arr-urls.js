var ARR_API_PORT = 49882;

function getArrApiUrl(action) {
    return "http://localhost:" + ARR_API_PORT + "/api/" + action + "?callback=?";
}

function getArrApiUrlPost(action) {
    return "http://localhost:" + ARR_API_PORT + "/api/" + action;
}