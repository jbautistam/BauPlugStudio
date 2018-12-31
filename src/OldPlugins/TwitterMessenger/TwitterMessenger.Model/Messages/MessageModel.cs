using System;

namespace Bau.Libraries.TwitterMessenger.Model.Messages
{
	/// <summary>
	///		Clase con los datos de un mensaje
	/// </summary>
	public class MessageModel : LibDataStructures.Base.BaseModel
	{
		/// <summary>
		///		Texto
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		///		Cuerpo del mensaje
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		///		URL
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		///		URL corta normal
		/// </summary>
		public string URLShort { get; set; }

		/// <summary>
		///		Fecha de alta del mensaje
		/// </summary>
		public DateTime DateNew { get; set; }
	}
}
