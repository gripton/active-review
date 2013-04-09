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

    // New Requirement
    self.newRequirement = ko.observable("New Requirement")

    self.addRequirement = function () {
        self.requirementsList.push(new Requirement("Added Requirement", self.newRequirement()));
    }

    // Edit Comment
    self.currentComment = ko.observable("");

    self.setComment = function (selectedRequirement) {
        self.selectedItem = selectedRequirement;
        self.currentComment(self.selectedItem.Comment());
    }

    self.saveComment = function () {
        self.selectedItem.Comment(self.currentComment());
        self.selectedItem = null;
    }

    // Edit Requirement
    self.currentRequirementName = ko.observable("");
    self.currentRequirement = ko.observable("");

    self.setEditRequirement = function (selectedRequirement) {
        self.selectedItem = selectedRequirement;
        self.currentRequirementName(self.selectedItem.name());
        self.currentRequirement(self.selectedItem.requirementText());
    }

    self.saveEditedRequirement = function(){
        self.selectedItem.name(self.currentRequirementName());
        self.selectedItem.requirementText(self.currentRequirement());
        self.selectedItem = null;
    }

    // New Question
    self.newQuestion = ko.observable("New Question");
    self.addQuestion = function () {
        self.questionList.push(new Question(self.newQuestion()));
    }

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

var requirementsModel = new ViewModel();
ko.applyBindings(requirementsModel); // This makes Knockout get to work
