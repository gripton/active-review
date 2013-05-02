function Session(data) {
    var self = this;
    self.ID = ko.observable(data.ID);
    self.title = ko.observable(data.Title);
    self.reviewer = ko.observable(data.Reviewer);
    self.creator = ko.observable(data.Creator);
    self.type = ko.observable(data.SessionStatus);

    //self.selectItem = function (data) {
    //    homeModel.setReviewer(data);
    //}
}

function getReviewers() {
    //get all reviewers
    //bind to drop down list in html
}

function getCurrentUser() {
    //accountIndexController
    //http handler
}

function createNewSession() {
    //new reviewSesison model/object
    //set title as untitled document
    //post to reviewIndexController
    //redraw myCreatedSessionsList
}

function getSessions(self) {
    $.getJSON("http://localhost:55519/api/reviewsession", function (allData) {
        var mappedSessions = $.map(allData, function (item) {
            var type = item.SessionStatus;

            if (type == 0) {
                self.myCreatedSessionsList.push(new Session(item));
            }
            else if (type == 1 && getCurrentUser() == item.Creator) {
                self.myActiveSessionsListCreator.push(new Session(item));
            }
            else if (type == 1 && getCurrentUser() == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
            }
            else if (type == 2) {
                self.myArchivedSessionsList.push(new Session(item));
            }
        });
    });
}

//View Model
function ViewModel() {
    var self = this;
    //self.selectedItem = null;

    self.myCreatedSessionsList = ko.observableArray([]);
    self.myActiveSessionsListCreator = ko.observableArray([]);
    self.myActiveSessionsListReviewer = ko.observableArray([]);
    self.myArchivedSessionsList = ko.observableArray([]);

    getSessions(self);
   


    //Remove Session
    self.removeSession = function(session) 
    {
        //self.myCreatedSessionsList.remove(session);
        //how to bind with modal
        //use delete verb with ID call using ajax
    }

    // Add Reviewer
    //self.currentReviewer = ko.observable("");

    //self.setReviewer = function (selectedSession) {
    //    self.selectedItem = selectedSession;
    //    self.currentReviewer(self.selectedItem.Reviewer());
    //}

    //self.saveReviewer = function () {
    //    self.selectedItem.Reviewer(self.currentReviewer());
    //    self.selectedItem = null;
    //}

}

var indexModel = new ViewModel();
ko.applyBindings(indexModel);