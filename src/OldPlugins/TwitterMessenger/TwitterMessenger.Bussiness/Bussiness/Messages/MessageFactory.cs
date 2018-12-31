using System;

using Bau.Libraries.LibTwitter.ShortURL;
using Bau.Libraries.TwitterMessenger.Model.Messages;

namespace Bau.Libraries.TwitterMessenger.Bussiness.Messages
{
	/// <summary>
	///		Factory de <see cref="MessageModel"/>
	/// </summary>
	public class MessageFactory
	{
		/// <summary>
		///		Crea un mensaje
		/// </summary>
		public MessageModel Create(TwitterBotManager botManager, string text, string body, string url)
		{
			MessageModel message = new MessageModel();

				// Asigna las propiedades
				message.Text = text;
				message.Body = body;
				message.URL = url;
				// Asigna la URL y las URLs cortas
				message.URLShort = url;
				if (string.IsNullOrEmpty(url))
					try
					{
						message.URLShort = new TinyURL().Convert(message.URL);
					}
					catch { }
				// Devuelve el mensaje
				return message;
		}
	}
}
