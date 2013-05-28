ko.bindingHandlers.loadingWhen = {
    init: function (element) {
        $("#"+element.id).hide();
    },
    update: function (element, valueAccessor) {
        var isLoading = ko.utils.unwrapObservable(valueAccessor());
        if (isLoading) {
            $("#" + element.id).show();
        }
        else {
            $("#" + element.id).hide();
        }
    }
};