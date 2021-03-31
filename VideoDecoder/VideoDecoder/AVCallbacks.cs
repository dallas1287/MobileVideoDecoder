using System;
using System.Collections.Generic;
using System.Text;

namespace VideoDecoder
{
    public struct AVIOInterruptCB
    {
        IntPtr callback; //int(*callback)(void*)
        IntPtr opaque; //void*
    }
}
