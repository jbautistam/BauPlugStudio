using System;

namespace Bau.Libraries.FtpManager.Application.EventArguments
{
	/// <summary>
	///		Mensajes de las subidas de archivos
	/// </summary>
	public class UploadEventArgs : EventArgs
	{
		public UploadEventArgs(string message, bool isError)
		{
			Message = message;
			IsError = isError;
		}

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; }

		/// <summary>
		///		Indica si es un error
		/// </summary>
		public bool IsError { get; }
	}
}