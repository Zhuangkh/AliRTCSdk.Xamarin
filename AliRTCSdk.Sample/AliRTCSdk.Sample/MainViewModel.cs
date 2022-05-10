using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;
using AliRTCSdk.Xamarin.Forms.Model;
using AliRTCSdk.Xamarin.Forms;
using System.Windows.Input;
using Xamarin.Forms;

namespace AliRTCSdk.Sample
{
    internal class MainViewModel : ObservableObject
    {
        public string[] AllRenderMode { get; } = Enum.GetNames(typeof(AliRtcRenderMode));

        AliRtcRenderMode selectRenderMode = AliRtcRenderMode.AliRtcRenderModeAuto;

        public AliRtcRenderMode SelectRenderMode
        {
            get => selectRenderMode;
            set => SetProperty(ref selectRenderMode, value);
        }
        public ICommand PreviewCommand { get; set; }

        public MainViewModel()
        {
            PreviewCommand = new Command(StartPreview);
        }


        private void StartPreview(object o)
        {
            if (o is RTCViewer viewer)
            {
                viewer.StartPreview();
            }
        }
    }
}
