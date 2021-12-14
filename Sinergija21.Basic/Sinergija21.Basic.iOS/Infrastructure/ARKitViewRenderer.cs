using ARKit;
using Sinergija21.Basic.Infrastructure;
using Sinergija21.Basic.iOS.Infrastructure;
using Sinergija21.Basic.iOS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ARView), typeof(ARKitViewRenderer))]
namespace Sinergija21.Basic.iOS.Infrastructure
{
    public class ARKitViewRenderer : ViewRenderer<ARView, ARSCNView>
    {
        private ARSCNView view;
        private AREventDelegate viewDelegate;

        protected override void OnElementChanged(ElementChangedEventArgs<ARView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {

            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    view = new ARSCNView()
                    {
                        //Delegate = new SceneViewPlanesDelegate()
                    };
                    //view.AutoenablesDefaultLighting = true;
                    view.AutomaticallyUpdatesLighting = true;

                    /*{
                        DebugOptions = ARSCNDebugOptions.ShowWorldOrigin
                    };*/
                    SetNativeControl(view);

                    ARSessionRunOptions options = ARSessionRunOptions.None;
                    //options = ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors;
                    //ARLibraryiOS.InitializeNative(view);
                    view.Session.Run(new ARWorldTrackingConfiguration()
                    {
                        PlaneDetection = ARPlaneDetection.Horizontal,
                        LightEstimationEnabled = true,
                        EnvironmentTexturing = AREnvironmentTexturing.Automatic,
                        WantsHdrEnvironmentTextures = true
                        //AutoFocusEnabled = true
                    }, options);
                    viewDelegate = new AREventDelegate(view);
                    view.Session.Delegate = viewDelegate;
                    ((iOSDisplayManager)DI.Display).AttachAR(view, viewDelegate);

                    //ServiceManager.Guide.StartAR();
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            //ServiceManager.Guide.StopAR();
            base.Dispose(disposing);
        }
    }
}
