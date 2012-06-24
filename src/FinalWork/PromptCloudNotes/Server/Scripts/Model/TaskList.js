function TaskList(name, description) {
    var self = this;
    self.name = name;
    self.description = description;
}

function TaskListViewModel() {
    var self = this;

    self.name = ko.observable("write");
    self.description = ko.observable("");

    self.addTaskList = function (data, event) {
        var name = self.name();
        var descr = self.description();
        var action = $(addTaskList).attr("action");
        jsUtils.Action("POST", action, { name: name, description: descr });
    }
}

$(function () {
    // Activates knockout.js
    var form = $("#createListModal");
    ko.applyBindings(new TaskListViewModel(), form[0]);
});