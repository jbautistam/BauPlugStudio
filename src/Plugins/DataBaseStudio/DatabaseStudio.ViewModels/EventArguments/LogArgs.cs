using System;

namespace Bau.Libraries.DatabaseStudio.ViewModels.EventArguments
{
	/// <summary>
	///		Argumentos de log
	/// </summary>
	public class LogArgs : EventArgs
	{
		public LogArgs(string type, string message)
		{
			Type = type;
			Message = message;
		}

		/// <summary>
		///		Tipo de log
		/// </summary>
		public string Type { get; }

		/// <summary>
		///		Mensaje de log
		/// </summary>
		public string Message { get; }
	}
}
