function TaskListViewModel() {
    var self = this;

    self.editUrl = null;
    self.listCreatorId = null;
    self.name = ko.observable("");
    self.description = ko.observable("");

    self.doActionTaskList = function (data, event) {
        var name = self.name();
        var descr = self.description();

        var action = self.editUrl;
        if (self.editUrl == null) {
            var modal = $(event.target).parents(".modal");
            var form = modal.find("form");
            action = form.attr("action");
        }
        jsUtils.Action("POST", action, { name: name, description: descr, creatorId: self.creatorId });
    }
}

$(function () {
    // Activates knockout.js
    var modalDiv = $("#createEditListModal");
    var taskListModel = new TaskListViewModel();
    ko.applyBindings(taskListModel, modalDiv[0]);

    modalDiv = $("#shareListModal");
    var shareModel = new ShareTaskListModel();
    ko.applyBindings(shareModel, modalDiv[0]);

    // events
    $(".link-create-tasklist").click(function (event) {
        event.preventDefault();
        taskListModel.editUrl = null;
        taskListModel.name("Insert here the name for the note list...");
        taskListModel.description("");
        $("#createEditListModal").modal('show');
    });

    $(".link-edit-tasklist").click(function (event) {
        event.preventDefault();
        var postUrl = $(event.target).attr("data-edit-list-url");
        var listCreatorId = $(event.target).attr("data-list-creatorId");
        var listName = $(event.target).attr("data-list-name");
        var listDescr = $(event.target).attr("data-list-description");
        taskListModel.editUrl = postUrl;
        taskListModel.listCreatorId = listCreatorId;
        taskListModel.name(listName);
        taskListModel.description(listDescr);
        $("#createEditListModal").modal('show');
    });

    $(".link-share-tasklist").click(function (event) {
        event.preventDefault();
        var target = $(event.target).attr("href");
        var listId = $(event.target).attr("data-list-id");
        var listCreatorId = $(event.target).attr("data-list-creatorId");
        var postUrl = $(event.target).attr("data-share-list-url");

        shareModel.listId = listId;
        shareModel.listCreatorId = listCreatorId;

        shareModel.getUsers();
        $("#shareListModal").modal('show');
    });

});

function ShareTaskListModel() {
    var self = this;

    self.listId = null;
    self.listCreatorId = null;

    self.users = ko.observableArray([]);
    self.selectedUser = ko.observable();

    self.doActionTaskList = function (data, event) {
        var user = self.selectedUser();
        // TODO error message instead of not closing the dialog...
        if (user) {
            var modal = $(event.target).parents(".modal");
            var form = modal.find("form");
            var action = form.attr("action");
            jsUtils.Action("POST", action, { userId: user.id(), listId: self.listId, creatorId: self.listCreatorId });
        }
    }

    self.getUsers = function () {
        $.getJSON("/users/TaskListNotShared?listId=" + self.listId, function (data) {
            $.each(data, function (i, item) {
                self.users.push({
                    id: ko.observable(item.id),
                    name: ko.observable(item.name)
                });
            });
        });
    }
}
