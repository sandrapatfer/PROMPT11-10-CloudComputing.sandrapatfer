package prompt.cloudnotes.model;

public class Note {
	
	private int _internalId;
	private String _serverId;
	private String _name;
	private String _description;
	
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
}
