var ProfileViewModel = function () {

    var self = this;
    self.accountId = ko.observable();
    self.account = new Account();
    self.websecurityUser = new Account();

    self.updateProfile = function () {
        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('account/' + self.account.Id() + '/account'),
            data: ko.toJSON(self.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                alert('success!');
            }
        });
    };

    self.changePassword = function () {
        $.ajax({
            type: "POST",
            url: "http://localhost:49882/api/account",
            data: ko.toJSON(this.Data),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function (data) { $('#myModal').modal('show'); }
        });
    };

    self.load = function () {
        ko.applyBindings(self);

        $.getJSON('user.user', function (allData) {
            ko.mapping.fromJS(allData, {}, self.websecurityUser);
            console.log(ko.toJSON(self.websecurityUser));
            $.getJSON(getArrApiUrl('account/' + self.websecurityUser.Username()), function (allData) {
                ko.mapping.fromJS(allData, {}, self.account);
                console.log(self.account.ScreenName());
            });
        });
    };

    self.load();
};


var profileViewModel = new ProfileViewModel();

