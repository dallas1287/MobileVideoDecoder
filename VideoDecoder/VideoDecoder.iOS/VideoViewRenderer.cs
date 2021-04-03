using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(VideoDecoder.CustomMediaView), typeof(VideoDecoder.iOS.VideoViewRenderer))]
namespace VideoDecoder.iOS
{
    public class VideoViewRenderer : ViewRenderer<CustomMediaView, IOSCustomUIView>
    {
        IOSCustomUIView mUiView;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomMediaView> e)
        {
            base.OnElementChanged(e);

            if(Control == null)
            {
                mUiView = new IOSCustomUIView();
                SetNativeControl(mUiView);
            }
        }
    }
}
