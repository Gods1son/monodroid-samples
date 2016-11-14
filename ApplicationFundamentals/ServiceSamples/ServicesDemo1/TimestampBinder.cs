using System;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;

namespace ServicesDemo1
{
	public class TimestampBinder : Binder, IGetTimestamp
	{
		public TimestampBinder(TimestampService service)
		{
			this.Service = service;
		}

		public TimestampService Service { get; private set; }

		public string GetFormattedTimestamp()
		{
			return Service?.GetFormattedTimestamp();
		}
	}	
}
