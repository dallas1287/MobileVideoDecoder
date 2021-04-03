using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using VideoToolbox;
using System.Timers;

namespace VideoDecoder.iOS
{
    public class IOSNativeDecoder
    {
        VTDecompressionSession decompSession;
        CGAffineTransform videoPreferredTransform;
        AVAssetReader assetReader;
        IOSCustomUIView mUiView;
        AVAssetReaderTrackOutput videoTrackOutput;
        AVAsset mAsset;
        Dictionary<long, Tuple<double, CVPixelBuffer>> mOutputFrames;
        CMTime mFrameDuration;
        long nextPts;
        Timer mPlaybackTimer;

        public IOSNativeDecoder(IOSCustomUIView view)
        {
            mUiView = view;
            mOutputFrames = new Dictionary<long, Tuple<double, CVPixelBuffer>>();
        }

        public void StartPlayback()
        {
            mPlaybackTimer = new Timer(33);
            mPlaybackTimer.AutoReset = true;
            mPlaybackTimer.Elapsed += new ElapsedEventHandler(delegate (Object source, ElapsedEventArgs e)
            {
                PushNextFrame();
            });
            mPlaybackTimer.Enabled = true;
        }

        public async Task InitAsync(string filepath)
        {
            if (!File.Exists(filepath))
                Console.WriteLine("Does not exist");
            var url = NSUrl.FromFilename(filepath);
            mAsset = AVAsset.FromUrl(url);
            nextPts = 0;
            BuildTrackOutput(mAsset);
            await ReadSampleBuffers();
        }

        void BuildTrackOutput(AVAsset asset)
        {
            NSError error;
            assetReader = AVAssetReader.FromAsset(asset, out error);
            if (error != null)
                Console.WriteLine("Could not create Asset Reader: {0}", error.Description);

            AVAssetTrack[] videoTracks = asset.GetTracks(AVMediaTypes.Video);
            AVAssetTrack videoTrack = videoTracks[0];
            CreateDecompSession(videoTrack);
            videoTrackOutput = AVAssetReaderTrackOutput.Create(videoTrack, (AVVideoSettingsUncompressed)null);
            if (assetReader.CanAddOutput(videoTrackOutput))
                assetReader.AddOutput(videoTrackOutput);
        }

        public Task ReadSampleBuffers()
        {
            if (assetReader.Status != AVAssetReaderStatus.Reading && !assetReader.StartReading())
                return Task.CompletedTask;

            while (assetReader.Status == AVAssetReaderStatus.Reading)
            {
                CMSampleBuffer sampleBuffer = videoTrackOutput.CopyNextSampleBuffer();
                if (sampleBuffer != null)
                {
                    VTDecodeFrameFlags flags = VTDecodeFrameFlags.EnableAsynchronousDecompression | VTDecodeFrameFlags.EnableTemporalProcessing;
                    VTDecodeInfoFlags flagOut;
                    decompSession.DecodeFrame(sampleBuffer, flags, IntPtr.Zero, out flagOut);
                    sampleBuffer.Dispose();
                }
                else if (assetReader.Status == AVAssetReaderStatus.Failed)
                {
                    Console.WriteLine("Asset reader failed with error: {0}", assetReader.Error.Description);
                }
                else if (assetReader.Status == AVAssetReaderStatus.Completed)
                {
                    foreach(var frame in mOutputFrames)
                    {
                        Console.WriteLine("Frame Key: " + frame.Key);
                    }
                    decompSession.Dispose();
                }
            }
            return Task.CompletedTask;
        }

        public void CreateDecompSession(AVAssetTrack track)
        {
            CMFormatDescription[] formatDescriptions = track.FormatDescriptions;
            var formatDesc = (CMVideoFormatDescription)formatDescriptions[0];
            videoPreferredTransform = track.PreferredTransform;

            VTVideoDecoderSpecification decoderSpec = new VTVideoDecoderSpecification();
            decoderSpec.EnableHardwareAcceleratedVideoDecoder = true;
            CVPixelBufferAttributes bufAttr = new CVPixelBufferAttributes();
            decompSession = VTDecompressionSession.Create(DecompressOutputCallback, formatDesc, decoderSpec, bufAttr);
        }

        private void DecompressOutputCallback(IntPtr sourceFrame, VTStatus status, VTDecodeInfoFlags flags, CVImageBuffer buffer, CMTime presentationTimeStamp, CMTime presentationDuration)
        {
            if (status != VTStatus.Ok)
                Console.WriteLine("Error decompresing frame: {0:#.###} error: {1} infoFlags: {2}", (float)presentationTimeStamp.Value / presentationTimeStamp.TimeScale, (int)status, flags);
            if (buffer == null)
                return;

            if(presentationTimeStamp.IsInvalid)
            {
                Console.WriteLine("Not a valid image");
                return;
            }

            mFrameDuration = presentationDuration;
            var pixelBuffer = Runtime.GetINativeObject<CVPixelBuffer>(buffer.Handle, false);
            mOutputFrames.Add(presentationTimeStamp.Value, new Tuple<double, CVPixelBuffer>(presentationTimeStamp.Seconds, pixelBuffer));
        }

        void PushNextFrame()
        {
            if (mOutputFrames.ContainsKey(nextPts))
            {
                mUiView.PushNextFrame(mOutputFrames[nextPts].Item2, mOutputFrames[nextPts].Item1, videoPreferredTransform);
                if (mOutputFrames.ContainsKey(nextPts - mFrameDuration.Value))
                    mOutputFrames.Remove(nextPts - mFrameDuration.Value);
                nextPts += mFrameDuration.Value;
            }
        }
    }
}
