using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoDecoder
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class AVRational
    {
        public int num;
        public int den;
    }
}
