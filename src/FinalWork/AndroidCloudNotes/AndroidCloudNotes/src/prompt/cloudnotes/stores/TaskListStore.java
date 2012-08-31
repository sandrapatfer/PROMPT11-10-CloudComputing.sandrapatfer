package prompt.cloudnotes.stores;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import prompt.cloudnotes.model.Note;
import prompt.cloudnotes.model.TaskList;

public class TaskListStore extends TStore<TaskList> {

	@Override
	public void add(List<TaskList> list) {
		// just replace the whole list
		ConcurrentHashMap<Long, TaskList> newStore = new ConcurrentHashMap<Long, TaskList>();
		for (TaskList l : list) {
			newStore.put((long)l.getInternalId(), l);
		}
		_store.set(newStore);
	}
	
	public void FillFromJson(JSONArray lists) {
		List<TaskList> newLists = new ArrayList<TaskList>();
		for (int i = 0; i < lists.length(); i++) {
			try {
				JSONObject list = lists.getJSONObject(i);
				TaskList tl = new TaskList();
				tl.setInternalId(i);
				tl.setServerId(list.getString("id"));
				tl.setName(list.getString("name"));
				newLists.add(tl);
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		add(newLists);
	}
}
