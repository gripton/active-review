function Profile() {
    var self = this;

    self.Firstname = ko.observable('Matt');
    self.Lastname = ko.observable('Schwartz');
    self.Username = ko.observable('thorfio');
    self.Email = ko.observable('thorfio@gmail.com');
}

var ProfileViewModel = function () {

    var self = this;
    self.myData = new Profile();

    self.fuck = function () {
        var test = ko.toJSON(this.myData);
        alert(test);
        $.post("http://localhost:55519/api/profile", test);
    };

    self.save = function () {
        $.ajax({
            type: "POST",
            url: "http://localhost:55519/api/profile",
            data: ko.toJSON(this.myData),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (data) { }
        });
    }
}

ko.applyBindings(new ProfileViewModel());


