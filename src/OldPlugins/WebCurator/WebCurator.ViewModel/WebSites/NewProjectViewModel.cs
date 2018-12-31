using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.WebSites
{
	/// <summary>
	///		ViewModel de alta de un <see cref="ProjectModel"/>
	/// </summary>
	public class NewProjectViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name, _description;

		public NewProjectViewModel(ProjectModel project)
		{
			Project = project;
			if (Project == null)
				Project = new ProjectModel(WebCuratorViewModel.Instance.PathLibrary);
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			Name = Project.Name;
			Description = Project.Description;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del proyecto");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{
				string pathParent = WebCuratorViewModel.Instance.PathLibrary;

					// Asigna los datos al proyecto
					Project.Name = Name;
					Project.Description = Description;
					// Graba el objeto
					new Application.Bussiness.WebSites.ProjectBussiness().Save(Project);
					// Cierra el formulario
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del proyecto
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción del proyecto
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }
	}
}
