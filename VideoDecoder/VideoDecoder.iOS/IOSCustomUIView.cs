using System;
using System.Collections.Generic;
using System.Timers;
using CoreAnimation;
using CoreGraphics;
using CoreVideo;
using Foundation;
using UIKit;

namespace VideoDecoder.iOS
{
    public class IOSCustomUIView : UIView
    {
        IOSNativeDecoder mDecoder;
        CVPixelBuffer curBuffer;
        double curPts;
        double lastPts;
        CGAffineTransform prefTransform;
        Timer mPlaybackTimer;

        public IOSCustomUIView()
        {
            mDecoder = new IOSNativeDecoder(this);
            var path = NSBundle.MainBundle.PathForResource("robot", ".mp4");
            mDecoder.Init(path);
            this.BackgroundColor = UIColor.Red;
            mPlaybackTimer = new Timer(33);
            mPlaybackTimer.AutoReset = false;
            mPlaybackTimer.Elapsed += new ElapsedEventHandler(delegate (Object source, ElapsedEventArgs e)
            {
                RequestNewFrame();
            });
            mPlaybackTimer.Enabled = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            if(curBuffer != null)
                DisplayPixelBuffer(curBuffer, curPts, prefTransform);
        }

        void RequestNewFrame()
        {
            Console.WriteLine("Requesting Next Frame");
            mDecoder.ReadSampleBuffers();
        }

        public void DisplayPixelBuffer(CVPixelBuffer pixelBuffer, double framePts, CGAffineTransform videoPreferredTransform)
        {
            nint width = pixelBuffer.Width;
            nint height = pixelBuffer.Height;
            var f = Frame;

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
            layer.DisplayPixelBuffer(pixelBuffer);
        }

        public void PushNextFrame(CVPixelBuffer pixelBuffer, double framePts, CGAffineTransform videoPreferredTransform)
        {
            curBuffer = pixelBuffer;
            lastPts = curPts;
            curPts = framePts;
            prefTransform = videoPreferredTransform;
            Console.WriteLine("caught next frame: " + framePts);
            InvokeOnMainThread(() =>
            {
                SetNeedsDisplay();
            });
        }

        void MoveTimeLine()
        {
            var layersForRemoval = new List<CALayer>();
            foreach (CALayer layer in Layer.Sublayers)
            {
                if (layer is EAGLLayer)
                {
                    CGRect frame = layer.Frame;
                    var newFrame = new CGRect(frame.Location.X + 20f, frame.Location.Y - 20f, frame.Width, frame.Height);
                    layer.Frame = newFrame;
                    CGRect screenBounds = UIScreen.MainScreen.Bounds;

                    if ((newFrame.Location.X >= screenBounds.Location.X + screenBounds.Width) ||
                        newFrame.Location.Y >= (screenBounds.Location.Y + screenBounds.Height))
                    {
                        layersForRemoval.Add(layer);
                    }
                }
            }

            foreach (var layer in layersForRemoval)
            {
                layer.RemoveFromSuperLayer();
                layer.Dispose();
            }
        }
    }
}
