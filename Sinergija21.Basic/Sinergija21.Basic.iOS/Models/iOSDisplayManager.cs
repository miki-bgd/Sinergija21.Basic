using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ARKit;
using SceneKit;
using Sinergija21.Basic.iOS.Infrastructure;
using Sinergija21.Basic.Models;
using UIKit;

namespace Sinergija21.Basic.iOS.Models
{
    class iOSDisplayManager : IDisplayManager
    {
        public event Action RefreshFrame;
        //public event Action<TappedPlaneEventArgs> PlaneTapped;
        //public event Action<TrackingState> TrackingStateUpdated;
        public event Action Initialized;

        //public TrackingState CurrentState { get; private set; }

        //public ModelWrapper DisplayedModel { get; private set; }

        public CameraProperties Camera { get; } = new CameraProperties();


        private CameraUpdater cameraUpdater;
        private ARSCNView view;
        private ARAnchor currentAnchor;

        private bool initialized = false;
        private bool planesVisible = true;
        public iOSDisplayManager()
        {

        }
        /*
        private void setTrackingState(TrackingState newState)
        {
            if (CurrentState != newState)
            {
                CurrentState = newState;
                TrackingStateUpdated?.Invoke(newState);
            }
        }
        private void addLight()
        {
            SCNLight light = new SCNLight();
            light.LightType = SCNLightType.Directional;
        }

        public void AddModel(ModelWrapper model)
        {
            if (ReferenceEquals(model, DisplayedModel))
                return;
            if (DisplayedModel != null)
                throw new NotSupportedException("Model is already displayed.");
            var anchor = (AnchoriOS)ServiceManager.UserSelectedPosition.LastAnchor;
            view.Session.AddAnchor(anchor.Anchor);
            currentAnchor = anchor.Anchor;
            var m = (ModelWrapperiOS)model;
            view.Scene.RootNode.AddChildNode(m.Node);
            DisplayedModel = model;

        }

        public void ReplaceModel(ModelWrapper newModel)
        {
            if (DisplayedModel != null)
            {
                var oldModel = (ModelWrapperiOS)DisplayedModel;
                newModel.Position = oldModel.Position;
                newModel.PositionClean = oldModel.PositionClean;
                newModel.RotationYDeg = oldModel.RotationYDeg;
                newModel.Scale = oldModel.Scale;
                oldModel.Node.RemoveFromParentNode();
                DisplayedModel.Dispose();
            }
            DisplayedModel = null;

            if (currentAnchor == null)
                AddModel(newModel);
            else
            {
                DisplayedModel = newModel;
                var m = (ModelWrapperiOS)newModel;
                view.Scene.RootNode.AddChildNode(m.Node);
            }
        }
        public void UpdateAnchor()
        {
            if (DisplayedModel == null)
                // Nothing to move.
                return;
            var m = (ModelWrapperiOS)DisplayedModel;
            DisplayedModel = null;
            m.Node.RemoveFromParentNode();
            AddModel(m);
        }
        public void RemoveModel()
        {
            if (DisplayedModel == null)
                return;
            var m = (ModelWrapperiOS)DisplayedModel;
            m.Node.RemoveFromParentNode();
            DisplayedModel = null;
        }*/

        private void setPlanesVisible(bool visible)
        {
            //((SceneViewPlanesDelegate)view.Delegate).SetPlanesVisibility(visible);

        }
        public void SetPlaneVisibility(bool visible)
        {
            if (initialized)
                setPlanesVisible(visible);
            else
                planesVisible = visible;
        }



        public int LoadModel(string path)
        {
            string file = "art.scnassets/earth/earth.dae";
            // Join all model parts before exporting it to .obj .
            //A7Settings.Instance.IsLoadingVisible = true;
            //Task.Run
            SCNScene sceneFile = null;
            //var t = Task.Run(() =>
            {
                var exists = System.IO.File.Exists(path);
                Foundation.NSError error;
                //if (path.StartsWith("art"))
                //    // Load file from asset
                //    sceneFile = SCNScene.FromFile(path);
                //else
                //    // Load file from cache.
                //sceneFile = SCNScene.FromUrl(new Foundation.NSUrl(path), new SCNSceneLoadingOptions(), out error);// (path, "", new SCNSceneLoadingOptions(){ );
                sceneFile = SCNScene.FromFile(file);
            }//);
            //await t;
            if (sceneFile == null)
            {
                //A7Settings.Instance.IsLoadingVisible = false;
                throw new ArgumentException("Unable to load model from file!");
            }
            var node = sceneFile.RootNode.First(x => x.Geometry != null);
            //node.Geometry.FirstMaterial.Emission.Contents = UIColor.White;
            var c = Camera.Position;
            var d = Camera.Direction;
            node.WorldPosition = SCNVector3.Add(new SCNVector3(c.X, c.Y, c.Z), SCNVector3.Normalize(new SCNVector3(d.X, d.Y, d.Z)));
            node.Light = new SCNLight()
            {
                Color = UIColor.White,
                Intensity = 1000,
                LightType = SCNLightType.Ambient,
                
            };
            //node.Geometry.FirstMaterial.Emission.Contents = UIColor.White;
            //foreach (var m in node.Geometry.Materials)
                //m.Emission.Contents = UIColor.White;
            //var model = new ModelWrapperiOS(node);
            //
            //
            //A7Settings.Instance.IsLoadingVisible = false;
            view.Scene.RootNode.AddChildNode(node);
            return 1;
        }


        internal void AttachAR(ARSCNView view, AREventDelegate viewDelegate)
        {
            this.view = view;
            cameraUpdater = new CameraUpdater(viewDelegate, Camera);
            viewDelegate.FrameUpdate += ViewDelegate_FrameUpdate;
            //viewDelegate.Tapped += ViewDelegate_TappedArPlane;
            if (!planesVisible)
                setPlanesVisible(false);


            var light = new SCNNode();
            light.Light = new SCNLight();
            light.Light.LightType = SCNLightType.Omni;
            light.Light.Intensity = 90;
            view.Scene.RootNode.AddChildNode(light);
            initialized = true;
        }


        private void ViewDelegate_FrameUpdate(ARFrame frame)
        {
            RefreshFrame?.Invoke();
            //setTrackingState(cameraUpdater.CurrentState);
            frame.Dispose();
        }

        public void Reset()
        {
            initialized = false;
            //RemoveModel();
            SetPlaneVisibility(false);
        }
        public void Dispose()
        {
            cameraUpdater?.Dispose();
        }

        public void SetLighting(bool enabled)
        { }

        public int DrawLine()
        {
            throw new NotImplementedException();
        }

        public int DrawSphere(Vector3 position, float radius)
        {
            throw new NotImplementedException();
        }

        public int DrawCoordinateSystem()
        {
            throw new NotImplementedException();
        }


        public void SetModelZRotation(int id, float angleDeg)
        {
            throw new NotImplementedException();
        }
    }
}
