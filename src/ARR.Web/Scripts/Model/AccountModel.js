function Account() {
    var self = this;

    self.ScreenName = ko.observable();
    self.Username = ko.observable();
    self.Password = ko.observable();

}

var AccountViewModel = function () {

    var self = this;
    self.Data = new Account();  

    self.save = function () {
        $.ajax({
            type: "POST",
            url: "http://localhost:49882/api/account",
            data: ko.toJSON(this.Data),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (data) { $('#myModal').modal('show'); }
        });
    }

    self.login = function () {
        $.ajax({
            type: "POST",
            url: "login.login",
            data: ko.toJSON(this.Data),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (response) {
                location.href = "home.html";
            },
            error: function (response) {
                alert("Ajax Request Failed");
                //this.showError(response);
                //completeFunction();
            }
        });
    }
}

ko.applyBindings(new AccountViewModel());


