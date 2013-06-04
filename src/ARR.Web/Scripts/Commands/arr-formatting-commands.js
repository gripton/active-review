ko.bindingHandlers.dateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor()
        var valueUnwrapped = new Date(ko.utils.unwrapObservable(value));

        $(element).text(valueUnwrapped.toDateString() + ' ' + valueUnwrapped.toLocaleTimeString());
    }
}