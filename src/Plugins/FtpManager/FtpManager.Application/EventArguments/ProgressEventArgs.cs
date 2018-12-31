using System;

namespace Bau.Libraries.FtpManager.Application.EventArguments
{
	/// <summary>
	///		Argumentos del evento de progreso
	/// </summary>
	public class ProgressEventArgs : EventArgs
	{
		public ProgressEventArgs(string connection, int taskId, string message, int progress, int maximum)
		{
			Connection = connection;
			TaskId = taskId;
			Message = message;
			Progress = progress;
			Maximum = maximum;
		}

		/// <summary>
		///		Conexión
		/// </summary>
		public string Connection { get; }

		/// <summary>
		///		Número de tarea
		/// </summary>
		public int TaskId { get; }

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; }

		/// <summary>
		///		Progreso (valor actual)
		/// </summary>
		public int Progress { get; }

		/// <summary>
		///		Número máximo
		/// </summary>
		public int Maximum { get; }
	}
}
