function Session(data) {
    var self = this;
    self.ID = ko.observable(data.Id);
    self.title = ko.observable(data.Title);
    self.creator = ko.observable(data.Creator);
    self.type = ko.observable(data.SessionStatus);

    self.reviewer = ko.observable(data.Reviewer);
    //TODO: replace above with method to select returned reviewer from populated drop down

    var sessionId = data.Id;
    self.editorUrl = "../Screens/Editor.html?reviewSession=" + sessionId;
    self.previewUrl = "../Screens/Editor.html?reviewSession=" + sessionId;
    self.spawnUrl = "../Screens/Editor.html?reviewSession=" + sessionId;
    self.questionnaireUrl = "../Screens/Questionnaire.html?reviewSession=" + sessionId;
    self.forumUrl = "../Screens/Forum.html?reviewSession=" + sessionId;
}

function getReviewers() {
    $.ajax({
        type: "GET",
        url: getArrApiUrl('account'),
        data: ko.toJSON(self),
        contentType: 'application/json',
        dataType: 'JSON',
        success: function () {
            //TODO: bind to drop down list in html
            alert("success");
        },
    });
}

var currentUser;

function getCurrentUser() {
    $.getJSON('user.user', function (userData) {
        currentUser = userData.Username;
    });
}

getCurrentUser();

function deleteSession(sessionId) {
    $.ajax({
        type: "DELETE",
        url: getArrApiUrl('reviewIndex/' + sessionId),
        data: ko.toJSON(self),
        contentType: 'application/json',
        dataType: 'JSON',
        success: function () {
            //TODO: redraw myCreatedSessionsList
            alert("success");
        },
    });
}

function getSessions(self) {
    $.getJSON(getArrApiUrl('reviewindex'), function (allData) {
        var mappedSessions = $.map(allData, function (item) {
            var type = item.SessionStatus;

            if (type == 0) {
                self.myCreatedSessionsList.push(new Session(item));
            }
            else if (type == 1 && currentUser == item.Creator) {
                self.myActiveSessionsListCreator.push(new Session(item));
            }
            else if (type == 1 && currentUser == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
            }
            else if (type == 3) {
                self.myArchivedSessionsList.push(new Session(item));
            }
        });
    });
}

//View Model for main index page
var IndexViewModel = function () {
    var self = this;
    self.selectedSession = null;

    self.reviewerViewModel = new ReviewerViewModel();

    self.myCreatedSessionsList = ko.observableArray([]);
    self.myActiveSessionsListCreator = ko.observableArray([]);
    self.myActiveSessionsListReviewer = ko.observableArray([]);
    self.myArchivedSessionsList = ko.observableArray([]);

    getSessions(self);

    self.createNewSession = function () {
        var reviewSession = this;
        reviewSession.Title = "Untitled Session";

        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('reviewIndex'),
            data: ko.toJSON(reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (data) {
                //self.myCreatedSessionsList.push(true);
            }, 
        });
    };

    //Remove Session
    self.removeSession = function () {
        //TODO: get selected session ID, pass to
        //var sessionId = 
        //deleteSession(sessionId);
        //self.myCreatedSessionsList.remove(session);
        //how to bind with modal
        //use delete verb with ID call using ajax
        //deleteSession(sessionId);
    }
}

// Class that handles the bindings for the Reviewer Interaction
function ReviewerViewModel() {
    var self = this;
    self.selectedSession = null;

    self.currentReviewer = ko.observable("");

    self.setReviewer = function (selectedSession) {
        self.selectedSession = selectedSession;
        self.currentReviewer(self.selectedSession.Reviewer());
    };

    self.saveReviewer = function () {
        self.selectedSession.Reviewer(self.currentReviewer());
        self.selectedSession = null;
    };
}

var indexModel = new IndexViewModel();
ko.applyBindings(indexModel);