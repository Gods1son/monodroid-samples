using System;
using Android.App;
using System.Threading;
using Android.Util;

namespace DemoService
{
	[Service(Exported = false)]
    public class DemoIntentService : IntentService
    {
		static readonly string TAG = typeof (DemoIntentService).Name;

        public DemoIntentService () : base("DemoIntentService")
        {
        }

        protected override void OnHandleIntent (Android.Content.Intent intent)
        {
			Log.Debug(TAG,  "Perform some long running work");

            Thread.Sleep (5000);

            Log.Debug(TAG,"work complete");
        }
    }
}

