using System;

namespace Bau.Libraries.LibDocWriter.Processor.EventArguments
{
	/// <summary>
	///		Argumento para los eventos de progreso
	/// </summary>
	public class EventArgsProgress : EventArgs
	{
		public EventArgsProgress(int actual, int total, string message = null)
		{
			Actual = actual;
			Total = total;
			Message = message;
		}

		/// <summary>
		///		Actual
		/// </summary>
		public int Actual { get; }

		/// <summary>
		///		Total
		/// </summary>
		public int Total { get; }

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; }
	}
}
