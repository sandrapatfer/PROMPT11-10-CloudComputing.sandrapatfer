function Note(name, description) {
    var self = this;
    self.name = name;
    self.description = description;
}

function NoteViewModel() {
    var self = this;

    self.listId = null;
    self.listCreatorId = null;
    self.postUrl = null;

    self.notes = ko.observableArray([]);

    self.name = ko.observable("new note");
    self.description = ko.observable("");

    self.addNote = function (data, event) {
        var name = self.name();
        var descr = self.description();
        jsUtils.Action("POST", self.postUrl,
        { name: name, description: descr, listId: self.listId, listCreatorId: self.listCreatorId },
        function (note) {
            if (note) {
                var match = ko.utils.arrayFirst(self.notes(), function (item) {
                    return item.id === note.id;
                });
                if (!match) {
                    self.notes.push({ id: note.id, name: ko.observable(name), description: ko.observable(descr) });
                }
                else {
                    match.name(name);
                    match.description(descr);
                }
            }
            var modal = $(event.target).parents(".modal");
            modal.modal('hide');
        });
    }

    self.editNoteClicked = function (note) {
        self.name(note.name());
        self.description(note.description());
        self.postUrl = "/Notes/Edit/" + note.id;
        $("#createNoteModal").modal('show');
    }

    self.checkNewNote = function (note) {
        if (note) {
            var match = ko.utils.arrayFirst(self.notes(), function (item) {
                return item.id === note.id;
            });
            if (!match) {
                self.notes.push({ id: note.id, name: ko.observable(name), description: ko.observable(descr) });
            }
            else {
                match.name(name);
                match.description(descr);
            }
        }
    }
}

$(function () {
    // Activates knockout.js
    var notesMainDiv = $("#list-notes");
    var noteModel = new NoteViewModel();
    var listId = notesMainDiv.attr("data-list-id");
    var userId = notesMainDiv.attr("data-user");
    noteModel.listId = listId;
    listCreatorId = notesMainDiv.attr("data-list-creatorId");
    noteModel.listCreatorId = listCreatorId;
    ko.applyBindings(noteModel, notesMainDiv[0]);


    var myHub = $.connection.notificationHub;
    $.connection.hub.start(function () {
        myHub.subscribe(userId, listId);
    });
    myHub.newNote = function (note) {
        noteModel.checkNewNote(note);
    };


    var url = "/Notes/?listId=" + listId + "&creatorId=" + listCreatorId;
    $.getJSON(url, function (data) {
        //noteModel.notes(data);
        $.each(data, function (i, item) {
            noteModel.notes.push({
                id: item.id,
                name: ko.observable(item.name),
                description: ko.observable(item.description)
            });
        });
    });

    // events
    $(".link-create-note").click(function (event) {
        event.preventDefault();
        var postUrl = $(event.target).attr("data-url");
        noteModel.postUrl = postUrl;
        noteModel.name("Insert here the name for the note...");
        noteModel.description("");
        $("#createNoteModal").modal('show');
    });

});