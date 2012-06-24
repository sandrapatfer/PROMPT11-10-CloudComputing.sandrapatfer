function Note(name, description) {
    var self = this;
    self.name = name;
    self.description = description;
}

function NoteViewModel() {
    var self = this;

    self.name = ko.observable("new note");
    self.description = ko.observable("");

    self.addNote = function (listId, data, event) {
        var name = self.name();
        var descr = self.description();
        var action = $(addNote).attr("action");
        jsUtils.Action("POST", action, { name: name, description: descr, listId: listId });
    }
}

$(function () {
    // Activates knockout.js
    ko.applyBindings(new NoteViewModel(), $("#createNoteModal")[0]);
});