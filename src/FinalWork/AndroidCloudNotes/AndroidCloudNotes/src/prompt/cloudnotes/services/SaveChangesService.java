package prompt.cloudnotes.services;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.model.Note;
import android.app.IntentService;
import android.app.Service;
import android.content.Intent;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;

public class SaveChangesService extends IntentService {

	private CloudNotesApp _application;
	
	public static String NoteKey = "NoteObject";
	
	public SaveChangesService() {
		super("Changes update service");
	}
	
	@Override
	public void onCreate() {
		super.onCreate();
		_application = (CloudNotesApp)getApplication();
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		Log.d(CloudNotesApp.TAG, "SaveChangesService.onHandleIntent");

		Bundle extras = intent.getExtras();
		if (extras == null) {
			return;
		}
		if (extras.containsKey(NoteKey)) {
			Note note = (Note)extras.get(NoteKey);
			Log.d(CloudNotesApp.TAG, "saving note changes");
		}
	}

}
