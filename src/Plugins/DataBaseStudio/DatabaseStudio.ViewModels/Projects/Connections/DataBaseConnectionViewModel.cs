using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models.Connections;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Connections
{
	/// <summary>
	///		ViewModel de <see cref="DatabaseConnectionModel"/>
	/// </summary>
	public class DataBaseConnectionViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{
		// Variables privadas
		private string _name, _description, _connectionString;
		private string _server, _user, _password, _dataBase;
		private int _port;
		private bool _integratedSecurity, _userEnabled, _canUseIntegratedSecurity;
		private string _fileName;
		private bool _useConnectionString, _isServerConnection, _isTemporalDataBase, _useAgent, _canUseAgent;

		public DataBaseConnectionViewModel(DatabaseConnectionModel connection)
		{
			// Inicializa los datos principales
			IsNew = connection == null;
			if (connection == null)
				Connection = new DatabaseConnectionModel();
			else
				Connection = connection;
			// Inicializa el viewModel
			InitViewModel();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitViewModel()
		{
			// Genera los elementos del combo
			ConnectionTypes.Add(new ControlItemViewModel("ODBC", DatabaseConnectionModel.DataBaseType.Odbc));
			ConnectionTypes.Add(new ControlItemViewModel("MySql", DatabaseConnectionModel.DataBaseType.MySql));
			ConnectionTypes.Add(new ControlItemViewModel("PostgreSql", DatabaseConnectionModel.DataBaseType.PostgreSql));
			ConnectionTypes.Add(new ControlItemViewModel("SqLite", DatabaseConnectionModel.DataBaseType.SqLite));
			ConnectionTypes.Add(new ControlItemViewModel("SqlServer", DatabaseConnectionModel.DataBaseType.SqlServer));
			ConnectionTypes.PropertyChanged += (sender, evntArgs) =>
													{
														if (evntArgs.PropertyName.Equals(nameof(ConnectionTypes.SelectedItem)))
															AssignDataBaseParameters();
													};
			// Cambia el valor que indica si es una conexión a SQL server y la seguridad integrada para que al poner un valor diferente salte el CheckProperty
			IsServerConnection = Connection.IsServerConnection;
			UseConnectionString = Connection.UseConnectionString;
			CanUseIntegratedSecurity = Connection.CanUseIntegratedSecurity;
			IsTemporalDataBase = Connection.Type == DatabaseConnectionModel.DataBaseType.SqLite;
			IntegratedSecurity = !Connection.IntegratedSecurity;
			// Inicializa las propiedades básicas
			Name = Connection.Name;
			Description = Connection.Description;
			ConnectionTypes.SelectItem(Connection.Type);
			ConnectionString = Connection.ConnectionString;
			Server = Connection.Server;
			Port = Connection.Port;
			User = Connection.User;
			Password = Connection.Password;
			IntegratedSecurity = Connection.IntegratedSecurity;
			DataBase = Connection.DataBase;
			FileName = Connection.FileName;
		}

		/// <summary>
		///		Cambia los parámetros básicos de la base de datos cuando se cambia el tipo
		/// </summary>
		private void AssignDataBaseParameters()
		{
			if (ConnectionTypes.IsSelected())
			{
				DatabaseConnectionModel.DataBaseType type = ConnectionTypes.GetSelectedItemTyped(DatabaseConnectionModel.DataBaseType.Odbc);

					IsServerConnection = DatabaseConnectionModel.CheckIsServerConnection(type);
					UseConnectionString = DatabaseConnectionModel.CheckUseConnectionString(type);
					CanUseIntegratedSecurity = DatabaseConnectionModel.CheckCanUserIntegratedSecurity(type);
					IsTemporalDataBase = type == DatabaseConnectionModel.DataBaseType.SqLite;
					Port = DatabaseConnectionModel.GetDefaultPort(type);
			}
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validated = false;

				// Comprueba los datos introducidos
				if (string.IsNullOrEmpty(Name))
					MainViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la conexión");
				else if (!ConnectionTypes.IsSelected())
					MainViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el tipo de conexión");
				else if (!ValidateParametersDataBase(out string error))
					MainViewModel.Instance.ControllerWindow.ShowMessage(error);
				else
					validated = true;
				// Devuelve el valor que indica si los datos son correctos
				return validated;
		}

		/// <summary>
		///		Comprueba los parámetros de base de datos
		/// </summary>
		private bool ValidateParametersDataBase(out string error)
		{
			// Inicializa los argumentos de salida
			error = "";
			// Comprueba los datos
			if (IsServerConnection)
			{
				if (string.IsNullOrEmpty(Server))
					error = "Introduzca el nombre del servidor";
				else if (IntegratedSecurity && !CanUseIntegratedSecurity)
					error = "No se puede utilizar seguridad integrada en este caso";
				else if (!IntegratedSecurity && string.IsNullOrEmpty(User))
					error = "Introduzca el nombre de usuario";
				else if (!IntegratedSecurity && string.IsNullOrEmpty(Password))
					error = "Introduzca la contraseña";
				else if (string.IsNullOrEmpty(DataBase))
					error = "Introduzca el nombre de base de datos";
			}
			else if (UseConnectionString && string.IsNullOrEmpty(ConnectionString))
				error = "Introduzca la cadena de conexión";
			else if (IsTemporalDataBase && string.IsNullOrEmpty(FileName))
				error = "Seleccione el nombre de archivo";
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Guarda los datos del formulario en el modelo
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{
				// Asigna las propiedades
				Connection.Name = Name;
				Connection.Description = Description;
				Connection.Type = ConnectionTypes.GetSelectedItemTyped(DatabaseConnectionModel.DataBaseType.Odbc);
				// Limpia los datos de conexión
				Connection.ConnectionString = string.Empty;
				Connection.Server = string.Empty;
				Connection.Port = 0;
				Connection.DataBase = string.Empty;
				Connection.IntegratedSecurity = false;
				Connection.User = string.Empty;
				Connection.Password = string.Empty;
				Connection.FileName = string.Empty;
				// Asigna los datos de la conexión
				if (Connection.IsServerConnection)
				{
					Connection.Server = Server;
					Connection.Port = Port;
					Connection.IntegratedSecurity = IntegratedSecurity;
					if (!IntegratedSecurity)
					{
						Connection.User = User;
						Connection.Password = Password;
					}
					Connection.DataBase = DataBase;
				}
				else if (Connection.UseConnectionString)
					Connection.ConnectionString = ConnectionString;
				else if (Connection.Type == DatabaseConnectionModel.DataBaseType.SqLite)
					Connection.FileName = FileName;
				// Lanza el evento de cerrar
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Conexión que se está modificando
		/// </summary>
		internal DatabaseConnectionModel Connection { get; }

		/// <summary>
		///		Indica si es una nueva conexión
		/// </summary>
		internal bool IsNew { get; }

		/// <summary>
		///		Nombre de la conexión
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
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString
		{
			get { return _connectionString; }
			set { CheckProperty(ref _connectionString, value); }
		}

		/// <summary>
		///		Tipos de conexión
		/// </summary>
		public ControlGenericListViewModel<DatabaseConnectionModel.DataBaseType> ConnectionTypes { get; } = new ControlGenericListViewModel<DatabaseConnectionModel.DataBaseType>();

		/// <summary>
		///		Servidor
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
		///		Usuario
		/// </summary>
		public string User
		{
			get { return _user; }
			set { CheckProperty(ref _user, value); }
		}

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password
		{
			get { return _password; }
			set { CheckProperty(ref _password, value); }
		}

		/// <summary>
		///		Indica si se debe utilizar la seguridad integrada
		/// </summary>
		public bool IntegratedSecurity
		{
			get { return _integratedSecurity; }
			set 
			{ 
				if (CheckProperty(ref _integratedSecurity, value))
					UserEnabled = !value;
			}
		}

		/// <summary>
		///		Base de datos
		/// </summary>
		public string DataBase
		{
			get { return _dataBase; }
			set { CheckProperty(ref _dataBase, value); }
		}

		/// <summary>
		///		Nombre de archivo de la base de datos
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Indica si el usuario está activo
		/// </summary>
		public bool UserEnabled
		{
			get { return _userEnabled; }
			set { CheckProperty(ref _userEnabled, value); }
		}

		/// <summary>
		///		Indica si se trata de una conexión a servidor (Sql Server, MySql, postgresql, etc...)
		/// </summary>
		public bool IsServerConnection
		{
			get { return _isServerConnection; }
			set { CheckProperty(ref _isServerConnection, value); }
		}

		/// <summary>
		///		Indica si es una base de datos temporal
		/// </summary>
		public bool IsTemporalDataBase
		{
			get { return _isTemporalDataBase; }
			set { CheckProperty(ref _isTemporalDataBase, value); }
		}

		/// <summary>
		///		Indica si el tipo de base de datos utiliza una cadena de conexión (OleDb, Odbc, SqLite)
		/// </summary>
		public bool UseConnectionString
		{
			get { return _useConnectionString; }
			set { CheckProperty(ref _useConnectionString, value); }
		}

		/// <summary>
		///		Indica si se puede utilizar la seguridad integrada
		/// </summary>
		public bool CanUseIntegratedSecurity
		{
			get { return _canUseIntegratedSecurity; }
			set { CheckProperty(ref _canUseIntegratedSecurity, value); }
		}

		/// <summary>
		///		Indica si se debe utilizar el agente para la lectura de datos en la base de datos
		/// </summary>
		public bool UseAgent
		{
			get { return _useAgent; }
			set { CheckProperty(ref _useAgent, value); }
		}

		/// <summary>
		///		Indica si se debe utilizar el agente para la lectura de datos en la base de datos
		/// </summary>
		public bool CanUseAgent
		{
			get { return _canUseAgent; }
			set { CheckProperty(ref _canUseAgent, value); }
		}
	}
}
