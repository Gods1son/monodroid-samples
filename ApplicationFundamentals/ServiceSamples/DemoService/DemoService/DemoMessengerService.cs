using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace StockService
{
	[Service()]
	public class DemoMessengerService : Service
	{
		public const int MSG_SAY_HELLO = 1;
		static readonly string TAG = typeof (DemoMessengerService).Name;
		readonly Messenger demoMessenger;

		public DemoMessengerService ()
		{
			demoMessenger = new Messenger (new DemoHandler ());
		}

		public override IBinder OnBind (Intent intent)
		{
			Log.Debug (TAG, "Some client bound to this service.");
			return demoMessenger.Binder;
		}

		class DemoHandler : Handler
		{
			public override void HandleMessage (Message msg)
			{
				switch (msg.What) {
				case MSG_SAY_HELLO:
					string text = msg.Data.GetString ("InputText");
					Log.Debug (TAG, "Someone has said hello! {0}", text);
					break;

				default:
					base.HandleMessage (msg);
					break;
				}
			}
		}
	}
}


