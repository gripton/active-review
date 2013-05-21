// Here's my data model
var QuestionViewModel = function (reviewSessionId) {
    var self = this;
    self.reviewSessionId = reviewSessionId;
    self.reviewSession = new ReviewSession();

    self.isLoading = ko.observable(false);

    self.questionNavigationViewModel = new QuestionNavigationViewModel(self);
    // Save Session
    self.saveQuestion = function () {
        $.ajax(getArrApiUrlPost('questions/' + self.reviewSessionId + "/save-questionnaire"), {
            data: ko.toJSON(self.reviewSession.Questions),
            dataType: "json",
            type: "put", 
            contentType: "application/json",
            success: function (result) { alert(result); }
        });
    };

    self.load = function () {
        //Need to set up the dirty flag for the review session now so it is fresh
        ko.applyBindings(self); // This makes Knockout get to work

        if (self.reviewSessionId != null) {
            self.isLoading(true);

            $.getJSON(getArrApiUrl('reviewsession/' + self.reviewSessionId), function (allData) {
                ko.mapping.fromJS(allData, {}, self.reviewSession);
                self.isLoading(false);
                self.questionNavigationViewModel.setQuestion();
            });
        }
    };

    // Load the review Session
    self.load();
};

var QuestionNavigationViewModel = function(questionViewModel) {
    var self = this;
    self.questionViewModel = questionViewModel;
    self.currentQuestion = ko.observable(null);
    self.questionIndex = ko.observable(0);

    self.enablePrevious = ko.observable(false);
    self.enableNext = ko.observable(true);
    self.enableComplete = ko.observable(false);

    self.previousQuestion = function () {
        self.questionIndex(self.questionIndex() - 1);
        self.setQuestion();
    };

    self.nextQuestion = function () {
        self.questionIndex(self.questionIndex() + 1);
        self.setQuestion();
    };

    self.setQuestion = function () {
        var questionList = questionViewModel.reviewSession.Questions();

        //make sure we haven't gone too far back
        if (self.questionIndex() < 0) {
            self.questionIndex(0);
        }
        
        //make sure we haven't gone too far forward
        if (self.questionIndex() >= questionList.length) {
            self.questionIndex(questionList.length - 1);
        }

        var enableNext = self.questionIndex() < questionList.length - 1;
        var enablePrevious = self.questionIndex() > 0;
        var enableComplete = self.questionIndex() === (questionList.length - 1);

        self.currentQuestion(questionList[self.questionIndex()]);
        self.enableNext(enableNext);
        self.enablePrevious(enablePrevious);
        self.enableComplete(enableComplete);
    };

    self.complete = function () {
        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('questions/' + reviewSessionId + "/complete-session"),
            data: ko.toJSON(self.questionViewModel.reviewSession.Questions),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                window.location = "Forum.html?reviewSession=" + reviewSessionId;
            },
        });
    };
};

var reviewSessionId = $.url(window.location).param('reviewSession');
var requirementsModel = new QuestionViewModel(reviewSessionId);

//We wanted to do some special processing on the ReviewEditor View Model because we're treating the 
//state as not being immediately persisted until the user requests that we persist it.
$(window).bind('beforeunload', function () {
    if (requirementsModel.isDirty()) {
        return "You may have unsaved changes.";
    }
});