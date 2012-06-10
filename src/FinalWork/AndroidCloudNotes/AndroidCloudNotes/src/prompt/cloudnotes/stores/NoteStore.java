package prompt.cloudnotes.stores;

import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

import prompt.cloudnotes.model.Note;

public class NoteStore extends TStore<Note> {

	@Override
	public void add(List<Note> list) {
		// just replace the whole list
		ConcurrentHashMap<Long, Note> newStore = new ConcurrentHashMap<Long, Note>();
		for (Note l : list) {
			newStore.put((long) l.getId(), l);
		}
		_store.set(newStore);
		
	}

}
