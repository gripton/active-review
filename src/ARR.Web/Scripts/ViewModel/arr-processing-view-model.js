var ProcessingViewModel = function() {
    var self = this;

    self.isProcessing = ko.observable(false);
    self.processingDisplay = ko.observable("Loading...");

    self.turnOnProcessing = function() {
        self.isProcessing(true);
    };

    self.turnOffProcessing = function() {
        self.isProcessing(false);
    };
};