var SessionStatus =
{
    CREATED: 0,
    RELEASED: 1,
    COMPLETED: 2,
    ARCHIVED: 3
};

function Session(data) {
    var self = this;
    self.ID = ko.observable(data.Id);
    self.title = ko.observable(data.Title);
    self.creator = ko.observable(data.Creator);
    self.type = ko.observable(data.SessionStatus);

    self.reviewer = ko.observable(data.Reviewer);
    //TODO: replace above with method to select returned reviewer from populated drop down

    self.selectedSession = ko.observable();

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
        url: getArrApiUrlPost('reviewIndex/' + sessionId),
        contentType: 'application/json',
        dataType: 'JSON'
    });
}

function getSessions(self) {
    $.getJSON(getArrApiUrl('reviewindex'), function (allData) {
        var mappedSessions = $.map(allData, function (item) {
            var type = item.SessionStatus;

            if (type == SessionStatus.CREATED) {
                self.myCreatedSessionsList.push(new Session(item));
            }
            else if (type == SessionStatus.RELEASED && currentUser == item.Creator) {
                self.myActiveSessionsListCreator.push(new Session(item));
            }
            else if (type == SessionStatus.RELEASED && currentUser == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
            }
            else if (type == SessionStatus.ARCHIVED) {
                self.myArchivedSessionsList.push(new Session(item));
            }
        });
    });
}

//View Model for main index page
var IndexViewModel = function () {
    var self = this;
    self.selectedSession = ko.observable();

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
            success: function (response) {
                self.myCreatedSessionsList.unshift(new Session({ Id: response, Title: reviewSession.Title }));
            },
        });
    };

    self.setSelectedSession = function (selectedSession) {
        self.selectedSession(selectedSession);
    }

    //Remove Session
    self.removeSession = function (selectedSession) {
        var sessionId = selectedSession.ID();
        deleteSession(sessionId);
        self.myCreatedSessionsList.remove(selectedSession);
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