using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataBaseStudio.Application.Bussiness;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Connections
{
	/// <summary>
	///		ViewModel para la definición de conexiones
	/// </summary>
	public class ConnectionViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name, _server;
		private bool _connectToFileDataBase;
		private string _dataBase, _dataBaseFileName;
		private bool _useWindowsAuthentification;
		private string _user, _password;
		private int _timeOut;

		public ConnectionViewModel(string fileName, string title)
		{ 
			// Guarda los datos
			FileName = fileName;
			// Carga la conexión
			Connection = new SchemaConnectionBussiness().Load(fileName);
			// Pasa los datos de la conexión a las propiedades
			InitForm(title);
		}

		/// <summary>
		///		Pasa las propiedades de la conexión a las propiedades
		/// </summary>
		private void InitForm(string title)
		{ 
			// Asigna las propiedades
			Name = Connection.Name;
			Server = Connection.Server;
			ConnectToFileDataBase = Connection.ConnectToFileDataBase;
			DataBase = Connection.DataBase;
			DataBaseFileName = Connection.DataBaseFileName;
			UseWindowsAuthentification = Connection.UseWindowsAuthentification;
			User = Connection.User;
			Password = Connection.Password;
			TimeOut = Connection.TimeOut;
			// Cambia el nombre por el título si no se había definido ninguno
			if (Name.IsEmpty() || Name.TrimIgnoreNull() == "-")
				Name = title;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false; // ... supone que los datos no son correctos

				// Comprueba los datos
				if (ConnectToFileDataBase)
				{
					if (DataBaseFileName.IsEmpty())
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el nombre de archivo");
					else
						validate = true;
				}
				else
				{
					if (Server.IsEmpty())
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de servidor");
					else if (DataBase.IsEmpty())
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de base de datos");
					else
						validate = true;
				}
				// Comprueba el resto de datos
				if (validate)
				{ 
					// Supone que no es correcto
					validate = false;
					// Comprueba los datos
					if (!UseWindowsAuthentification && User.IsEmpty())
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de usuario");
					else if (!UseWindowsAuthentification && Password.IsEmpty())
						DataBaseStudioViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la contraseña de usuario");
					else
						validate = true;
				}
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
				Connection.Server = Server;
				Connection.ConnectToFileDataBase = ConnectToFileDataBase;
				Connection.DataBase = DataBase;
				Connection.DataBaseFileName = DataBaseFileName;
				Connection.UseWindowsAuthentification = UseWindowsAuthentification;
				Connection.User = User;
				Connection.Password = Password;
				Connection.TimeOut = TimeOut;
				// Graba los datos
				new SchemaConnectionBussiness().Save(Connection, FileName);
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
		///		Nombre del servidor
		/// </summary>
		public string Server
		{
			get { return _server; }
			set { CheckProperty(ref _server, value); }
		}

		/// <summary>
		///		Indica si se debe conectar a un archivo de base de datos
		/// </summary>
		public bool ConnectToFileDataBase
		{
			get { return _connectToFileDataBase; }
			set { CheckProperty(ref _connectToFileDataBase, value); }
		}

		/// <summary>
		///		Base de datos (o archivo de base de datos)
		/// </summary>
		public string DataBase
		{
			get { return _dataBase; }
			set { CheckProperty(ref _dataBase, value); }
		}

		/// <summary>
		///		Nombre de archivo de base de datos
		/// </summary>
		public string DataBaseFileName
		{
			get { return _dataBaseFileName; }
			set { CheckProperty(ref _dataBaseFileName, value); }
		}

		/// <summary>
		///		Indica si se va a utilizar autentificación de Windows
		/// </summary>
		public bool UseWindowsAuthentification
		{
			get { return _useWindowsAuthentification; }
			set { CheckProperty(ref _useWindowsAuthentification, value); }
		}

		/// <summary>
		///		Usuario de SQL Server
		/// </summary>
		public string User
		{
			get { return _user; }
			set { CheckProperty(ref _user, value); }
		}

		/// <summary>
		///		Contraseña de SQL Server
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
		private string FileName { get; set; }

		/// <summary>
		///		Conexión
		/// </summary>
		private SchemaConnectionModel Connection { get; set; }
	}
}