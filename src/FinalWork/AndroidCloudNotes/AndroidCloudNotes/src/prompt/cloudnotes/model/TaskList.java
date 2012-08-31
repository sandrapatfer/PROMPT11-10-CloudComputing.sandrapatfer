package prompt.cloudnotes.model;

import prompt.cloudnotes.stores.NoteStore;

public class TaskList {
	private int _internalId;
	private String _serverId;
	private String _name;
	private String _description;
	private NoteStore _notes;
	
	public int getInternalId() {
		return _internalId;
	}
	public void setInternalId(int id) {
		_internalId = id;
	}

	public String getServerId() {
		return _serverId;
	}
	public void setServerId(String string) {
		_serverId = string;
	}

	public String getName() {
		return _name;
	}
	public void setName(String name) {
		_name = name;
	}
	
	public String getDescription() {
		return _description;
	}
	public void setDescription(String description) {
		_description = description;
	}

	public NoteStore getNotes() {
		return _notes;
	}
	public void setNotes(NoteStore notes) {
		_notes = notes;
	}

}

