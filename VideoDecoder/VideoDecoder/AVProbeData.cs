using System;
using System.Collections.Generic;
using System.Text;

namespace VideoDecoder
{
    public struct AVProbeData
    {
        string filename; //const char*
        string buf; //unsigned char*
        int buf_size;
        string mime_type; //const char*
    }
}
