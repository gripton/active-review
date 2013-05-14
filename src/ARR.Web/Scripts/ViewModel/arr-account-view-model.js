var AccountViewModel = function() {

    var self = this;
    self.accountId = -1//accountId;
    self.account = new Account();
    self.websecurityUser = new Account();

    self.create = function() {
        $.ajax({
            type: "POST",
            url: "http://localhost:49882/api/account",
            data: ko.toJSON(self.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function(data) { $('#myModal').modal('show'); }
        });
    };
    
    self.updateProfile = function () {
        $.ajax({
            type: "PUT",
            url: getArrApiUrlPost('account/' + self.reviewSessionId),
            data: ko.toJSON(self.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                alert('success!');
            }
        });
    };

    self.changePassword = function() {
        $.ajax({
            type: "POST",
            url: "http://localhost:49882/api/account",
            data: ko.toJSON(this.Data),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function(data) { $('#myModal').modal('show'); }
        });
    };

    self.login = function() {
        $.ajax({
            type: "POST",
            url: "login.login",
            data: ko.toJSON(this.Data),
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
    
    self.load = function () {
        ko.applyBindings(self);
        
        $.getJSON(getArrApiUrl('user.user'), function (allData) {
            ko.mapping.fromJS(allData, {}, self.websecurityUser);
            console.log(ko.toJSON(self.websecurityUser));
        });

        $.getJSON(getArrApiUrl('account/' + self.websecurityUser.Id), function (allData) {
                ko.mapping.fromJS(allData, {}, self.account);
                console.log(ko.toJSON(self.account));
            });
    };

    self.load();
};


var accountViewModel = new AccountViewModel();

