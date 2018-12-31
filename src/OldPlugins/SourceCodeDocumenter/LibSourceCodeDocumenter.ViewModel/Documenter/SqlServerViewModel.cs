using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSourceCodeDocumenter.Application.Model;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter
{
	/// <summary>
	///		ViewModel para los proyectos de documentación de SQLServer
	/// </summary>
	public class SqlServerViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _name, _description, _server, _dataBase;
		private bool _useDataBaseFile, _useWindowsAuthentification;
		private string _dataBaseFile, _user, _password;

		public SqlServerViewModel(string title, string fileName, string projectPath)
		{
			LoadDocumenter(fileName, projectPath);
		}

		/// <summary>
		///		Carga los datos de una documentación
		/// </summary>
		private void LoadDocumenter(string fileName, string projectPath)
		{ 
			// Abre el proyecto de documentación
			SourceCodeProject = new Application.Bussiness.SqlServerBussiness().Load(fileName);
			// Asigna las propiedades
			ProjectPath = projectPath;
			FileName = fileName;
			Name = SourceCodeProject.Name;
			if (Name.IsEmpty())
				Name = System.IO.Path.GetFileNameWithoutExtension(FileName);
			Description = SourceCodeProject.Description;
			Server = SourceCodeProject.Server;
			DataBase = SourceCodeProject.DataBase;
			UseDataBaseFile = SourceCodeProject.UseDataBaseFile;
			DataBaseFile = SourceCodeProject.DataBaseFile;
			UseWindowsAuthentification = SourceCodeProject.UseWindowsAuthentification;
			User = SourceCodeProject.User;
			Password = SourceCodeProject.Password;
			// Indica que aún no se ha hecho ninguna modificación
			base.IsUpdated = false;
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (Name.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre");
				else if (Server.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Introuzca el nombre del servidor");
				else if (DataBaseFile.IsEmpty() && UseDataBaseFile)
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el nombre de archivo de base de datos");
				else if (DataBase.IsEmpty() && !UseDataBaseFile)
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la base de datos");
				else if (!UseWindowsAuthentification && User.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de usuario");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Pasa los valores del formulario al objeto
				SourceCodeProject.Name = Name;
				SourceCodeProject.Description = Description;
				SourceCodeProject.Server = Server;
				SourceCodeProject.DataBase = DataBase;
				SourceCodeProject.UseDataBaseFile = UseDataBaseFile;
				SourceCodeProject.DataBaseFile = DataBaseFile;
				SourceCodeProject.UseWindowsAuthentification = UseWindowsAuthentification;
				SourceCodeProject.User = User;
				SourceCodeProject.Password = Password;
				// ... y graba el objeto
				new Application.Bussiness.SqlServerBussiness().Save(SourceCodeProject, FileName);
				// Indica que no hay modificaciones pendientes
				IsUpdated = false;
				SaveCommand.OnCanExecuteChanged();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Objeto de documentación
		/// </summary>
		private SqlServerModel SourceCodeProject { get; set; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		///		Directorio del proyecto
		/// </summary>
		public string ProjectPath { get; private set; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Servidor
		/// </summary>
		public string Server
		{
			get { return _server; }
			set { CheckProperty(ref _server, value); }
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
		///		Indica si se debe utilizar un archivo de base de datos
		/// </summary>
		public bool UseDataBaseFile
		{
			get { return _useDataBaseFile; }
			set { CheckProperty(ref _useDataBaseFile, value); }
		}

		/// <summary>
		///		Archivo de base de datos
		/// </summary>
		public string DataBaseFile
		{
			get { return _dataBaseFile; }
			set { CheckProperty(ref _dataBaseFile, value); }
		}

		/// <summary>
		///		Indica si se debe utilizar la autentificación de Windows
		/// </summary>
		public bool UseWindowsAuthentification
		{
			get { return _useWindowsAuthentification; }
			set { CheckProperty(ref _useWindowsAuthentification, value); }
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
	}
}
