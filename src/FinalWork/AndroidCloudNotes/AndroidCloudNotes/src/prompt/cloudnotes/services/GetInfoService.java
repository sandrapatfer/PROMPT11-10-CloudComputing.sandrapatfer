package prompt.cloudnotes.services;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.Utils;
import android.app.IntentService;
import android.content.Intent;
import android.util.Log;

import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.HttpResponse;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class GetInfoService extends IntentService {

	private CloudNotesApp _application;
	
	public GetInfoService() {
		super("Get Info Service");
		
		_application = (CloudNotesApp)getApplication();
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		
		if (_application == null) {
			_application = (CloudNotesApp)getApplication();
		}
		
		Log.d(CloudNotesApp.TAG, "GetInfoService.onHandleIntent");
		
		DefaultHttpClient client = new DefaultHttpClient();
		HttpGet getRequest = new HttpGet("http://10.0.2.2:53484/api/lists");
		getRequest.addHeader("accept", "aplication/json");
		getRequest.addHeader("authorization", "Bearer " + _application.Token);
		
		try {
			HttpResponse response = client.execute(getRequest);
			if (response.getStatusLine().getStatusCode() != 200) {
				
			}
			
			String output = Utils.ConvertStreamToString(response.getEntity().getContent());
			Log.d(CloudNotesApp.TAG, "Output from Server ...." + output);

			JSONArray lists = new JSONArray(output);
			Log.d(CloudNotesApp.TAG, "json from Server ...." + lists.length());
			
			client.getConnectionManager().shutdown();

	
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}

}
