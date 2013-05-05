ko.bindingHandlers.loadingWhen = {
    init: function () {
        $("#Loader").hide();
        $("#PageMask").hide();
    },
    update: function (element, valueAccessor) {
        var isLoading = ko.utils.unwrapObservable(valueAccessor());

        if (isLoading) {
            $("#Loader").show();
            $("#PageMask").show();
        }
        else {
            $("#Loader").hide();
            $("#PageMask").hide();
        }
    }
};