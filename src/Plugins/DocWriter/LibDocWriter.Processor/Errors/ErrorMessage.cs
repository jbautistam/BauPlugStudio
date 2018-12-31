using System;

namespace Bau.Libraries.LibDocWriter.Processor.Errors
{
	/// <summary>
	///		Mensaje de error
	/// </summary>
	public class ErrorMessage
	{
		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; internal set; }

		/// <summary>
		///		Token que ha generado el error
		/// </summary>
		public string Token { get; internal set; }

		/// <summary>
		///		Mensaje de error
		/// </summary>
		public string Message { get; internal set; }

		/// <summary>
		///		Fila
		/// </summary>
		public int Row { get; internal set; }

		/// <summary>
		///		Columna
		/// </summary>
		public int Column { get; internal set; }
	}
}
