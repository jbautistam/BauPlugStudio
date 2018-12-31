using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.LibXmppClient.EventArguments;
using Bau.Libraries.LibXmppClient.Users;
using Bau.Libraries.BauMessenger.ViewModel.Explorer.JabberNodes;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.BauMessenger.ViewModel.Explorer
{
	/// <summary>
	///		ViewModel para el árbol de conexiones
	/// </summary>
	public class PaneTreeConnectionViewModel : BauMvvm.ViewModels.Forms.BasePaneViewModel
	{
		// Variables privadas
		private TreeConnectionViewModel _tree;
		private bool _isStatusOnline, _isStatusAway, _isStatusChat, _isStatusDnd, _isStatusXa;

		public PaneTreeConnectionViewModel()
		{
			InitProperties();
			InitXmppEventHandlers();
			// Inicializa los comandos
			NewConnectionCommand = new BaseCommand("Nueva conexión",
												   parameter => ExecuteAction(nameof(NewConnectionCommand), parameter));
			NewUserCommand = new BaseCommand("Nuevo usuario",
											 parameter => ExecuteAction(nameof(NewUserCommand), parameter));
			NewContactCommand = new BaseCommand("Nuevo contacto",
												parameter => ExecuteAction(nameof(NewContactCommand), parameter),
												parameter => CanExecuteAction(nameof(NewContactCommand), parameter))
										.AddListener(this, nameof(Tree.SelectedNode));
			NewGroupCommand = new BaseCommand("Nuevo grupo",
											  parameter => ExecuteAction(nameof(NewGroupCommand), parameter),
											  parameter => CanExecuteAction(nameof(NewGroupCommand), parameter))
										.AddListener(this, nameof(Tree.SelectedNode));
			StartChatCommand = new BaseCommand("Abrir ventana de chat",
											   parameter => ExecuteAction(nameof(StartChatCommand), parameter),
											   parameter => CanExecuteAction(nameof(StartChatCommand), parameter))
									.AddListener(this, nameof(Tree.SelectedNode));
			ConnectCommand = new BaseCommand("Conexión",
											 parameter => Connect(),
											 parameter => CanExecuteAction(nameof(ConnectCommand), parameter))
									.AddListener(this, nameof(Tree.SelectedNode));
			DisconnectCommand = new BaseCommand("Desconexión",
												parameter => Disconnect(),
												parameter => CanExecuteAction(nameof(DisconnectCommand), parameter))
									.AddListener(this, nameof(Tree.SelectedNode));
			SetUserStatusCommand = new BaseCommand("Cambiar estado",
												   parameter => ChangeUserStatus(parameter as string),
												   parameter => CanExecuteAction(nameof(SetUserStatusCommand), parameter))
									.AddListener(this, nameof(Tree.SelectedNode));
		}

		/// <summary>
		///		Inicializa los comandos
		/// </summary>
		private void InitProperties()
		{   
			Tree = new TreeConnectionViewModel();
			Tree.LoadNodes();
		}

		/// <summary>
		///		Inicializa los manejadores de eventos de XMPP
		/// </summary>
		private void InitXmppEventHandlers()
		{   
			// Inicializa la variable que actúa como manager de XMPP en el árbol
			XmppClient = BauMessengerViewModel.Instance.BauMessenger;
			// Añade los manejadores de eventos
			XmppClient.ManagerJabber.MessageReceived += (sender, evntArgs) => OpenChatWindow(evntArgs.Connection, evntArgs.Contact, evntArgs.Body);
			XmppClient.ManagerJabber.ChangedStatus += (sender, evntArgs) => Tree.TreatChangedStatus(evntArgs);
			XmppClient.ManagerJabber.SubscriptionRequested += (sender, evntArgs) =>
									{ 
										//! Este es especial: crea un manejador de eventos porque se tiene que esperar a que se trate el
										//! evento antes de devolver el resultado
										//? No sé si esto tiene ya sentido
										TreatSubscriptionRequested(sender, evntArgs);
									};
			XmppClient.ManagerJabber.FormRequested += (sender, evntArgs) => TreatRequestForm(evntArgs);
			XmppClient.ManagerJabber.RosterUpdated += (sender, evntArgs) => Tree.LoadNodes();
			XmppClient.ManagerJabber.ChangedUserStatus += (sender, evntArgs) => SetUserStatus(evntArgs.Connection, evntArgs.User.Status.Status, evntArgs.User.Status.Message);
		}

		/// <summary>
		///		Trata el cambio de estado del usuario
		/// </summary>
		private void SetUserStatus(JabberConnection connection, JabberContactStatus.Availability status, string message)
		{
			IsStatusAway = status == JabberContactStatus.Availability.Away;
			IsStatusChat = status == JabberContactStatus.Availability.Chat;
			IsStatusDnd = status == JabberContactStatus.Availability.Dnd;
			IsStatusOnline = status == JabberContactStatus.Availability.Online;
			IsStatusXa = status == JabberContactStatus.Availability.Xa;
		}

		/// <summary>
		///		Trata la solicitud de suscripción
		/// </summary>
		private void TreatSubscriptionRequested(object sender, SubscriptionRequestEventArgs evntArgs)
		{
			switch (BauMessengerViewModel.Instance.ControllerWindow.ShowQuestionCancel
										  ($"Ha recibido una petición de suscripción de '{evntArgs.Jid}'. ¿Desea aceptarla?"))
			{
				case SystemControllerEnums.ResultType.Yes:
						evntArgs.Status = SubscriptionRequestEventArgs.SubscriptionStatus.Accepted;
					break;
				case SystemControllerEnums.ResultType.No:
						evntArgs.Status = SubscriptionRequestEventArgs.SubscriptionStatus.Refused;
					break;
				case SystemControllerEnums.ResultType.Cancel:
						evntArgs.Status = SubscriptionRequestEventArgs.SubscriptionStatus.Wait;
					break;
			}
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewConnectionCommand):
						CreateConnection();
					break;
				case nameof(NewUserCommand):
						CreateUser();
					break;
				case nameof(NewContactCommand):
						OpenPropertiesNewContact();
					break;
				case nameof(NewGroupCommand):
						OpenPropertiesGroup(null);
					break;
				case nameof(StartChatCommand):
						OpenChatWindow(Tree.GetSelectedContact());
					break;
				case nameof(DeleteCommand):
						if (Tree.GetSelectedContact() == null)
							DeleteConnection(Tree.GetSelectedConnection());
						else
							DeleteContact(Tree.GetSelectedContact());
					break;
				case nameof(RefreshCommand):
						Tree.LoadNodes();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			JabberConnection connection = Tree.GetSelectedConnection();
			ContactNodeViewModel nodeContact = Tree.GetSelectedContact();

				switch (action)
				{
					case nameof(NewContactCommand):
					case nameof(NewGroupCommand):
						return (connection != null && connection.IsConnected) || nodeContact != null || Tree.GetSelectedGroup() != null;
					case nameof(DeleteCommand):
						return connection != null || nodeContact != null;
					case nameof(StartChatCommand):
						return connection != null;
					case nameof(ConnectCommand):
						return connection != null && !connection.IsConnected;
					case nameof(DisconnectCommand):
					case nameof(SetUserStatusCommand):
						return connection != null && connection.IsConnected;
					case nameof(RefreshCommand):
						return true;
					default:
						return false;
				}
		}

		/// <summary>
		///		Abre una ventana de chat
		/// </summary>
		private void OpenChatWindow(ContactNodeViewModel nodeContact)
		{
			if (nodeContact != null)
				OpenChatWindow(nodeContact.GetConnectionNode()?.Connection, nodeContact.Contact, null);
		}

		/// <summary>
		///		Abre una ventana de chat
		/// </summary>
		private void OpenChatWindow(JabberConnection connection, JabberContact contact, string message)
		{
			if (XmppClient.ChatsOpen.Add(connection, contact))
				BauMessengerViewModel.Instance.ViewsController.OpenChatView(new Chat.ChatViewModel(connection, contact, message));
		}

		/// <summary>
		///		Crea una conexión
		/// </summary>
		private void CreateConnection()
		{
			if (BauMessengerViewModel.Instance.ViewsController.OpenNewConnection() == SystemControllerEnums.ResultType.Yes)
				Tree.LoadNodes();
		}

		/// <summary>
		///		Crea un usuario
		/// </summary>
		private void CreateUser()
		{
			string server = "jabberes.org";

				if (BauMessengerViewModel.Instance.ControllerWindow.ShowInputString("Introduzca el nombre del servidor donde desea crear el usuario",
																					ref server) == SystemControllerEnums.ResultType.Yes &&
					!server.IsEmpty())
				{
					LibXmppClient.JabberManager xmppRegisterClient = new LibXmppClient.JabberManager();

						// Asigna el manejador de eventos
						xmppRegisterClient.FormRequested += (sender, evntArgs) => TreatRequestForm(evntArgs);
						// Comienza el registro
						try
						{
							if (!xmppRegisterClient.RegisterBegin(server, out string error))
								BauMessengerViewModel.Instance.ControllerWindow.ShowMessage(error);
							else
								BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("El usuario se ha creado correctamente");
						}
						catch (Exception exception)
						{
							BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Excepción al conectar con el servidor." + exception.Message);
						}
				}
		}

		/// <summary>
		///		Trata la solicitud de apertura de un formulario
		/// </summary>
		private void TreatRequestForm(FormRequestedEventArgs evntArgs)
		{
			if (BauMessengerViewModel.Instance.ViewsController.OpenFormXmppView(evntArgs.Form, "Crear usuario") != SystemControllerEnums.ResultType.Yes)
				evntArgs.Cancel = true;
		}

		/// <summary>
		///		Abre la ventana de propiedades de un contacto
		/// </summary>
		private void OpenPropertiesNewContact()
		{
			BaseNodeViewModel node = Tree.GetSelectedNode();

				if (node == null)
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Seleccione una conexión en el árbol");
				else
				{
					JabberConnection connection = node.GetConnectionNode()?.Connection;

						if (connection == null)
							BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Seleccione una conexión en el árbol");
						else
							BauMessengerViewModel.Instance.ViewsController.OpenPropertiesContact
										(new Contacts.ContactViewModel(connection, null));
				}
		}

		/// <summary>
		///		Abre la ventana de propiedades de un grupo
		/// </summary>
		private void OpenPropertiesGroup(GroupNodeViewModel nodeGroup)
		{
			BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Crear un nuevo grupo");
		}

		/// <summary>
		///		Elimina un contacto
		/// </summary>
		private void DeleteContact(ContactNodeViewModel nodeContact)
		{
			if (nodeContact != null &&
					  BauMessengerViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar el contacto {nodeContact.Contact.FullJid}?"))
			{
				JabberConnection connection = nodeContact.GetConnectionNode()?.Connection;

					if (connection == null)
						BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra la conexión del contacto");
					else
						connection.DeleteContact(nodeContact.Contact);
			}
		}

		/// <summary>
		///		Elimina una conexión
		/// </summary>
		private void DeleteConnection(JabberConnection connection)
		{
			if (connection != null &&
				BauMessengerViewModel.Instance.ControllerWindow.ShowQuestion
					  ($"¿Realmente desea eliminar la conexión con el servidor {connection.Host.Address} del usuario {connection.User.FullJid}?"))
			{ 
				if (!XmppClient.ManagerJabber.Connections.Remove(connection))
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("No se ha podido eliminar la conexión");
				else
				{ 
					// Graba la configuración
					SaveConfiguration();
					// ... y actualiza el árbol
					Tree.LoadNodes();
				}
			}
		}

		/// <summary>
		///		Conecta
		/// </summary>
		private void Connect()
		{
			JabberConnection connection = Tree.GetSelectedConnection();

				if (connection != null)
					try
					{
						connection.Connect();
					}
					catch (Exception exception)
					{
						BauMessengerViewModel.Instance.ControllerWindow.ShowMessage($"No se puede conectar al servidor.{Environment.NewLine}{exception.Message}");
					}
		}

		/// <summary>
		///		Desconecta
		/// </summary>
		private void Disconnect()
		{
			JabberConnection connection = Tree.GetSelectedConnection();

				try
				{
					if (connection != null)
						connection.Disconnect();
					Tree.LoadNodes();
				}
				catch (Exception exception)
				{
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage($"No se puede desconectar del servidor.{Environment.NewLine}{exception.Message}");
				}
		}

		/// <summary>
		///		Modifica el estado del usuario
		/// </summary>
		private void ChangeUserStatus(string newStatus)
		{
			JabberConnection connection = Tree.GetSelectedConnection();

				if (connection != null)
				{
					if (newStatus.EqualsIgnoreCase("Away"))
						connection.SetStatus(JabberContactStatus.Availability.Away, null);
					else if (newStatus.EqualsIgnoreCase("Chat"))
						connection.SetStatus(JabberContactStatus.Availability.Chat, null);
					else if (newStatus.EqualsIgnoreCase("Dnd"))
						connection.SetStatus(JabberContactStatus.Availability.Dnd, null);
					else if (newStatus.EqualsIgnoreCase("Offline"))
						connection.SetStatus(JabberContactStatus.Availability.Offline, null);
					else if (newStatus.EqualsIgnoreCase("Online"))
						connection.SetStatus(JabberContactStatus.Availability.Online, null);
					else if (newStatus.EqualsIgnoreCase("Xa"))
						connection.SetStatus(JabberContactStatus.Availability.Xa, null);
				}
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{
			XmppClient.SaveConfiguration();
		}

		/// <summary>
		///		Arbol de conexiones
		/// </summary>
		public TreeConnectionViewModel Tree
		{
			get { return _tree; }
			set { CheckObject(ref _tree, value); }
		}

		/// <summary>
		///		Indica si está en estado online
		/// </summary>
		public bool IsStatusOnline
		{
			get { return _isStatusOnline; }
			set { CheckProperty(ref _isStatusOnline, value); }
		}

		/// <summary>
		///		Indica si está en estado away
		/// </summary>
		public bool IsStatusAway
		{
			get { return _isStatusAway; }
			set { CheckProperty(ref _isStatusAway, value); }
		}

		/// <summary>
		///		Indica si está en estado chat
		/// </summary>
		public bool IsStatusChat
		{
			get { return _isStatusChat; }
			set { CheckProperty(ref _isStatusChat, value); }
		}

		/// <summary>
		///		Indica si está en estado dnd
		/// </summary>
		public bool IsStatusDnd
		{
			get { return _isStatusDnd; }
			set { CheckProperty(ref _isStatusDnd, value); }
		}

		/// <summary>
		///		Indica si está en estado Xa
		/// </summary>
		public bool IsStatusXa
		{
			get { return _isStatusXa; }
			set { CheckProperty(ref _isStatusXa, value); }
		}

		/// <summary>
		///		Comando para crear una nueva conexión
		/// </summary>
		public BaseCommand NewConnectionCommand { get; }

		/// <summary>
		///		Comando para crear un nuevo grupo
		/// </summary>
		public BaseCommand NewUserCommand { get; }

		/// <summary>
		///		Comando para crear un nuevo contacto
		/// </summary>
		public BaseCommand NewContactCommand { get; }

		/// <summary>
		///		Comando para crear un nuevo grupo
		/// </summary>
		public BaseCommand NewGroupCommand { get; }

		/// <summary>
		///		Comando para abrir una ventana de chat
		/// </summary>
		public BaseCommand StartChatCommand { get; }

		/// <summary>
		///		Comando de conectar a un servidor
		/// </summary>
		public BaseCommand ConnectCommand { get; }

		/// <summary>
		///		Comando de desconectar a un servidor
		/// </summary>
		public BaseCommand DisconnectCommand { get; }

		/// <summary>
		///		Comando para cambiar el estado del usuario
		/// </summary>
		public BaseCommand SetUserStatusCommand { get; }

		/// <summary>
		///		Manager de XMPP
		/// </summary>
		private Controllers.Xmpp.XmppManager XmppClient { get; set; }
	}
}
