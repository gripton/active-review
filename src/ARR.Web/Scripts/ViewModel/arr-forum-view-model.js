// Here's my data model
var ForumViewModel = function (reviewSessionId) {
    var self = this;

    setupErrorHandling(self);

    //Retrive the user from the arr-security-commands
    self.currentUser = loggedInUser();
    self.reviewSessionId = reviewSessionId;
    self.reviewSession = new ReviewSession();
    self.questionList = ko.observableArray();
    self.addFeedbackViewModel = new AddFeedbackViewModel(self);
    self.archiveSessionViewModel = new ArchiveSessionViewModel(self);
    self.processingViewModel = new ProcessingViewModel();

    self.ChatMessages = ko.observableArray([
        new ChatMessage("Here are my initial Comments", "Tom", Date.now()),
        new ChatMessage("And some friendly chit chat to follow that up", "Dan", Date.now())
    ]);

    self.newMessage = ko.observable("");
    
    self.load = function () {
        //Need to set up the dirty flag for the review session now so it is fresh
        ko.applyBindings(self); // This makes Knockout get to work

        if (self.reviewSessionId != null) {
            self.processingViewModel.turnOnProcessing("Loading...");

            $.getJSON(getArrApiUrl('reviewsession/' + self.reviewSessionId), function (allData) {
                ko.mapping.fromJS(allData, {}, self.reviewSession);

                for (var k = 0; k < self.reviewSession.Questions().length; k++) {
                    self.questionList.push(new QuestionFeedback(self.reviewSession.Questions()[k]));
                }
                self.processingViewModel.turnOffProcessing();
                setScrollDisplay("Left");
                setScrollDisplay("Right");
            });
        }
    };

    // Load the review Session
    self.load();
};

var AddFeedbackViewModel = function(forumViewModel) {
    var self = this;
    self.forumViewModel = forumViewModel;

    self.saveFeedback = function (selectedQuestion) {
        self.forumViewModel.processingViewModel.turnOnProcessing("Saving Feedback...");
        var f = new Feedback();
        f.Text(selectedQuestion.NewFeedback());
        //f.Username = self

        if (selectedQuestion.Question.Feedbacks() == null) {
            selectedQuestion.Question.Feedbacks([]);
        }
        selectedQuestion.Question.Feedbacks.push(f);

        $.ajax(getArrApiUrlPost('questions/' + self.forumViewModel.reviewSessionId + '/provide-feedback'), {
            data: ko.toJSON(forumViewModel.reviewSession.Questions()),
            dataType: "json",
            type: "put",
            contentType: "application/json",
            success: function () {
                // Clear out the feedback text box that we just added
                selectedQuestion.NewFeedback('');
                setScrollDisplay("Left");
                self.forumViewModel.processingViewModel.turnOffProcessing();
                displayMessage('Feedback Saved');
            }
        });
    };
};

// ViewModel that drives the archival functionality.
var ArchiveSessionViewModel = function (forumViewModel) {
    var self = this;
    self.forumViewModel = forumViewModel;

    self.canSeeQuestionnaire = ko.computed(function () {
        var isNotCreator = self.forumViewModel.reviewSession.Creator() != self.forumViewModel.currentUser;
        var isReleased = self.forumViewModel.reviewSession.SessionStatus() == SessionStatus.RELEASED;
        return isNotCreator && isReleased;
    });


    self.canArchive = ko.computed(function () {
        var isCreator = self.forumViewModel.reviewSession.Creator() == self.forumViewModel.currentUser;
        var isComplete = self.forumViewModel.reviewSession.SessionStatus() == SessionStatus.COMPLETED;
        return (isCreator && isComplete);
    });

    self.archiveSession = function () {
        self.forumViewModel.processingViewModel.turnOnProcessing("Archiving...");
        
        $.ajax(getArrApiUrlPost('reviewsession/' + self.forumViewModel.reviewSessionId + '/archive'), {
            dataType: "json",
            type: "put",
            contentType: "application/json",
            success: function () {
                alert("Your session has been archived.");
                window.location = "Home.html";
            }
        });
    };
};


var reviewSessionId = $.url(window.location).param('reviewSession');
var forumModel = new ForumViewModel(reviewSessionId);