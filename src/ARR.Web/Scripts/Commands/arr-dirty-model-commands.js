﻿// Courtesy of - http://www.knockmeout.net/2011/05/creating-smart-dirty-flag-in-knockoutjs.html
// I've modified it and simplified it so that it applies only to the specified root model that we
// Are interacting with, but so that it also allows us to reset the state on demand (i.e. save)
ko.dirtyFlag = function (root, isInitiallyDirty) {
    var result = function () { },
        _initialState = ko.observable(ko.toJSON(root)),
        _isInitiallyDirty = ko.observable(isInitiallyDirty);

    result.isDirty = ko.computed(function () {
        return _isInitiallyDirty() || _initialState() !== ko.toJSON(root);
    });

    result.reset = function () {
        _initialState(ko.toJSON(root));
        _isInitiallyDirty(false);
    };

    return result;
};
