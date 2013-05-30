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

    self.newSession = ko.observable();
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

var Reviewer = function (screenName, domain) {
    var self = this;
    self.screenName = screenName;
    self.domain = domain;
};

function getReviewers(self, selectedSession) {
    $.getJSON(getArrApiUrl('account'), function(data) {
        var mappedReviewers = $.map(data, function (reviewer) {
            self.reviewers.push(new Reviewer(reviewer.ScreenName, reviewer.AreaOfExpertise));
        });

        var assignedReviewer = selectedSession.reviewer();
        if (assignedReviewer != null && assignedReviewer != undefined) {
            setAssignedReviewer(assignedReviewer);
        }
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
        dataType: 'JSON',
        success: function () {
            displayMessage("Review Session deleted", false);
        },
    });
}

function getSessions(self) {
    $.getJSON(getArrApiUrl('reviewindex'), function (allData) {
        var mappedSessions = $.map(allData, function (item) {
            var type = item.SessionStatus;

            if (type == SessionStatus.CREATED && currentUser == item.Creator) {
                self.myCreatedSessionsList.push(new Session(item));
            }
            else if ((type == SessionStatus.RELEASED || type == SessionStatus.COMPLETED) && currentUser == item.Creator) {
                self.myActiveSessionsListCreator.push(new Session(item));
            }
            else if (type == SessionStatus.RELEASED && currentUser == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
            }
            else if (type == SessionStatus.COMPLETED && currentUser == item.Reviewer) {
                self.myActiveSessionsListReviewer.push(new Session(item));
                self.locked = ko.observable(true);
            }
            else if (type == SessionStatus.ARCHIVED && (currentUser == item.Reviewer || currentUser == item.Creator)) {
                self.myArchivedSessionsList.push(new Session(item));
            }
        });
    });
}

//View Model for main index page
var IndexViewModel = function () {
    var self = this;
    setupErrorHandling(self);
    self.selectedSession = ko.observable();
    
    self.reviewers = ko.observableArray([]);
    self.selectedReviewer = ko.observable();

    self.myCreatedSessionsList = ko.observableArray([]);
    self.myActiveSessionsListCreator = ko.observableArray([]);
    self.myActiveSessionsListReviewer = ko.observableArray([]);
    self.myArchivedSessionsList = ko.observableArray([]);

    getSessions(self);
    
    self.createNewSession = function () {
        var reviewSession = this;
        reviewSession.Title = "Untitled Session";
        reviewSession.Creator = currentUser;

        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('reviewIndex'),
            data: ko.toJSON(reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (response) {
                self.myCreatedSessionsList.unshift(new Session({ Id: response, Title: reviewSession.Title }));
                displayMessage("New session created", false);
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

    self.getReviewers = function (selectedSession) {
        self.reviewers.removeAll();        
        getReviewers(self, selectedSession);
    }

    //assign-reviewer
    self.assignReviewer = function () {
        var sessionData = this;
        sessionData.Id = this.selectedSession().ID();
        sessionData.Reviewer = this.selectedReviewer();

        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('reviewindex/' + sessionData.Id + "/assign-reviewer"),
            data: ko.toJSON(sessionData),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                displayMessage("Reviewer assigned", false);
            },
        });
    }
}

function setAssignedReviewer(reviewer) {
    $("#reviewer > option").each(function () {
        if (this.value == reviewer) {
            $("#reviewer").val(reviewer).attr('selected', true);
            //self.selectedReviewer = ko.observable(reviewer);
            return false;
        }
    });
}

var indexModel = new IndexViewModel();
ko.applyBindings(indexModel);