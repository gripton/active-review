function Question(questionText) {
    var self = this;
    self.questionText = questionText;
    self.answer = ko.observable("Your answer Here")
    self.Feedback = ko.observableArray([
        new Feedback("OriginalFeedback", "Tom", Date.now())
    ]);

    self.newFeedback = ko.observable("");

    self.selectItem = function (data) {
        requirementsModel.setFeedback(data);
    }
}

function Feedback(feedback, userName, time) {
    var self = this;
    self.feedback = feedback;
    self.user = userName;
    self.time = time;
}

function ChatMessage(message, userName, time) {
    var self = this;
    self.message = message;
    self.user = userName;
    self.time = time;
}

// Here's my data model
var ViewModel = function () {
    var self = this;
    self.selectedItem = null;

    self.questionList = ko.observableArray([
        new Question("This is Question #1"),
        new Question("This is Question #2"),
        new Question("This is Question #3")
    ]);

    self.ChatMessages = ko.observableArray([
        new ChatMessage("Here are my initial Comments", "Tom", Date.now()),
        new ChatMessage("And some friendly chit chat to follow that up", "Dan", Date.now())
    ]);

    self.saveFeedback = function () {
        self.selectedItem.Feedback().push(new Feedback(self.currentFeedback(),"Tom", Date.now()));
        self.selectedItem = null;
    }

    self.newMessage = ko.observable("");
};

var requirementsModel = new ViewModel();
ko.applyBindings(requirementsModel); // This makes Knockout get to work
