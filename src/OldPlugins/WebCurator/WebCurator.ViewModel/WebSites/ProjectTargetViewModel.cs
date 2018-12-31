using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.WebSites
{
	/// <summary>
	///		ViewModel de <see cref="ProjectTargetModel"/>
	/// </summary>
	public class ProjectTargetViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _fileName;
		private string _sectionWithPages, _sectionMenu, _sectionTagMenuFileName;

		public ProjectTargetViewModel(ProjectModel project, ProjectTargetModel target)
		{
			Project = project;
			Target = target;
			if (Target == null)
				Target = new ProjectTargetModel();
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			ProjectFileName = Target.ProjectFileName;
			SectionWithPages = "".Concatenate(Target.SectionWithPages);
			SectionMenus = "".Concatenate(Target.SectionMenus);
			SectionTagMenuFileName = Target.SectionTagMenuFileName;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (ProjectFileName.IsEmpty() || !System.IO.File.Exists(ProjectFileName))
					WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del proyecto");
				else if (!System.IO.Path.GetExtension(ProjectFileName).EqualsIgnoreCase(".wdx"))
					WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("El archivo de proyecto no tiene la extensión adecuada");
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
					Target.ProjectFileName = ProjectFileName;
					Target.SectionWithPages = SectionWithPages.SplitToList();
					Target.SectionMenus = SectionMenus.SplitToList();
					Target.SectionTagMenuFileName = SectionTagMenuFileName;
					// Graba el objeto
					if (!Project.ProjectsTarget.Exists(Target.GlobalId))
						Project.ProjectsTarget.Add(Target);
					// Cierra el formulario
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del archivo del proyecto destino
		/// </summary>
		public string ProjectFileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Nombre de las secciones que tienen páginas de categoría asociadas
		/// </summary>
		public string SectionWithPages
		{
			get { return _sectionWithPages; }
			set { CheckProperty(ref _sectionWithPages, value); }
		}

		/// <summary>
		///		Nombre del archivo del menú superior
		/// </summary>
		public string SectionMenus
		{
			get { return _sectionMenu; }
			set { CheckProperty(ref _sectionMenu, value); }
		}

		/// <summary>
		///		Nombre del archivo del menú de tags
		/// </summary>
		public string SectionTagMenuFileName
		{
			get { return _sectionTagMenuFileName; }
			set { CheckProperty(ref _sectionTagMenuFileName, value); }
		}

		/// <summary>
		///		Datos del proyecto
		/// </summary>
		public ProjectModel Project { get;}

		/// <summary>
		///		Datos del proyecto destino
		/// </summary>
		public ProjectTargetModel Target { get;}
	}
}
