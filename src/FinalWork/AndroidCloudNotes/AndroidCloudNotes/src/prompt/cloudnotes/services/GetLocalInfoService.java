package prompt.cloudnotes.services;

import prompt.cloudnotes.CloudNotesApp;
import android.app.IntentService;
import android.content.Intent;
import android.util.Log;

public class GetLocalInfoService extends IntentService {

	public GetLocalInfoService() {
		super("Get Local Info Service");
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		Log.d(CloudNotesApp.TAG, "GetLocalInfoService.onHandleIntent");
			
	}

}
