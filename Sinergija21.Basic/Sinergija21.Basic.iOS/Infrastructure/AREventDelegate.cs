using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ARKit;
using CoreGraphics;
using SceneKit;
using UIKit;

namespace Sinergija21.Basic.iOS.Infrastructure
{
    /// <summary>
    /// Delegate to handle events from ARSession.
    /// </summary>
    public class AREventDelegate : ARSessionDelegate
    {
        /// <summary>
        /// AR has prepared new Frame.
        /// </summary>
        public event Action<ARFrame> FrameUpdate;
        /// <summary>
        /// Provided nodes have been tapped.
        /// </summary>
        public event Action<IEnumerable<ARAnchor>> Tapped;
        public event Action<ARAnchor> TappedArPlane;


        private ARSCNView view { get; }

        public AREventDelegate(ARSCNView v)
        {
            view = v;

            var tapGesture = new UITapGestureRecognizer(handleTap);
            var recognizers = new List<UIGestureRecognizer>();
            recognizers.Add(tapGesture);
            recognizers.AddRange(view.GestureRecognizers);
            view.GestureRecognizers = recognizers.ToArray();
        }
        private void handleTap(UIGestureRecognizer gestureRecognize)
        {
            CGPoint p = gestureRecognize.LocationInView(view);
            SCNHitTestResult[] hitResults = view.HitTest(p, (SCNHitTestOptions)null);

            var list = new List<SCNVector3>();
            if (hitResults.Length > 0)
            {
                SCNHitTestResult result = hitResults[0];
                list.Add(result.WorldCoordinates);
                //if (result.Node != null)
                //list.Add(result.Node);
            }
            /*if (list.Any())
                Tapped?.Invoke(list.Select(v =>
                {
                    var matrix = Matrix4x4.Identity;
                    matrix.Translation = new Vector3(v.X, v.Y, v.Z),// v.ToNumerics();
                    var anchor = new ARAnchor(matrix.ToAR().ToNTk());
                    return anchor;
                }));*/
        }


        public override void DidUpdateFrame(ARSession session, ARFrame frame)
        {
            FrameUpdate?.Invoke(frame);
            frame.Dispose();
        }
        public override void DidAddAnchors(ARSession session, ARAnchor[] anchors)
        {
            foreach (var a in anchors)
            {
                var planeAnchor = a as ARPlaneAnchor;
                if (planeAnchor == null)
                    continue;
                TappedArPlane?.Invoke(planeAnchor);
            }
        }

    }
}
