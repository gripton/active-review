var AccountViewModel = function() {

    var self = this;
    self.account = new Account();

    self.create = function() {
        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('account/'),
            data: ko.toJSON(this.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function(data) { $('#myModal').modal('show'); }
        });
    };
};

ko.applyBindings(new AccountViewModel());

