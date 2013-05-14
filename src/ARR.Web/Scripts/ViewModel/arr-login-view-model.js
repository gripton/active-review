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
            success: function(response) {
                location.href = "home.html";
            },
            error: function(response) {
                alert("Ajax Request Failed");
                //this.showError(response);
                //completeFunction();
            }
        });
    };
};

ko.applyBindings(new LoginViewModel());

