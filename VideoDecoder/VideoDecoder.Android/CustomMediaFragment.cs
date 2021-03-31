using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.Fragment.App;
using System;
using System.Diagnostics;
using System.IO;

namespace VideoDecoder.Droid
{
    class CustomMediaFragment : Fragment, TextureView.ISurfaceTextureListener
    {
        AndroidCustomView mTexture;
        AndroidNativeDecoder mDecoder;
        Stopwatch mStopwatch;

        public CustomMediaFragment()
        {
            mStopwatch = new Stopwatch();
        }

        public CustomMediaFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomMediaView Element { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) => inflater.Inflate(Resource.Layout.CustomMediaFragment, null);
        public override void OnViewCreated(Android.Views.View view, Bundle savedInstanceState)
        {
            mTexture = view.FindViewById<AndroidCustomView>(Resource.Id.androidcustomview);
            mTexture.SurfaceTextureListener = this;
        }

        void ConfigureTransform(int viewWidth, int viewHeight)
        {
            if (mTexture == null)
                return;
        }

        #region ISurfaceTextureListener
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            //mTexture?.ClearCanvas(new Color(0,255,0));
            string filename = "/storage/emulated/0/Documents/eevee_master.mp4";
            if (!File.Exists(filename))
                throw new InvalidDataException();
            mDecoder = new AndroidNativeDecoder();
            mDecoder.InitDecoder(filename, mTexture.SurfaceTexture);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            Console.WriteLine("Size Changed: " + width + "x" + height);
            ConfigureTransform(width, height);
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            mStopwatch.Stop();
            double elapsed = (mStopwatch.ElapsedTicks / (double)Stopwatch.Frequency) * 1000;
            Console.WriteLine("Surface Texture Updated: " + elapsed);
            mDecoder.Callback.LastShown = elapsed;
            mStopwatch.Restart();
        }
        #endregion
    }
}