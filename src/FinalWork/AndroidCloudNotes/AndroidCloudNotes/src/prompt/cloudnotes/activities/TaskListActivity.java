package prompt.cloudnotes.activities;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.R;
import prompt.cloudnotes.providers.TaskListProviderContract;
import android.app.Activity;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

public class TaskListActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		Log.d(CloudNotesApp.TAG, "TaskListActivity.onCreate");
				
        setContentView(R.layout.tasklist_view);
        
        TextView nameTextView = (TextView)findViewById(R.id.text_taskListName);
        TextView descrTextView = (TextView)findViewById(R.id.text_taskListDescription);
        Button btnSave = (Button)findViewById(R.id.btn_taskListSave);
        final Button btnNotes = (Button)findViewById(R.id.btn_notes);
		
		Uri itemUri = getIntent().getData();
		if (itemUri != null) {
			Cursor cursor = getContentResolver().query(itemUri,
				new String[] {TaskListProviderContract.COL_ID, TaskListProviderContract.COL_NAME, TaskListProviderContract.COL_DESCR},
				null, null, null);
			if (cursor.moveToFirst()) {
				String userText = cursor.getString(cursor.getColumnIndex(TaskListProviderContract.COL_NAME)); 
				nameTextView.setText(userText);
				descrTextView.setText(cursor.getString(cursor.getColumnIndex(TaskListProviderContract.COL_DESCR)));
			}
			else {
				Log.e(CloudNotesApp.TAG, "query returned no rows!");
			}
		}
		else {
			Log.d(CloudNotesApp.TAG, "creating a new list");
			btnNotes.setEnabled(false);
		}
			
		
		btnSave.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
				// TODO send intent to service
				btnNotes.setEnabled(true);
			}
		});
		
		btnNotes.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
				Log.i(CloudNotesApp.TAG, "btn_notes onClick");
				Intent intent = new Intent();
				intent.setClass(TaskListActivity.this, NotesActivity.class);				
				startActivity(intent);				
			}
		});
	}

}
