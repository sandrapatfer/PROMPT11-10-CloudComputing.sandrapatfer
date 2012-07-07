package prompt.cloudnotes.activities;

import prompt.cloudnotes.CloudNotesApp;
import prompt.cloudnotes.R;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
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
        
        // TODO read from preferences
        webView.loadUrl("http://10.0.2.2:53484/OAuth2/Auth");
        
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
				
				if (url.toLowerCase().contains("approval")) {
					CookieSyncManager.getInstance().sync();
					// Get the code cookie
					String cookie = CookieManager.getInstance().getCookie(url);
					if (cookie != null) {
						// Cookie is a string like NAME=VALUE [; NAME=VALUE]
						String[] pairs = cookie.split(";");
						for (String pair : pairs) {
							String[] parts = pair.split("=", 2);
							if (parts.length == 2 && parts[0].trim().equalsIgnoreCase("code")) {
								Log.d(CloudNotesApp.TAG, "code: " + parts[1]);
								
								Intent result = new Intent();
								result.putExtra(LoginActivity.CODE_TAG, parts[1].trim());
								setResult(RESULT_OK, result);
								finish();
								return;
							}
						}
					}
					
					Log.e(CloudNotesApp.TAG, "Cookie not found in URL");
				}
			}

			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				super.onPageStarted(view, url, favicon);
	
				setTitle(url);
			}
		
/*		@Override
		public void onPageStarted(WebView view, String url, Bitmap favicon) {
			
			// TODO remove!!!
			if (url.contains("localhost:53484")) {
				url = url.replace("localhost:53484", "10.0.2.2:53484");
			}
				
			super.onPageStarted(view, url, favicon);
		}*/

		@Override
		public boolean shouldOverrideUrlLoading(WebView view, String url) {

			// TODO remove!!!
			if (url.contains("localhost:53484")) {
				url = url.replace("localhost:53484", "10.0.2.2:53484");
				view.loadUrl(url);
				return true;
			}
			
			return super.shouldOverrideUrlLoading(view, url);
		}
        });		
	}

}