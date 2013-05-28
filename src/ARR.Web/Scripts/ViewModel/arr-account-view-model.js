var AccountViewModel = function() {
    var self = this;
    self.account = new Account();

    self.processingViewModel = new ProcessingViewModel();

    self.create = function () {
        self.processingViewModel.turnOnProcessing("Creating Account");
        $.ajax({
            type: "POST",
            url: getArrApiUrlPost('account/'),
            data: ko.toJSON(this.account),
            contentType: 'application/json',
            dataType: 'JSON',
            success: function () {
                self.processingViewModel.turnOffProcessing();
                $('#myModal').modal('show');
            }
        });
    };
};

ko.applyBindings(new AccountViewModel());

