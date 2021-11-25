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
			DrawCoordinateSystem(0.5f);

		}
		private void DrawLine(float length, float thickness = 0.02f)
		{
			Func<Color, Vector3, Node> createNode = (c, v) =>
			{
				var node = new Node();
				node.SetParent(ArSceneView.Scene);
				MaterialFactory
					.MakeOpaqueWithColor(Context, c)
					.ThenAccept(
						new DelegateConsumer<Material>(m =>
						{
							var model = ShapeFactory.MakeCube(v, v.Scaled(0.5f), m);
							node.Renderable = model;
						}
						));
				return node;
			};
			float l = length;
			float t = thickness;
			var line = createNode(new Color(255, 0, 0), new Vector3(l, t, t));
		}
		private void DrawCoordinateSystem(float length, float thickness = 0.02f)
		{
			Func<Color, Vector3, Node> createNode = (c, v) =>
			{
				var node = new Node();
				node.SetParent(ArSceneView.Scene);
				MaterialFactory
					.MakeOpaqueWithColor(Context, c)
					.ThenAccept(
						new DelegateConsumer<Material>(m =>
						{
							var model = ShapeFactory.MakeCube(v, v.Scaled(0.5f), m);
							node.Renderable = model;
						}
						));
				return node;
			};
			float l = length;
			float t = thickness;
			var xAxis = createNode(new Color(255, 0, 0), new Vector3(l, t, t));
			var yAxis = createNode(new Color(0, 255, 0), new Vector3(t, l, t));
			var zAxis = createNode(new Color(0, 0, 255), new Vector3(t, t, l));
		}
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
}