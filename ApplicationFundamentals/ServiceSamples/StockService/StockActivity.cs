using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace StockService
{
	[Activity (Label = "StockService", MainLauncher = true, Icon = "@drawable/Icon")]
	public class StockActivity : ListActivity
	{
		static readonly string TAG = typeof (StockActivity).Name;

		bool isBound;
		StockServiceBinder binder;
		StockServiceConnection stockServiceConnection;
		StockReceiver stockReceiver;
		Intent stockServiceIntent;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			stockServiceIntent = new Intent (this, typeof (StockService));
			stockReceiver = new StockReceiver ();
		}

		protected override void OnStart ()
		{
			base.OnStart ();

			IntentFilter intentFilter = new IntentFilter (StockService.StocksUpdatedAction) { Priority = (int)IntentFilterPriority.HighPriority };
			RegisterReceiver (stockReceiver, intentFilter);

			stockServiceConnection = new StockServiceConnection (this);
			BindService (stockServiceIntent, stockServiceConnection, Bind.AutoCreate);

			ScheduleStockUpdates ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();

			if (isBound) {
				UnbindService (stockServiceConnection);

				isBound = false;
			}

			UnregisterReceiver (stockReceiver);
		}

		void ScheduleStockUpdates ()
		{
			if (!IsAlarmSet ()) {
				AlarmManager alarm = (AlarmManager)GetSystemService (Context.AlarmService);
				PendingIntent pendingServiceIntent = PendingIntent.GetService (this, 0, stockServiceIntent, PendingIntentFlags.CancelCurrent);
				alarm.SetRepeating (AlarmType.Rtc, 0, 15000, pendingServiceIntent);
			} else {
				Log.Debug (TAG, "Alarm is already set.");
			}
		}

		bool IsAlarmSet ()
		{
			return PendingIntent.GetBroadcast (this, 0, stockServiceIntent, PendingIntentFlags.NoCreate) != null;
		}

		void GetStocks ()
		{
			if (isBound) {
				List<Stock> stocks = binder.GetStockService ().GetStocks ();
				if (stocks != null) {
					ListAdapter = new ArrayAdapter<Stock> (this, Resource.Layout.StockItemView, stocks);
				} else {
					Log.Debug (TAG, "Stocks are null.");
				}
			}

		}

		class StockReceiver : BroadcastReceiver
		{
			public override void OnReceive (Context context, Intent intent)
			{
				((StockActivity)context).GetStocks ();
				InvokeAbortBroadcast ();
			}
		}

		class StockServiceConnection : Java.Lang.Object, IServiceConnection
		{
			readonly StockActivity parent;

			public StockServiceConnection (StockActivity activity)
			{
				parent = activity;
			}

			public void OnServiceConnected (ComponentName name, IBinder service)
			{
				StockServiceBinder stockServiceBinder = service as StockServiceBinder;
				if (stockServiceBinder != null) {
					parent.binder = stockServiceBinder;
					parent.isBound = true;
				} else {
					parent.isBound = false;
				}
			}

			public void OnServiceDisconnected (ComponentName name)
			{
				parent.isBound = false;
			}
		}
	}
}