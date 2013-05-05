// Base Requirement Class Holds the necessary data for an individual Requirement
function Requirement() {
    var self = this;
    self.Content = ko.observable("");
    self.Comment = ko.observable("");
}

function Question() {
    var self = this;
    self.Content = ko.observable("");
    self.Answer = ko.observable("");
    self.Feedbacks = ko.observableArray([]);
}

function ReviewSession() {
    var self = this;
    self.Title = ko.observable("New Review Session");
    self.Creator = ko.observable("");
    self.Review = ko.observable("");
    self.LastModified = ko.observable(Date.now());
    self.Requirements = ko.observableArray([]);
    self.Questions = ko.observableArray([]);
    self.SessionStatus = ko.observable();
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

function SpawnReview() {
    var self = this;
    self.name = ko.observable("");
    self.requirements = ko.observableArray();
    self.questions = ko.observableArray();

    self.addRequirement = function(requirement) {
        self.requirements.push(new SpawnRequirement(requirement));
    };

    self.addQuestion = function(question) {
        self.questions.push(new SpawnQuestion(question));
    };
}