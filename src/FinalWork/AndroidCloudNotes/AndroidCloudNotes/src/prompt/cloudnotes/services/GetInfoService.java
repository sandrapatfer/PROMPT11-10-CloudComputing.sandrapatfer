package prompt.cloudnotes.services;

import prompt.cloudnotes.CloudNotesApp;
import android.app.IntentService;
import android.content.Intent;
import android.util.Log;

public class GetInfoService extends IntentService {

	public GetInfoService() {
		super("Get Info Service");
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		Log.d(CloudNotesApp.TAG, "GetInfoService.onHandleIntent");
		
		HttpClient
	}

}
