using AliRTCSdk.Xamarin.Forms.Model;
using Com.Alivc.Rtc;

internal class Converter
{
    public static AliRtcEngine.AliRtcRenderMode GetRenderMode(AliRtcRenderMode mode)
    {
        switch (mode)
        {
            case AliRtcRenderMode.AliRtcRenderModeClip:
                return AliRtcEngine.AliRtcRenderMode.AliRtcRenderModeClip;
            case AliRtcRenderMode.AliRtcRenderModeAuto:
                return AliRtcEngine.AliRtcRenderMode.AliRtcRenderModeAuto;
            case AliRtcRenderMode.AliRtcRenderModeStretch:
                return AliRtcEngine.AliRtcRenderMode.AliRtcRenderModeStretch;
            case AliRtcRenderMode.AliRtcRenderModeFill:
                return AliRtcEngine.AliRtcRenderMode.AliRtcRenderModeFill;
        }
        return AliRtcEngine.AliRtcRenderMode.AliRtcRenderModeNoChange;
    }

    public static AliRtcEngine.AliRtcVideoTrack GetVideoTrack(AliRtcVideoTrack videoTrack)
    {
        switch (videoTrack)
        {
            case AliRtcVideoTrack.AliRtcVideoTrackNo:
                return AliRtcEngine.AliRtcVideoTrack.AliRtcVideoTrackNo;
            case AliRtcVideoTrack.AliRtcVideoTrackCamera:
                return AliRtcEngine.AliRtcVideoTrack.AliRtcVideoTrackCamera;
            case AliRtcVideoTrack.AliRtcVideoTrackScreen:
                    return AliRtcEngine.AliRtcVideoTrack.AliRtcVideoTrackScreen;
            case AliRtcVideoTrack.AliRtcVideoTrackBoth:
                    return AliRtcEngine.AliRtcVideoTrack.AliRtcVideoTrackBoth;
        }
        return AliRtcEngine.AliRtcVideoTrack.AliRtcVideoTrackNo;
    }
}

