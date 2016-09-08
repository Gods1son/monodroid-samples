
using Android.App;
using Android.Content;

namespace StockService
{
	[BroadcastReceiver]
	[IntentFilter (new string [] { StockService.StocksUpdatedAction }, Priority = (int)IntentFilterPriority.LowPriority)]
	public class StockNotificationReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			NotificationManager nMgr = (NotificationManager)context.GetSystemService (Context.NotificationService);
			Notification notification = new Notification (Resource.Drawable.icon, "New stock data is available");
			PendingIntent pendingIntent = PendingIntent.GetActivity (context, 0, new Intent (context, typeof (StockActivity)), 0);
			notification.SetLatestEventInfo (context, "Stocks Updated", "New stock data is available", pendingIntent);
			nMgr.Notify (0, notification);
		}
	}
}

