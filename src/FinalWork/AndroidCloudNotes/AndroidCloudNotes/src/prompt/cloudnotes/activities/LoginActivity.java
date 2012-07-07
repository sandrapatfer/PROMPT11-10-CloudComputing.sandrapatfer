package prompt.cloudnotes.activities;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONException;
import org.json.JSONObject;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.Utils;
import prompt.cloudnotes.services.GetInfoService;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class LoginActivity extends Activity {
	
	public static final String CODE_TAG = "code";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		Log.d(CloudNotesApp.TAG, "Verifiying login");
		Intent intent = new Intent();
		intent.setClass(this, AuthActivity.class);
		startActivityForResult(intent, 0);
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);

		Log.d(CloudNotesApp.TAG, "LoginActivity.onActivityResult");

		if (requestCode != 0) {
			return;
		}
		
		if (resultCode != RESULT_OK && data == null) {
			return;
		}
		
		String code = data.getStringExtra(CODE_TAG);
	
		HttpClient client = new DefaultHttpClient();
		HttpPost postRequest = new HttpPost("http://10.0.2.2:53484/OAuth2/Token");
		postRequest.addHeader("accept", "aplication/json");

		List<NameValuePair> formData = new ArrayList<NameValuePair>();
		formData.add(new BasicNameValuePair("code", code));
		formData.add(new BasicNameValuePair("redirect_uri", "http://10.0.2.2:53484"));
		formData.add(new BasicNameValuePair("grant_type", "authorization_code"));
		formData.add(new BasicNameValuePair("client_id", "android notes"));
		formData.add(new BasicNameValuePair("client_secret", "1"));
		
		try {
			postRequest.setEntity(new UrlEncodedFormEntity(formData));
			HttpResponse response = client.execute(postRequest);
			
			String output = Utils.ConvertStreamToString(response.getEntity().getContent());
			Log.d(CloudNotesApp.TAG, "Output from Server: " + output);
			
			// TODO authenticate the application as a user in the server???
			// at the moment the token request is not authenticated 
			
			JSONObject jsonResponse = new JSONObject(output);
			String token = jsonResponse.getString("access_token");
			Log.d(CloudNotesApp.TAG, "Token: " + token);
			CloudNotesApp.Token = token;
			
			Log.d(CloudNotesApp.TAG, "Starting refresh service");
			Intent msg = new Intent();
			msg.setClass(this, GetInfoService.class);
			startService(msg);
			
			Log.d(CloudNotesApp.TAG, "Starting lists activity");
			Intent lists = new Intent();
			lists.setClass(this, TaskListsActivity.class);
			startActivity(lists);

		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		} catch (ClientProtocolException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

}
