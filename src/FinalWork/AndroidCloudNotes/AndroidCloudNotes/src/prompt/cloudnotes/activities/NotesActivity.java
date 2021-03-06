package prompt.cloudnotes.activities;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.R;
import prompt.cloudnotes.providers.NoteProviderContract;
import android.app.Activity;
import android.content.ContentUris;
import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CursorAdapter;
import android.widget.ListView;
import android.widget.TextView;

public class NotesActivity extends Activity {

	private Cursor _cursor;
	private final int COL_INDEX_ID = 0;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        Log.d(CloudNotesApp.TAG, "NotesActivity.onCreate");

        setContentView(R.layout.notes_list_view);
        
        Button btn = (Button)findViewById(R.id.btn_note_create);
        btn.setOnClickListener(new View.OnClickListener() {
			public void onClick(View v) {
				Log.i(CloudNotesApp.TAG, "btn_note_create onClick");
				Intent intent = new Intent();
				intent.setClass(NotesActivity.this, NoteActivity.class);				
				startActivity(intent);
			}
		});
        
        ListView listView = (ListView)findViewById(R.id.notes_listview);
        
        _cursor = getContentResolver().query(Uri.parse(NoteProviderContract.URI), 
        		new String[] { NoteProviderContract.COL_ID, NoteProviderContract.COL_NAME}, 
        		"list = 1", null, null);
        startManagingCursor(_cursor);
        
        final NoteAdapter adapter = new NoteAdapter(this, _cursor);
        listView.setAdapter(adapter);

    }
    private class NoteAdapter extends CursorAdapter {
    	
    	public NoteAdapter(Context context, Cursor cursor) {
    		super(context, cursor);
    	}

    	@Override
    	public void bindView(View view, Context context, Cursor cursor) {
    		int colIndex = cursor.getColumnIndex(NoteProviderContract.COL_NAME);
    		String text = cursor.getString(colIndex);
    		TextView noteNameView = (TextView) view.findViewById(R.id.noteitem_name);
    		noteNameView.setText(text);
    	}

    	@Override
    	public View newView(Context context, Cursor cursor, ViewGroup parent) {
    		View newItemView = NotesActivity.this.getLayoutInflater().inflate(R.layout.notes_list_view_item, null);
    		TextView noteNameView = (TextView) newItemView.findViewById(R.id.noteitem_name);
    		int index = cursor.getColumnIndex(NoteProviderContract.COL_NAME);
    		String text = cursor.getString(index);
    		noteNameView.setText(text);
			final Uri itemUri = ContentUris.withAppendedId(Uri.parse(NoteProviderContract.URI), cursor.getLong(COL_INDEX_ID));
    		
            Button btn = (Button)newItemView.findViewById(R.id.btn_note_edit);
            btn.setOnClickListener(new View.OnClickListener() {        	
    			public void onClick(View v) {
    				Log.i(CloudNotesApp.TAG, "btn_note_edit onClick");
    				Intent intent = new Intent();
    				intent.setClass(NotesActivity.this, NoteActivity.class)
    					.setData(itemUri);
    				startActivity(intent);
    			}
    		});
                        
    		return newItemView;
    	}
    	
		@Override
		protected void onContentChanged() {
			super.onContentChanged();
			
			Log.d(CloudNotesApp.TAG, "NoteAdapter.onContentChanged");

			_cursor = getContentResolver().query(Uri.parse(NoteProviderContract.URI),
					new String[] {NoteProviderContract.COL_ID, NoteProviderContract.COL_NAME},
					null, null, null);
			startManagingCursor(_cursor);
			changeCursor(_cursor);				
		}
    }
}
