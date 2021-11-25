using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinergija21.Basic.Droid.Infrastructure
{

	/// <summary>
	/// A way to perform operation in Action delegate where IConsumer interface is needed.
	/// </summary>
	public class DelegateConsumer<T> : Java.Lang.Object, Java.Util.Functions.IConsumer
		where T : Java.Lang.Object
	{
		private Action<T> _completed;
		public DelegateConsumer(Action<T> action)
		{
			_completed = action;
		}
		public void Accept(Java.Lang.Object t)
		{
			_completed(global::Android.Runtime.Extensions.JavaCast<T>(t));
		}
	}
}