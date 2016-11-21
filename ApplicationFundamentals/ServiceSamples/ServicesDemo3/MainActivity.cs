using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Util;
using System;

namespace ServicesDemo3
{
	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		static readonly string TAG = typeof(MainActivity).FullName;
		static readonly string SERVICE_STARTED_KEY = "has_service_been_started";

		Button stopServiceButton;
		Button startServiceButton;
		Intent serviceToStart;
		MyReceiver broadcastReceiver;
		bool isStarted = false;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Main);

			broadcastReceiver = new MyReceiver();

			if (savedInstanceState != null)
			{
				isStarted = savedInstanceState.GetBoolean(SERVICE_STARTED_KEY, false);
			}

			serviceToStart = new Intent(this, typeof(TimestampService));

			stopServiceButton = FindViewById<Button>(Resource.Id.stop_timestamp_service_button);
			startServiceButton = FindViewById<Button>(Resource.Id.start_timestamp_service_button);
			if (isStarted)
			{
				stopServiceButton.Click += StopServiceButton_Click;
				stopServiceButton.Enabled = true;
				startServiceButton.Enabled = false;
			}
			else
			{
				startServiceButton.Click += StartServiceButton_Click;
				startServiceButton.Enabled = true;
				stopServiceButton.Enabled = false;
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			IntentFilter filter = new IntentFilter(Constants.NOTIFICATION_BROADCAST_ACTION);

			// Using the LocalBroadcastManager means that messages will only be dispatched to within the context of this
			// application. This is a security measure to prevent other apps on the device from intercepting the 
			// broadcast.
			Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).RegisterReceiver(broadcastReceiver, filter);
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			outState.PutBoolean(SERVICE_STARTED_KEY, isStarted);
			base.OnSaveInstanceState(outState);
		}

		protected override void OnPause()
		{
			Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).UnregisterReceiver(broadcastReceiver);
			base.OnPause();
		}
		protected override void OnDestroy()
		{
			Log.Info(TAG, "Activity is being destroyed; stop the service.");

			StopService(serviceToStart);
			base.OnDestroy();
		}
		void StopServiceButton_Click(object sender, System.EventArgs e)
		{
			stopServiceButton.Click -= StopServiceButton_Click;
			stopServiceButton.Enabled = false;

			Log.Info(TAG, "User requested that the service be stopped.");
			StopService(serviceToStart);
			isStarted = false;

			startServiceButton.Click += StartServiceButton_Click;
			startServiceButton.Enabled = true;
		}

		void StartServiceButton_Click(object sender, System.EventArgs e)
		{
			startServiceButton.Enabled = false;
			startServiceButton.Click -= StartServiceButton_Click;

			StartService(serviceToStart);
			Log.Info(TAG, "User requested that the service be started.");

			isStarted = true;
			stopServiceButton.Click += StopServiceButton_Click;

			stopServiceButton.Enabled = true;
		}
	}

	
}

