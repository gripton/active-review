// Base Requirement Class Holds the necessary data for an individual Requirement
function Requirement(name, requirementText) {
    var self = this;
//    self.name = ko.observable(name);
    self.content = ko.observable(requirementText);
    //self.Comment = ko.observable("No Comment");
}

function Question(questionText) {
    var self = this;
    self.content = questionText;
}

function ReviewSession(name) {
    var self = this;
//    self.Name = name;
    self.requirements = ko.observableArray([]);
    self.questions = ko.observableArray([]);
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
    self.requirements = ko.observableArray();
    self.questions = ko.observableArray();

    self.addRequirement = function (requirement) {
        self.requirements.push(new SpawnRequirement(requirement));
    }

    self.addQuestion = function (question) {
        self.questions.push(new SpawnQuestion(question));
    }
}