using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.AR.Sceneform;
using Google.AR.Sceneform.Assets;
using Google.AR.Sceneform.Math;
using Google.AR.Sceneform.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinergija21.Basic.Droid.Model3dCreators
{
	internal class ExternalModelCreator
	{
		public static Node LoadGlb(string path, Vector3 position)
		{
			var builder = ModelRenderable.InvokeBuilder();
			var javaClass = Java.Lang.Class.FromType(builder.GetType());
			var method = javaClass.GetMethod("setSource",
				Java.Lang.Class.FromType(typeof(global::Android.Content.Context)),
				Java.Lang.Class.FromType(typeof(Android.Net.Uri))
			);
			//method.Invoke(builder, activity, Android.Net.Uri.Parse(path));

			var methods = javaClass.GetMethods();
			var methods1 = methods.Select(x => x.GetParameters())
				.ToList();
			var model = RenderableSource
				.InvokeBuilder()
				.SetSource(Application.Context, Android.Net.Uri.Parse(path), RenderableSource.SourceType.Glb)
				.Build();
			method = methods[13];
			method.Invoke(builder, Application.Context, model);

			Node n = new Node()
			{
				WorldPosition = position
			};
			builder.Build(renderable =>
			{
				n.Renderable = renderable;
			});
			return n;
		}
	}
}