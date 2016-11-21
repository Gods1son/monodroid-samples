using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Util;
using System;

namespace ServicesDemo3
{

	[BroadcastReceiver]
	[IntentFilter(new string[] {Constants.NOTIFICATION_BROADCAST_ACTION })]
	public class MyReceiver : BroadcastReceiver
	{
		static readonly string TAG = typeof(MyReceiver).FullName;

		public override void OnReceive(Context context, Intent intent)
		{


			string message = intent.GetStringExtra(Constants.BROADCAST_MESSAGE_KEY);
			UpdateNotification(context, message);
			Log.Debug(TAG, "Broadcast received : " + message);
		}

		void UpdateNotification(Context context,  string message)
		{
			Notification.Builder notificationBuilder = new Notification.Builder(context)
				.SetSmallIcon(Resource.Drawable.ic_stat_name)
				.SetContentTitle(context.Resources.GetString(Resource.String.app_name))
				.SetContentText(message);

			var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			notificationManager.Notify(Constants.SERVICE_RUNNING_NOTIFICATION_ID, notificationBuilder.Build());
		}
	}
}
