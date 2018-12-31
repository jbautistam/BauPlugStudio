using System;

using Bau.Libraries.LibXmppClient.Core.Forms;
using Bau.Libraries.BauMessenger.ViewModel.Contacts;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.BauMessenger.Controllers
{
	/// <summary>
	///		Controlador de ventanas de BauMessenger
	/// </summary>
	public class ViewsController : Libraries.BauMessenger.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre el panel de mensajes de Jabber
		/// </summary>
		public void OpenBauMessengerView()
		{
			BauMessengerPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane
								("BAU_MESSENGER_EXPLORER", LayoutEnums.DockPosition.Right,
								 "BauMessenger", new Views.Explorer.TreeConnectionsView());
		}

		/// <summary>
		///		Abre una ventana de chat
		/// </summary>
		public void OpenChatView(Libraries.BauMessenger.ViewModel.Chat.ChatViewModel chatViewModel)
		{
			Views.Chat.ChatView chat = new Views.Chat.ChatView(chatViewModel);

				// Abre el formulario
				BauMessengerPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument
						($"JABBER_CHAT_{chatViewModel.Connection.Host.Address}_{chatViewModel.Connection.User.FullJid}_{chatViewModel.Contact.FullJid}",
						 chatViewModel.Contact.FullName, chat);
		}

		/// <summary>
		///		Abre el formulario de propiedades de un contacto
		/// </summary>
		public SystemControllerEnums.ResultType OpenPropertiesContact(ContactViewModel viewModel)
		{
			return BauMessengerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Contact.ContactView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de nueva conexión
		/// </summary>
		public SystemControllerEnums.ResultType OpenNewConnection()
		{
			return BauMessengerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Connections.NewConnectionView());
		}

		/// <summary>
		///		Abre el formulario para obtener los datos de un formulario de Jabber
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormXmppView(JabberForm xmppform, string title)
		{
			return BauMessengerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Forms.XmppDataFormView(xmppform, title));
		}

		/// <summary>
		///		Imagen de un servidor conectado
		/// </summary>
		public string ImageServerConnected
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/ServerConnected.png"; }
		}

		/// <summary>
		///		Imagen de un servidor desconectado
		/// </summary>
		public string ImageServerDisconnected
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/ServerDisconnected.png"; }
		}

		/// <summary>
		///		Imagen de un grupo
		/// </summary>
		public string ImageGroup
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/Group.png"; }
		}

		/// <summary>
		///		Imagen de un usuario pendiente de aprobar solicitud
		/// </summary>
		public string ImageUserPendingRequest
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserPendingRequest.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Away
		/// </summary>
		public string ImageUserStatusAway
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserStatusAway.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Chat
		/// </summary>
		public string ImageUserStatusChat
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserStatusChat.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Dnd
		/// </summary>
		public string ImageUserStatusDnd
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserStatusDnd.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Offline
		/// </summary>
		public string ImageUserStatusOffline
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserStatusOffline.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Online
		/// </summary>
		public string ImageUserStatusOnline
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserStatusOnline.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Xa
		/// </summary>
		public string ImageUserStatusXa
		{
			get { return "pack://application:,,,/BauMessenger.Plugin;component/Resources/UserStatusXa.png"; }
		}
	}
}
