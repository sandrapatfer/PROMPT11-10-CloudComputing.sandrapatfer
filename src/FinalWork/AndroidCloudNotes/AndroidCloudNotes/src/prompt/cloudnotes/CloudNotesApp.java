package prompt.cloudnotes;

import java.util.ArrayList;
import java.util.List;

import prompt.cloudnotes.activities.AuthActivity;
import prompt.cloudnotes.activities.TaskListsActivity;
import prompt.cloudnotes.model.TaskList;
import prompt.cloudnotes.providers.TaskListProviderContract;
import prompt.cloudnotes.services.GetInfoService;
import prompt.cloudnotes.stores.TaskListStore;
import android.app.Application;
import android.content.Intent;
import android.net.Uri;
import android.util.Log;

public class CloudNotesApp extends Application
{
	public static final String TAG = "CLOUD_NOTES";
	
	public static String Token;
	
	public TaskListStore Store;
	
	@Override
	public void onCreate() {
		
		Log.d(TAG, "CloudNotesApp.onCreate");
		
		Store = new TaskListStore();
		
		super.onCreate();
				
	}

}
