var ProcessingViewModel = function() {
    var self = this;

    self.isProcessing = ko.observable(false);
    self.processingDisplay = ko.observable("Loading...");

    self.turnOnProcessing = function (display) {
        self.processingDisplay(display);
        self.isProcessing(true);
    };

    self.turnOffProcessing = function () {
        self.processingDisplay("");
        self.isProcessing(false);
    };
};