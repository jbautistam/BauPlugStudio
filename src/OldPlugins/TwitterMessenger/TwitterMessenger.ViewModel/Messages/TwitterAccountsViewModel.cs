using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibTwitter;
using Bau.Libraries.LibTwitter.Messages;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.TwitterMessenger.ViewModel.Messages
{
	/// <summary>
	///		ViewModel para las cuentas de Twitter
	/// </summary>
	public class TwitterAccountsViewModel : BasePaneViewModel
	{   
		// Eventos públicos
		public event EventHandler MessagesHtmlChanged;
		// Enumerados privados
		private enum ShowType
		{
			Messages,
			MyMessages,
			Followers,
			Following
		}
		// Variables privadas
		private TwitterAccountsCollection _accounts;
		private TwitterAccount _selectedAccount;
		private string _message, _htmlMessages, _htmlMessageDescription, _externalScreenName;

		public TwitterAccountsViewModel()
		{ 
			// Inicializa los comandos propios
			ValidateAccountCommand = new BaseCommand("Validar cuenta", parameter => ExecuteAction(nameof(ValidateAccountCommand), parameter),
													 parameter => CanExecuteAction(nameof(ValidateAccountCommand), parameter))
										 .AddListener(this, nameof(SelectedAccount))
										 .AddListener(this, nameof(IsUserValidated));
			PropertiesCommand.AddListener(this, nameof(SelectedAccount));
			DeleteCommand.AddListener(this, nameof(SelectedAccount));
			SendCommand = new BaseCommand("Enviar mensaje", parameter => ExecuteAction(nameof(SendCommand), parameter),
										  parameter => CanExecuteAction(nameof(SendCommand), parameter))
								.AddListener(this, nameof(Message))
								.AddListener(this, nameof(SelectedAccount));
			AddHyperlinkCommand = new BaseCommand("Añadir hipervínculo", parameter => ExecuteAction(nameof(AddHyperlinkCommand), parameter),
												  parameter => CanExecuteAction(nameof(AddHyperlinkCommand), parameter))
											.AddListener(this, "SelectedAccount");
			ShowTimeLineCommand = new BaseCommand("Mostrar timeline", parameter => ExecuteAction(nameof(ShowTimeLineCommand), parameter),
												  parameter => CanExecuteAction(nameof(ShowTimeLineCommand), parameter))
											.AddListener(this, nameof(SelectedAccount));
			ShowMyTweetsCommand = new BaseCommand("Mostrar mis Tweets", parameter => ExecuteAction(nameof(ShowMyTweetsCommand), parameter),
												  parameter => CanExecuteAction(nameof(ShowMyTweetsCommand), parameter))
											.AddListener(this, nameof(SelectedAccount));
			ShowFollowersCommand = new BaseCommand("Mostrar seguidores", parameter => ExecuteAction(nameof(ShowFollowersCommand), parameter),
												   parameter => CanExecuteAction(nameof(ShowFollowersCommand), parameter))
											.AddListener(this, nameof(SelectedAccount));
			ShowFriendsCommand = new BaseCommand("Mostrar amigos", parameter => ExecuteAction(nameof(ShowFriendsCommand), parameter),
												 parameter => CanExecuteAction(nameof(ShowFriendsCommand), parameter))
											.AddListener(this, nameof(SelectedAccount));
			RemoveExternalUserCommand = new BaseCommand("Eliminar usuario seleccionado", parameter => ExecuteAction(nameof(RemoveExternalUserCommand), parameter),
														parameter => CanExecuteAction(nameof(RemoveExternalUserCommand), parameter))
											.AddListener(this, nameof(ExternalScreenNameSelected));
			ShowBrowserExternalUserCommand = new BaseCommand("Mostrar Twitter usuario seleccionado", parameter => ExecuteAction(nameof(ShowBrowserExternalUserCommand), parameter),
															 parameter => CanExecuteAction(nameof(ShowBrowserExternalUserCommand), parameter))
											.AddListener(this, nameof(ExternalScreenNameSelected));
			// Carga el combo de cuentas
			LoadComboAccounts();
			// Añade el manejador de eventos al combo
			ComboAccounts.PropertyChanged += (sender, evntArgs) =>
													{
														if (evntArgs.PropertyName == nameof(SelectedAccount))
															SelectedAccount = ComboAccounts.SelectedTag as TwitterAccount;
													};
			// Inicializa los mensajes
			LoadTwitterMesssages(ShowType.MyMessages);
		}

		/// <summary>
		///		Carga el combo de cuentas
		/// </summary>
		private void LoadComboAccounts()
		{ 
			// Inicializa el combo
			ComboAccounts = new BauMvvm.ViewModels.Forms.ControlItems.ComboItems.ComboViewModel(this, nameof(ComboAccounts));
			// Añade los elementos básicos
			ComboAccounts.AddItem(null, "<Cuentas>", null);
			// Añade las cuentas
			foreach (TwitterAccount account in Accounts)
				ComboAccounts.AddItem(ComboAccounts.Items.Count + 1, account.ScreenName, account);
			// Selecciona el primer elemento
			ComboAccounts.SelectedID = null;
		}

		/// <summary>
		///		Ejecuta la acción asociada a un comando
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewCommand):
						OpenFormUpdateUser(null);
					break;
				case nameof(PropertiesCommand):
						OpenFormUpdateUser(SelectedAccount);
					break;
				case nameof(DeleteCommand):
						DeleteAccount(SelectedAccount);
					break;
				case nameof(ValidateAccountCommand):
					ValidateAccount(SelectedAccount);
					break;
				case nameof(SendCommand):
						SendMessage(SelectedAccount);
					break;
				case nameof(AddHyperlinkCommand):
						AddHyperlink();
					break;
				case nameof(ShowTimeLineCommand):
						LoadTwitterMesssages(ShowType.Messages);
					break;
				case nameof(ShowMyTweetsCommand):
						LoadTwitterMesssages(ShowType.MyMessages);
					break;
				case nameof(ShowFollowersCommand):
						LoadTwitterMesssages(ShowType.Followers);
					break;
				case nameof(ShowFriendsCommand):
						LoadTwitterMesssages(ShowType.Following);
					break;
				case nameof(RemoveExternalUserCommand):
						RemoveExternalUser();
					break;
				case nameof(ShowBrowserExternalUserCommand):
						ShowBrowserExternalUser();
					break;
				case nameof(RefreshCommand):
						LoadTwitterMesssages(ActualMessagesType);
					break;
			}
		}

		/// <summary>
		///		Comprueba si puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewCommand):
				case nameof(RefreshCommand):
					return true;
				case nameof(PropertiesCommand):
				case nameof(DeleteCommand):
				case nameof(AddHyperlinkCommand):
				case nameof(ValidateAccountCommand):
				case nameof(ShowTimeLineCommand):
				case nameof(ShowMyTweetsCommand):
				case nameof(ShowFollowersCommand):
				case nameof(ShowFriendsCommand):
					return SelectedAccount != null && !SelectedAccount.ScreenName.IsEmpty();
				case nameof(RemoveExternalUserCommand):
				case nameof(ShowBrowserExternalUserCommand):
					return SelectedAccount != null && !ExternalScreenNameSelected.IsEmpty();
				case nameof(SendCommand):
					return SelectedAccount != null && !SelectedAccount.NeedAuthorization && MessageCharNumber > 0;
				default:
					return false;
			}
		}

		/// <summary>
		///		Abre el formulario de modificar el usuario
		/// </summary>
		private void OpenFormUpdateUser(TwitterAccount account)
		{
			if (TwitterMessengerViewModel.Instance.ViewsController.OpenFormPropertiesAccount
						(new Accounts.AccountViewModel(account)) == SystemControllerEnums.ResultType.Yes)
				LoadComboAccounts();
		}

		/// <summary>
		///		Borra una cuenta de usuario
		/// </summary>
		private void DeleteAccount(TwitterAccount account)
		{
			if (account != null && TwitterMessengerViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea quitar este usuario"))
			{ 
				// Añade la cuenta
				account.Manager.Accounts.RemoveByScreenName(account.ScreenName);
				// Actualiza las cuentas
				RefreshAccounts(account);
			}
		}

		/// <summary>
		///		Valida una cuenta en Twitter
		/// </summary>
		private void ValidateAccount(TwitterAccount account)
		{
			if (account != null && TwitterMessengerViewModel.Instance.ViewsController.OpenFormValidateAccount(account) == SystemControllerEnums.ResultType.Yes)
			{ 
				// Actualiza las cuentas
				RefreshAccounts(account);
				// Actualiza el combo
				LoadComboAccounts();
			}
		}

		/// <summary>
		///		Envía un mensaje
		/// </summary>
		private void SendMessage(TwitterAccount account)
		{
			if (account != null && !account.NeedAuthorization)
			{ 
				// Envía el mensaje
				account.StatusCommand.Send(Message);
				// Comprueba los errores
				if (account.HasError)
					TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage("Error al enviar el mensaje: " + account.LastError.Message);
				else
					Message = "";
			}
			else
				TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage("Seleccione una cuenta válida para enviar el mensaje");
		}

		/// <summary>
		///		Añade un hipervínculo
		/// </summary>
		private void AddHyperlink()
		{
			string hyperlink = "";

			if (TwitterMessengerViewModel.Instance.ControllerWindow.ShowInputString("Introduzca un hipervínculo", ref hyperlink) == SystemControllerEnums.ResultType.Yes)
			{
				string shortUrl = new LibTwitter.ShortURL.TinyURL().Convert(hyperlink);

					// Añade el hipervínculo
					if (!shortUrl.IsEmpty())
						Message += shortUrl;
					else
						TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage("No se ha podido convertir el vínculo: " + hyperlink);
			}
		}

		/// <summary>
		///		Actualiza las cuentas
		/// </summary>
		private void RefreshAccounts(TwitterAccount account)
		{   
			// Graba los datos y recarga
			AccountsRepository.Save(account.Manager.Accounts, TwitterMessengerViewModel.Instance.FileAccounts);
			AccountsRepository.Load(TwitterMessengerViewModel.Instance.FileAccounts, account.Manager);
			// Actualiza el combo
			LoadComboAccounts();
		}

		/// <summary>
		///		Carga los mensajes de Twitter en el explorador
		/// </summary>
		private void LoadTwitterMesssages(ShowType type)
		{
			TwitterAccount account = SelectedAccount;

				// Guarda el tipo de mensajes mostrados
				ActualMessagesType = type;
				// Muestra los mensajes
				if (TwitterMessengerViewModel.Instance.OAuthConsumerKey.IsEmpty() || TwitterMessengerViewModel.Instance.OAuthConsumerSecret.IsEmpty())
				{
					HtmlMessages = "<html><head></head><body><p>Configure las claves de aplicacion</p></body></html>";
					HtmlMessageDescription = "Configure las claves de aplicación";
				}
				else if (account == null)
				{
					HtmlMessages = "<html><head></head><body><p>Seleccione una cuenta</p></body></html>";
					HtmlMessageDescription = "Seleccione una cuenta";
				}
				else
				{
					TwitterMessagesCollection messages = new TwitterMessagesCollection();

						// Carga la colección
						switch (type)
						{
							case ShowType.Messages:
									if (ExternalScreenNameSelected.IsEmpty())
										messages = account.Manager.TwitterMessenger.GetPublicTimeLine(account);
									else
										messages = account.Manager.TwitterMessenger.GetUserTimeLine(account, ExternalScreenNameSelected);
								break;
							case ShowType.MyMessages:
									if (ExternalScreenNameSelected.IsEmpty())
										messages = account.Manager.TwitterMessenger.GetUserTimeLine(account, null);
									else
										messages = account.Manager.TwitterMessenger.GetUserTimeLine(account, ExternalScreenNameSelected);
								break;
							case ShowType.Followers:
									if (ExternalScreenNameSelected.IsEmpty())
										messages = account.Manager.TwitterMessenger.GetFollowers(account, null);
									else
										messages = account.Manager.TwitterMessenger.GetFollowers(account, ExternalScreenNameSelected);
								break;
							case ShowType.Following:
									if (ExternalScreenNameSelected.IsEmpty())
										messages = account.Manager.TwitterMessenger.GetFriends(account, null);
									else
										messages = account.Manager.TwitterMessenger.GetFriends(account, ExternalScreenNameSelected);
								break;
						}
					// Carga el HTML
					HtmlMessages = messages.ToHTML();
					// Modifica el mensaje de cantidad
					switch (type)
					{
						case ShowType.Messages:
						case ShowType.MyMessages:
								HtmlMessageDescription = GetMessageNumber(messages.Count, "mensaje", "mensajes");
							break;
						case ShowType.Followers:
								HtmlMessageDescription = GetMessageNumber(messages.Count, "seguidor", "seguidores");
							break;
						case ShowType.Following:
								HtmlMessageDescription = GetMessageNumber(messages.Count, "siguiendo", "siguiendo");
							break;
					}
				}
				// Lanza el evento de mensajes modificados
				MessagesHtmlChanged?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///		Obtiene un mensaje con un número y una descripción en singular o plural
		/// </summary>
		private string GetMessageNumber(int number, string singular, string plural)
		{
			if (number == 0)
				return "Ningún " + singular;
			else if (number == 1)
				return "Un " + singular;
			else
				return number.ToString() + " " + plural;
		}

		/// <summary>
		///		Trata una llamada de una función JavaScript
		/// </summary>
		public void TreatExplorerFunction(string argument)
		{
			TwitterAccount account = SelectedAccount;

				if (account != null)
				{
					string[] argumentParts = argument.Split('|');

						if (argumentParts.Length > 1)
						{
							string error = null;

								// Realiza la función
								switch (argumentParts[0])
								{
									case TwitterMessage.See:
											ShowDataUser(argumentParts[1]);
										break;
									case TwitterMessage.Follow:
											AddFriend(account, argumentParts[1], true, out error);
										break;
									case TwitterMessage.Unfollow:
											RemoveFriend(account, argumentParts[1], out error);
										break;
									case TwitterMessage.Reply:
											Message = "RP " + argumentParts[1];
										break;
									case TwitterMessage.Retweet:
											Message = "RT " + argumentParts[1];
										break;
									case TwitterMessage.Link:
											if (argumentParts[1].StartsWith("@"))
												ShowDataUser(argumentParts[1]);
											else if (argumentParts[1].StartsWith("#"))
												ShowTwitterBrowser(argumentParts[1]);
											else if (argumentParts[1].StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
													 argumentParts[1].StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
												TwitterMessengerViewModel.Instance.ViewsController.ShowWebBrowser(argumentParts[1]);
										break;
								}
								// Añade el error
								if (!error.IsEmpty())
									TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage(error);
								else
									LoadTwitterMesssages(ActualMessagesType);
						}
				}
		}

		/// <summary>
		///		Muestra los datos de un usuario en un navegador
		/// </summary>
		private void ShowDataUser(string user)
		{ 
			// Cambia el usuario seleccionado
			ExternalScreenNameSelected = user;
			// Carga los mensajes del usuario seleccionado
			LoadTwitterMesssages(ShowType.Messages);
		}

		/// <summary>
		///		Comienza a seguir un usuario
		/// </summary>
		private bool AddFriend(TwitterAccount account, string screenName, bool follow, out string error)
		{
			bool added = false;

				// Inicializa los valores de salida
				error = "";
				// Añade el amigo
				account.UserCommand.AddFriend(screenName, follow);
				// Comprueba los errores
				if (account.HasError)
					error = account.LastError.Message;
				else
					added = true;
				// Devuelve el valor que indica si se ha añadido el contacto
				return added;
		}

		/// <summary>
		///		Quita un amigo de una cuenta
		/// </summary>
		public bool RemoveFriend(TwitterAccount account, string screenName, out string error)
		{
			bool removed = false;

				// Inicializa los valores de salida
				error = "";
				// Añade el amigo
				account.UserCommand.RemoveFriend(screenName);
				// Obtiene el error
				if (account.HasError)
					error = account.LastError.Message;
				else
					removed = true;
				// Devuelve el valor que indica si se ha eliminado
				return removed;
		}

		/// <summary>
		///		Elimina el usuario externo seleccionado
		/// </summary>
		private void RemoveExternalUser()
		{
			ExternalScreenNameSelected = null;
			LoadTwitterMesssages(ShowType.Messages);
		}

		/// <summary>
		///		Muestra un navegador con los datos de la cuenta del usuario externo seleccionado
		/// </summary>
		private void ShowBrowserExternalUser()
		{
			if (!ExternalScreenNameSelected.IsEmpty())
				ShowTwitterBrowser(ExternalScreenNameSelected);
		}

		/// <summary>
		///		Muestra un navegador con una página de Twitter
		/// </summary>
		private void ShowTwitterBrowser(string strAccount)
		{
			TwitterMessengerViewModel.Instance.ViewsController.ShowWebBrowser("http://www.twitter.com/" + strAccount);
		}

		/// <summary>
		///		Cuenta seleccionada
		/// </summary>
		public TwitterAccount SelectedAccount
		{
			get { return _selectedAccount; }
			set
			{
				if (!ReferenceEquals(_selectedAccount, value))
				{ 
					// Asigna la cuenta seleccionada y notifica
					_selectedAccount = value;
					OnPropertyChanged(nameof(SelectedAccount));
					OnPropertyChanged(nameof(NeedAuthorization));
					OnPropertyChanged(nameof(IsUserValidated));
					// Quita el usuario externo seleccionado
					ExternalScreenNameSelected = null;
					// Carga los mensajes
					LoadTwitterMesssages(ShowType.Messages);
				}
			}
		}

		/// <summary>
		///		Nombre de usuario seleccionado (como externo)
		/// </summary>
		public string ExternalScreenNameSelected
		{
			get { return _externalScreenName; }
			set { CheckProperty(ref _externalScreenName, value); }
		}

		/// <summary>
		///		Manager de Twitter
		/// </summary>
		private ManagerTwitter Manager
		{
			get { return TwitterMessengerViewModel.Instance.TwitterMessenger.ManagerTwitter; }
		}

		/// <summary>
		///		Cuentas
		/// </summary>
		public TwitterAccountsCollection Accounts
		{
			get
			{ 
				// Carga las cuentas si no estaban en memoria
				if (_accounts == null)
				{	
					// Carga las cuentas
					AccountsRepository.Load(TwitterMessengerViewModel.Instance.FileAccounts, Manager);
					// Recupera las cuentas
					_accounts = Manager.Accounts;
				}
				// Devuelve la colección de cuentas
				return _accounts;
			}
		}

		/// <summary>
		///		ViewModel para el combo de cuentas
		/// </summary>
		public BauMvvm.ViewModels.Forms.ControlItems.ComboItems.ComboViewModel ComboAccounts { get; private set; }

		/// <summary>
		///		Indica si el usuario está validado
		/// </summary>
		public bool IsUserValidated
		{
			get { return SelectedAccount != null && !NeedAuthorization; }
		}

		/// <summary>
		///		Indica si el usuario necesita autorización
		/// </summary>
		public bool NeedAuthorization
		{
			get { return SelectedAccount != null && SelectedAccount.NeedAuthorization; }
		}

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message
		{
			get { return _message; }
			set
			{
				if (!_message.EqualsIgnoreNull(value))
				{
					_message = value;
					OnPropertyChanged(nameof(Message));
					OnPropertyChanged(nameof(MessageCharNumber));
					OnPropertyChanged(nameof(MessageChars));
				}
			}
		}

		/// <summary>
		///		Número de caracteres del mensaje
		/// </summary>
		public int MessageCharNumber
		{
			get
			{
				if (Message.IsEmpty())
					return 0;
				else
					return Message.Trim().Length;
			}
		}

		/// <summary>
		///		Número de caracteres del mensaje
		/// </summary>
		public string MessageChars
		{
			get { return GetMessageNumber(MessageCharNumber, "carácter", "caracteres"); }
		}

		/// <summary>
		///		Descripción con el número de mensajes de la lista
		/// </summary>
		public string HtmlMessageDescription
		{
			get { return _htmlMessageDescription; }
			set { CheckProperty(ref _htmlMessageDescription, value); }
		}

		/// <summary>
		///		Mensajes Html
		/// </summary>
		public string HtmlMessages
		{
			get { return _htmlMessages; }
			set { CheckProperty(ref _htmlMessages, value); }
		}

		/// <summary>
		///		Tipo de mensajes mostrado
		/// </summary>
		private ShowType ActualMessagesType { get; set; }

		/// <summary>
		///		Comando para validar una cuenta
		/// </summary>
		public BaseCommand ValidateAccountCommand { get; }

		/// <summary>
		///		Comando para enviar un mensaje
		/// </summary>
		public BaseCommand SendCommand { get; }

		/// <summary>
		///		Comando para añadir un hipervínculo al mensaje
		/// </summary>
		public BaseCommand AddHyperlinkCommand { get; }

		/// <summary>
		///		Comando para mostrar el timeline
		/// </summary>
		public BaseCommand ShowTimeLineCommand { get; }

		/// <summary>
		///		Comando para mostrar mis tweets
		/// </summary>
		public BaseCommand ShowMyTweetsCommand { get; }

		/// <summary>
		///		Comando para mostrar los seguidores
		/// </summary>
		public BaseCommand ShowFollowersCommand { get; }

		/// <summary>
		///		Comando para mostrar los amigos
		/// </summary>
		public BaseCommand ShowFriendsCommand { get; }

		/// <summary>
		///		Comando para eliminar el usuario externo seleccionado
		/// </summary>
		public BaseCommand RemoveExternalUserCommand { get; }

		/// <summary>
		///		Comando para mostrar un navegador con el usuario externo seleccionado
		/// </summary>
		public BaseCommand ShowBrowserExternalUserCommand { get; }
	}
}
