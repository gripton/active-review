function getReviewSession() {
    var reviewSession = new ReviewSession("Demo Review Session");

    reviewSession.requirementsList.push(new Requirement("Requirement One", "<p>Here is a requirement</p><ul><li>Acceptance One</li></ul>"));
    reviewSession.requirementsList.push(new Requirement("Requirement Two", "<p>Here is another requirement</p><ul><li>Acceptance One</li></ul>"));

    reviewSession.questionList.push(new Question("This is Question #1"));
    reviewSession.questionList.push(new Question("This is Question #2"));
    reviewSession.questionList.push(new Question("This is Question #3"));

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
        $.post("http://localhost:55519/api/reviewsession", {
            data: ko.toJSON({ session: self }),
            dataType: "json",
            type: "post",
            contentType: "application/json",
            success: function (result) { alert(result) }
        });
    };
};

// Class that handles the bindings for the new Requirement Interaction
function NewRequirementViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;
    self.newRequirement = ko.observable("")

    self.addRequirement = function () {
        self.reviewSession.requirementsList.push(new Requirement("Added Requirement", self.newRequirement()));
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
        self.currentRequirement(self.selectedRequirement.requirementText());
    }

    self.saveEditedRequirement = function () {
        self.selectedRequirement.name(self.currentRequirementName());
        self.selectedRequirement.requirementText(self.currentRequirement());
        self.selectedRequirement = null;
    }
}

// Class that handles bindings for the New Question interaction
function NewQuestionViewModel(reviewSession) {
    var self = this;
    self.reviewSession = reviewSession;

    self.newQuestion = ko.observable("");
    self.addQuestion = function () {
        self.reviewSession.questionList.push(new Question(self.newQuestion()));
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

        for (var i = 0; i < reviewSession.requirementsList().length; i++) {
            spawnedReview.addRequirement(reviewSession.requirementsList()[i]);
        }

        for (var i = 0; i < reviewSession.questionList().length; i++) {
            spawnedReview.addQuestion(reviewSession.questionList()[i]);
        }

        self.spawnInstance(spawnedReview);
    };
}

var requirementsModel = new ReviewEditorViewModel();
ko.applyBindings(requirementsModel); // This makes Knockout get to work
