using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.AR.Sceneform;
using Google.AR.Sceneform.Math;
using Google.AR.Sceneform.Rendering;
using Sinergija21.Basic.Droid.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinergija21.Basic.Droid.Model3dCreators
{
	internal class LineCreator
	{
		public static Node Create(Vector3 position, float length, float thickness = 0.02f)
		{
			Node n = new Node()
			{
				WorldPosition = position,
			};
			float l = length;
			float t = thickness;
			var color = new Color(255, 0, 0);
			MaterialFactory
				.MakeOpaqueWithColor(Application.Context, color)
				.ThenAccept(
					new DelegateConsumer<Material>(m =>
					{
						var model = ShapeFactory.MakeCube(new Vector3(length, thickness, thickness), position, m);
						n.Renderable = model;
					}
				));
			return n;
		}
	}
}