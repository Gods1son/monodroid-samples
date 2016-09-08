using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace DemoMessengerClient
{
	[Activity (Label = "DemoMessengerClient", MainLauncher = true)]
    public class DemoMessengerActivity : Activity
    {
        bool isBound = false;
		Messenger mService;
		DemoServiceConnection mConnection;
		Button sayHelloButton;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.Main);
			mConnection = new DemoServiceConnection (this);
            sayHelloButton = FindViewById<Button> (Resource.Id.callMessenger);
			sayHelloButton.Click += Button_Click;
        }

        protected override void OnStart ()
        {
            base.OnStart ();

			Intent demoServiceIntent = new Intent ();
			demoServiceIntent.SetComponent (new ComponentName ("DemoService.DemoService", "com.xamarin.example.demoservice.DemoMessagerService"));
			BindService (demoServiceIntent, mConnection, Bind.AutoCreate);
        }

		void Button_Click (object sender, System.EventArgs e)
		{
			if (isBound) {
				Message message = Message.Obtain ();
				Bundle b = new Bundle ();
				b.PutString ("InputText", "text from DemoMessengerClient");
				message.Data = b;
				mService.Send (message);
			}
		}

		protected override void OnStop ()
        {
            if (isBound) {
				sayHelloButton.Click -= Button_Click;
                UnbindService (mConnection);
                isBound = false;
            }
			base.OnStop ();
		}

        class DemoServiceConnection : Java.Lang.Object, IServiceConnection
        {
			DemoMessengerActivity parentActivity;

            public DemoServiceConnection (DemoMessengerActivity activity)
            {
                parentActivity = activity;
            }
          
            public void OnServiceConnected (ComponentName name, IBinder service)
            {
                parentActivity.mService = new Messenger (service);
                parentActivity.isBound = true;
            }

            public void OnServiceDisconnected (ComponentName name)
            {
                parentActivity.mService.Dispose ();
                parentActivity.mService = null;
                parentActivity.isBound = false;
            }
        }
    }
}