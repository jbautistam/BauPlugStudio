using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Controllers.Xmpp
{
	/// <summary>
	///		Colección de ventanas de chat abiertas
	/// </summary>
	internal class XmppChatOpenCollection
	{
		/// <summary>
		///		Añade una conexión
		/// </summary>
		internal bool Add(JabberConnection connection, JabberContact contact)
		{
			bool added = false;

				// Añade el chat si no existía
				if (!Exists(connection, contact))
				{ 
					// Añade el chat
					Chats.Add(GetKey(connection, contact));
					// Indica que se ha añadido
					added = true;
				}
				// Devuelve el valor que indica si se ha añadido
				return added;
		}

		/// <summary>
		///		Obtiene el índice de una conexión
		/// </summary>
		internal int IndexOf(JabberConnection connection, JabberContact contact)
		{
			string key = GetKey(connection, contact);

				// Busca el índice
				for (int index = 0; index < Chats.Count; index++)
					if (Chats[index].EqualsIgnoreCase(key))
						return index;
				// Si ha llegado hasta aquí es porque no ha encontrado nada
				return -1;
		}

		/// <summary>
		///		Comprueba si existe una conexión
		/// </summary>
		internal bool Exists(JabberConnection connection, JabberContact contact)
		{
			return IndexOf(connection, contact) >= 0;
		}

		/// <summary>
		///		Elimina un chat de la colección
		/// </summary>
		internal void Remove(JabberConnection connection, JabberContact contact)
		{
			int index = IndexOf(connection, contact);

				if (index >= 0)
					Chats.RemoveAt(index);
		}

		/// <summary>
		///		Obtiene la clave del chat
		/// </summary>
		private string GetKey(JabberConnection connection, JabberContact contact)
		{
			return $"{connection.Host.Address}_{connection.User.Jid}_{contact.Jid}";
		}

		/// <summary>
		///		Colección de chat
		/// </summary>
		internal List<string> Chats { get; } = new List<string>();
	}
}
