function ChatMessage(message, userName, time) {
    var self = this;
    self.message = message;
    self.user = userName;
    self.time = time;
};

function Feedback() {
    var self = this;
    self.Username = ko.observable();
    self.Text = ko.observable();
    self.Created = ko.observable("");
};

function QuestionFeedback(question) {
    var self = this;
    self.Question = question;
    self.NewFeedback = ko.observable();
}