using AliRTCSdk.Android;
using AliRTCSdk.Xamarin.Forms;
using Android.Content;
using Android.Views;
using Android.Widget;
using Com.Alivc.Rtc;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.FastRenderers;

[assembly: ExportRenderer(typeof(RTCViewer), typeof(RTCViewerRenderer))]
namespace AliRTCSdk.Android
{
    public class RTCViewerRenderer : FrameLayout, IVisualElementRenderer, IViewRenderer
    {
        private int? defaultLabelFor;
        private bool disposed;
        private RTCViewer element;
        private VisualElementTracker visualElementTracker;
        private VisualElementRenderer visualElementRenderer;
        private AliRtcEngine.AliRtcVideoCanvas canvas = new AliRtcEngine.AliRtcVideoCanvas();

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        private RTCViewer Element
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
                OnElementChanged(new ElementChangedEventArgs<RTCViewer>(oldElement, element));
            }
        }

        public RTCViewerRenderer(Context context) : base(context)
        {
            visualElementRenderer = new VisualElementRenderer(this);
            var sv = AliRtcEngine.GetInstance(context).CreateRenderSurfaceView(context);
            this.AddView(sv);
            canvas.View = sv;
            AliRtcEngine.GetInstance(context).SetLocalViewConfig(canvas, AliRtcEngine.AliRtcVideoTrack.AliRtcVideoTrackCamera);
        }

        void OnElementChanged(ElementChangedEventArgs<RTCViewer> e)
        {
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
                e.OldElement.PreviewStatusChange -= StartPreview;
            }
            if (e.NewElement != null)
            {
                this.EnsureId();

                e.NewElement.PropertyChanged += OnElementPropertyChanged;
                e.NewElement.PreviewStatusChange += StartPreview;

                ElevationHelper.SetElevation(this, e.NewElement);
            }

            ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
        }

        void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ElementPropertyChanged?.Invoke(this, e);
            switch (e.PropertyName)
            {
                case nameof(Element.RenderMode):
                    canvas.RenderMode = Converter.GetRenderMode(Element.RenderMode);
                    break;
                case nameof(Element.VideoTrack):
                    AliRtcEngine.GetInstance(this.Context).SetLocalViewConfig(canvas, Converter.GetVideoTrack(Element.VideoTrack));
                    break;
            }
        }

        #region AliEngine Method
        void StartPreview(object sender, EventArgs e)
        {
            AliRtcEngine.GetInstance(this.Context).StartPreview();
        }
        #endregion


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

                    if (Platform.GetRenderer(Element) == this)
                    {
                        Platform.SetRenderer(Element, null);
                    }
                }
            }

            base.Dispose(disposing);
        }

        #region IViewRenderer

        void IViewRenderer.MeasureExactly() => MeasureExactly(this, Element, Context);

        static void MeasureExactly(global::Android.Views.View control, VisualElement element, Context context)
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

        #endregion

        #region IVisualElementRenderer

        VisualElement IVisualElementRenderer.Element => Element;

        VisualElementTracker IVisualElementRenderer.Tracker => visualElementTracker;

        ViewGroup IVisualElementRenderer.ViewGroup => null;

        global::Android.Views.View IVisualElementRenderer.View => this;

        SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            Measure(widthConstraint, heightConstraint);
            SizeRequest result = new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), new Size(Context.ToPixels(20), Context.ToPixels(20)));
            return result;
        }

        void IVisualElementRenderer.SetElement(VisualElement element)
        {
            if (!(element is RTCViewer rTCViewer))
            {
                throw new ArgumentException($"{nameof(element)} must be of type {nameof(RTCViewer)}");
            }

            if (visualElementTracker == null)
            {
                visualElementTracker = new VisualElementTracker(this);
            }
            Element = rTCViewer;
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

        #endregion

        static class MeasureSpecFactory
        {
            public static int GetSize(int measureSpec)
            {
                const int modeMask = 0x3 << 30;
                return measureSpec & ~modeMask;
            }

            public static int MakeMeasureSpec(int size, MeasureSpecMode mode) => size + (int)mode;
        }
    }
}
