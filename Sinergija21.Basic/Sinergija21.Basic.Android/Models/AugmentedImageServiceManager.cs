using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sinergija21.Basic.Droid.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.AR.Core;
using Android.Graphics;
using System.IO;
using Google.AR.Sceneform;
using System.Threading.Tasks;
using Google.AR.Sceneform.Math;

namespace Sinergija21.Basic.Droid.Models
{
	internal class AugmentedImageServiceManager
	{
		public static AugmentedImageServiceManager Instance { get; } = new AugmentedImageServiceManager();

		private CustomArFragment fragment;
		private Dictionary<string, bool> images { get; }
		private AugmentedImageServiceManager()
		{
			images = new Dictionary<string, bool>
			{
				{"marker.jpg", false }
			};
		}

		public void Initialize(CustomArFragment fragment)
		{
			this.fragment = fragment;			

            fragment.ArSceneView.Scene.Update += Scene_FirstUpdate;
		}

        private void Scene_FirstUpdate(object sender, Scene.UpdateEventArgs e)
        {
			fragment.ArSceneView.Scene.Update -= Scene_FirstUpdate;
			FirstRun();
			fragment.ArSceneView.Scene.Update += Scene_Update;
        }

        private void FirstRun()
        {
			// Setup image database - AR tries to recognize images added to here.
			var session = fragment.ArSceneView.Session;
			var db = new AugmentedImageDatabase(session);
			var config = new Config(session);

			foreach (var img in images)
			{
				Bitmap bmp = loadImageFromAssets(img.Key);
				float width = 0.5f;// [Optional]
				db.AddImage(img.Key, bmp, width);
			}

			//config.SetFocusMode(Config.FocusMode.Auto);
			config.SetAugmentedImageDatabase(db);
			config.SetUpdateMode(Config.UpdateMode.LatestCameraImage);

			session.Configure(config);
		}

        private void Scene_Update(object sender, Google.AR.Sceneform.Scene.UpdateEventArgs e)
        {
			// Check images for each frame.
			var frame = fragment.ArSceneView.ArFrame;
			IEnumerable<AugmentedImage> aImages = frame
				.GetUpdatedTrackables(Java.Lang.Class.FromType(typeof(AugmentedImage)))
				.Cast<AugmentedImage>()
				.ToList();
			foreach(var aImg in aImages)
            {
				// Image is already recognized.
				if (images[aImg.Name])
					continue;
				if (aImg.TrackingState == TrackingState.Tracking)
                {
					images[aImg.Name] = true;
					// Create anchor to which model is attached to.
					var anchor = aImg.CreateAnchor(aImg.CenterPose);
					var anchorNode = new AnchorNode(anchor);
					anchorNode.SetParent(fragment.ArSceneView.Scene);

					// Load 3d model.
					var n = ((AndroidDisplayManager)DI.Display).LoadModelInternal("a7logo.glb", new Vector3());
					n.SetParent(anchorNode);
					float scale = aImg.ExtentX;
					//n.LocalScale = new Google.AR.Sceneform.Math.Vector3(scale, scale, scale);
					// Set 3d model "up".
					//n.LocalRotation = Quaternion.AxisAngle(new Vector3(1, 0, 0), -90);


                }
            }
		}

        private static Bitmap loadImageFromAssets(string img)
		{
			using (Stream s = Application.Context.Assets.Open(img))
			{
				var b = BitmapFactory.DecodeStream(s);
				return b;
			}
		}
	}
}