package prompt.cloudnotes.stores;

import java.util.Iterator;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicReference;

public abstract class TStore<T> implements Iterable<T> {

	protected AtomicReference<ConcurrentHashMap<Long, T>> _store;

	public TStore() {
		_store = new AtomicReference<ConcurrentHashMap<Long, T>>();
		_store.set(new ConcurrentHashMap<Long, T>());
	}
	
	public Iterator<T> iterator() {
		return _store.get().values().iterator();
	}

	public T get(String id) {
		return _store.get().get(id);
	}

	public abstract void add(List<T> list);

}
