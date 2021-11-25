using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.AR.Sceneform;
using Google.AR.Sceneform.UX;
using Sinergija21.Basic.Droid.Model3dCreators;
using Sinergija21.Basic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sinergija21.Basic.Droid.Models
{
	internal class AndroidDisplayManager : IDisplayManager
	{
		public event Action Initialized;

		private ArFragment fragment;
		private readonly Dictionary<int, Node> nodes = new Dictionary<int, Node>();
		private int nodeId = 0;
		public int DrawCoordinateSystem()
		{
			var n = CoordinateSystemCreator.Create(0.5f);
			n.SetParent(fragment.ArSceneView.Scene);
			int id = nodeId++;
			nodes.Add(id, n);
			return id;
		}

		public int DrawLine()
		{
			var n = LineCreator.Create(new Google.AR.Sceneform.Math.Vector3(0, 0, 0), 0.5f);
			n.SetParent(fragment.ArSceneView.Scene);
			int id = nodeId++;
			nodes.Add(id, n);
			return id;
		}

		public void RotateModelZ(int id, float angleDeg)
		{
			throw new NotImplementedException();
		}

		public void Initialize(ArFragment fragment)
		{
			this.fragment = fragment;
			Initialized?.Invoke();
		}
	}
}