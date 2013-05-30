var ProfileViewModel = function () {

    var self = this;
    self.accountId = ko.observable();
    self.account = new Account();
    self.websecurityUser = new Account();
    self.processingViewModel = new ProcessingViewModel();

    setupErrorHandling(self);

    self.updateProfile = function () {
        self.processingViewModel.turnOnProcessing("Updating Profile...");
        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('account/' + self.account.Id() + '/account'),
            data: ko.toJSON(self.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                self.processingViewModel.turnOffProcessing();
                displayMessage("Profile Successfully Updated");
            }
        });
    };

    self.changePassword = function () {
        self.processingViewModel.turnOnProcessing("Changing Password...");
        $.ajax({
            type: "POST",
            url: "http://localhost:49882/api/account",
            data: ko.toJSON(this.Data),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function() {
                self.processingViewModel.turnOffProcessing();
                displayMessage("Password Change Successful");
            }
        });
    };

    self.load = function () {
        ko.applyBindings(self);
        self.processingViewModel.turnOnProcessing("Loading Account...");
        $.getJSON('user.user', function (allData) {
            ko.mapping.fromJS(allData, {}, self.websecurityUser);
            $.getJSON(getArrApiUrl('account/' + self.websecurityUser.Username()), function (allData) {
                ko.mapping.fromJS(allData, {}, self.account);
                self.processingViewModel.turnOffProcessing();
            });
        });
    };

    self.load();
};


var profileViewModel = new ProfileViewModel();

