using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoDecoder.Droid
{
    class AndroidCustomSurfaceView : SurfaceView
    {
        public AndroidCustomSurfaceView(Context context) : this(context, null)
        {
        }

        public AndroidCustomSurfaceView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public AndroidCustomSurfaceView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected AndroidCustomSurfaceView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

    }
}