ko.bindingHandlers.tinymce = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().tinymceOptions ||
        {
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "bbcode",
            theme_advanced_buttons1: "bold,italic,underline,undo,redo,link,unlink,image,forecolor,styleselect,removeformat,cleanup,code",
            theme_advanced_buttons2: "",
            theme_advanced_buttons3: "",
            theme_advanced_toolbar_location: "bottom",
            theme_advanced_toolbar_align: "center",
            theme_advanced_styles: "Code=codeStyle;Quote=quoteStyle",

            // Skin options
            skin: "o2k7",
            skin_variant: "silver",

            // Drop lists for link/image/media/template dialogs
            template_external_list_url: "js/template_list.js",
            external_link_list_url: "js/link_list.js",
            external_image_list_url: "js/image_list.js",
            media_external_list_url: "js/media_list.js",
        };
        var modelValue = valueAccessor();
        var value = ko.utils.unwrapObservable(valueAccessor());
        var el = $(element);

        //handle edits made in the editor. Updates after an undo point is reached.
        options.setup = function (ed) {
            ed.onChange.add(function (val, l) {
                if (ko.isWriteableObservable(modelValue)) {
                    modelValue(l.content);
                }
            });
        };

        //handle destroying an editor 
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            setTimeout(function () { $(element).tinymce().remove(); }, 0);
        });
        //$(element).tinymce(options);
        setTimeout(function () { $(element).tinymce(options); }, 0);
        el.html(value);

    },
    update: function (element, valueAccessor) {
        var el = $(element);
        var value = ko.utils.unwrapObservable(valueAccessor());
        var id = el.attr('id');

        //handle programmatic updates to the observable
        // also makes sure it doesn't update it if it's the same. 
        // otherwise, it will reload the instance, causing the cursor to jump.
        if (id !== undefined) {
            if (tinyMCE.getInstanceById(id) !== undefined) {
                var content = tinyMCE.getInstanceById(id).getContent({ format: 'raw' });
                if (content !== value) {
                    el.html(value);
                }
            }
        }
    }
};

