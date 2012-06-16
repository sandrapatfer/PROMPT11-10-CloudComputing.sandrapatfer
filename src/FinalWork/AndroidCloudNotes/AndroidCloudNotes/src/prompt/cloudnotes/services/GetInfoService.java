package prompt.cloudnotes.services;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import prompt.cloudnotes.CloudNotesApp;
import android.app.IntentService;
import android.content.Intent;
import android.util.Log;

import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.HttpResponse;

public class GetInfoService extends IntentService {

	public GetInfoService() {
		super("Get Info Service");
	}

	@Override
	protected void onHandleIntent(Intent intent) {
		Log.d(CloudNotesApp.TAG, "GetInfoService.onHandleIntent");
		
		DefaultHttpClient client = new DefaultHttpClient();
		HttpGet getRequest = new HttpGet("http://localhost:8080/api/lists");
		getRequest.addHeader("accept", "aplication/json");
		
		try {
			HttpResponse response = client.execute(getRequest);
			if (response.getStatusLine().getStatusCode() != 200) {
				
			}
			
			BufferedReader br = new BufferedReader(new InputStreamReader((response.getEntity().getContent())));

			String output;
			Log.d(CloudNotesApp.TAG, "Output from Server .... \n");
			while ((output = br.readLine()) != null) {
				Log.d(CloudNotesApp.TAG, output);
			}

			client.getConnectionManager().shutdown();

	
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}

}
