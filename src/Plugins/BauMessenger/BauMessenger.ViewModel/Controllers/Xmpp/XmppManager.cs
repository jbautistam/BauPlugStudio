using System;
using System.Collections.Generic;

using Bau.Libraries.LibXmppClient;

namespace Bau.Libraries.BauMessenger.ViewModel.Controllers.Xmpp
{
	/// <summary>
	///		Manager de las comunicaciones con Jabber
	/// </summary>
	internal class XmppManager
	{   
		// Variables privadas
		private JabberManager jabberManager = null;

		/// <summary>
		///		Añade una conexión a la colección de conexiones
		/// </summary>
		internal void AddConnection(string address, int port, bool useTls, string login, string password)
		{ 
			// Añade la conexión
			ManagerJabber.AddConnection(new LibXmppClient.Servers.JabberServer(address, port, useTls),
										new LibXmppClient.Users.JabberUser(address, login, password));
			// Graba la configuración
			SaveConfiguration();
		}

		/// <summary>
		///		Comprueba si existe una conexión
		/// </summary>
		internal bool Exists(string address, string login)
		{
			return ManagerJabber.Exists(address, login);
		}

		/// <summary>
		///		Carga la configuración
		/// </summary>
		internal void LoadConfiguration()
		{
			new Repository.JabberConnectionRepository().Load(ManagerJabber, BauMessengerViewModel.Instance.FileAccounts);
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		internal void SaveConfiguration()
		{
			new Repository.JabberConnectionRepository().Save(ManagerJabber, BauMessengerViewModel.Instance.FileAccounts);
		}

		/// <summary>
		///		Manager de Jabber
		/// </summary>
		internal JabberManager ManagerJabber
		{
			get
			{ 
				// Obtiene el manager de Jabber
				if (jabberManager == null)
					jabberManager = new JabberManager();
				// Devuelve el manager de Jabber
				return jabberManager;
			}
		}

		/// <summary>
		///		Colección de chats abiertos
		/// </summary>
		internal XmppChatOpenCollection ChatsOpen { get; } = new XmppChatOpenCollection();
	}
}
