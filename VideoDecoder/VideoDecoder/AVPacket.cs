using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoDecoder
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct AVPacket
    {
        IntPtr buf; //AVBufferRef*
        long pts;
        long dts;
        IntPtr data; //uint8_t*
        int size;
        int stream_index;
        int flags;
        IntPtr side_data; //AVPacketSideData*
        int side_data_elems;
        long duration;
        long pos;
        long convergence_duration; //attribute_deprecated in64_t
    }
}
