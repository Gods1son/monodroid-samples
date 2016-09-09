using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace DemoService
{
	[Service(Exported=false)]
	public class DemoService : Service
	{
		static readonly string TAG = typeof (DemoService).Name;

		DemoServiceBinder binder;

		public override StartCommandResult OnStartCommand (Android.Content.Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug (TAG, "DemoService started");

			StartServiceInForeground ();

			DoWork ();

			return StartCommandResult.NotSticky;
		}

		void StartServiceInForeground ()
		{
			PendingIntent pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof(DemoActivity)), 0);

			Notification.Builder builder = new Notification.Builder (ApplicationContext);
			builder.SetContentTitle ("Demo Service")
				   .SetContentText ("Demo Service is running in the foreground.")
				   .SetSmallIcon (Resource.Drawable.icon)
				   .SetContentIntent (pendingIntent);
			
			StartForeground ((int)NotificationFlags.ForegroundService, builder.Build());
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
            
			Log.Debug (TAG, "DemoService stopped");       
		}

		void SendNotification ()
		{
			PendingIntent pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof (DemoActivity)), 0);

			Notification.Builder builder = new Notification.Builder (ApplicationContext);

			builder.SetContentTitle ("Demo Service")
				   .SetContentText ("Message from " + this.GetType ().Name)
			       .SetContentIntent(pendingIntent)
				   .SetSmallIcon (Resource.Drawable.icon);
			
			NotificationManager nMgr = (NotificationManager)GetSystemService (NotificationService);
			nMgr.Notify (0, builder.Build());
		}

		public void DoWork ()
		{
			Toast.MakeText (this, "The demo service has started", ToastLength.Long).Show ();

			Thread t = new Thread (() => {

				SendNotification ();

				Thread.Sleep (5000);

				Log.Debug (TAG, "Stopping foreground");
				StopForeground (true);

				StopSelf ();
			}
			);

			t.Start ();
		}

		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			binder = new DemoServiceBinder (this);
			return binder;
        }

		public string GetText ()
		{
			return "Some text from " + this.GetType().FullName;
		}
	}

	public class DemoServiceBinder : Binder
	{
		DemoService service;
    
		public DemoServiceBinder (DemoService service)
		{
			this.service = service;
		}

		public DemoService GetDemoService ()
		{
			return service;
		}
	}

}