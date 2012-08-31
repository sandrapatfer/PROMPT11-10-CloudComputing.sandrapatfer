package prompt.cloudnotes.providers;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.model.Note;
import prompt.cloudnotes.model.TaskList;
import android.content.ContentProvider;
import android.content.ContentValues;
import android.content.UriMatcher;
import android.database.Cursor;
import android.database.MatrixCursor;
import android.database.MatrixCursor.RowBuilder;
import android.net.Uri;
import android.util.Log;

public class NoteProvider extends ContentProvider {
	
	private CloudNotesApp _application;

	private static final UriMatcher _uriMatcher = new UriMatcher(UriMatcher.NO_MATCH);

	private static final int INDEX_LIST_LIST = 0;
	private static final int INDEX_LIST_ITEM = 1;
	private static final String[] _mimes = new String[] { 
		"vnd.android.cursor.dir/note",
		"vnd.android.cursor.item/note"
	};
	
	static {
		_uriMatcher.addURI(NoteProviderContract.AUTHORITY, "/", INDEX_LIST_LIST);
		_uriMatcher.addURI(NoteProviderContract.AUTHORITY, "#", INDEX_LIST_ITEM);
	}

	@Override
	public boolean onCreate() {
		_application = (CloudNotesApp)getContext().getApplicationContext();
		Log.d(CloudNotesApp.TAG, "NoteProvider.onCreate");
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
		if (selection == null || selection.isEmpty()) {
			return null;
		}
		Pattern pattern = Pattern.compile("list\\s*=\\s*(.*)"); 
		Matcher matcher = pattern.matcher(selection);
		if (!matcher.find() && matcher.groupCount() == 2) {
			return null;
		}
		String strListId = matcher.group(1);
		TaskList list = _application.Store.get(strListId);

		MatrixCursor cursor = new MatrixCursor(projection);
		if (match == INDEX_LIST_ITEM) {
			String itemId = uri.getLastPathSegment();
			Note n = list.getNotes().get(itemId);
			RowBuilder newRow = cursor.newRow();			
			for (String col : projection) {
				newRow.add(getNoteColumn(n, col));
			}
		}
		else if (match == INDEX_LIST_LIST) {
			Iterable<Note> notes = list.getNotes();
			for (Note n : notes) {
				RowBuilder newRow = cursor.newRow();
				for (String col : projection) {
					newRow.add(getNoteColumn(n, col));
				}
			}
			
			cursor.setNotificationUri(getContext().getContentResolver(), Uri.parse(NoteProviderContract.URI));
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
	
	private Object getNoteColumn(Note note, String colName) {
		if (colName == NoteProviderContract.COL_ID) {
			return note.getInternalId();
		}
		if (colName == NoteProviderContract.COL_NAME) {
			return note.getName();
		}
		if (colName == NoteProviderContract.COL_DESCR) {
			return note.getDescription();
		}
		
		return null;
	}
}

