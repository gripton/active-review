// The ReviewEditor View mode
var ReviewEditorViewModel = function (reviewSessionId) {
    var self = this;
    setupErrorHandling(self);
    self.reviewSessionId = reviewSessionId;
    self.selectedRequirement = null;
    self.reviewSession = new ReviewSession();

    //ViewModels for Requirement Functionality
    self.newRequirementViewModel = new NewRequirementViewModel(self);
    self.editCommentViewModel = new EditCommentViewModel();
    self.deleteRequirementViewModel = new DeleteRequirementViewModel(self);
    self.editRequirementViewModel = new EditRequirementViewModel();

    //ViewModels for Question Functionality
    self.newQuestionViewModel = new NewQuestionViewModel(self);
    self.deleteQuestionViewModel = new DeleteQuestionViewModel(self);
    self.editQuestionViewModel = new EditQuestionViewModel();

    //ViewModels for Session Functionality
    self.renameSessionViewModel = new RenameSessionViewModel(self);
    self.spawnReviewViewModel = new SpawnReviewViewModel(self);
    self.assginSessionViewModel = new AssginSessionViewModel(self);

    self.processingViewModel = new ProcessingViewModel();

    self.release = function () {
        self.processingViewModel.turnOnProcessing("Releasing Session...");
        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('reviewsession/' + self.reviewSessionId + "/release-session"),
            data: ko.toJSON(self.reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                alert('Your session has been released.');
                self.processingViewModel.turnOffProcessing();
                window.location = "Home.html";
            },
        });
    };

    self.save = function () {
        self.processingViewModel.turnOnProcessing("Saving...");
        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('reviewsession/' + self.reviewSessionId + "/session"),
            data: ko.toJSON(self.reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                displayMessage("Review Session Saved", false);
                self.dirtyFlag.reset();
                self.processingViewModel.turnOffProcessing();
            },
        });
    };

    self.load = function () {
        //Need to set up the dirty flag for the review session now so it is fresh
        self.dirtyFlag = new ko.dirtyFlag(self.reviewSession);
        self.isDirty = ko.computed(function () {
            return self.dirtyFlag.isDirty();
        }, self.reviewSession);
        ko.applyBindings(self); // This makes Knockout get to work

        if (self.reviewSessionId != null) {

            self.processingViewModel.turnOnProcessing("Loading...");
            $.getJSON(getArrApiUrl('reviewsession/' + self.reviewSessionId), function (allData) {
                ko.mapping.fromJS(allData, {}, self.reviewSession);
                sizeContent();
                setScrollDisplay("Left");
                setScrollDisplay("Right");
                
                $.getJSON(getArrApiUrl('account'), function (data) {
                    $.map(data, function (reviewer) {
                        self.assginSessionViewModel.reviewerList.push(new Reviewer(reviewer.ScreenName, reviewer.Username, reviewer.AreaOfExpertise));
                    });

                    self.processingViewModel.turnOffProcessing();
                    self.dirtyFlag.reset();
                });
            });
        } 
    };

    self.load();
};

// Class that handles the bindings for the new Requirement Interaction
function NewRequirementViewModel(reviewSessionModel) {
    var self = this;
    self.reviewSessionModel = reviewSessionModel;
    self.newRequirement = ko.observable("");

    self.addRequirement = function () {
        var requirement = new Requirement();
        requirement.Content(self.newRequirement());
        requirement.Comment("");
        
        // If the Requirements list is null, we need to instantiate it to an empty array
        if (self.reviewSessionModel.reviewSession.Requirements() == null) {
            self.reviewSessionModel.reviewSession.Requirements([]);
        }
        self.reviewSessionModel.reviewSession.Requirements.push(requirement);

        //Clear out the old question
        self.newRequirement("");

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Left");
        setScrollableToBottom("Left");
    };
}

// Class that handles the bindings for the Edit Comment Interaction
function EditCommentViewModel() {
    var self = this;
    self.selectedRequirement = null;

    self.currentComment = ko.observable("");

    self.setComment = function(selectedRequirement) {
        self.selectedRequirement = selectedRequirement;
        self.currentComment(self.selectedRequirement.Comment());
    };

    self.saveComment = function() {
        self.selectedRequirement.Comment(self.currentComment());
        self.selectedRequirement = null;
    };
}

