var currentUser = loggedInUser();

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

    self.selectedSession = ko.observable();
    self.isLocked = ko.observable(false);
    self.allowQuestionnaire = ko.observable(true);

    if (data.SessionStatus == SessionStatus.COMPLETED) {
        self.isLocked = ko.observable(true);
        self.allowQuestionnaire = ko.observable(false);
    }

    var sessionId = data.Id;
    self.editorUrl = "../Screens/Editor.html?reviewSession=" + sessionId;
    self.previewUrl = "../Screens/Preview.html?reviewSession=" + sessionId;
    self.spawnUrl = "../Screens/Editor.html?reviewSession=" + sessionId;
    self.questionnaireUrl = "../Screens/Questionnaire.html?reviewSession=" + sessionId;
    self.forumUrl = "../Screens/Forum.html?reviewSession=" + sessionId;
    self.summaryUrl = "../Screens/Summary.html?reviewSession=" + sessionId;
}

var Reviewer = function (screenName, username, domain) {
    var self = this;
    self.screenName = screenName;
    self.username = username;
    self.domain = domain;
};

function deleteSession(sessionId) {
    $.ajax({
        type: "DELETE",
        url: getArrApiUrlPost('reviewIndex/' + sessionId),
        contentType: 'application/json',
        dataType: 'JSON',
        success: function () {
            displayMessage("Review Session deleted", false);
        },
    });
}

function getSessions(self, message) {
    self.processingViewModel.turnOnProcessing("Loading...");
    $.getJSON(getArrApiUrl('reviewindex'), function (allData) {
        var mappedSessions = $.map(allData, function (item) {
            var type = item.SessionStatus;

            if (type == SessionStatus.CREATED && currentUser == item.Creator) {
                self.myCreatedSessionsList.push(new Session(item));
            }
            if (type == SessionStatus.RELEASED && currentUser == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
            }
            if (type == SessionStatus.COMPLETED && currentUser == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
                self.locked = ko.observable(true);
            }
            if ((type == SessionStatus.RELEASED || type == SessionStatus.COMPLETED) && currentUser == item.Creator) { 
                self.myActiveSessionsListCreator.push(new Session(item));
            }
            if (type == SessionStatus.ARCHIVED && (currentUser == item.Reviewer || currentUser == item.Creator)) {
                self.myArchivedSessionsList.push(new Session(item));
            }
            self.processingViewModel.turnOffProcessing();
            
            if (message) {
                displayMessage(message, false);
            }
        });
    });
}

//View Model for main index page
var IndexViewModel = function(message) {
    var self = this;
    setupErrorHandling(self);
    self.selectedSession = ko.observable();

    self.reviewers = ko.observableArray([]);
    self.selectedReviewer = ko.observable();

    self.myCreatedSessionsList = ko.observableArray([]);
    self.myActiveSessionsListCreator = ko.observableArray([]);
    self.myActiveSessionsListReviewer = ko.observableArray([]);
    self.myArchivedSessionsList = ko.observableArray([]);

    self.processingViewModel = new ProcessingViewModel();


    getSessions(self, message);

    self.createNewSession = function () {
        self.processingViewModel.turnOnProcessing("Creating New Session");
        var reviewSession = new IndexViewModel();
        reviewSession.Title = "Untitled Session";
        reviewSession.Creator = currentUser;

        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('reviewIndex'),
            data: ko.toJSON(reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function(response) {
                self.myCreatedSessionsList.unshift(new Session({ Id: response, Title: reviewSession.Title }));
                $("tr.myCreatedSessions:first").attr('style', 'background-color: #FAFAB1');
                self.processingViewModel.turnOffProcessing();
                displayMessage("New session created", false);
            },
        });
    };

    self.setSelectedSession = function(selectedSession) {
        self.selectedSession(selectedSession);
    };

    self.removeSession = function(selectedSession) {
        var sessionId = selectedSession.ID();
        deleteSession(sessionId);
        self.myCreatedSessionsList.remove(selectedSession);
    };

    self.getReviewers = function(selectedSession) {
        self.reviewers.removeAll();

        $.getJSON(getArrApiUrl('account'), function(data) {
            $.map(data, function(reviewer) {
                self.reviewers.push(new Reviewer(reviewer.ScreenName, reviewer.Username, reviewer.AreaOfExpertise));
            });

            var assignedReviewer = selectedSession.reviewer();

            self.selectedReviewer(assignedReviewer);
        });
    };

    //assign-reviewer
    self.assignReviewer = function () {
        self.processingViewModel.turnOnProcessing("Assigning User...");
        var sessionData = this;
        sessionData.Id = this.selectedSession().ID();
        sessionData.Reviewer = this.selectedReviewer();

        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('reviewindex/' + sessionData.Id + "/assign-reviewer"),
            data: ko.toJSON(sessionData),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function() {
                displayMessage("Reviewer assigned", false);
                self.selectedSession().reviewer(sessionData.Reviewer);
                self.turnOffProcessing();
            },
        });
    };
};

var message = $.url(window.location).param('message');

var indexModel = new IndexViewModel(message);
ko.applyBindings(indexModel);