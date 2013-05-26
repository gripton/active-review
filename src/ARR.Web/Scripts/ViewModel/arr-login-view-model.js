var LoginViewModel = function() {

    var self = this;
    self.account = new Account();
    
    self.login = function () {
        $("#InvalidPassword").hide();
        $.ajax({
            type: "POST",
            url: "login.login",
            data: ko.toJSON(this.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (response) {
                if (response.Status == 'Authenticated') {
                    $.cookie('arr_account', self.account.Username);
                    location.href = "/screens/home.html";
                } else {
                    $("#InvalidPassword").show();
                }
            },
            error: function() {
                alert("Ajax Request Failed");
                //this.showError(response);
                //completeFunction();
            }
        });
    };
};

ko.applyBindings(new LoginViewModel());

