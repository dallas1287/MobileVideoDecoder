using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using Xamarin.Essentials;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace VideoDecoder.Droid
{ 
    public class Decoder
    {
		public async Task SaveCountAsync(int count)
		{
			var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "count.txt");
			using (var writer = File.CreateText(backingFile))
			{
				await writer.WriteLineAsync(count.ToString());
			}

			var external = Path.Combine("/storage/emulated/0/Documents", "count.txt");
			using (var writer = File.CreateText(external))
			{
				await writer.WriteLineAsync(count.ToString());
			}
		}

		public Task CopyVideoIfNotExists(Assembly assembly, string filename)
        {
            string filepath = Path.Combine("/storage/emulated/0/Documents", "eevee_master.mp4");
			if (!File.Exists(filepath))
			{
				//filename = "VideoDecoder.Droid.Resources.raw.eevee_master.mp4";
				Stream inputStream = assembly.GetManifestResourceStream(filename);
				using (var file = new FileStream(filepath, FileMode.Create, FileAccess.Write))
				{
					inputStream.CopyTo(file);
				}
			}
            return Task.CompletedTask;
        }

        public void LoadFile(string filename)
		{
			if (!File.Exists(filename))
				return;
			av_register_all();
			IntPtr fmtCtx = avformat_alloc_context();
			if (fmtCtx != IntPtr.Zero)
			{
				IntPtr dummy = IntPtr.Zero;
				int output = avformat_open_input(ref fmtCtx, filename, IntPtr.Zero, ref dummy);
				if (output < 0)
				{
					Console.WriteLine("Not opened: " + output);
					StringBuilder msg = new StringBuilder();
					int ret = av_strerror(output, msg, 256);
					string err = msg.ToString();
					Console.WriteLine(err);
					return;
				}

				AVFormatContext m_formatCtx = (AVFormatContext)Marshal.PtrToStructure(fmtCtx, typeof(AVFormatContext));
				IntPtr ps = m_formatCtx.streams;
				Console.WriteLine(ps);
				//IntPtr pStream = (IntPtr)Marshal.ReadIntPtr(m_formatCtx.streams);
				//AVStream stream = (AVStream)Marshal.PtrToStructure(pStream, typeof(AVStream));
				m_formatCtx.seek2any = 1;
				//int index = stream.index;
				//Console.WriteLine(index);
			}
		}

		[DllImport("libavformat.so", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern void av_register_all();

		[DllImport("libavformat.so", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr avformat_alloc_context();

        [DllImport("libavformat.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern int avformat_open_input([In, Out] ref IntPtr fmtCtx, [MarshalAs(UnmanagedType.LPStr)] string filename, IntPtr fmt, ref IntPtr dict);

		[DllImport("libavutil.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
		public static extern int av_strerror(int errnum, StringBuilder msg, uint size);

		[DllImport("libavcodec.so", CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public static extern UIntPtr avcodec_find_encoder(AVCodecID id);
    }
}