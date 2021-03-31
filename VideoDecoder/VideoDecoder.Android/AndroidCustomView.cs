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
    class AndroidCustomView : TextureView
    {
        int mRatioWidth = 0;
        int mRatioHeight = 0;
        readonly object locker = new object();

        public AndroidCustomView(Context context) : this(context, null)
        {
        }

        public AndroidCustomView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public AndroidCustomView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected AndroidCustomView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public void SetAspectRatio(int width, int height)
        {
            if (width == 0 || height == 0)
            {
                throw new ArgumentException("Size can't be negative.");
            }

            mRatioWidth = width;
            mRatioHeight = height;
            RequestLayout();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);

            if (mRatioWidth == 0 || mRatioHeight == 0)
            {
                SetMeasuredDimension(width, height);
            }
            else
            {
                if (width < (float)height * mRatioWidth / mRatioHeight)
                {
                    SetMeasuredDimension(width, width * mRatioHeight / mRatioWidth);
                }
                else
                {
                    SetMeasuredDimension(height * mRatioWidth / mRatioHeight, height);
                }
            }
        }

        public void ClearCanvas(Android.Graphics.Color color)
        {
            using var canvas = LockCanvas(null);
            lock (locker)
            {
                try
                {
                    canvas.DrawColor(color);
                }
                finally
                {
                    UnlockCanvasAndPost(canvas);
                }
                Invalidate();
            }
        }
    }
}