// The ReviewEditor View mode
var GuideViewModel = function (reviewSessionId) {
    var self = this;
    self.reviewSessionId = reviewSessionId;
}

var reviewSessionId = $.url(window.location).param('reviewSession');

//Instantiate the requirements model that we will be using throughout the page life
var guideModel = new GuideViewModel(reviewSessionId);
ko.applyBindings(guideModel); // This makes Knockout get to work
