$(function () {
    $.ajaxSetup({
        error: function (jqXHR, exception) {
            if (jqXHR.status === 0) {
                alert('Not connect.\n Verify Network.');
            } else if (jqXHR.status == 404) {
                alert('Requested page not found. [404]');
            } else if (jqXHR.status == 500) {
                alert('Internal Server Error [500].');
                var obj = JSON.parse(jqXHR.responseText);
                alert(obj.ExceptionMessage)
            } else if (exception === 'parsererror') {
                alert('Requested JSON parse failed.');
            } else if (exception === 'timeout') {
                alert('Time out error.');
            } else if (exception === 'abort') {
                alert('Ajax request aborted.');
            } else {
                alert('Uncaught Error.\n' + jqXHR.responseText);
            }
        }
    });
});

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
var ReviewEditorViewModel = function (reviewSessionId) {
    var self = this;
    self.reviewSessionId = reviewSessionId;
    self.selectedRequirement = null;
    self.isLoading = ko.observable(false);
    self.reviewSession = new ReviewSession();

    //self.reviewSession = getReviewSession();
    self.newRequirementViewModel = new NewRequirementViewModel(self);
    self.editCommentViewModel = new EditCommentViewModel();
    self.editRequirementViewModel = new EditRequirementViewModel();
    self.newQuestionViewModel = new NewQuestionViewModel(self);
    self.spawnReviewViewModel = new SpawnReviewViewModel(self);

    self.save = function () {
        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('reviewsession'),
            data: ko.toJSON(self.reviewSession),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                alert('success!');
                self.dirtyFlag.reset();
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
            self.isLoading(true);
            
            $.getJSON(getArrApiUrl('reviewsession/' + self.reviewSessionId), function (allData) {
                ko.mapping.fromJS(allData, {}, self.reviewSession);
                self.isLoading(false);
                self.dirtyFlag.reset();
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
        self.reviewSessionModel.reviewSession.Requirements.push(requirement);

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
        
        self.reviewSessionModel.reviewSession.Questions.push(question);

        //Need to recalculate the view in order to adjust the scrolling
        setScrollDisplay("Right");
        setScrollableToBottom("Right");
    };
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

        for (var k = 0; k < reviewSession.questions().length; k++) {
            spawnedReview.addQuestion(reviewSession.questions()[k]);
        }

        self.spawnInstance(spawnedReview);
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

