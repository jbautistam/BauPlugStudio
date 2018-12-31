using System;

namespace Bau.Libraries.FtpManager.Model.Connections
{
	/// <summary>
	///		Clase con los datos de una conexión
	/// </summary>
	public class FtpConnectionModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Enumerados públicos
	  /// <summary>
	  ///		Protocolos
	  /// </summary>
		public enum Protocol
		{
			/// <summary>Ftp</summary>
			Ftp,
			/// <summary>FtpS</summary>
			FtpS,
			/// <summary>FtpEs</summary>
			FtpEs,
			/// <summary>SFtp</summary>
			SFtp
		}

		/// <summary>
		///		Protocolo
		/// </summary>
		public Protocol FtpProtocol { get; set; }

		/// <summary>
		///		Nombre del servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Puerto
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		///		Tiempo de espera
		/// </summary>
		public int TimeOut { get; set; } = 20;

		/// <summary>
		///		Usuario de SQL Server
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña de SQL Server
		/// </summary>
		public string Password { get; set; }
	}
}
