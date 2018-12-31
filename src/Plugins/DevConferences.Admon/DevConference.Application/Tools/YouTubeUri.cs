using System;

namespace Bau.Libraries.DevConference.Application.Tools
{
    /// <summary>An URI to a YouTube MP4 video or audio stream. </summary>
    public class YouTubeUri
    {

		/// <summary>Enumeration of stream qualities. </summary>
		public enum YouTubeQuality : short
		{
			// video
			Quality144P,
			Quality240P,
			Quality270P,
			Quality360P,
			Quality480P,
			Quality520P,
			Quality720P,
			Quality1080P,
			Quality2160P,

			// audio
			QualityLow,
			QualityMedium,
			QualityHigh,

			NotAvailable,
			Unknown
		}

		/// <summary>Enumeration of thumbnail sizes. </summary>
		public enum YouTubeThumbnailSize : short
		{
			Small,
			Medium,
			Large
		}

        /// <summary>Gets stream's video quality. </summary>
        public YouTubeQuality VideoQuality
        {
            get
            {
                switch (Itag)
                {
                    // video & audio
                    case 5: return YouTubeQuality.Quality240P;
                    case 6: return YouTubeQuality.Quality270P;
                    case 17: return YouTubeQuality.Quality144P;
                    case 18: return YouTubeQuality.Quality360P;
                    case 22: return YouTubeQuality.Quality720P;
                    case 34: return YouTubeQuality.Quality360P;
                    case 35: return YouTubeQuality.Quality480P;
                    case 36: return YouTubeQuality.Quality240P;
                    case 37: return YouTubeQuality.Quality1080P;
                    case 38: return YouTubeQuality.Quality2160P;

                    // 3d video & audio
                    case 82: return YouTubeQuality.Quality360P;
                    case 83: return YouTubeQuality.Quality480P;
                    case 84: return YouTubeQuality.Quality720P;
                    case 85: return YouTubeQuality.Quality520P;

                    // video only
                    case 133: return YouTubeQuality.Quality240P;
                    case 134: return YouTubeQuality.Quality360P;
                    case 135: return YouTubeQuality.Quality480P;
                    case 136: return YouTubeQuality.Quality720P;
                    case 137: return YouTubeQuality.Quality1080P;
                    case 138: return YouTubeQuality.Quality2160P;
                    case 160: return YouTubeQuality.Quality144P;

                    // audio only
                    case 139: return YouTubeQuality.NotAvailable;
                    case 140: return YouTubeQuality.NotAvailable;
                    case 141: return YouTubeQuality.NotAvailable;
                }

                return YouTubeQuality.Unknown;
            }
        }

        /// <summary>Gets stream's audio quality. </summary>
        public YouTubeQuality AudioQuality
        {
            get
            {
                switch (Itag)
                {
                    // video & audio
                    case 5: return YouTubeQuality.QualityLow;
                    case 6: return YouTubeQuality.QualityLow;
                    case 17: return YouTubeQuality.QualityLow;
                    case 18: return YouTubeQuality.QualityMedium;
                    case 22: return YouTubeQuality.QualityHigh;
                    case 34: return YouTubeQuality.QualityMedium;
                    case 35: return YouTubeQuality.QualityMedium;
                    case 36: return YouTubeQuality.QualityLow;
                    case 37: return YouTubeQuality.QualityHigh;
                    case 38: return YouTubeQuality.QualityHigh;

                    // 3d video & audio
                    case 82: return YouTubeQuality.QualityMedium;
                    case 83: return YouTubeQuality.QualityMedium;
                    case 84: return YouTubeQuality.QualityHigh;
                    case 85: return YouTubeQuality.QualityHigh;

                    // video only
                    case 133: return YouTubeQuality.NotAvailable;
                    case 134: return YouTubeQuality.NotAvailable;
                    case 135: return YouTubeQuality.NotAvailable;
                    case 136: return YouTubeQuality.NotAvailable;
                    case 137: return YouTubeQuality.NotAvailable;
                    case 138: return YouTubeQuality.NotAvailable;
                    case 160: return YouTubeQuality.NotAvailable;

                    // audio only
                    case 139: return YouTubeQuality.QualityLow;
                    case 140: return YouTubeQuality.QualityMedium;
                    case 141: return YouTubeQuality.QualityHigh;
                }
                return YouTubeQuality.Unknown;
            }
        }

        internal bool IsValid
        {
            get { return Uri != null && Uri.ToString().StartsWith("http") && Itag > 0 && Type != null; }
        }

        /// <summary>Gets the Itag of the stream. </summary>
        public int Itag { get; internal set; }

        /// <summary>Gets the <see cref="Uri"/> of the stream. </summary>
        public Uri Uri { get; internal set; }

        /// <summary>Gets the stream type. </summary>
        public string Type { get; internal set; }

        /// <summary>Gets a value indicating whether the stream has audio. </summary>
        public bool HasAudio
        {
            get { return AudioQuality != YouTubeQuality.Unknown && AudioQuality != YouTubeQuality.NotAvailable; }
        }

        /// <summary>Gets a value indicating whether the stream has video. </summary>
        public bool HasVideo
        {
            get { return VideoQuality != YouTubeQuality.Unknown && VideoQuality != YouTubeQuality.NotAvailable; }
        }

        /// <summary>Gets a value indicating whether the stream has 3D video. </summary>
        public bool Is3DVideo
        {
            get
            {
                if (VideoQuality == YouTubeQuality.Unknown)
                    return false;

                return Itag >= 82 && Itag <= 85;
            }
        }
	}
}