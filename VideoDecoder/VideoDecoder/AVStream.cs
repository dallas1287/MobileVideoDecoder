using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace VideoDecoder
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct AVStream
    {
        public int index;
        public int id;
        public IntPtr priv_data; //void*
        public AVRational time_base;
        public long start_time; //int64_t
        public long duration; //int64_t
        public long nb_frames; //int64_t
        public int disposition;
        public AVDiscard discard; //enum AVDiscard
        public AVRational sample_aspect_ratio;
        public IntPtr metadata; //AVDictionary*
        public AVRational avg_frame_rate;
        public AVPacket attached_pic;
        public IntPtr side_data; //AVPacketSideData*
        public int nb_side_data;
        public int event_flags;
        //public BigAnonymousStruct info;
        public int pts_wrap_bits;
        public long first_dts; //int64_t
        public long cur_dts; //int64_t
        public int last_IP_pts;
        public int probe_packets;
        public int codec_info_nb_frames;
        public AVStreamParseType needs_parsing; //enum AVSTReamParseType
        public IntPtr parser; //AVCodecParserContext*
        public IntPtr last_in_packet_buffer; //AVPacketList*
        public AVProbeData probe_data;
        public long pts_buffer; //int64_t[MAX_REORDER_DELAY + 1] ??
        public IntPtr index_entries; //AVIndexEntry*
        public int nb_index_entries;
        public uint index_entries_allocated_size;
        public AVRational r_frame_rate;
        public int stream_identifier;
        public long interleaver_chunk_size;
        public long interleaver_chunk_duration;
        public int request_probe;
        public int skip_to_keyframe;
        public int skip_samples;
        public long start_skip_samples;
        public long first_discard_sample;
        public long last_discard_sample;
        public int nb_decoded_frames;
        public long mux_ts_offset;
        public long pts_wrap_reference;
        public int pts_wrap_behavior;
        public int update_initial_durations_done;
        public long pts_reorder_error; //int64_t[MAX_REORDER_DELAY +1]
        public byte pts_reorder_error_count; //uint8_t[MAX_REORDER_DELAY +1]
        public long last_dts_for_order_check;
        public byte dts_ordered;
        public byte dts_misordered;
        public int inject_global_side_data;
        public string recommended_encoder_configuration; //char*
        public AVRational display_aspect_ratio;
        public IntPtr priv_pts; //struct FFFRac*
        public IntPtr av_internal; //AVStreamInternal --orig = internal
        public IntPtr codecpar; //AVCodecParameters*
    }
}
