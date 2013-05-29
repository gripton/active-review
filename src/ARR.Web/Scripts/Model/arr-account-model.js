function Account() {
    var self = this;

    self.Id = ko.observable();
    self.ScreenName = ko.observable();
    self.EmailAddress = ko.observable();
    self.Organization = ko.observable();
    self.AreaOfExpertise = ko.observable();
    self.ConfirmPassword = ko.observable();
    self.Password = ko.observable();
}

