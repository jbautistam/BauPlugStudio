using System;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel con los datos de un documento de DocWriter
	/// </summary>
	public class ProjectViewModel : BaseFormViewModel
	{ 
		// Variables privadas
		private string _name, _title, _description, _keyWords, _urlBase;
		private string _pageMain;
		private int _itemsPerCategory, _itemsPerSiteMap, _maxWidthImage, _thumbsWidth;
		private bool _addWebTitle;
		private int _paragraphsSummaryNumber;
		private string _webMaster, _copyright, _editor, _variablesText, _postCompileCommands;
		private Documents.TemplateViewModel _templates;

		public ProjectViewModel(ProjectModel project)
		{
			Project = project;
			LoadProject(project);
		}

		/// <summary>
		///		Inicializa los combos
		/// </summary>
		private void InitCombos()
		{
			ComboWebType = new Helper.ComboViewHelper(this).LoadComboWebTypes("ComboWebType");
		}

		/// <summary>
		///		Carga los datos del proyecto
		/// </summary>
		private void LoadProject(ProjectModel project)
		{ 
			// Inicializa los combos
			InitCombos();
			// Carga los datos del proyecto
			project = new Application.Bussiness.Solutions.ProjectBussiness().Load(project.Solution, project.File.FullFileName);
			// Muestra los datos
			Name = project.Name;
			if (project.Title.IsEmpty())
				Title = project.Name;
			else
				Title = project.Title;
			ComboWebType.SelectedID = (int) project.WebType;
			Description = project.Description;
			KeyWords = project.KeyWords;
			UrlBase = project.URLBase;
			PageMain = project.PageMain;
			ItemsPerCategory = project.ItemsPerCategory;
			ItemsPerSiteMap = project.ItemsPerSiteMap;
			MaxWidthImage = project.MaxWidthImage;
			ThumbsWidth = project.ThumbsWidth;
			AddWebTitle = project.AddWebTitle;
			ParagraphsSummaryNumber = project.ParagraphsSummaryNumber;
			WebMaster = project.WebMaster;
			Copyright = project.Copyright;
			Editor = project.Editor;
			VariablesText = project.VariablesText;
			PostCompileCommands = project.PostCompileCommands;
			// Inicializa las plantillas
			Templates = new Documents.TemplateViewModel(this, project.Templates);
			// Indica que aún no se ha hecho ninguna modificación
			IsUpdated = false;
		}

		/// <summary>
		///		Comprueba los datos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (Title.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el título del proyecto");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{
				ProjectModel project = new ProjectModel(Project.Solution);

					// Asigna los datos de archivo
					project.File.FullFileName = Project.File.FullFileName;
					// Asigna los datos del formulario al objeto
					project.Name = Name;
					project.Title = Title;
					project.WebType = (ProjectModel.WebDefinitionType) (ComboWebType.SelectedID ?? 0);
					project.Description = Description;
					project.KeyWords = KeyWords;
					project.URLBase = UrlBase;
					project.PageMain = PageMain;
					project.ItemsPerCategory = ItemsPerCategory;
					project.ItemsPerSiteMap = ItemsPerSiteMap;
					project.MaxWidthImage = MaxWidthImage;
					project.ThumbsWidth = ThumbsWidth;
					project.AddWebTitle = AddWebTitle;
					project.ParagraphsSummaryNumber = ParagraphsSummaryNumber;
					project.WebMaster = WebMaster;
					project.Copyright = Copyright;
					project.Editor = Editor;
					project.VariablesText = VariablesText;
					project.PostCompileCommands = PostCompileCommands;
					// Asigna las plantillas
					project.Templates = Templates.GetTemplates();
					// Graba el documento
					new Application.Bussiness.Solutions.ProjectBussiness().Save(project);
					// Indica que no hay modificaciones pendientes
					IsUpdated = false;
			}
		}

		/// <summary>
		///		Borra el archivo
		/// </summary>
		private void Delete()
		{
			if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea borrar este proyecto?"))
			{ 
				// Borra el archivo
				// new Application.Bussiness.Solutions.ProjectBussiness().Delete(Project);
				// Cierra el documento
				base.IsUpdated = false;
				base.Close(BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes);
			}
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
				case nameof(DeleteCommand):
						Delete();
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
				case nameof(DeleteCommand):
				case nameof(SaveCommand):
					return true;
				default:
					return false;
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
		///		Título del documento
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

		/// <summary>
		///		Descripción del documento
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Palabras clave
		/// </summary>
		public string KeyWords
		{
			get { return _keyWords; }
			set { CheckProperty(ref _keyWords, value); }
		}

		/// <summary>
		///		Url base del proyecto
		/// </summary>
		public string UrlBase
		{
			get { return _urlBase; }
			set { CheckProperty(ref _urlBase, value); }
		}

		/// <summary>
		///		Página principal
		/// </summary>
		public string PageMain
		{
			get { return _pageMain; }
			set { CheckProperty(ref _pageMain, value); }
		}

		/// <summary>
		///		Número de elementos por categoría
		/// </summary>
		public int ItemsPerCategory
		{
			get { return _itemsPerCategory; }
			set { CheckProperty(ref _itemsPerCategory, value); }
		}

		/// <summary>
		///		Número de elementos por mapa del sitio
		/// </summary>
		public int ItemsPerSiteMap
		{
			get { return _itemsPerSiteMap; }
			set { CheckProperty(ref _itemsPerSiteMap, value); }
		}

		/// <summary>
		///		Ancho máximo de una imagen
		/// </summary>
		public int MaxWidthImage
		{
			get { return _maxWidthImage; }
			set { CheckProperty(ref _maxWidthImage, value); }
		}

		/// <summary>
		///		Ancho máximo de un thumbnail
		/// </summary>
		public int ThumbsWidth
		{
			get { return _thumbsWidth; }
			set { CheckProperty(ref _thumbsWidth, value); }
		}

		/// <summary>
		///		Indica si se debe añadir el título de la web al título de los artículos
		/// </summary>
		public bool AddWebTitle
		{
			get { return _addWebTitle; }
			set { CheckProperty(ref _addWebTitle, value); }
		}

		/// <summary>
		///		Número de párrafos resumen para RSS
		/// </summary>
		public int ParagraphsSummaryNumber
		{
			get { return _paragraphsSummaryNumber; }
			set { CheckProperty(ref _paragraphsSummaryNumber, value); }
		}

		/// <summary>
		///		Nombre del WebMaster del sitio
		/// </summary>
		public string WebMaster
		{
			get { return _webMaster; }
			set { CheckProperty(ref _webMaster, value); }
		}

		/// <summary>
		///		Copyright del sitio
		/// </summary>
		public string Copyright
		{
			get { return _copyright; }
			set { CheckProperty(ref _copyright, value); }
		}

		/// <summary>
		///		Editor del sitio
		/// </summary>
		public string Editor
		{
			get { return _editor; }
			set { CheckProperty(ref _editor, value); }
		}

		/// <summary>
		///		Variables a utilizar en la compilación
		/// </summary>
		public string VariablesText
		{
			get { return _variablesText; }
			set { CheckProperty(ref _variablesText, value); }
		}

		/// <summary>
		///		Comandos a ejecutar tras la compilación del proyecto
		/// </summary>
		public string PostCompileCommands
		{
			get { return _postCompileCommands; }
			set { CheckProperty(ref _postCompileCommands, value); }
		}

		/// <summary>
		///		Combo de tipo de Web
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboWebType { get; private set; }

		/// <summary>
		///		Plantillas
		/// </summary>
		public Documents.TemplateViewModel Templates
		{
			get { return _templates; }
			private set { CheckObject(ref _templates, value); }
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		protected ProjectModel Project { get; }
	}
}
