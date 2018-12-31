using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSourceCodeDocumenter.Application.Model;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter
{
	/// <summary>
	///		ViewModel para documentación de proyectos de código fuente
	/// </summary>
	public class SourceCodeViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private string _name, _description, _sourceFileName;

		public SourceCodeViewModel(string title, string fileName, string projectPath)
		{
			LoadDocumenter(fileName, projectPath);
		}

		/// <summary>
		///		Carga los datos de una documentación
		/// </summary>
		private void LoadDocumenter(string fileName, string projectPath)
		{ 
			// Abre el proyecto de documentación
			SourceCodeProject = new Application.Bussiness.SourceCodeBussiness().Load(fileName);
			// Asigna las propiedades
			ProjectPath = projectPath;
			FileName = fileName;
			Name = SourceCodeProject.Name;
			if (Name.IsEmpty())
				Name = System.IO.Path.GetFileNameWithoutExtension(FileName);
			Description = SourceCodeProject.Description;
			SourceFileName = SourceCodeProject.SourceFileName;
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
				else if (SourceFileName.IsEmpty())
					SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el nombre del proyecto o solución a documentar");
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
				SourceCodeProject.SourceFileName = SourceFileName;
				// ... y graba el objeto
				new Application.Bussiness.SourceCodeBussiness().Save(SourceCodeProject, FileName);
				// Indica que no hay modificaciones pendientes
				base.IsUpdated = false;
				SaveCommand.OnCanExecuteChanged();
			}
		}

		/// <summary>
		///		Objeto de documentación
		/// </summary>
		private SourceCodeModel SourceCodeProject { get; set; }

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
		///		Nombre del archivo origen
		/// </summary>
		public string SourceFileName
		{
			get { return _sourceFileName; }
			set { CheckProperty(ref _sourceFileName, value); }
		}
	}
}
