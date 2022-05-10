using AliRTCSdk.Xamarin.Forms.Model;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("AliRTCSdk.Android")]
namespace AliRTCSdk.Xamarin.Forms
{

    public class RTCViewer : View
    {
        public RTCViewer()
        {

        }

        internal event EventHandler PreviewStatusChange;

        public void StartPreview()
        {
            PreviewStatusChange?.Invoke(this, EventArgs.Empty);
        }

        public AliRtcRenderMode RenderMode
        {
            get { return (AliRtcRenderMode)GetValue(RenderModeProperty); }
            set { SetValue(RenderModeProperty, value); }
        }

        public static readonly BindableProperty RenderModeProperty =
            BindableProperty.Create(nameof(RenderMode), typeof(AliRtcRenderMode), typeof(RTCViewer), AliRtcRenderMode.AliRtcRenderModeAuto);

        public AliRtcVideoTrack VideoTrack
        {
            get { return (AliRtcVideoTrack)GetValue(VideoTrackProperty); }
            set { SetValue(VideoTrackProperty, value); }
        }

        public static readonly BindableProperty VideoTrackProperty =
            BindableProperty.Create(nameof(VideoTrack), typeof(AliRtcVideoTrack), typeof(RTCViewer), AliRtcVideoTrack.AliRtcVideoTrackCamera);

    }
}
