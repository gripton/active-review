function triggerModalOnFirstVisit(cookieName, modalName) {
    if ($.cookie(cookieName) == null) {
        $(modalName).modal('show');
        $.cookie(cookieName, true, { expires: 9999, path: "/" });
    }

}