package prompt.cloudnotes.model;

import java.io.Serializable;

public class Note implements Serializable {
	
	private int _id;
	private String _name;
	private String _description;
	
	public int getId() {
		return _id;
	}

	public void setId(int id) {
		_id = id;
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
