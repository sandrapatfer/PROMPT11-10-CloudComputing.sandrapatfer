package prompt.cloudnotes.providers;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.model.TaskList;
import android.content.ContentProvider;
import android.content.ContentValues;
import android.content.UriMatcher;
import android.database.Cursor;
import android.database.MatrixCursor;
import android.database.MatrixCursor.RowBuilder;
import android.net.Uri;
import android.util.Log;

public class TaskListProvider extends ContentProvider {
	
	private CloudNotesApp _application;

	private static final UriMatcher _uriMatcher = new UriMatcher(UriMatcher.NO_MATCH);

	private static final int INDEX_LIST_LIST = 0;
	private static final int INDEX_LIST_ITEM = 1;
	private static final String[] _mimes = new String[]
	{ 
		"vnd.android.cursor.dir/list",
		"vnd.android.cursor.item/list"
	};
	
	static
	{
		_uriMatcher.addURI(TaskListProviderContract.AUTHORITY, "/", INDEX_LIST_LIST);
		_uriMatcher.addURI(TaskListProviderContract.AUTHORITY, "#", INDEX_LIST_ITEM);
	}


	@Override
	public boolean onCreate() {
		_application = (CloudNotesApp)getContext().getApplicationContext();
		Log.d(CloudNotesApp.TAG, "TaskListProvider.onCreate");
		return false;
	}

	@Override
	public String getType(Uri uri) {
		int match = _uriMatcher.match(uri);
		if (match == UriMatcher.NO_MATCH) {
			return null;
		}
		return _mimes[match];
	}

	@Override
	public Cursor query(Uri uri, String[] projection, String selection, String[] selectionArgs, String sortOrder) {
		int match = _uriMatcher.match(uri);
		if (match == UriMatcher.NO_MATCH) {
			return null;
		}

		MatrixCursor cursor = new MatrixCursor(projection);
		if (match == INDEX_LIST_ITEM) {
			String itemId = uri.getLastPathSegment();
			TaskList tl = _application.Store.get(itemId);
			RowBuilder newRow = cursor.newRow();			
			for (String col : projection) {
				newRow.add(getTaskListColumn(tl, col));
			}
		}
		else if (match == INDEX_LIST_LIST) {
			Iterable<TaskList> list = _application.Store;
			for (TaskList s : list) {
				RowBuilder newRow = cursor.newRow();
				for (String col : projection) {
					newRow.add(getTaskListColumn(s, col));
				}
			}
			cursor.setNotificationUri(getContext().getContentResolver(), Uri.parse(TaskListProviderContract.URI));
		}
		return cursor;
	}

	@Override
	public int delete(Uri arg0, String arg1, String[] arg2) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public Uri insert(Uri uri, ContentValues values) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public int update(Uri uri, ContentValues values, String selection,
			String[] selectionArgs) {
		// TODO Auto-generated method stub
		return 0;
	}
	
	private Object getTaskListColumn(TaskList list, String colName) {
		if (colName == TaskListProviderContract.COL_ID) {
			return list.getInternalId();
		}
		if (colName == TaskListProviderContract.COL_NAME) {
			return list.getName();
		}
		if (colName == TaskListProviderContract.COL_DESCR) {
			return list.getDescription();
		}
		
		return null;
	}
}

