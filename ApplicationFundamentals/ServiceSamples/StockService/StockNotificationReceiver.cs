
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
			NotificationManager manager = (NotificationManager)context.GetSystemService (Context.NotificationService);
			PendingIntent pendingIntent = PendingIntent.GetActivity (context, 0, new Intent (context, typeof (StockActivity)), 0);

			Notification.Builder builder = new Notification.Builder (context.ApplicationContext);
			builder.SetContentTitle ("Stocks Update")
				   .SetContentText ("New stock data is available")
				   .SetSmallIcon (Resource.Drawable.icon)
				   .SetContentIntent (pendingIntent);
			manager.Notify (0, builder.Build());
		}
	}
}

