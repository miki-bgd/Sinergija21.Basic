using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sinergija21.Basic
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			DI.Display.Initialized += Display_Initialized;
		}

		private void Display_Initialized()
		{
			DI.Display.DrawCoordinateSystem();
			DI.Display.DrawSphere(new System.Numerics.Vector3(0.5f, 0.1f, 0.5f), 0.1f);
		}

		private async void AddLine_Clicked(object sender, EventArgs e)
		{
			int lineId = DI.Display.DrawLine();
			float currentAngle = 0;
			float angleStepDeg = 9;
			while(true)
			{
				await Task.Delay(50);
				DI.Display.SetModelZRotation(lineId, currentAngle);
				currentAngle += angleStepDeg;
				if (currentAngle > 360)
					currentAngle -= 360;
			}
		}
	}
}
