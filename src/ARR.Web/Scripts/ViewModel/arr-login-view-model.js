var LoginViewModel = function() {

    var self = this;
    self.account = new Account();
    
    self.login = function() {
        $.ajax({
            type: "POST",
            url: "login.login",
            data: ko.toJSON(this.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                $.cookie('arr_account', self.account.Username);
                location.href = "/screens/home.html";            },
            error: function() {
                alert("Ajax Request Failed");
                //this.showError(response);
                //completeFunction();
            }
        });
    };
};

ko.applyBindings(new LoginViewModel());

