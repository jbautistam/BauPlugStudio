using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.SourceEditor.Model.Plugins;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Controllers
{
	/// <summary>
	///		Manager para comunicar este plugin con SourceEditor
	/// </summary>
	internal class SourceEditorPluginManager : IPluginSourceEditor
	{ 
		// Constantes privadas
		internal const string ProjectType = "SourceCodeDocumenter";
		// Constantes con las acciones
		internal const string ActionCompile = "Compile";
		// Constantes con las extensiones
		internal const string ExtensionProject = "scprj";
		internal const string ExtensionSource = "scddcx";
		internal const string ExtensionSqlServer = "sqlserver";
		internal const string ExtensionOleDb = "oledb";
		internal const string ExtensionStructXml = "sct";
		internal const string ExtensionTemplate = "tpt";
		internal const string ExtensionPage = "docp";

		/// <summary>
		///		Inicializa el plugin sobre SourceEditor
		/// </summary>
		internal void InitPlugin(MessageRequestPlugin message)
		{ 
			// Inicializa las definiciones de proyecto
			InitProjectDefinitions();
			// Añade el plugin al mensaje
			message?.CreatePlugin(ExtensionProject, SourceCodeDocumenterViewModel.Instance.SourceEditorPluginManager);
		}

		/// <summary>
		///		Inicializa las definiciones del proyecto
		/// </summary>
		private void InitProjectDefinitions()
		{
			ProjectDefinitionModel definition = new ProjectDefinitionModel("Documentación de código fuente",
																		   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileProject,
																		   SourceCodeDocumenterViewModel.Instance.ModuleName,
																		   ProjectType, ExtensionProject);

				// Añade los diferentes tipos de archivos de documentación
				definition.FilesDefinition.Add(definition, "Código fuente",
											   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileProject,
											   ExtensionSource, false);
				definition.FilesDefinition.Add(definition, "Base datos SQL Server",
											   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileDataBase,
											   ExtensionSqlServer, false);
				definition.FilesDefinition.Add(definition, "Base datos OleDB",
											   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileDataBase,
											   ExtensionOleDb, false);
				definition.FilesDefinition.Add(definition, "XML de estructuras de documentación",
											   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileStructXml,
											   ExtensionStructXml, true);
				definition.FilesDefinition.Add(definition, "Plantilla de documentos",
											   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileStructXml,
											   ExtensionTemplate, true);
				definition.FilesDefinition.Add(definition, "Página de documentación",
											   SourceCodeDocumenterViewModel.Instance.ViewsController.IconFileStructXml,
											   ExtensionPage, true,
											   SourceCodeDocumenterViewModel.Instance.ViewsController.GetPageTemplateFileName(), "xml");
				// Añade las opciones de menú al archivo de proyecto
				definition.Menus.Add(MenuModel.MenuType.Command, ActionCompile, "_Generar",
									 SourceCodeDocumenterViewModel.Instance.ViewsController.IconMenuGenerate);
				// Asigna la definición de proyecto
				Definition = definition;
		}

		/// <summary>
		///		Abre un archivo
		/// </summary>
		public bool OpenFile(FileModel file, bool isNew)
		{
			bool isOpen = true;

				// Abre el archivo
				if (file is ProjectModel && file.FullFileName.EndsWith(ExtensionProject, StringComparison.CurrentCultureIgnoreCase))
				{ 
					// Copia las plantillas
					if (isNew)
						PrepareProject(file.SearchProject().PathBase);
					// Abre el formulario de proyectos
					SourceCodeDocumenterViewModel.Instance.ViewsController.OpenFormProject
															(new Documenter.DocumenterProjectViewModel(file.Title, file.FullFileName, file.SearchProject().PathBase));
				}
				else if (file.Extension.EqualsIgnoreCase(SourceEditorPluginManager.ExtensionSource))
					SourceCodeDocumenterViewModel.Instance.ViewsController.OpenFormSourceCode
															(new Documenter.SourceCodeViewModel(file.Title, file.FullFileName, file.SearchProject().PathBase));
				else if (file.Extension.EqualsIgnoreCase(SourceEditorPluginManager.ExtensionSqlServer))
					SourceCodeDocumenterViewModel.Instance.ViewsController.OpenFormSqlServer
															(new Documenter.SqlServerViewModel(file.Title, file.FullFileName, file.SearchProject().PathBase));
				else if (file.Extension.EqualsIgnoreCase(SourceEditorPluginManager.ExtensionOleDb))
					SourceCodeDocumenterViewModel.Instance.ViewsController.OpenFormOleDb
															(new Documenter.OleDbViewModel(file.Title, file.FullFileName, file.SearchProject().PathBase));
				else if (file.Extension.EqualsIgnoreCase(SourceEditorPluginManager.ExtensionStructXml))
					SourceCodeDocumenterViewModel.Instance.ViewsController.OpenFormXml(file.Title, file.FullFileName, file.SearchProject().PathBase);
				else if (file.Extension.EqualsIgnoreCase(SourceEditorPluginManager.ExtensionTemplate))
					SourceCodeDocumenterViewModel.Instance.ViewsController.OpenFormXml(file.Title, file.FullFileName, file.SearchProject().PathBase);
				else
					isOpen = false;
				// Devuelve el valor que indica si se ha abierto
				return isOpen;
		}

		/// <summary>
		///		Prepara el proyecto
		/// </summary>
		private void PrepareProject(string pathProject)
		{ 
			// Copia las plantillas
			SourceCodeDocumenterViewModel.Instance.ViewsController.CopyTemplates(pathProject);
			// Crea los directorios
			LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.Combine(pathProject, "Sources"));
			LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.Combine(pathProject, "Pages"));
			LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.Combine(pathProject, "Build"));
		}

		/// <summary>
		///		Ejecuta una acción sobre un archivo
		/// </summary>
		public bool ExecuteAction(FileModel file, MenuModel menu)
		{   
			// Ejecuta la acción
			if (menu.Key.EqualsIgnoreCase(SourceEditorPluginManager.ActionCompile))
				Compile(file);
			// Devuelve el valor que indica que se ha ejecutado
			return true;
		}

		/// <summary>
		///		Compila un archivo
		/// </summary>
		private void Compile(FileModel file)
		{
			if (SourceCodeDocumenterViewModel.Instance.CompilerFileName.IsEmpty())
				SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("No se ha configurado el nombre del ejecutable que realiza la documentación." +
																					" Seleccione Herramientas | Configuración para configurar el documentador");
			else if (!System.IO.File.Exists(SourceCodeDocumenterViewModel.Instance.CompilerFileName))
				SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el ejecutable que realiza la documentación." +
																					" Seleccione Herramientas | Configuración para configurar el documentador");
			else if (file == null)
				SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el proyecto que desea documentar");
			else if (!file.FileName.EndsWith(ExtensionProject, StringComparison.CurrentCultureIgnoreCase))
				SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("No se puede documentar este tipo de proyectos");
			else if (!System.IO.File.Exists(file.FullFileName))
				SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el archivo");
			else if (SourceCodeDocumenterViewModel.Instance.HostController.TasksProcessor.ExistsByType(typeof(CodeDocumentationProcessor)))
				SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage("Ya se está documentando un proyecto");
			else
			{
				CodeDocumentationProcessor processor = new CodeDocumentationProcessor(SourceCodeDocumenterViewModel.Instance.ModuleName,
																					  file.FullFileName, SourceCodeDocumenterViewModel.Instance.CompilerFileName);

				// Asigna el manejador de eventos
				processor.EndProcess += (sender, evntArgs) =>
												{
													if (evntArgs.Errors != null && evntArgs.Errors.Count > 0)
													{
														string message = "Errores en la generación de la documentación";

															// Añade los errores
															foreach (string error in evntArgs.Errors)
																message = message.AddWithSeparator(error, Environment.NewLine);
															// Muestra el error
															SourceCodeDocumenterViewModel.Instance.ControllerWindow.ShowMessage(message);
													}
												};
				// Añade el procesador a la cola
				SourceCodeDocumenterViewModel.Instance.HostController.TasksProcessor.Process(processor);
			}
		}

		/// <summary>
		///		Carga los elementos hijo de un archivo --> En este caso no hace nada, simplemente implementa la interface
		/// </summary>
		public OwnerChildModelCollection LoadOwnerChilds(FileModel file, OwnerChildModel parent)
		{
			return null;
		}

		/// <summary>
		///		Cambia el nombre de un archivo --> En este caso no hace nada, simplemente implementa la interface
		/// </summary>
		public bool Rename(FileModel file, string newFileName, string title)
		{
			return true;
		}

		/// <summary>
		///		Definición de proyectos, archivos y menús
		/// </summary>
		public ProjectDefinitionModel Definition { get; private set; }
	}
}
