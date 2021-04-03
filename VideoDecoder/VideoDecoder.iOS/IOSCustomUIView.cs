using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreGraphics;
using CoreVideo;
using Foundation;
using UIKit;

namespace VideoDecoder.iOS
{
    public class IOSCustomUIView : UIView
    {
        IOSNativeDecoder mDecoder;
        Stopwatch mStopwatch;
        bool firstShow = true;

        public IOSCustomUIView()
        {
            this.BackgroundColor = UIColor.Red;
            mDecoder = new IOSNativeDecoder(this);
            var path = NSBundle.MainBundle.PathForResource("robot", ".mp4");
            var t = new Task(() =>
            {
                _ = mDecoder.InitAsync(path);
            });
            t.Start();
            mStopwatch = new Stopwatch();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        public override void Draw(CGRect rect)
        {
            if(firstShow)
            {
                mDecoder.StartPlayback();
                firstShow = false;
            }
            base.Draw(rect);
        }

        public void DisplayPixelBuffer(CVPixelBuffer pixelBuffer, double framePts, CGAffineTransform videoPreferredTransform)
        {
            nint width = pixelBuffer.Width;
            nint height = pixelBuffer.Height;
            var f = Frame;

            if (Layer.Sublayers == null)
            {
                var layer = new EAGLLayer();
                if (Math.Abs(videoPreferredTransform.xx + 1f) < float.Epsilon)
                    layer.AffineTransform.Rotate(NMath.PI);
                else if (Math.Abs(videoPreferredTransform.yy) < float.Epsilon)
                    layer.AffineTransform.Rotate(NMath.PI / 2);

                layer.Frame = f;
                layer.PresentationRect = new CGSize(f.Width, f.Height);
                layer.DrawsAsynchronously = true;
                layer.TimeCode = framePts.ToString("0.000");
                layer.SetupGL();

                Layer.AddSublayer(layer);
            }
            ((EAGLLayer)Layer.Sublayers[0]).TimeCode = framePts.ToString("0.000");
            ((EAGLLayer)Layer.Sublayers[0]).DisplayPixelBuffer(pixelBuffer);
            mStopwatch.Stop();
            double elapsed = (mStopwatch.ElapsedTicks / (double)Stopwatch.Frequency) * 1000;
            Console.WriteLine("Texture Updated: " + elapsed);
            mStopwatch.Restart();
        }

        public void PushNextFrame(CVPixelBuffer pixelBuffer, double framePts, CGAffineTransform videoPreferredTransform)
        {
            InvokeOnMainThread(() =>
            {
                DisplayPixelBuffer(pixelBuffer, framePts, videoPreferredTransform);
                SetNeedsDisplay();
            });
        }
    }
}
