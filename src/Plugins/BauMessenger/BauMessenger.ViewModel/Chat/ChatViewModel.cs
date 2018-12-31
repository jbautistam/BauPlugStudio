using System;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.LibXmppClient.EventArguments;
using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Chat
{
	/// <summary>
	///		ViewModel para una ventana de chat
	/// </summary>
	public class ChatViewModel : BaseFormViewModel
	{ 
		// Variables privadas
		private string _chatLog, _chatLogRtf, _messageToSend;
		private RtfTextBuilder _rtfBuilder = new RtfTextBuilder();

		public ChatViewModel(JabberConnection connection, JabberContact contact, string message) : base(false)
		{ 
			// Inicializa las propiedades
			Connection = connection;
			Contact = contact;
			// Inicializa los comandos
			SendMessageCommand = new BaseCommand("Enviar mensaje",
												 parameter => ExecuteAction(nameof(SendMessageCommand), parameter),
												 parameter => CanExecuteAction(nameof(SendMessageCommand), parameter))
										.AddListener(this, nameof(MessageToSend));
			// Inicializa la variable que actúa como manager de XMPP en el árbol
			XmppClient = BauMessengerViewModel.Instance.BauMessenger;
			// Añade los manejadores de eventos
			XmppClient.ManagerJabber.MessageReceived += (sender, evntArgs) => TreatMessageReceived(evntArgs.Contact, evntArgs.Body);
			XmppClient.ManagerJabber.ChangedStatus += (sender, evntArgs) => TreatChangedStatus(evntArgs);
			XmppClient.ManagerJabber.ChangedUserStatus += (sender, evntArgs) => SetUserStatus(evntArgs.Connection, evntArgs.User.Status.Status, evntArgs.User.Status.Message);
			// Añade el mensaje inicial
			if (!message.IsEmpty())
				ShowMessage(Contact, message);
		}

		/// <summary>
		///		Trata el mensaje recibido
		/// </summary>
		private void TreatMessageReceived(JabberContact contact, string message)
		{
			if (contact.EqualsTo(Contact))
				ShowMessage(contact, message);
		}

		/// <summary>
		///		Cambia el estado del usuario
		/// </summary>
		private void SetUserStatus(JabberConnection connection, JabberContactStatus.Availability status, string message)
		{
			if (connection.Host.EqualsTo(Connection.Host) && connection.User.EqualsTo(Connection.User))
				ShowStatus(Connection.User, status, null);
		}

		/// <summary>
		///		Trata el cambio de estado de un contacto
		/// </summary>
		private void TreatChangedStatus(ChangedStatusEventArgs evntArgs)
		{
			if (evntArgs.Connection.Host.EqualsTo(Connection.Host) && evntArgs.Contact.EqualsTo(Contact))
				ShowStatus(evntArgs.Contact, evntArgs.NewStatus.Status, evntArgs.NewStatus.Message);
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SendMessageCommand):
						SendMessage();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SendMessageCommand):
					return !MessageToSend.IsEmpty();
				default:
					return false;
			}
		}

		/// <summary>
		///		Envía un mensaje
		/// </summary>
		private void SendMessage()
		{
			if (!MessageToSend.IsEmpty())
			{ 
				// Envía el mensaje
				Connection.SendMessage(Contact, MessageToSend);
				// Muestra el mensaje
				ShowMessage(Connection.User, MessageToSend);
				// y lo borra
				MessageToSend = "";
			}
		}

		/// <summary>
		///		Muestra el estado
		/// </summary>
		private void ShowStatus(JabberContact contact, JabberContactStatus.Availability status, string message)
		{ 
			// Añade la cabecera
			ShowHeader(contact);
			// Añade el mensaje
			_rtfBuilder.SetColor(RtfTextBuilder.RtfColor.Olive);
			_rtfBuilder.AddText($"ha cambiado su estado a {status.ToString()} - {message}");
			// Muestra el Rtf
			ShowRtf();
		}

		/// <summary>
		///		Muestra el mensaje
		/// </summary>
		private void ShowMessage(JabberContact contact, string message)
		{   
			// Añade la cabecera
			ShowHeader(contact);
			// Añade el cuerpo del mensaje
			_rtfBuilder.AddNewLine();
			_rtfBuilder.AddTab();
			_rtfBuilder.SetColor(GetColorMessage(contact));
			_rtfBuilder.AddText(message);
			// Muestra el Rtf
			ShowRtf();
		}

		/// <summary>
		///		Muestra la cabecera con la hora
		/// </summary>
		private void ShowHeader(JabberContact contact)
		{ 
			// Añade un párrafo
			_rtfBuilder.AddParagraph();
			// Añade la hora
			_rtfBuilder.SetColor();
			_rtfBuilder.AddText(string.Format("[{0:HH:mm}]", DateTime.Now), true);
			// Añade el contacto
			_rtfBuilder.SetColor(GetColorHeader(contact));
			_rtfBuilder.AddText(contact.FullJid + ":");
		}

		/// <summary>
		///		Obtiene el color para la cabecera de un contacto (su nombre)
		/// </summary>
		private RtfTextBuilder.RtfColor GetColorHeader(JabberContact contact)
		{
			if (contact.EqualsTo(Connection.User))
				return RtfTextBuilder.RtfColor.Blue;
			else
				return RtfTextBuilder.RtfColor.Red;
		}

		/// <summary>
		///		Obtiene el color para el mensaje de un contacto
		/// </summary>
		private RtfTextBuilder.RtfColor GetColorMessage(JabberContact contact)
		{
			if (contact.EqualsTo(Connection.User))
				return RtfTextBuilder.RtfColor.Gray;
			else
				return RtfTextBuilder.RtfColor.Black;
		}

		/// <summary>
		///		Muestra el RTF
		/// </summary>
		private void ShowRtf()
		{ 
			// Cierra el texto
			_rtfBuilder.EndParagraph();
			// y muestra el Rtf
			ChatLogRtf = _rtfBuilder.RtfText;
			System.Diagnostics.Debug.WriteLine(ChatLogRtf);
		}

		/// <summary>
		///		Cierra la ventana de chat (la elimina de la lista de chats abiertos)
		/// </summary>
		public override void Close(SystemControllerEnums.ResultType result)
		{
			XmppClient.ChatsOpen.Remove(Connection, Contact);
		}

		/// <summary>
		///		Conexión a Jabber
		/// </summary>
		public JabberConnection Connection { get; }

		/// <summary>
		///		Contacto de Jabber
		/// </summary>
		public JabberContact Contact { get; }

		/// <summary>
		///		Manager de Jabber
		/// </summary>
		private Controllers.Xmpp.XmppManager XmppClient { get; }

		/// <summary>
		///		Chat
		/// </summary>
		public string ChatLog
		{
			get { return _chatLog; }
			set { CheckProperty(ref _chatLog, value); }
		}

		/// <summary>
		///		Rtf para el log de chat
		/// </summary>
		public string ChatLogRtf
		{
			get { return _chatLogRtf; }
			set { CheckProperty(ref _chatLogRtf, value); }
		}

		/// <summary>
		///		Mensaje a enviar
		/// </summary>
		public string MessageToSend
		{
			get { return _messageToSend; }
			set { CheckProperty(ref _messageToSend, value); }
		}

		/// <summary>
		///		Comando de envío de un mensaje
		/// </summary>
		public BaseCommand SendMessageCommand { get; }
	}
}
