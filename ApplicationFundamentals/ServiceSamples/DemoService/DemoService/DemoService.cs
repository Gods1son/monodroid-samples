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
			Notification ongoing = new Notification (Resource.Drawable.icon, "DemoService in foreground");
			PendingIntent pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof(DemoActivity)), 0);
			ongoing.SetLatestEventInfo (this, "DemoService", "DemoService is running in the foreground", pendingIntent);

			StartForeground ((int)NotificationFlags.ForegroundService, ongoing);
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
            
			Log.Debug (TAG, "DemoService stopped");       
		}

		void SendNotification ()
		{
			NotificationManager nMgr = (NotificationManager)GetSystemService (NotificationService);
			Notification notification = new Notification (Resource.Drawable.icon, "Message from demo service");
			PendingIntent pendingIntent = PendingIntent.GetActivity (this, 0, new Intent (this, typeof(DemoActivity)), 0);
			notification.SetLatestEventInfo (this, "Demo Service Notification", "Message from demo service", pendingIntent);
			nMgr.Notify (0, notification);
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