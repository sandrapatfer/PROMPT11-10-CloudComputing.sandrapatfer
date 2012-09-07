package prompt.cloudnotes.activities;

import java.net.URI;
import java.net.URISyntaxException;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.client.utils.URLEncodedUtils;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.R;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.Window;
import android.webkit.CookieManager;
import android.webkit.CookieSyncManager;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;


public class AuthActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
        Log.d(CloudNotesApp.TAG, "AuthActivity.onCreate");

        // show loading progress
        requestWindowFeature(Window.FEATURE_PROGRESS);
        
        setContentView(R.layout.web_view);

        WebView webView = (WebView) findViewById(R.id.webview);
        
        webView.getSettings().setJavaScriptEnabled(true);
        
        webView.loadUrl(CloudNotesApp.WEB_APP_URL + "/OAuth2/Auth?response_type=code&client_id="
        + CloudNotesApp.CLIENT_ID + "&redirect_uri=" + CloudNotesApp.WEB_APP_REDIRECT_URI);
        
        webView.setWebChromeClient(new WebChromeClient() {
        	
			@Override
			public void onProgressChanged(WebView view, int newProgress) {
				super.onProgressChanged(view, newProgress);

				// Show loading progress in activity's title bar.
				setProgress(newProgress*100);
			}
		});
        
        webView.setWebViewClient(new WebViewClient() {
        
			@Override
			public void onPageFinished(WebView view, String url) {
				super.onPageFinished(view, url);
				
				Log.d(CloudNotesApp.TAG, "Page completed: " + url);
				
				if (url.toLowerCase().startsWith(CloudNotesApp.WEB_APP_REDIRECT_URI)) {
					
					try {
						List<NameValuePair> parts = URLEncodedUtils.parse(new URI(url), null);
						String pairName = parts.get(0).getName(); 
						if (pairName.contentEquals("code")) {
							Log.d(CloudNotesApp.TAG, "code: " + parts.get(0).getValue());
							
							Intent result = new Intent();
							result.putExtra(CloudNotesApp.CODE_TAG, parts.get(0).getValue().trim());
							setResult(RESULT_OK, result);
							finish();
							return;
							
						}
					} catch (URISyntaxException e) { 
						// TODO Auto-generated catch block
						e.printStackTrace();
					}					
				}
				else if (url.toLowerCase().contains("notapproved")) {
					// TODO show a pop up error to the user
					Log.d(CloudNotesApp.TAG, "ERROR authenticating, closing activity");
					Intent result = new Intent();
					setResult(RESULT_CANCELED, result);
					finish();
				}
			}

			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				super.onPageStarted(view, url, favicon);
	
				setTitle(url);
			}
        
			@Override
			public void onReceivedError(WebView view, int errorCode,
					String description, String failingUrl) {
				super.onReceivedError(view, errorCode, description, failingUrl);
				
				if (failingUrl.startsWith("http://localhost")) {
					failingUrl = failingUrl.replace("http://localhost", "http://10.0.2.2");
					view.loadUrl(failingUrl);
				}
			}
        });		
	}

}
