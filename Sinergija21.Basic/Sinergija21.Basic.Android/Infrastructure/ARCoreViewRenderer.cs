using Android.Widget;
using Sinergija21.Basic.Infrastructure;
using Sinergija21.Basic.Droid.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Views;
using Android.Support.V4.App;
using Google.AR.Sceneform.UX;
using Android.Content;
using Xamarin.Forms.Platform.Android.FastRenderers;

[assembly: ExportRenderer(typeof(ARView), typeof(ARCoreViewRenderer))]
namespace Sinergija21.Basic.Droid.Infrastructure
{
	public class ARCoreViewRenderer : FrameLayout, IVisualElementRenderer, IViewRenderer
	{

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		VisualElement IVisualElementRenderer.Element => Element;
		VisualElementTracker IVisualElementRenderer.Tracker => visualElementTracker;
		ViewGroup IVisualElementRenderer.ViewGroup => null;
		Android.Views.View IVisualElementRenderer.View => this;

		int? defaultLabelFor;
		bool disposed;
		ARView element;
		VisualElementTracker visualElementTracker;
		VisualElementRenderer visualElementRenderer;
		FragmentManager fragmentManager;

		private FragmentManager FragmentManager { get { fragmentManager = (Context.GetActivity() as FragmentActivity)?.SupportFragmentManager; return fragmentManager; } }


		ARView Element
		{
			get => element;
			set
			{
				if (element == value)
				{
					return;
				}

				var oldElement = element;
				element = value;
				OnElementChanged(new ElementChangedEventArgs<ARView>(oldElement, element));
			}
		}

		public ARCoreViewRenderer(Context context) : base(context)
		{
			visualElementRenderer = new VisualElementRenderer(this);
		}

		void OnElementChanged(ElementChangedEventArgs<ARView> e)
		{

			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
			}
			if (e.NewElement != null)
			{
				this.EnsureId();

				e.NewElement.PropertyChanged += OnElementPropertyChanged;

				ElevationHelper.SetElevation(this, e.NewElement);
			}
			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
		}

		void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			ElementPropertyChanged?.Invoke(this, e);

			switch (e.PropertyName)
			{
				case "Width":
					break;
			}
		}

		void IViewRenderer.MeasureExactly() => MeasureExactly(this, Element, Context);

		static void MeasureExactly(Android.Views.View control, VisualElement element, Context context)
		{
			if (control == null || element == null)
			{
				return;
			}

			double width = element.Width;
			double height = element.Height;

			if (width <= 0 || height <= 0)
			{
				return;
			}

			int realWidth = (int)context.ToPixels(width);
			int realHeight = (int)context.ToPixels(height);

			int widthMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realWidth, MeasureSpecMode.Exactly);
			int heightMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realHeight, MeasureSpecMode.Exactly);

			control.Measure(widthMeasureSpec, heightMeasureSpec);
		}

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			Measure(widthConstraint, heightConstraint);
			SizeRequest result = new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), new Size(Context.ToPixels(20), Context.ToPixels(20)));
			return result;
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (!(element is ARView camera))
			{
				throw new ArgumentException($"{nameof(element)} must be of type {nameof(ARView)}");
			}

			if (visualElementTracker == null)
			{
				visualElementTracker = new VisualElementTracker(this);
			}
			Element = camera;
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			if (defaultLabelFor == null)
			{
				defaultLabelFor = LabelFor;
			}
			LabelFor = (int)(id ?? defaultLabelFor);
		}

		void IVisualElementRenderer.UpdateLayout() => visualElementTracker?.UpdateLayout();


		static class MeasureSpecFactory
		{
			public static int GetSize(int measureSpec)
			{
				const int modeMask = 0x3 << 30;
				return measureSpec & ~modeMask;
			}

			public static int MakeMeasureSpec(int size, MeasureSpecMode mode) => size + (int)mode;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}


			disposed = true;

			if (disposing)
			{
				SetOnClickListener(null);
				SetOnTouchListener(null);

				if (visualElementTracker != null)
				{
					visualElementTracker.Dispose();
					visualElementTracker = null;
				}

				if (visualElementRenderer != null)
				{
					visualElementRenderer.Dispose();
					visualElementRenderer = null;
				}

				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;

					if (Xamarin.Forms.Platform.Android.Platform.GetRenderer(Element) == this)
					{
						Xamarin.Forms.Platform.Android.Platform.SetRenderer(Element, null);
					}
				}
			}

			base.Dispose(disposing);
		}
	}
}