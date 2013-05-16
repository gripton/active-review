// Here's my data model
var ForumViewModel = function (reviewSessionId) {
    var self = this;
    self.reviewSessionId = reviewSessionId;
    self.reviewSession = new ReviewSession();
    self.questionList = ko.observableArray();
    self.addFeedbackViewModel = new AddFeedbackViewModel(self);

    self.ChatMessages = ko.observableArray([
        new ChatMessage("Here are my initial Comments", "Tom", Date.now()),
        new ChatMessage("And some friendly chit chat to follow that up", "Dan", Date.now())
    ]);

    self.newMessage = ko.observable("");
    
    self.load = function () {
        //Need to set up the dirty flag for the review session now so it is fresh
        ko.applyBindings(self); // This makes Knockout get to work

        if (self.reviewSessionId != null) {
            //self.isLoading(true);

            $.getJSON(getArrApiUrl('reviewsession/' + self.reviewSessionId), function (allData) {
                ko.mapping.fromJS(allData, {}, self.reviewSession);

                for (var k = 0; k < self.reviewSession.Questions().length; k++) {
                    self.questionList.push(new QuestionFeedback(self.reviewSession.Questions()[k]));
                }
                setScrollDisplay("Left");
                //self.isLoading(false);
            });
        }
    };

    // Load the review Session
    self.load();
};

var AddFeedbackViewModel = function(forumViewModel) {
    var self = this;
    self.forumViewModel = forumViewModel;

    self.saveFeedback = function (selectedQuestion) {
        var f = new Feedback();
        f.Text(selectedQuestion.NewFeedback());
        //f.Username = self

        if (selectedQuestion.Question.Feedbacks() == null) {
            selectedQuestion.Question.Feedbacks([]);
        }
        selectedQuestion.Question.Feedbacks.push(f);

        $.ajax(getArrApiUrlPost('feedback/' + self.forumViewModel.reviewSessionId + '/feedback'), {
            data: ko.toJSON(selectedQuestion.Question),
            dataType: "json",
            type: "post",
            contentType: "application/json",
            success: function (result) {
                alert(result);
                // Clear out the feedback text box that we just added
                selectedQuestion.NewFeedback('');
                setScrollDisplay("Left");
            }
        });
    };
};


var reviewSessionId = $.url(window.location).param('reviewSession');
var forumModel = new ForumViewModel(reviewSessionId);