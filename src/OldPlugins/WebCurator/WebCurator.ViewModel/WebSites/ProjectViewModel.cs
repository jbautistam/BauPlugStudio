using System;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.WebSites
{
	/// <summary>
	///		ViewModel de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectViewModel : BaseFormViewModel
	{ 
		// Constantes privadas
		private const string ActionNewProjectTarget = "NewProjectTarget";
		private const string ActionUpdateProjectTarget = "UpdateProjectTarget";
		private const string ActionDeleteProjectTarget = "DeleteProjectTarget";
		// Variables privadas
		private string _name, _description;
		private string _pathImageSources, _filesXMLSentences, _filesRssSources;
		private int _numberDocuments, _maxImageWidth, _thumbWidth, _hoursBetweenGenerate;
		private ControlGenericListViewModel<ListItems.ProjectTarget.ProjectTargetItemViewModel> _targets;

		public ProjectViewModel(ProjectModel project)
		{
			// Inicializa el proyecto
			Project = project;
			if (Project == null)
				Project = new ProjectModel(WebCuratorViewModel.Instance.PathLibrary);
			// Inicializa los comandos
			NewProjectTargetCommand = new BaseCommand("Nuevo proyecto destino",
													  parameter => ExecuteAction(ActionNewProjectTarget, parameter),
													  parameter => CanExecuteAction(ActionNewProjectTarget, parameter));
			UpdateProjectTargetCommand = new BaseCommand("Modificar proyecto destino",
														 parameter => ExecuteAction(ActionUpdateProjectTarget, parameter),
														 parameter => CanExecuteAction(ActionUpdateProjectTarget, parameter));
			DeleteProjectTargetCommand = new BaseCommand("Borrar proyecto destino",
														 parameter => ExecuteAction(ActionDeleteProjectTarget, parameter),
														 parameter => CanExecuteAction(ActionDeleteProjectTarget, parameter));
			// Inicializa las propiedades
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{ 
			// Inicializa las propiedades
			Name = Project.Name;
			Description = Project.Description;
			NumberDocuments = Project.NumberDocuments;
			MaxImageWidth = Project.MaxImageWidth;
			ThumbWidth = Project.ThumbWidth;
			HoursBetweenGenerate = Project.HoursBetweenGenerate;
			PathImagesSources = "".Concatenate(Project.PathImagesSources);
			FilesXMLSentences = "".Concatenate(Project.FilesXMLSentences);
			FilesRssSources = "".Concatenate(Project.FilesRssSources);
			LoadListProjects();
			// Indica que no ha habido modificaciones
			IsUpdated = false;
		}

		/// <summary>
		///		Carga la lista de proyectos
		/// </summary>
		private void LoadListProjects()
		{ 
			// Crea la colección de proyectos destino
			ProjectsTarget = new ControlGenericListViewModel<ListItems.ProjectTarget.ProjectTargetItemViewModel>();
			// Añade los proyectos
			foreach (ProjectTargetModel target in Project.ProjectsTarget)
				ProjectsTarget.Add(new ListItems.ProjectTarget.ProjectTargetItemViewModel(target));
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SaveCommand):
						Save();
					break;
				case ActionNewProjectTarget:
						OpenFormUpdateProjectTarget(null);
					break;
				case ActionUpdateProjectTarget:
						OpenFormUpdateProjectTarget(GetSelectedProjectTarget());
					break;
				case ActionDeleteProjectTarget:
						DeleteProjectTarget(GetSelectedProjectTarget());
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SaveCommand):
				case ActionNewProjectTarget:
				case ActionUpdateProjectTarget:
				case ActionDeleteProjectTarget:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Obtiene el proyecto destino seleccionado
		/// </summary>
		private ProjectTargetModel GetSelectedProjectTarget()
		{
			return ProjectsTarget.SelectedItem?.Tag as ProjectTargetModel;
		}

		/// <summary>
		///		Abre el formulario de modificación de proyecto destino
		/// </summary>
		private void OpenFormUpdateProjectTarget(ProjectTargetModel target)
		{
			if (WebCuratorViewModel.Instance.ViewsController.OpenUpdateProjectTarget
							  (new ProjectTargetViewModel(Project, target)) == BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes)
			{
				LoadListProjects();
				IsUpdated = true;
			}
		}

		/// <summary>
		///		Borra el proyecto destino
		/// </summary>
		private void DeleteProjectTarget(ProjectTargetModel target)
		{
			if (target != null &&
				 WebCuratorViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea quitar " + target.ProjectFileName + "?"))
			{ 
				// Borra el proyecto
				Project.ProjectsTarget.RemoveByID(target.GlobalId);
				// Actualiza la lista
				LoadListProjects();
				// Indica que ha habido modificaciones
				IsUpdated = true;
			}
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
				else if (HoursBetweenGenerate < 1)
					WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el número de horas entre generación de proyectos");
				else if (FilesXMLSentences.IsEmpty() && FilesRssSources.IsEmpty())
					WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el nombre de al menos un archivo de frases o un archivo RSS");
				else if (!CheckFileSentencesNames(FilesXMLSentences.SplitToList(), out string error))
					WebCuratorViewModel.Instance.ControllerWindow.ShowMessage(error);
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Comprueba los nombres de los archivos de frases
		/// </summary>
		private bool CheckFileSentencesNames(System.Collections.Generic.List<string> sentences, out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Comprueba los proyectos
			foreach (string sentence in sentences)
				if (!System.IO.Path.GetExtension(sentence).EqualsIgnoreCase("." + Model.Sentences.FileSentencesModel.Extension))
					error = error.AddWithSeparator("El archivo '" + sentence + "' no tiene la extensión adecuada", Environment.NewLine);
				else if (!System.IO.File.Exists(sentence))
					error = error.AddWithSeparator("No existe el archivo '" + sentence + "'", Environment.NewLine);
			// Devuelve el valor que indica si los datos son correctos
			return error.IsEmpty();
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{
				string pathParent = WebCuratorViewModel.Instance.PathLibrary;

					// Asigna los datos al proyecto
					Project.Name = Name;
					Project.Description = Description;
					Project.NumberDocuments = NumberDocuments;
					Project.MaxImageWidth = MaxImageWidth;
					Project.ThumbWidth = ThumbWidth;
					Project.HoursBetweenGenerate = HoursBetweenGenerate;
					Project.PathImagesSources = PathImagesSources.SplitToList();
					Project.FilesXMLSentences = FilesXMLSentences.SplitToList();
					Project.FilesRssSources = FilesRssSources.SplitToList();
					// Graba el objeto
					new Application.Bussiness.WebSites.ProjectBussiness().Save(Project);
					// Indica que no quedan modificaciones pendientes
					IsUpdated = false;
			}
		}

		/// <summary>
		///		Nombre del proyecto
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value, "Name"); }
		}

		/// <summary>
		///		Descripción del proyecto
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value, "Description"); }
		}

		/// <summary>
		///		Número de documentos
		/// </summary>
		public int NumberDocuments
		{
			get { return _numberDocuments; }
			set { CheckProperty(ref _numberDocuments, value); }
		}

		/// <summary>
		///		Ancho máximo de las imágenes
		/// </summary>
		public int MaxImageWidth
		{
			get { return _maxImageWidth; }
			set { CheckProperty(ref _maxImageWidth, value); }
		}

		/// <summary>
		///		Ancho de los thumbnails
		/// </summary>
		public int ThumbWidth
		{
			get { return _thumbWidth; }
			set { CheckProperty(ref _thumbWidth, value); }
		}

		/// <summary>
		///		Horas entre generación de documentos
		/// </summary>
		public int HoursBetweenGenerate
		{
			get { return _hoursBetweenGenerate; }
			set { CheckProperty(ref _hoursBetweenGenerate, value); }
		}

		/// <summary>
		///		Proyectos destino
		/// </summary>
		public ControlGenericListViewModel<ListItems.ProjectTarget.ProjectTargetItemViewModel> ProjectsTarget
		{
			get { return _targets; }
			set { CheckObject(ref _targets, value); }
		}

		/// <summary>
		///		Directorios origen de imágenes
		/// </summary>
		public string PathImagesSources
		{
			get { return _pathImageSources; }
			set { CheckProperty(ref _pathImageSources, value); }
		}

		/// <summary>
		///		Archivos origen de Rss
		/// </summary>
		public string FilesRssSources
		{
			get { return _filesRssSources; }
			set { CheckProperty(ref _filesRssSources, value); }
		}

		/// <summary>
		///		XML de las frases utilizadas
		/// </summary>
		public string FilesXMLSentences
		{
			get { return _filesXMLSentences; }
			set { CheckProperty(ref _filesXMLSentences, value); }
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get { return Project.FileName; }
		}

		/// <summary>
		///		Comando de nuevo proyecto destino
		/// </summary>
		public BaseCommand NewProjectTargetCommand { get; }

		/// <summary>
		///		Comando de modificar proyecto destino
		/// </summary>
		public BaseCommand UpdateProjectTargetCommand { get; }

		/// <summary>
		///		Comando de borrar proyecto destino
		/// </summary>
		public BaseCommand DeleteProjectTargetCommand { get; }
	}
}
