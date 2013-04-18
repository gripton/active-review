// Base Requirement Class Holds the necessary data for an individual Requirement
function Requirement(name, requirementText) {
    var self = this;
    self.name = ko.observable(name);
    self.requirementText = ko.observable(requirementText);
    self.Comment = ko.observable("No Comment");
}

function Question(questionText) {
    var self = this;
    self.questionText = questionText;
}

function ReviewSession(name) {
    var self = this;
    self.Name = name;
    self.requirementsList = ko.observableArray([]);
    self.questionList = ko.observableArray([]);
}

function SpawnRequirement(requirement) {
    var self = this;
    self.Requirement = requirement;
    self.Copy = ko.observable(true);
}

function SpawnQuestion(question) {
    var self = this;
    self.Question = question;
    self.Copy = ko.observable(true);
}

function SpawnReview(reviewModel) {
    var self = this;
    self.name = ko.observable("");
    self.requirementsList = ko.observableArray();
    self.questionList = ko.observableArray();

    self.addRequirement = function (requirement) {
        self.requirementsList.push(new SpawnRequirement(requirement));
    }

    self.addQuestion = function (question) {
        self.questionList.push(new SpawnQuestion(question));
    }
}