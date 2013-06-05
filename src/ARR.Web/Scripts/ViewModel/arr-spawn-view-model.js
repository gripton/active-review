// Class that handles the bindings for the Spawn functionality
function SpawnReviewViewModel() {
    var self = this;
    self.reviewSession = new ReviewSession();

    self.spawnInstance = ko.observable(null);

    // Grabs all the pertinent pieces of the review that need to be migrated and creates an instance
    // Of a spawned review.
    self.spawnSetup = function (reviewSession) {
        self.reviewSession = reviewSession;
        var spawnedReview = new SpawnReview();

        spawnedReview.Title(reviewSession.Title() + " clone");

        if (reviewSession.Requirements()) {
            for (var i = 0; i < reviewSession.Requirements().length; i++) {
                spawnedReview.addRequirement(reviewSession.Requirements()[i]);
            }
        }
        if (reviewSession.Questions()) {
            for (var k = 0; k < reviewSession.Questions().length; k++) {
                spawnedReview.addQuestion(reviewSession.Questions()[k]);
            }
        }
        self.spawnInstance(spawnedReview);
    };

    self.spawn = function () {
        var session = new ReviewSession();
        var spawnedReview = self.spawnInstance();
        session.Title(spawnedReview.Title());
        for (var i = 0; i < spawnedReview.Requirements().length; i++) {
            if (spawnedReview.Requirements()[i].Copy()) {
                session.Requirements().push(spawnedReview.Requirements()[i].Requirement);
            }
        }

        for (var k = 0; k < spawnedReview.Questions().length; k++) {
            if (spawnedReview.Questions()[k].Copy()) {
                session.Questions().push(spawnedReview.Questions()[k].Question);
            }
        }

        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('reviewsession'),
            data: ko.toJSON(session),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (response) {
                self.spawnCallBack(response);
                $('body').scrollTop(0);
                displayMessage("Session was sucessfully spawned", false);
            },
        });
    };

    self.spawnCallBack = function (response) {
    };
}
