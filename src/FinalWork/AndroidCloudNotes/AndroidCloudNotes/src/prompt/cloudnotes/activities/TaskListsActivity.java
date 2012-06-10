package prompt.cloudnotes.activities;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.R;
import prompt.cloudnotes.providers.NoteProviderContract;
import prompt.cloudnotes.providers.TaskListProviderContract;
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
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.CursorAdapter;
import android.widget.TextView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ListView;

public class TaskListsActivity extends Activity
{
	private Cursor _cursor;
	private final int COL_INDEX_ID = 0;

	/** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        Log.d(CloudNotesApp.TAG, "TaskListsActivity.onCreate");

        CloudNotesApp app = (CloudNotesApp)getBaseContext().getApplicationContext();
        app.Store.fillTestValue();
		app.getContentResolver().notifyChange(Uri.parse(TaskListProviderContract.URI), null);
		app.getContentResolver().notifyChange(Uri.parse(NoteProviderContract.URI), null);
        
        setContentView(R.layout.tasklist_list_view);
        
        Button btn = (Button)findViewById(R.id.btn_tasklist_create);
        btn.setOnClickListener(new View.OnClickListener() {
			public void onClick(View v) {
				Log.i(CloudNotesApp.TAG, "btn_tasklist_create onClick");
				Intent intent = new Intent();
				intent.setClass(TaskListsActivity.this, TaskListActivity.class);				
				startActivity(intent);
			}
		});

        
        ListView listView = (ListView)findViewById(R.id.tasklist_listview);
        
        _cursor = getContentResolver().query(Uri.parse(TaskListProviderContract.URI), 
        		new String[] { TaskListProviderContract.COL_ID, TaskListProviderContract.COL_NAME}, 
        		null, null, null);
        startManagingCursor(_cursor);
        
        final TaskListAdapter adapter = new TaskListAdapter(this, _cursor);
        listView.setAdapter(adapter);

    }
    
    private class TaskListAdapter extends CursorAdapter {
    	
    	public TaskListAdapter(Context context, Cursor cursor) {
    		super(context, cursor);
    	}

    	@Override
    	public void bindView(View view, Context context, Cursor cursor) {
    		int colIndex = cursor.getColumnIndex(TaskListProviderContract.COL_NAME);
    		String text = cursor.getString(colIndex);
    		TextView listNameView = (TextView) view.findViewById(R.id.tasklistitem_name);
    		listNameView.setText(text);
    	}

    	@Override
    	public View newView(Context context, Cursor cursor, ViewGroup parent) {
    		View newItemView = TaskListsActivity.this.getLayoutInflater().inflate(R.layout.tasklist_list_view_item, null);
    		TextView listNameView = (TextView) newItemView.findViewById(R.id.tasklistitem_name);
    		int index = cursor.getColumnIndex(TaskListProviderContract.COL_NAME);
    		String text = cursor.getString(index);
    		listNameView.setText(text);
			final Uri itemUri = ContentUris.withAppendedId(Uri.parse(TaskListProviderContract.URI), cursor.getLong(COL_INDEX_ID));
    		
            Button btn = (Button)newItemView.findViewById(R.id.btn_tasklist_edit);
            btn.setOnClickListener(new View.OnClickListener() {        	
    			public void onClick(View v) {
    				Log.i(CloudNotesApp.TAG, "btn_tasklist_edit onClick");
    				Intent intent = new Intent();
    				intent.setClass(TaskListsActivity.this, TaskListActivity.class)
    					.setData(itemUri);
    				startActivity(intent);
    			}
    		});
                        
            btn = (Button)newItemView.findViewById(R.id.btn_notes);
            btn.setOnClickListener(new View.OnClickListener() {        	
    			public void onClick(View v) {
    				Log.i(CloudNotesApp.TAG, "btn_notes onClick");
    				Intent intent = new Intent();
    				intent.setClass(TaskListsActivity.this, NotesActivity.class)
    					.setData(itemUri);
    				startActivity(intent);
    			}
    		});

    		return newItemView;
    	}
    	
		@Override
		protected void onContentChanged() {
			super.onContentChanged();
			
			Log.d(CloudNotesApp.TAG, "TaskListAdapter.onContentChanged");

			_cursor = getContentResolver().query(Uri.parse(TaskListProviderContract.URI),
					new String[] {TaskListProviderContract.COL_ID, TaskListProviderContract.COL_NAME},
					null, null, null);
			startManagingCursor(_cursor);
			changeCursor(_cursor);				
		}


    }
}