using Android.Views;
using System;
using Android.Media;
using System.Collections.Specialized;
using Java.Nio;
using Android.Graphics;
using System.Diagnostics;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace VideoDecoder.Droid
{
    class AndroidNativeDecoder
    {
        private MediaCodec mDecoder;
        private DecoderCallback mCallback;
        private MediaExtractor mExtractor;
        private Surface mSurface;

        const long fps = 1000000 / 30;

        public AndroidNativeDecoder()
        {
        }

        public DecoderCallback Callback { get => mCallback; private set => mCallback = value; }

        public void InitDecoder(string path, SurfaceTexture texture)
        {
            StringCollection stringCollection = new StringCollection();
            var codecList = new MediaCodecList(MediaCodecListKind.AllCodecs).GetCodecInfos();
            foreach (var s in stringCollection)
                Console.WriteLine(s);

            mExtractor = new MediaExtractor();
            MediaFormat fmt = new MediaFormat();
            mExtractor.SetDataSource(path);
            for (int i = 0; i < mExtractor.TrackCount; ++i)
            {
                fmt = mExtractor.GetTrackFormat(i);
                Console.WriteLine(fmt.GetString(MediaFormat.KeyMime));
                Console.WriteLine(fmt.GetInteger(MediaFormat.KeyFrameRate));
                mExtractor.SelectTrack(i);
            }
            mCallback = new DecoderCallback();
            mCallback.Extractor = mExtractor;
            MediaCodecList list = new MediaCodecList(MediaCodecListKind.AllCodecs);
            string found = list.FindDecoderForFormat(fmt);
            try
            {
                mDecoder = MediaCodec.CreateByCodecName(found);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Couldn't create decoder" + e.Message);
                return;
            }
            mDecoder.SetCallback(mCallback);
            mSurface = new Surface(texture);
            mDecoder.Configure(fmt, mSurface, MediaCodecConfigFlags.None, null);
            try
            {
                mDecoder.Start();
            }
            catch(System.Exception e)
            {
                Console.WriteLine("Didn't start: " + e.Message);
            }
        }
    }

    class DecoderCallback : MediaCodec.Callback
    {
        Timer mPlaybackTimer;
        private double mLastShown;
        List<double> mFrames;

        public DecoderCallback()
        {
            mFrames = new List<double>();
        }
        public MediaExtractor Extractor { get; set; }
        public double LastShown { get => mLastShown; set
            {
                mLastShown = value;
                mFrames.Add(mLastShown);
            }
        }
        public override void OnError(MediaCodec codec, MediaCodec.CodecException e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        public override void OnInputBufferAvailable(MediaCodec codec, int index)
        {
            ByteBuffer inputBuffer = codec.GetInputBuffer(index);
            int retBufferSize = Extractor.ReadSampleData(inputBuffer, 0);

            int trackIndex = Extractor.SampleTrackIndex;
            long presentationTimeUs = Extractor.SampleTime;
            long size = Extractor.SampleSize;
            //Console.WriteLine(presentationTimeUs);
            //var flags = Extractor.SampleFlags;
            //if (flags.HasFlag(MediaExtractorSampleFlags.Sync))
            //    Console.WriteLine("Is Key Frame");
            //if (flags.HasFlag(MediaExtractorSampleFlags.PartialFrame))
            //    Console.WriteLine("Is Partial Frame");
            Extractor.Advance();

            codec.QueueInputBuffer(index, 0, retBufferSize > 0 ? retBufferSize : 0, presentationTimeUs, retBufferSize > 0 ? MediaCodecBufferFlags.None : MediaCodecBufferFlags.EndOfStream);
        }

        public override void OnOutputBufferAvailable(MediaCodec codec, int index, MediaCodec.BufferInfo info)
        {
            ByteBuffer outputBuffer = codec.GetOutputBuffer(index);
            MediaFormat fmt = codec.GetOutputFormat(index);

            double delay = GetDelay();
            if (delay <= 0.0)
            {
                codec.ReleaseOutputBuffer(index, render: true);
            }
            else
            {
                mPlaybackTimer = new Timer(delay);
                mPlaybackTimer.AutoReset = false;
                mPlaybackTimer.Elapsed += new ElapsedEventHandler(delegate (Object source, ElapsedEventArgs e)
                {
                    codec.ReleaseOutputBuffer(index, render: true);
                });
                mPlaybackTimer.Enabled = true;
            }
        }

        public override void OnOutputFormatChanged(MediaCodec codec, MediaFormat format)
        {
            Console.WriteLine("OutputFormat Changed: " + format.GetInteger(MediaFormat.KeyColorFormat) + " " + format.GetInteger(MediaFormat.KeyWidth) + " x " + format.GetInteger(MediaFormat.KeyHeight));
        }

        private double GetDelay()
        {
            if (LastShown <= 0)
                return 0.0;

            double delay = 16.66 - (LastShown - 33.33);
            //Console.WriteLine("LastShown: " + LastShown + " Difference: " + delay);
            return delay;
        }
    }
}