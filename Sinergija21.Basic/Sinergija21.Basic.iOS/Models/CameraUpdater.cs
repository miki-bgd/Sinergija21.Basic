using System;
using System.Numerics;
using Sinergija21.Basic.iOS.Infrastructure;

namespace Sinergija21.Basic.iOS.Models
{
    class CameraUpdater : IDisposable
    {
        //public TrackingState CurrentState { get; private set; }
        public CameraProperties Properties { get; private set; }

        private AREventDelegate arDelegate;
        public CameraUpdater(AREventDelegate arDelegate, CameraProperties cameraProperties)
        {
            Properties = cameraProperties;
            this.arDelegate = arDelegate;
            this.arDelegate.FrameUpdate += ArDelegate_FrameUpdate;
        }

        private void ArDelegate_FrameUpdate(ARKit.ARFrame frame)
        {

            var m = frame.Camera.Transform;
            // Camera matrix transform is different.
            Properties.Position = new Vector3(m.M14, m.M24, m.M34);// m.GetTranslation();
            var cameraZ = new Vector3(m.M13, m.M23, m.M33);
            Properties.Direction = Vector3.Negate(cameraZ);//.Negate();// m.GetZAxis().Negate();
            // On iOS, AR keeps coordinate system rotation (even without tracking),
            // with sensors, so we can assume this is Up vector.
            // Otherwise, we would need to get up vector with orientation, and then
            // transform it to AR world.
            var x = Vector3.Cross(new Vector3(0, 1, 0), Properties.Direction);
            Properties.Up = Vector3.Cross(Properties.Direction, x);// m.GetYAxis();

            // Maybe not needed.
            //adjustCamera(m);

            var arState = frame.Camera.TrackingState;
            //CurrentState = arState == ARKit.ARTrackingState.Normal
            //    ? TrackingState.Tracking
            //    : TrackingState.NoTracking;
        }

        public void Dispose()
        {
            arDelegate.FrameUpdate -= ArDelegate_FrameUpdate;
        }
    }
    public class CameraProperties
    {
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Up { get; set; }

        public CameraProperties()
        {
            Position = new Vector3();
            Direction = new Vector3(0, 0, -1);
            Up = new Vector3(0, 1, 0);
        }
    }
}
