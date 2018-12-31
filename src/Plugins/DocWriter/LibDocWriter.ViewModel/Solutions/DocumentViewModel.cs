using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction;
using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel con los datos de un documento de DocWriter
	/// </summary>
	public class DocumentViewModel : BaseFormViewModel
	{ 
		// Eventos públicos
		public event EventHandler<EventArguments.EndFileCopyEventArgs> EndFileCopy;
		public event EventHandler<EventArguments.EditorInsertTextEventArgs> InsertTextEditor;
		// Variables privadas
		private string _title, _description, _keyWords, _content, _urlImageSummary;
		private bool _showAtRss, _isRecursive;
		private string _lastDefaultPath;
		private Documents.TemplateViewModel _templates;

		public DocumentViewModel(SolutionModel solution, FileModel file)
		{
			// Inicializa las propiedades
			Solution = solution;
			File = file;
			LoadDocument(file);
			// Inicializa los comandos y eventos
			PasteImageCommand = new BaseCommand("Pegar imagen", parameter => PasteImage());
			InstructionCommand = new BaseCommand("Instruction", parameter => ExecuteCommandInstruction(parameter));
			InstructionsEditCommand = new BaseCommand("Editar instrucciones", parameter => OpenListInstructions());
			PropertyChanged += (sender, evntArgs) =>
			{
				if (evntArgs.PropertyName.EqualsIgnoreNull(nameof(ComboDocumentScope)))
				{
					OnPropertyChanged(nameof(IsDocumentDataVisible));
					OnPropertyChanged(nameof(IsPageDataVisible));
					OnPropertyChanged(nameof(IsSectionDataVisible));
					OnPropertyChanged(nameof(IsPageTagDataVisible));
				}
			};
		}

		/// <summary>
		///		Inicializa los combos
		/// </summary>
		private void InitCombos()
		{
			Helper.ComboViewHelper objHelper = new Helper.ComboViewHelper(this);

				// Combo con el modo de visualización de páginas hija
				ComboModeShowChildItems = new MVVM.ViewModels.ComboItems.ComboViewModel(this, nameof(ComboModeShowChildItems));
				ComboModeShowChildItems.AddItem((int) DocumentModel.ShowChildsMode.None, "No mostrar");
				ComboModeShowChildItems.AddItem((int) DocumentModel.ShowChildsMode.SortByDate, "Por fecha");
				ComboModeShowChildItems.AddItem((int) DocumentModel.ShowChildsMode.SortByPages, "Por orden");
				ComboModeShowChildItems.SelectedID = (int) DocumentModel.ShowChildsMode.None;
				// Combo de ámbito de documento
				ComboDocumentScope = new MVVM.ViewModels.ComboItems.ComboViewModel(this, nameof(ComboDocumentScope));
				ComboDocumentScope.AddItem((int) DocumentModel.ScopeType.Unknown, "<Seleccione un elemento>");
				ComboDocumentScope.AddItem((int) DocumentModel.ScopeType.Web, "Web");
				ComboDocumentScope.AddItem((int) DocumentModel.ScopeType.Page, "Página");
				ComboDocumentScope.AddItem((int) DocumentModel.ScopeType.Sitemap, "Mapa del sitio");
				ComboDocumentScope.AddItem((int) DocumentModel.ScopeType.News, "Noticias");
				ComboDocumentScope.SelectedID = (int) DocumentModel.ScopeType.Unknown;
				// Combo de copia de imágenes
				ComboCopyImages = new MVVM.ViewModels.ComboItems.ComboViewModel(this, nameof(ComboCopyImages));
				ComboCopyImages.AddItem(null, "<Selección de copia de imágenes>");
				ComboCopyImages.AddItem((int) EventArguments.EndFileCopyEventArgs.CopyImageType.Normal, "Copiar imágenes");
				ComboCopyImages.AddItem((int) EventArguments.EndFileCopyEventArgs.CopyImageType.Gallery, "Copiar imágenes como galería");
				ComboCopyImages.SelectedID = null;
				// Añade el manejador de eventos
				ComboCopyImages.PropertyChanged += (sender, evntArgs) =>
														{
															if (evntArgs.PropertyName == nameof(ComboCopyImages.SelectedItem))
															{
																if (ComboCopyImages.SelectedID != null)
																	PasteMultipleImages((EventArguments.EndFileCopyEventArgs.CopyImageType) (ComboCopyImages.SelectedID ?? 0));
																ComboCopyImages.SelectedID = null;
															}
														};
				// Añade los elementos al combo de instrucciones
				LoadComboInstructions();
		}

		/// <summary>
		///		Carga el combo de instrucciones
		/// </summary>
		private void LoadComboInstructions()
		{
			EditorInstructionModelCollection instructions = new EditorInstructionBussiness().Load(DocWriterViewModel.Instance.FileNameEditorInstructions);

				// Inicializa el combo
				ComboInstructions = new MVVM.ViewModels.ComboItems.ComboViewModel(this, nameof(ComboInstructions));
				// Añade los elementos básicaos
				ComboInstructions.AddItem(null, "<Instrucciones>", null);
				// Añade las instrucciones
				foreach (EditorInstructionModel instruction in instructions)
					ComboInstructions.AddItem(ComboInstructions.Items.Count + 1, instruction.Name, instruction.Code);
				// Añade el elemento de insertar nueva instrucción
				ComboInstructions.AddItem(0, "Insertar instrucción", null);
				// Selecciona el primer elemento
				ComboInstructions.SelectedID = null;
				// Añade el manejador de eventos
				ComboInstructions.PropertyChanged += (sender, evntArgs) =>
														{
															if (evntArgs.PropertyName == nameof(ComboInstructions.SelectedItem))
															{
																if (ComboInstructions.SelectedID == 0)
																	OpenFormInstructions();
																else
																	ExecuteInsertInstruction();
															}
														};
		}

		/// <summary>
		///		Carga los datos del documento
		/// </summary>
		private void LoadDocument(FileModel file)
		{
			DocumentModel document = new Application.Bussiness.Documents.DocumentBussiness().Load(file);

				// Inicializa los combos
				InitCombos();
				// Asigna el título inicial
				Title = file.Title;
				// Si hay algo en el documento, muestra los datos
				if (!document.Title.IsEmpty())
					Title = document.Title;
				Description = document.Description;
				KeyWords = document.KeyWords;
				Content = document.Content;
				ShowAtRss = document.ShowAtRSS;
				UrlImageSummary = document.URLImageSummary;
				ComboModeShowChildItems.SelectedID = (int) document.ModeShow;
				IsRecursive = document.IsRecursive;
				ComboDocumentScope.SelectedID = (int) document.IDScope;
				// Carga los árboles de páginas
				TreeTags = new Documents.TreeDocumentsViewModel(this, FileModel.DocumentType.Tag, File, document.Tags);
				TreeTags.PropertyChanged += (sender, evntArgs) =>
				{
					if (evntArgs.PropertyName.EqualsIgnoreCase(nameof(TreeTags.IsTreeeUpdated)))
						IsUpdated = true;
				};
				// Carga la lista de páginas hija
				ListChildPages = new Documents.PagesListViewModel(this, document.File.Project, document);
				ListChildPages.LoadData();
				ListChildPages.PropertyChanged += (sender, evntArgs) =>
														{
															if (evntArgs.PropertyName.EqualsIgnoreCase(nameof(ListChildPages.ItemsUpdated)))
																IsUpdated = true;
														};
				// Inicializa las plantillas
				Templates = new Documents.TemplateViewModel(this, document.Templates);
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
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el título de la página");
				else if (!CanCompile(out string error))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage($"Error en la compilación.{Environment.NewLine}{error}");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Comprueba si se puede compilar el contenido de la página
		/// </summary>
		private bool CanCompile(out string error)
		{
			string fileProject = GetFileNameForGenerator();

				// Inicializa los valores de salida
				error = "";
				// Si tenemos realmente nombre de proyecto
				if (string.IsNullOrWhiteSpace(fileProject))
					error = "No se puede encontrar el archivo de proyecto";
				else
				{
					Processor.Generator generator;

						// Crea el objeto de generación
						generator = new Processor.Generator(Solution.FullFileName, fileProject, DocWriterViewModel.Instance.PathGeneration, false, true);
						// Compila el contenido de la página
						generator.Compile(Content);
						// Muestra los errores
						foreach (Processor.Errors.ErrorMessage errorMessage in generator.Errors)
							error = error.AddWithSeparator($"{errorMessage.Message} - Token {errorMessage.Token} - Fila {errorMessage.Row} - Columna {errorMessage.Column}",
														   Environment.NewLine);
				}
				// Devuelve el valor que indica si la compilación es correcta
				return string.IsNullOrWhiteSpace(error);
		}

		/// <summary>
		///		Obtiene un nombre de archivo para generar
		/// </summary>
		private string GetFileNameForGenerator()
		{
			string fileProject = File?.FullFileName;

				// Obtiene el nombre del proyecto
				if (string.IsNullOrWhiteSpace(fileProject))
				{
					if (Solution.Projects.Count > 0)
						fileProject = Solution.Projects[0]?.File?.FullFileName;
				}
				// Devuelve el nombre de archivo encontrado
				return fileProject;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{
				DocumentModel document = new DocumentModel(File);

					// Asigna los datos
					document.Title = Title;
					document.Description = Description;
					document.KeyWords = KeyWords;
					document.Content = Content;
					document.ShowAtRSS = ShowAtRss;
					document.URLImageSummary = UrlImageSummary;
					document.ModeShow = (DocumentModel.ShowChildsMode) (ComboModeShowChildItems.SelectedID ?? (int) DocumentModel.ShowChildsMode.None);
					document.IsRecursive = IsRecursive;
					document.File.FileType = File.FileType;
					document.IDScope = (DocumentModel.ScopeType) (ComboDocumentScope.SelectedID ?? (int) DocumentModel.ScopeType.Unknown);
					// Añade las páginas seleccionadas
					document.Tags = TreeTags.GetIsCheckedFiles();
					document.ChildPages = ListChildPages.GetPages();
					// Asigna las plantillas
					document.Templates = Templates.GetTemplates();
					// Graba el documento
					new Application.Bussiness.Documents.DocumentBussiness().Save(File, document);
					// Indica que no hay modificaciones pendientes
					IsUpdated = false;
			}
		}

		/// <summary>
		///		Ejecuta el comando de inserción de instrucción seleccionada en el combo
		/// </summary>
		public void ExecuteInsertInstruction()
		{
			if (ComboInstructions.SelectedTag != null)
				RaiseEventInsertText(ComboInstructions.SelectedTag as string);
		}

		/// <summary>
		///		Pega la imagen del portapapeles sobre el documento
		/// </summary>
		private void PasteImage()
		{
			if (!new Helper.ClipboardHelper().PasteImage(File.Path, File.Project.MaxWidthImage, File.Project.ThumbsWidth, 
														 out string fileName, out string error))
			{
				if (!error.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage(error);
			}
			else
				UrlImageSummary = GetUrlFileNameRelative(fileName.Substring(File.Project.File.Path.Length + 1));
		}

		/// <summary>
		///		Pega una serie de imágenes sobre el documento
		/// </summary>
		private void PasteMultipleImages(EventArguments.EndFileCopyEventArgs.CopyImageType idCopyMode)
		{
			string [] filesSource;

				// Obtiene los nombres de las imágenes
				filesSource = DocWriterViewModel.Instance.DialogsController.OpenDialogLoadFilesMultiple(LastDefaultPath, "Todos los archivos (*.*)|*.*", null, null);
				// Copia las imágenes
				if (filesSource != null && filesSource.Length > 0)
				{
					string [] filesTarget;

					// Copia las imágenes
					filesTarget = new Helper.ClipboardHelper().CopyImages(filesSource, File.Path, File.Project.MaxWidthImage,
																		  File.Project.ThumbsWidth, out string error);
					// Muestra el mensaje de error
					if (!error.IsEmpty())
						DocWriterViewModel.Instance.ControllerWindow.ShowMessage(error);
					else if (filesSource != null && filesSource.Length > 0)
					{ 
						// Borra los archivos antiguos
						if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion("¿Desea eliminar los archivos originales?"))
							foreach (string fileSource in filesSource)
								Bau.Libraries.LibCommonHelper.Files.HelperFiles.KillFile(fileSource);
						// Asigna el directorio al directorio predeterminado
						LastDefaultPath = System.IO.Path.GetDirectoryName(filesSource [0]);
						// Lanza el evento de fin de copia de archivos
						RaiseEventEndFileCopy(filesSource, filesTarget, idCopyMode);
					}
				}
		}

		/// <summary>
		///		Obtiene el nombre de archivo relativo al directorio del proyecto
		/// </summary>
		private string GetUrlFileNameRelative(string fileName)
		{
			if (fileName.StartsWithIgnoreNull(File.Project.File.Path, StringComparison.CurrentCultureIgnoreCase))
				return fileName.Substring(File.Project.File.Path.Length + 1);
			else
				return fileName;
		}

		/// <summary>
		///		Lanza el evento de fin de copia de archivos
		/// </summary>
		private void RaiseEventEndFileCopy(string [] filesSource, string [] filesTarget, EventArguments.EndFileCopyEventArgs.CopyImageType idCopyMode)
		{
			if (filesSource != null && filesSource.Length > 0 &&
					  filesTarget != null && filesTarget.Length > 0 &&
					  EndFileCopy != null)
			{ 
				// Convierte los nombres de archivo destino a nombres de archivo relativos
				for (int index = 0; index < filesTarget.Length; index++)
					filesTarget [index] = GetUrlFileNameRelative(filesTarget [index]);
				// Lanza el evento
				EndFileCopy(this, new EventArguments.EndFileCopyEventArgs(filesSource, filesTarget, idCopyMode));
			}
		}

		/// <summary>
		///		Borra el archivo
		/// </summary>
		private void Delete()
		{
			if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea borrar este documento?"))
			{ 
				// Borra el archivo
				new Application.Bussiness.Solutions.FileBussiness().Delete(File);
				// Cierra el documento
				base.IsUpdated = false;
				base.Close(SystemControllerEnums.ResultType.Yes);
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
		///		Abre el formulario de instrucciones
		/// </summary>
		private void OpenFormInstructions()
		{
			if (DocWriterViewModel.Instance.ViewsController.OpenFormEditorInstruction(null) == SystemControllerEnums.ResultType.Yes)
				LoadComboInstructions();
		}

		/// <summary>
		///		Abre el formulario con la lista de instrucciones
		/// </summary>
		private void OpenListInstructions()
		{
			if (DocWriterViewModel.Instance.ViewsController.OpenFormInstructionsLists() == SystemControllerEnums.ResultType.Yes)
				LoadComboInstructions();
		}

		/// <summary>
		///		Inserta una instrucción a partir de un parámetro
		/// </summary>
		private void ExecuteCommandInstruction(object parameter)
		{
			if (parameter != null && parameter is string)
			{
				string instruction = parameter as string;

					if (instruction.EqualsIgnoreCase("p"))
						RaiseEventInsertText(Environment.NewLine + "%p ");
					else if (instruction.EqualsIgnoreCase("b"))
						RaiseEventInsertText(" #b  # ", -3);
					else if (instruction.EqualsIgnoreCase("i"))
						RaiseEventInsertText(" #i  # ", -3);
					else if (instruction.EqualsIgnoreCase("a"))
						RaiseEventInsertText(" #a { href = \" \" target = \"_blank\" } # ", -3);
					else if (instruction.EqualsIgnoreCase("e"))
						RaiseEventInsertText(" #a { href = \" \" target = \"_blank\" } # ", -3);
					else if (instruction.EqualsIgnoreCase("ul"))
						RaiseEventInsertText(Environment.NewLine + "%ul" + Environment.NewLine + "\t%li");
					else if (instruction.EqualsIgnoreCase("ol"))
						RaiseEventInsertText(Environment.NewLine + "%ol" + Environment.NewLine + "\t%li");
			}
		}

		/// <summary>
		///		Lanza el evento de inserción de texto en el editor
		/// </summary>
		private void RaiseEventInsertText(string text, int intOffset = 0)
		{
			if (!string.IsNullOrWhiteSpace(text) && InsertTextEditor != null)
				InsertTextEditor(this, new EventArguments.EditorInsertTextEventArgs(text, intOffset));
		}

		/// <summary>
		///		Solución
		/// </summary>
		public SolutionModel Solution { get; }

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
		///		Contenido
		/// </summary>
		public string Content
		{
			get { return _content; }
			set { CheckProperty(ref _content, value); }
		}

		/// <summary>
		///		URL de la imagen
		/// </summary>
		public string UrlImageSummary
		{
			get { return _urlImageSummary; }
			set { CheckProperty(ref _urlImageSummary, value); }
		}

		/// <summary>
		///		Indica si se debe mostrar en las noticias
		/// </summary>
		public bool ShowAtRss
		{
			get { return _showAtRss; }
			set { CheckProperty(ref _showAtRss, value); }
		}

		/// <summary>
		///		Combo de elementos hijo
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboModeShowChildItems { get; private set; }

		/// <summary>
		///		Indica si los elementos hijo se deben cargar de forma recursiva
		/// </summary>
		public bool IsRecursive
		{
			get { return _isRecursive; }
			set { CheckProperty(ref _isRecursive, value); }
		}

		/// <summary>
		///		Tipo de documento
		/// </summary>
		public string DocumentType
		{
			get
			{
				switch (File.FileType)
				{
					case FileModel.DocumentType.Document:
						return "Documento";
					case FileModel.DocumentType.Tag:
						return "Etiqueta";
					case FileModel.DocumentType.Template:
						return "Plantilla";
					case FileModel.DocumentType.Section:
						return "Sección";
					case FileModel.DocumentType.SiteMap:
						return "Mapa del sitio";
					case FileModel.DocumentType.File:
						return "Otros";
					default:
						return "Desconocido";
				}
			}
		}

		/// <summary>
		///		Combo de ámbito de documento
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboDocumentScope { get; private set; }

		/// <summary>
		///		Combo para copiar imágenes
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboCopyImages { get; private set; }

		/// <summary>
		///		Arbol de etiquetas
		/// </summary>
		public Documents.TreeDocumentsViewModel TreeTags { get; private set; }

		/// <summary>
		///		Lista de páginas hija
		/// </summary>
		public Documents.PagesListViewModel ListChildPages { get; private set; }

		/// <summary>
		///		Plantillas
		/// </summary>
		public Documents.TemplateViewModel Templates
		{
			get { return _templates; }
			set { CheckObject(ref _templates, value); }
		}

		/// <summary>
		///		Indica si se deben mostrar los datos del documento
		/// </summary>
		public bool IsDocumentDataVisible
		{
			get
			{
				return File.FileType == FileModel.DocumentType.Document || File.FileType == FileModel.DocumentType.Template ||
					   File.FileType == FileModel.DocumentType.Tag || File.FileType == FileModel.DocumentType.SiteMap;
			}
		}

		/// <summary>
		///		Indica si se deben mostrar los datos de la página
		/// </summary>
		public bool IsPageDataVisible
		{
			get
			{
				return File.FileType == FileModel.DocumentType.Document;
			}
		}

		/// <summary>
		///		Indica si se deben mostrar los datos de la página o etiqueta
		/// </summary>
		public bool IsPageTagDataVisible
		{
			get
			{
				return File.FileType == FileModel.DocumentType.Document || File.FileType == FileModel.DocumentType.Tag;
			}
		}

		/// <summary>
		///		Indica si se deben mostrar los datos de la sección
		/// </summary>
		public bool IsSectionDataVisible
		{
			get { return File.FileType == FileModel.DocumentType.Section; }
		}

		/// <summary>
		///		Directorio seleccionado por el usuario la última vez
		/// </summary>
		public string LastDefaultPath
		{
			get
			{ 
				// Si no se ha asignado ningún nombre de último directorio, se coge el mismo directorio que el archivo
				if (_lastDefaultPath.IsEmpty() || !System.IO.Directory.Exists(_lastDefaultPath))
					_lastDefaultPath = File.Path;
				// Devuelve el último directorio seleccionado por el usuario
				return _lastDefaultPath;
			}
			set { _lastDefaultPath = value; }
		}

		/// <summary>
		///		Archivo
		/// </summary>
		protected FileModel File { get; private set; }

		/// <summary>
		///		Comando de pegar imagen
		/// </summary>
		public BaseCommand PasteImageCommand { get; }

		/// <summary>
		///		Comando de instrucción
		/// </summary>
		public BaseCommand InstructionCommand { get; }

		/// <summary>
		///		Comando de mantenimiento de instrucciones
		/// </summary>
		public BaseCommand InstructionsEditCommand { get; }

		/// <summary>
		///		ViewModel para el combo de instrucciones
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboInstructions { get; private set; }
	}
}
