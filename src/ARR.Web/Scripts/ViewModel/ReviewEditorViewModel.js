function getReviewSession() {
    var reviewSession = new ReviewSession("Demo Review Session");

    reviewSession.requirements.push(new Requirement("Requirement One", "<p>Here is a requirement</p><ul><li>Acceptance One</li></ul>"));
    reviewSession.requirements.push(new Requirement("Requirement Two", "<p>Here is another requirement</p><ul><li>Acceptance One</li></ul>"));

    reviewSession.questions.push(new Question("This is Question #1"));
    reviewSession.questions.push(new Question("This is Question #2"));
    reviewSession.questions.push(new Question("This is Question #3"));

    return reviewSession;
}

// The ReviewEditor View mode
var ReviewEditorViewModel = function () {
    var self = this;
    self.selectedRequirement = null;

    self.reviewSession = getReviewSession();
    self.newRequirementViewModel = new NewRequirementViewModel(self.reviewSession);
    self.editCommentViewModel = new EditCommentViewModel(self.reviewSession);
    self.editRequirementViewModel = new EditRequirementViewModel(self.reviewSession);
    self.newQuestionViewModel = new NewQuestionViewModel(self.reviewSession);
    self.spawnReviewViewModel = new SpawnReviewViewModel(self.reviewSession);

    self.save = function () {
        $.ajax({
            type: "POST",
            url: "http://localhost:55519/api/reviewsession",
            data: ko.toJSON(self.reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (data) {  }
        });
        // We'll want to reset the dirty flag on successful save. TODO: Figure out how to incorporate this into the success of the ajax call?
        self.dirtyFlag.reset();
    }

    self.dirtyFlag = new ko.dirtyFlag(self.reviewSession);

    self.isDirty = ko.computed(function () {
        return self.dirtyFlag.isDirty();
    }, self.reviewSession);
};

// Class that handles the bindings for the new Requirement Interaction
function NewRequirementViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;
    self.newRequirement = ko.observable("")

    self.addRequirement = function () {
        self.reviewSession.requirements.push(new Requirement("Added Requirement", self.newRequirement()));

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Left");
        setScrollableToBottom("Left")
    }
}

// Class that handles the bindings for the Edit Comment Interaction
function EditCommentViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;
    self.selectedRequirement = null;

    self.currentComment = ko.observable("");

    self.setComment = function (selectedRequirement) {
        self.selectedRequirement = selectedRequirement;
        self.currentComment(self.selectedRequirement.Comment());
    }

    self.saveComment = function () {
        self.selectedRequirement.Comment(self.currentComment());
        self.selectedRequirement = null;
    }
}

// Class that handles the bindings for the Edit Requirement Interaction
function EditRequirementViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;
    self.selectedRequirement = null;

    // Edit Requirement
    self.currentRequirementName = ko.observable("");
    self.currentRequirement = ko.observable("");

    self.setEditRequirement = function (selectedRequirement) {
        self.selectedRequirement = selectedRequirement;
        self.currentRequirementName(self.selectedRequirement.name());
        self.currentRequirement(self.selectedRequirement.content());
    }

    self.saveEditedRequirement = function () {
        self.selectedRequirement.name(self.currentRequirementName());
        self.selectedRequirement.content(self.currentRequirement());
        self.selectedRequirement = null;

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Left");
        setScrollableToBottom("Left")
    }
}

// Class that handles bindings for the New Question interaction
function NewQuestionViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;

    self.newQuestion = ko.observable("");
    self.addQuestion = function () {
        self.reviewSession.questions.push(new Question(self.newQuestion()));

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Right")
        setScrollableToBottom("Right")
    }
}

// Class that handles the bindings for the Spawn functionality
function SpawnReviewViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;

    self.spawnInstance = ko.observable(null);

    // Grabs all the pertinent pieces of the review that need to be migrated and creates an instance
    // Of a spawned review.
    self.spawn = function () {
        var spawnedReview = new SpawnReview(reviewSession);

        spawnedReview.name(reviewSession.name + " clone");

        for (var i = 0; i < reviewSession.requirements().length; i++) {
            spawnedReview.addRequirement(reviewSession.requirements()[i]);
        }

        for (var i = 0; i < reviewSession.questions().length; i++) {
            spawnedReview.addQuestion(reviewSession.questions()[i]);
        }

        self.spawnInstance(spawnedReview);
    };
}

//Instantiate the requirements model that we will be using throughout the page life
var requirementsModel = new ReviewEditorViewModel();
ko.applyBindings(requirementsModel); // This makes Knockout get to work

//We wanted to do some special processing on the ReviewEditor View Model because we're treating the 
//state as not being immediately persisted until the user requests that we persist it.
$(window).bind('beforeunload', function () {
    if (requirementsModel.isDirty()) {
        return "You may have unsaved changes.";
    }
});
