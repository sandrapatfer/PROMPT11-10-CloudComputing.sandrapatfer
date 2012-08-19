function TaskList(name, description) {
    var self = this;
    self.name = name;
    self.description = description;
}

function TaskListViewModel() {
    var self = this;

    self.name = ko.observable("Insert here the name for the note list...");
    self.description = ko.observable("");

    self.doActionTaskList = function (data, event) {
        var name = self.name();
        var descr = self.description();
        var modal = $(event.target).parents(".modal");
        var form = modal.find("form");
        var action = form.attr("action");
        jsUtils.Action("POST", action, { name: name, description: descr });
    }
}

$(function () {
    // Activates knockout.js
    var form = $("#createListModal");
    ko.applyBindings(new TaskListViewModel(), form[0]);

    // events
    $(".link-edit-tasklist").click(function (event) {
        event.preventDefault();
        var target = $(event.target).attr("href");
        $.get(target, function (data) {
            $("#modalContainer").html(data);
            $("#editListModal").modal('show');
        });
    });

    $(".link-share-tasklist").click(function (event) {
        event.preventDefault();
        var target = $(event.target).attr("href");
        $.get(target, function (data) {
            $("#modalContainer").html(data);
            $("#shareListModal").modal('show');
        });
    });

});

function ShareTaskListModel(listId) {
    var self = this;
    self.listId = listId;

    self.users = ko.observableArray([]);
    self.selectedUser = ko.observable();

    self.doActionTaskList = function (data, event) {
        var user = self.selectedUser();
        // TODO error message instead of not closing the dialog...
        if (user) {
            var modal = $(event.target).parents(".modal");
            var form = modal.find("form");
            var action = form.attr("action");
            jsUtils.Action("POST", action, { userId: user.id() });
        }
    }

    $.getJSON("/users/TaskListNotShared?listId=" + self.listId, function (data) {
        $.each(data, function (i, item) {
            self.users.push({
                id: ko.observable(item.id),
                name: ko.observable(item.name)
            });
        });
    });
}
