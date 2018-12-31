using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.BauMessenger.ViewModel.Connections
{
	/// <summary>
	///		ViewModel para la definición de contactos
	/// </summary>
	public class ConnectionViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _address;
		private int _port;
		private bool _useTls;
		private string _login, _password, _repeatPassword;

		public ConnectionViewModel()
		{
			Address = "jabberes.org";
			Port = 5222;
			UseTls = true;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false; // ... supone que los datos no son correctos

				// Comprueba los datos
				if (Address.IsEmpty())
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre o la dirección IP del servidor");
				else if (Login.IsEmpty())
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el código de usuario");
				else if (Password.IsEmpty())
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la contraseña del usuario");
				else if (!Password.Equals(RepeatPassword))
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Ambas contraseñas deben ser iguales");
				else if (BauMessengerViewModel.Instance.BauMessenger.Exists(Address, Login))
					BauMessengerViewModel.Instance.ControllerWindow.ShowMessage("Ya existe una conexión para esa dirección y usuario");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos de la cuenta
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{   
				// Asigna los datos de la cuenta
				BauMessengerViewModel.Instance.BauMessenger.AddConnection(Address, Port, UseTls, Login, Password);
				// Indica que no ha habido modificaciones
				IsUpdated = false;
				SaveCommand.OnCanExecuteChanged();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Código de usuario
		/// </summary>
		public string Address
		{
			get { return _address; }
			set { CheckProperty(ref _address, value); }
		}

		/// <summary>
		///		Puerto
		/// </summary>
		public int Port
		{
			get { return _port; }
			set { CheckProperty(ref _port, value); }
		}

		/// <summary>
		///		Indica si se debe utilizar TLS para la conexión
		/// </summary>
		public bool UseTls
		{
			get { return _useTls; }
			set { CheckProperty(ref _useTls, value); }
		}

		/// <summary>
		///		Código ee usuario
		/// </summary>
		public string Login
		{
			get { return _login; }
			set { CheckProperty(ref _login, value); }
		}

		/// <summary>
		///		Contraseña de usuario
		/// </summary>
		public string Password
		{
			get { return _password; }
			set { CheckProperty(ref _password, value); }
		}

		/// <summary>
		///		Contraseña: repetición para validación
		/// </summary>
		public string RepeatPassword
		{
			get { return _repeatPassword; }
			set { CheckProperty(ref _repeatPassword, value); }
		}
	}
}