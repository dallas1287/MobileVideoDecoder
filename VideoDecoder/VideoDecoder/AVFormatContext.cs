using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoDecoder
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct AVFormatContext
    {
        public IntPtr av_class; //AVClass*
        public IntPtr iformat; //AVInputFormat*
        public IntPtr oformat; //AVOutputFormat*
        public IntPtr priv_data; //void*
        public IntPtr pb; //AVIOContext*
        public int ctx_flags;
        public uint nb_streams;
        public IntPtr streams; //AVStream**
        public IntPtr url; //char*
        public long start_time; //int64_t
        public long duration; //int64_t
        public long bit_rate; //int64_t
        public uint packet_size; //unsigned int
        public int max_delay;
        public int flags;
        public long probesize; //int64_t
        public long max_analyze_duration; //int64_t
        public IntPtr key; //const uint8_t*
        public int keylen;
        public uint nb_programs; //unsigned int
        public IntPtr programs; //AVProgram**
        public AVCodecID video_codec_id; //enum AVCodecID
        public AVCodecID audio_codec_id; //enum AVCodecID
        public AVCodecID subtitle_codec_id; //enum AVCodecID
        public uint max_index_size; //unsigned int
        public uint max_picture_buffer; //unsigned int
        public uint nb_chapters; //unsigned int
        public IntPtr chapters; //AVChapter**
        public IntPtr metadata; //AVDictionary*
        public long start_time_realtime; //int64_t
        public int fps_probe_size;
        public int error_recognition;
        public AVIOInterruptCB interrupt_callback; //AVIOInterruptCB
        public int debug;
        public long max_interleave_data; //int64_t
        public int strict_std_compliance;
        public int event_flags;
        public int max_ts_probe;
        public int avoid_negative_ts;
        public int ts_id;
        public int audio_preload;
        public int max_chunk_duration;
        public int max_chunk_size;
        public int use_wallclock_as_timestamps;
        public int avio_flags;
        public AVDurationEstimationMethod duration_estimation_method; //enum AVDurationEstimationMethod
        public long skip_initial_bytes; //int64_t
        public uint correct_ts_overflow; //unsigned int
        public int seek2any;
        public int flush_packets;
        public int probe_score;
        public int format_probesize;
        public IntPtr codec_whitelist; //char*
        public IntPtr format_whitelist; //char*
        public IntPtr av_internal; //AVForamtInternal --original name : internal
        public int io_repositioned;
        public IntPtr video_codec; //AVCodec*
        public IntPtr audio_codec; //AVCodec*
        public IntPtr subtitle_codec; //AVCodec*
        public IntPtr data_codec; //AVCodec*
        public int metadata_header_padding;
        public IntPtr opaque; //void*
        public IntPtr control_message_cb; //av_format_control_message int(*av_format_control_message)(struct AVFormatContext* s, int type void* data, size_t data_size)
        public long output_ts_offset; //int64_t
        public IntPtr dump_separator; //uint8_t*
        public AVCodecID data_codec_id; //enum AVCodecID
        public string protocol_whitelist; //char*
        public IntPtr io_open; //int(*io_open) --function ptr
        public IntPtr io_close; //void(*io_close) --function ptr
        public string protocol_blacklist; //char*
        public int max_streams;
        public int skip_estimate_duration_from_pts;
        public int max_probe_packets;

    }
}
