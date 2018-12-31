using System;

using Bau.Libraries.LibXmppClient.Core.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.BauMessenger.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre el panel de mensajes de Jabber
		/// </summary>
		void OpenBauMessengerView();

		/// <summary>
		///		Abre una ventana de chat
		/// </summary>
		void OpenChatView(Chat.ChatViewModel chatViewModel);

		/// <summary>
		///		Abre el formulario de modificación de un contacto
		/// </summary>
		SystemControllerEnums.ResultType OpenPropertiesContact(Contacts.ContactViewModel viewModel);

		/// <summary>
		///		Abre el formulario de nueva conexión
		/// </summary>
		SystemControllerEnums.ResultType OpenNewConnection();

		/// <summary>
		///		Abre el formulario para rellenar un formulario de XMPP
		/// </summary>
		SystemControllerEnums.ResultType OpenFormXmppView(JabberForm xmppform, string title);

		/// <summary>
		///		Imagen de servidor desconectado
		/// </summary>
		string ImageServerDisconnected { get; }

		/// <summary>
		///		Imagen de servidor conectado
		/// </summary>
		string ImageServerConnected { get; }

		/// <summary>
		///		Imagen de grupo
		/// </summary>
		string ImageGroup { get; }

		/// <summary>
		///		Imagen de un usuario pendiente de aprobar solicitud
		/// </summary>
		string ImageUserPendingRequest { get; }

		/// <summary>
		///		Imagen de un usuario en estado Away
		/// </summary>
		string ImageUserStatusAway { get; }

		/// <summary>
		///		Imagen de un usuario en estado Chat
		/// </summary>
		string ImageUserStatusChat { get; }

		/// <summary>
		///		Imagen de un usuario en estado Dnd
		/// </summary>
		string ImageUserStatusDnd { get; }

		/// <summary>
		///		Imagen de un usuario en estado Offline
		/// </summary>
		string ImageUserStatusOffline { get; }

		/// <summary>
		///		Imagen de un usuario en estado Online
		/// </summary>
		string ImageUserStatusOnline { get; }

		/// <summary>
		///		Imagen de un usuario en estado Xa
		/// </summary>
		string ImageUserStatusXa { get; }
	}
}
