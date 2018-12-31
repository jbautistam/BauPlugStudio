using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.FtpManager.Application.Bussiness;
using Bau.Libraries.FtpManager.Model.Connections;

namespace Bau.Libraries.FtpManager.ViewModel.Connections
{
	/// <summary>
	///		ViewModel para la definición de conexiones
	/// </summary>
	public class FtpConnectionViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name, _description, _server;
		private string _user, _password;
		private int _port, _timeOut;
		private FtpConnectionModel.Protocol _protocol;

		public FtpConnectionViewModel(string fileName, string title)
		{ 
			// Guarda los datos
			FileName = fileName;
			// Carga la conexión
			Connection = new FtpConnectionBussiness().Load(fileName);
			// Pasa los datos de la conexión a las propiedades
			InitForm(title);
		}

		/// <summary>
		///		Pasa las propiedades de la conexión a las propiedades
		/// </summary>
		private void InitForm(string title)
		{ 
			// Inicializa los combos
			InitCombos();
			// Asigna las propiedades
			Name = Connection.Name;
			Description = Connection.Description;
			Server = Connection.Server;
			Port = Connection.Port;
			Protocol = Connection.FtpProtocol;
			User = Connection.User;
			Password = Connection.Password;
			TimeOut = Connection.TimeOut;
			// Asigna el combo
			ComboFtpProtocol.SelectedID = (int) Protocol;
			// Cambia el nombre por el título si no se había definido ninguno
			if (Name.IsEmpty() || Name.TrimIgnoreNull() == "-")
				Name = title;
		}

		/// <summary>
		///		Inicializa los combos
		/// </summary>
		private void InitCombos()
		{ 
			ComboFtpProtocol = new BauMvvm.ViewModels.Forms.ControlItems.ComboItems.ComboViewModel(this, nameof(ComboFtpProtocol));
			ComboFtpProtocol.AddItem((int) FtpConnectionModel.Protocol.Ftp, "Ftp");
			ComboFtpProtocol.AddItem((int) FtpConnectionModel.Protocol.FtpS, "FtpS");
			ComboFtpProtocol.AddItem((int) FtpConnectionModel.Protocol.FtpEs, "FtpEs");
			ComboFtpProtocol.AddItem((int) FtpConnectionModel.Protocol.SFtp, "SFtp");
			ComboFtpProtocol.SelectedID = (int) FtpConnectionModel.Protocol.Ftp;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false; // ... supone que los datos no son correctos

				// Comprueba los datos
				if (Name.IsEmpty())
					FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de servidor");
				else if (Server.IsEmpty())
					FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de máquina de la conexión Ftp o su dirección IP");
				else if (User.IsEmpty())
					FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de usuario");
				else if (Password.IsEmpty())
					FtpManagerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la contraseña de usuario");
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
				// Asigna los datos al objeto
				Connection.Name = Name;
				Connection.Description = Description;
				Connection.Server = Server;
				Connection.Port = Port;
				Connection.FtpProtocol = (FtpConnectionModel.Protocol) ComboFtpProtocol.SelectedID;
				Connection.User = User;
				Connection.Password = Password;
				Connection.TimeOut = TimeOut;
				// Graba los datos
				new FtpConnectionBussiness().Save(Connection, FileName);
				// Indica que no ha habido modificaciones
				IsUpdated = false;
				SaveCommand.OnCanExecuteChanged();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre descriptivo de la conexión
		/// </summary>		
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción de la conexión
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Nombre del servidor
		/// </summary>
		public string Server
		{
			get { return _server; }
			set { CheckProperty(ref _server, value); }
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
		///		Combo de protocolos Ftp
		/// </summary>
		public BauMvvm.ViewModels.Forms.ControlItems.ComboItems.ComboViewModel ComboFtpProtocol { get; private set; }

		/// <summary>
		///		Protocolo
		/// </summary>
		public FtpConnectionModel.Protocol Protocol
		{
			get { return _protocol; }
			set { CheckProperty(ref _protocol, value); }
		}

		/// <summary>
		///		Usuario de Ftp
		/// </summary>
		public string User
		{
			get { return _user; }
			set { CheckProperty(ref _user, value); }
		}

		/// <summary>
		///		Contraseña de Ftp
		/// </summary>
		public string Password
		{
			get { return _password; }
			set { CheckProperty(ref _password, value); }
		}

		/// <summary>
		///		Tiempo de espera de la conexión
		/// </summary>
		public int TimeOut
		{
			get { return _timeOut; }
			set { CheckProperty(ref _timeOut, value); }
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		private string FileName { get; }

		/// <summary>
		///		Conexión
		/// </summary>
		private FtpConnectionModel Connection { get; }
	}
}