// Class that handles the bindings for the Edit Comment Interaction
function DeleteRequirementViewModel(reviewSessionModel) {
    var self = this;
    self.reviewSessionModel = reviewSessionModel;
    self.deleteRequirement = function (selectedRequirement) {
        var requirements = self.reviewSessionModel.reviewSession.Requirements;
        requirements.remove(selectedRequirement);

    };
}

// Class that handles the bindings for the Renaming the Session Interaction
function RenameSessionViewModel(reviewSessionModel) {
    var self = this;
    self.reviewSessionModel = reviewSessionModel;

    self.currentTitle = ko.observable("");

    self.setTitle = function () {
        self.currentTitle(self.reviewSessionModel.reviewSession.Title());
    };

    self.saveTitle = function () {
        self.reviewSessionModel.reviewSession.Title(self.currentTitle());
    };
}

// Class that handles the bindings for the Edit Comment Interaction
function AssginSessionViewModel(reviewSessionModel) {
    var self = this;
    self.reviewSessionModel = reviewSessionModel;
    self.reviewerList = ko.observableArray([]);
    self.selectedReviewer = ko.observable();

    self.setReviewer = function () {
        self.selectedReviewer(self.reviewSessionModel.reviewSession.Reviewer());
    };

    self.assignReviewer = function () {
        self.reviewSessionModel.reviewSession.Reviewer(self.selectedReviewer());
    };
}

// Class that handles the bindings for the Edit Requirement Interaction
function EditRequirementViewModel() {
    var self = this;
    self.selectedRequirement = null;

    // Edit Requirement
    self.currentRequirementName = ko.observable("");
    self.currentRequirement = ko.observable("");

    self.setEditRequirement = function(selectedRequirement) {
        self.selectedRequirement = selectedRequirement;
        self.currentRequirement(self.selectedRequirement.Content());
    };

    self.saveEditedRequirement = function() {
        self.selectedRequirement.Content(self.currentRequirement());
        self.selectedRequirement = null;

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Left");
        setScrollableToBottom("Left");
    };
}

// Class that handles bindings for the New Question interaction
function NewQuestionViewModel(reviewSessionModel) {
    var self = this;
    self.reviewSessionModel = reviewSessionModel;

    self.newQuestion = ko.observable("");
    self.addQuestion = function () {
        var question = new Question();
        question.Content(self.newQuestion());
        question.Answer("");
        
        // If the Questions list is null, we need to instantiate it to an empty array
        if (self.reviewSessionModel.reviewSession.Questions() == null) {
            self.reviewSessionModel.reviewSession.Questions([]);
        }
        self.reviewSessionModel.reviewSession.Questions.push(question);

        //clear out the question text field
        self.newQuestion("");
        
        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Right");
        setScrollableToBottom("Right");
    };
}

// Class that handles the bindings for the Edit Requirement Interaction
function EditQuestionViewModel() {
    var self = this;
    self.selectedQuestion = null;

    // Edit Requirement
    self.currentQuestion = ko.observable("");

    self.setEditQuestion = function (selectedQuestion) {
        self.selectedQuestion = selectedQuestion;
        self.currentQuestion(self.selectedQuestion.Content());
    };

    self.saveEditedQuestion = function () {
        self.selectedQuestion.Content(self.currentQuestion());
        self.selectedQuestion = null;

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Right");
    };
}

// Class that handles the bindings for the Edit Comment Interaction
function DeleteQuestionViewModel(reviewSessionModel) {
    var self = this;
    self.reviewSessionModel = reviewSessionModel;
    self.deleteQuestion = function (selectedQuestion) {
        var questions = self.reviewSessionModel.reviewSession.Questions;
        questions.remove(selectedQuestion);
    };
}

var reviewSessionId = $.url(window.location).param('reviewSession');

//Instantiate the requirements model that we will be using throughout the page life
var requirementsModel = new ReviewEditorViewModel(reviewSessionId);

//We wanted to do some special processing on the ReviewEditor View Model because we're treating the 
//state as not being immediately persisted until the user requests that we persist it.
$(window).bind('beforeunload', function () {
    if (requirementsModel.isDirty()) {
        return "You may have unsaved changes.";
    }
});

