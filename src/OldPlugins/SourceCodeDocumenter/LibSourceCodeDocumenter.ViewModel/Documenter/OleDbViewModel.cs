using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSourceCodeDocumenter.Application.Model;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter
{
	/// <summary>
	///		ViewModel para los proyectos de documentación de OleDb
	/// </summary>
	public class OleDbViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _name, _description, _connectionString;

		public OleDbViewModel(string title, string fileName, string projectPath)
		{
			LoadDocumenter(fileName, projectPath);
		}

		/// <summary>
		///		Carga los datos de una documentación
		/// </summary>
		private void LoadDocumenter(string fileName, string projectPath)
		{ 
			// Abre el proyecto de documentación
			SourceCodeProject = new Application.Bussiness.OleDbBussiness().Load(fileName);
			// Asigna las propiedades
			ProjectPath = projectPath;
			FileName = fileName;
			Name = SourceCodeProject.Name;
			if (Name.IsEmpty())
				Name = System.IO.Path.GetFileNameWithoutExtension(FileName);
			Description = SourceCodeProject.Description;
			ConnectionString = SourceCodeProject.ConnectionString;
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
				else if (ConnectionString.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Introuzca la cadena de conexión");
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
				SourceCodeProject.ConnectionString = ConnectionString;
				// ... y graba el objeto
				new Application.Bussiness.OleDbBussiness().Save(SourceCodeProject, FileName);
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
		private OleDbModel SourceCodeProject { get; set; }

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
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString
		{
			get { return _connectionString; }
			set { CheckProperty(ref _connectionString, value); }
		}
	}
}
