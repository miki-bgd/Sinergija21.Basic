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
	internal class CoordinateSystemCreator
	{
		public static Node Create(float length, float thickness = 0.02f)
		{
			Node n = new Node();
			var xAxis = new Node();
			var yAxis = new Node();
			var zAxis = new Node();
			n.AddChild(xAxis);
			n.AddChild(yAxis);
			n.AddChild(zAxis);

			Action<Node, Color, Vector3> createNode = (node, c, v) =>
			{
				MaterialFactory
					.MakeOpaqueWithColor(Application.Context, c)
					.ThenAccept(
						new DelegateConsumer<Material>(m =>
						{
							var model = ShapeFactory.MakeCube(v, v.Scaled(0.5f), m);
							node.Renderable = model;
						}
						));
			};
			float l = length;
			float t = thickness;
			createNode(xAxis, new Color(255, 0, 0), new Vector3(l, t, t));
			createNode(yAxis, new Color(0, 255, 0), new Vector3(t, l, t));
			createNode(zAxis, new Color(0, 0, 255), new Vector3(t, t, l));
			return n;
		}
	}
}