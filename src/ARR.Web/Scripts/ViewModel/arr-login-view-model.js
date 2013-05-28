var LoginViewModel = function() {

    var self = this;
    self.account = new Account();

    self.processingViewModel = new ProcessingViewModel();

    self.login = function () {
        self.processingViewModel.turnOnProcessing("Logging in");
        $("#InvalidPassword").hide();
        $.ajax({
            type: "POST",
            url: "login.login",
            data: ko.toJSON(this.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (response) {
                if (response.Status == 'Authenticated') {
                    $.removeCookie('arr_account');
                    $.cookie('arr_account', self.account.Username, { path: "/" });
                    location.href = "/screens/home.html";
                } else {
                    $("#InvalidPassword").show();
                }
                self.processingViewModel.turnOffProcessing();
            },
            error: function() {
                alert("Ajax Request Failed");
                self.processingViewModel.turnOffProcessing();
            }
        });
    };
};

ko.applyBindings(new LoginViewModel());

