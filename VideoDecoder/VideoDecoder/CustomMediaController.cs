using System;
using System.Collections.Generic;
using System.Text;

namespace VideoDecoder
{
	public enum MediaElementState
	{
		Closed,
		Opening,
		Buffering,
		Playing,
		Paused,
		Stopped,
	}
	public interface ICustomMediaController
    {
		double BufferingProgress { get; set; }

		MediaElementState CurrentState { get; set; }

		TimeSpan? Duration { get; set; }

		TimeSpan Position { get; set; }

		int VideoHeight { get; set; }

		int VideoWidth { get; set; }

		double Volume { get; set; }

		void OnMediaEnded();

		void OnMediaFailed();

		void OnMediaOpened();

		void OnSeekCompleted();
	}
}
