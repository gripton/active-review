ko.bindingHandlers.tinymce = {
    init: function (element, valueAccessor, allBindingsAccessor, context) {
        var options = allBindingsAccessor().tinymceOptions || {};
        var modelValue = valueAccessor();
        var value = ko.utils.unwrapObservable(valueAccessor());
        var el = $(element)

        //handle edits made in the editor. Updates after an undo point is reached.
        options.setup = function (ed) {
            ed.onChange.add(function (ed, l) {
                if (ko.isWriteableObservable(modelValue)) {
                    modelValue(l.content);
                }
            });
        };

        //handle destroying an editor 
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            setTimeout(function () { $(element).tinymce().remove() }, 0)
        });
        //$(element).tinymce(options);
        setTimeout(function () { $(element).tinymce(options); }, 0);
        el.html(value);

    },
    update: function (element, valueAccessor, allBindingsAccessor, context) {
        var el = $(element)
        var value = ko.utils.unwrapObservable(valueAccessor());
        var id = el.attr('id')

        //handle programmatic updates to the observable
        // also makes sure it doesn't update it if it's the same. 
        // otherwise, it will reload the instance, causing the cursor to jump.
        if (id !== undefined) {
            var content = tinyMCE.getInstanceById(id).getContent({ format: 'raw' })
            if (content !== value) {
                el.html(value);
            }
        }
    }
};