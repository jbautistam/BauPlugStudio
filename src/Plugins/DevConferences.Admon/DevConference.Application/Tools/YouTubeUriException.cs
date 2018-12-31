using System;

namespace Bau.Libraries.DevConference.Application.Tools
{
    /// <summary>
	///		Excepción lanzada cuando no se encuentra una URL de YouTube correcta
	///	</summary>
    public class YouTubeUriException : Exception
    {
        internal YouTubeUriException(string message) : base(message) { }

		public YouTubeUriException() : base()
		{
		}

		protected YouTubeUriException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
		{
		}

		public YouTubeUriException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
