using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace DemoService
{
	[Activity (Label = "DemoService", MainLauncher = true)]
	public class DemoActivity : Activity
	{
		bool isBound = false;
		bool isConfigurationChange = false;
		DemoServiceBinder binder;
		DemoServiceConnection demoServiceConnection;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button start = FindViewById<Button> (Resource.Id.startService);

			start.Click += delegate {
				StartService (new Intent (ApplicationContext, typeof (DemoService)));
			};

			Button stop = FindViewById<Button> (Resource.Id.stopService);

			stop.Click += delegate {
				StopService (new Intent(ApplicationContext, typeof(DemoService)));
			};

			Button callService = FindViewById<Button> (Resource.Id.callService);

			callService.Click += delegate {
				if (isBound) {
					RunOnUiThread (() => {
						string text = binder.GetDemoService ().GetText ();
						Console.WriteLine ("{0} returned from DemoService", text);
					}
					);
				}
			};

			// restore from connection there was a configuration change, such as a device rotation
			demoServiceConnection = LastNonConfigurationInstance as DemoServiceConnection;

			if (demoServiceConnection != null) {
				binder = demoServiceConnection.Binder;
			}
		}

		protected override void OnStart ()
		{
			base.OnStart ();

			Intent demoServiceIntent = new Intent (ApplicationContext, typeof(DemoService));
			demoServiceConnection = new DemoServiceConnection (this);
			ApplicationContext.BindService (demoServiceIntent, demoServiceConnection, Bind.AutoCreate);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			if (!isConfigurationChange) {
				if (isBound) {
					UnbindService (demoServiceConnection);
					isBound = false;
				}
			}
		}

		// return the service connection if there is a configuration change
		public override Java.Lang.Object OnRetainNonConfigurationInstance ()
		{
			base.OnRetainNonConfigurationInstance ();

			isConfigurationChange = true;

			return demoServiceConnection;
		}

		class DemoServiceConnection : Java.Lang.Object, IServiceConnection
		{
			DemoActivity activity;
			DemoServiceBinder binder;

			public DemoServiceBinder Binder {
				get {
					return binder;
				}
			}

			public DemoServiceConnection (DemoActivity activity)
			{
				this.activity = activity;
			}

			public void OnServiceConnected (ComponentName name, IBinder service)
			{
				DemoServiceBinder demoServiceBinder = service as DemoServiceBinder;
				if (demoServiceBinder != null) {
					activity.binder = demoServiceBinder;
					activity.isBound = true;

					// keep instance for preservation across configuration changes
					this.binder = demoServiceBinder;
				}
			}

			public void OnServiceDisconnected (ComponentName name)
			{
				activity.isBound = false;
			}
		}
	}
}


