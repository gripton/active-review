var initialData = [
    { name: "Session Title 1" },
    { name: "Session Title 2" },
    { name: "Session Title 3" },
    { name: "aSession Title 4" },
    { name: "Session Title 5" }
];

var PagedGridModel = function (items) {
    var self = this;

    self.items = ko.observableArray(items);

    self.sortByName = function () {
        self.items.sort(function (a, b) {
            return a.name > b.name ? -1 : 1;
        });
    };

    self.jumpToFirstPage = function () {
        self.gridViewModel.currentPageIndex(0);
    };

    self.createNewSession = function () {
        return;
    }

    editSession = function () {
        return;
    };

    previewSession = function () {
        return;
    };

    assignReviewer = function () {
        return;
    };

    spawnSession = function () {
        return;
    };

    self.deleteSession = function (item) {
        self.items.remove(item);
    };

    self.gridViewModel = new ko.simpleGrid.viewModel({
        data: self.items,
        columns: [
            { headerText: "Session Title", rowText: "name" }
        ],
        pageSize: 4
    });
};

ko.applyBindings(new PagedGridModel(initialData));