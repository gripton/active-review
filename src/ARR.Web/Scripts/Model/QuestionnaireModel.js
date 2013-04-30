function Requirement(name, requirementText) {
    var self = this;
    self.name = ko.observable(name);
    self.requirementText = ko.observable(requirementText);
    self.Comment = ko.observable("No Comment");

    self.selectItem = function (data) {
        requirementsModel.setComment(data);
    }

    self.selectRequirement = function (data) {
        requirementsModel.setEditRequirement(data);
    }
}

function Question(questionText) {
    var self = this;
    self.questionText = questionText;
    self.answer = ko.observable("Your answer Here")
    self.Feedback = ko.observable("");

    self.selectItem = function (data) {
        requirementsModel.setFeedback(data);
    }
}

// Here's my data model
var ViewModel = function () {
    var self = this;
    self.selectedItem = null;

    // Requirement List
    self.requirementsList = ko.observableArray([
        new Requirement("Requirement One", "<p>Here is a requirement</p><ul><li>Acceptance One</li></ul>"),
        new Requirement("Requirement Two", "<p>Here is another requirement</p><ul><li>Acceptance One</li></ul>")
    ]);

    self.questionList = ko.observableArray([
        new Question("This is Question #1"),
        new Question("This is Question #2"),
        new Question("This is Question #3")
    ]);

    self.questionIndex = ko.observable(0);
    self.currentQuestion = ko.observable(self.questionList()[self.questionIndex()]);
    self.enablePrevious = ko.observable(false);
    self.enableNext = ko.observable(true);

    self.previousQuestion = function () {
        if (self.questionIndex() > 0) {
            self.questionIndex(self.questionIndex() - 1);
            self.currentQuestion(self.questionList()[self.questionIndex()]);
            self.enableNext(true);
            self.enablePrevious(self.questionIndex() > 0)
        }
    }

    self.nextQuestion = function () {
        if (self.questionIndex() < self.questionList().length - 1) {
            self.questionIndex(self.questionIndex() + 1);
            self.currentQuestion(self.questionList()[self.questionIndex()]);
            self.enableNext(self.questionIndex() < self.questionList().length - 1);
            self.enablePrevious(true);
        }
    }

    // Save Session
    self.save = function () {
        $.post("http://localhost:55519/api/reviewsession", {
            data: ko.toJSON({ session: self }),
            dataType: "json",
            type: "post",
            contentType: "application/json",
            success: function (result) { alert(result) }
        });
    };

    // Edit Feedback
    self.currentFeedback = ko.observable("");

    self.setFeedback = function (selectedRequirement) {
        self.selectedItem = selectedRequirement;
        self.currentFeedback(self.selectedItem.Feedback());
    }

    self.saveFeedback = function () {
        self.selectedItem.Feedback(self.currentFeedback());
        self.selectedItem = null;
    }
};

var requirementsModel = new ViewModel();
ko.applyBindings(requirementsModel); // This makes Knockout get to work
