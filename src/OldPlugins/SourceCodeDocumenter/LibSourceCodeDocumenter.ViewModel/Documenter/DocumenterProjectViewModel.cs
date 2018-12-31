using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSourceCodeDocumenter.Application.Model;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter
{
	/// <summary>
	///		ViewModel para un proyecto de documentación
	/// </summary>
	public class DocumenterProjectViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _name, _description, _pathGenerate, _pathTemplates, _pathPages;
		private bool _showPublic, _showInternal, _showProtected, _showPrivate;

		public DocumenterProjectViewModel(string title, string fileName, string projectPath)
		{
			LoadDocumenter(fileName, projectPath);
		}

		/// <summary>
		///		Carga los datos de una documentación
		/// </summary>
		private void LoadDocumenter(string fileName, string projectPath)
		{ 
			// Abre el proyecto de documentación
			Project = new Application.Bussiness.DocumenterProjectBussiness().Load(fileName);
			// Asigna las propiedades
			ProjectPath = projectPath;
			FileName = fileName;
			Name = Project.Name;
			if (Name.IsEmpty())
				Name = System.IO.Path.GetFileNameWithoutExtension(FileName);
			Description = Project.Description;
			PathGenerate = Project.PathGenerate;
			PathPages = Project.PathPages;
			PathTemplates = Project.PathTemplates;
			ShowPublic = Project.ShowPublic;
			ShowInternal = Project.ShowInternal;
			ShowProtected = Project.ShowProtected;
			ShowPrivate = Project.ShowPrivate;
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
				else if (PathTemplates.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el directorio de plantillas");
				else if (!System.IO.Directory.Exists(PathTemplates))
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el directorio de plantillas");
				else if (PathGenerate.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el directorio de generación de la documentación");
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
				Project.Name = Name;
				Project.Description = Description;
				Project.PathGenerate = PathGenerate;
				Project.PathPages = PathPages;
				Project.PathTemplates = PathTemplates;
				Project.ShowPublic = ShowPublic;
				Project.ShowInternal = ShowInternal;
				Project.ShowProtected = ShowProtected;
				Project.ShowPrivate = ShowPrivate;
				// ... y graba el objeto
				new Application.Bussiness.DocumenterProjectBussiness().Save(Project, FileName);
				// Indica que no hay modificaciones pendientes
				base.IsUpdated = false;
				SaveCommand.OnCanExecuteChanged();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Proyecto de documentación
		/// </summary>
		private DocumenterProjectModel Project { get; set; }

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
		///		Directorio de generación
		/// </summary>
		public string PathGenerate
		{
			get { return _pathGenerate; }
			set { CheckProperty(ref _pathGenerate, value); }
		}

		/// <summary>
		///		Directorio de páginas adicionales
		/// </summary>
		public string PathPages
		{
			get { return _pathPages; }
			set { CheckProperty(ref _pathPages, value); }
		}

		/// <summary>
		///		Directorio de plantillas
		/// </summary>
		public string PathTemplates
		{
			get { return _pathTemplates; }
			set { CheckProperty(ref _pathTemplates, value); }
		}

		/// <summary>
		///		Indica si se deben documentar las estructuras públicas
		/// </summary>
		public bool ShowPublic
		{
			get { return _showPublic; }
			set { CheckProperty(ref _showPublic, value); }
		}

		/// <summary>
		///		Indica si se deben documentar las estructuras internas
		/// </summary>
		public bool ShowInternal
		{
			get { return _showInternal; }
			set { CheckProperty(ref _showInternal, value); }
		}

		/// <summary>
		///		Indica si se deben documentar las estructuras protegidas
		/// </summary>
		public bool ShowProtected
		{
			get { return _showProtected; }
			set { CheckProperty(ref _showProtected, value); }
		}

		/// <summary>
		///		Indica si se deben documentar las estructuras privadas
		/// </summary>
		public bool ShowPrivate
		{
			get { return _showPrivate; }
			set { CheckProperty(ref _showPrivate, value); }
		}
	}
}
