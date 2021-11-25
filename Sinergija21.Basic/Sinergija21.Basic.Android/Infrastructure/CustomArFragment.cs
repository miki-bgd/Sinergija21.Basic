using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.AR.Sceneform;
using Google.AR.Sceneform.Math;
using Google.AR.Sceneform.Rendering;
using Google.AR.Sceneform.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinergija21.Basic.Droid.Infrastructure
{
	internal class CustomArFragment : ArFragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SessionInitialization += CustomArFragment_SessionInitialization;
		}

		private void CustomArFragment_SessionInitialization(object sender, SessionInitializationEventArgs e)
		{
			// When session is initialized, we can add 3d models.
			//DrawLine(1);
			//DrawCoordinateSystem(0.5f);

		}
		
	}
}