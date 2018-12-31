using System;
using System.Linq;

namespace Bau.Libraries.TwitterMessenger.Model.Messages
{
	/// <summary>
	///		Colección de <see cref="MessageModel"/>
	/// </summary>
	public class MessagesModelCollection : LibDataStructures.Base.BaseModelCollection<MessageModel>
	{
		/// <summary>
		///		Añade un mensaje
		/// </summary>
		public void Add(string text, string url, string urlShort)
		{
			Add(new MessageModel { Text = text, URL = url, URLShort = urlShort });
		}

		/// <summary>
		///		Busca un mensaje asociado a una URL
		/// </summary>
		public MessageModel SearchByURL(string url)
		{
			return this.FirstOrDefault<MessageModel>(message => !string.IsNullOrEmpty(message.URL) && message.URL.Equals(url, StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		///		Comprueba si existe un mensaje asociado a una URL
		/// </summary>
		public bool ExistsURL(string url)
		{
			return SearchByURL(url) != null;
		}

		/// <summary>
		///		Ordena los mensajes por fecha
		/// </summary>
		public void SortByDate(bool ascending)
		{
			Sort(new Comparer.MessageModelByDateComparer(ascending));
		}
	}
}
