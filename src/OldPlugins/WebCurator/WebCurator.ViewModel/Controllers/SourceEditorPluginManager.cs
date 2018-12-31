using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.SourceEditor.Model.Plugins;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.WebCurator.Model.Sentences;
using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.Controllers
{
	/// <summary>
	///		Manager para comunicar este plugin con SourceEditor
	/// </summary>
	internal class SourceEditorPluginManager : IPluginSourceEditor
	{ 
		// Constantes privadas
		private const string ProjectType = "WebCurator";
		private const string ActionCompile = "Compile_WebCurator";
		private const string ExtensionProject = "wcurprj";

		/// <summary>
		///		Inicializa el plugin sobre SourceEditor
		/// </summary>
		internal void InitPlugin(MessageRequestPlugin message)
		{ 
			// Inicializa las definicioens de proyecto
			InitProjectDefinitions();
			// Añade el plugin al mensaje
			message?.CreatePlugin(ExtensionProject, this);
		}

		/// <summary>
		///		Inicializa las definiciones del proyecto
		/// </summary>
		private void InitProjectDefinitions()
		{
			ProjectDefinitionModel definition = new ProjectDefinitionModel("WebCurator de sitios Web",
																		   WebCuratorViewModel.Instance.ViewsController.IconFileProject,
																		   WebCuratorViewModel.Instance.ModuleName,
																		   ProjectType, ExtensionProject);

				// Añade los archivos que no se deben mostrar
				definition.FilesDefinition.AddHidden(definition, GenerationResultModel.Extension);
				definition.FilesDefinition.AddHidden(definition, GenerationResultProjectModel.Extension);
				// Añade los archivos
				definition.FilesDefinition.Add(definition, "Sitio Web",
											   WebCuratorViewModel.Instance.ViewsController.IconFileWebSite,
											   Model.WebSites.ProjectModel.Extension, false).
							Menus.Add(MenuModel.MenuType.Command, ActionCompile, "_Generar",
									  WebCuratorViewModel.Instance.ViewsController.IconMenuGenerate);
				definition.FilesDefinition.Add(definition, "Sentencias",
											   WebCuratorViewModel.Instance.ViewsController.IconFileSentence,
											   FileSentencesModel.Extension, false);
				// Asigna la definición de proyecto
				Definition = definition;
		}

		/// <summary>
		///		Abre un archivo
		/// </summary>
		public bool OpenFile(FileModel file, bool isNew)
		{
			bool isOpen = false;

				// Abre el archivo
				if (file.Extension.EqualsIgnoreCase(Model.WebSites.ProjectModel.Extension))
				{
					Model.WebSites.ProjectModel project = new Application.Bussiness.WebSites.ProjectBussiness().Load(file.FullFileName);

						// Abre el archivo
						WebCuratorViewModel.Instance.ViewsController.OpenFormUpdateFile(new WebSites.ProjectViewModel(project));
						// Indica que se ha abierto correctamente
						isOpen = true;
				}
				// Devuelve el valor que indica si se ha abierto
				return isOpen;
		}

		/// <summary>
		///		Ejecuta una acción sobre un archivo --> En este caso simplemente implementa la interface
		/// </summary>
		public bool ExecuteAction(FileModel file, MenuModel menu)
		{   
			// Ejecuta los comandos
			if (menu.Key.EqualsIgnoreCase(ActionCompile))
				Compile(file);
			// Indica que se ha ejecutado correctamente
			return true;
		}

		/// <summary>
		///		Compila un archivo
		/// </summary>
		private void Compile(FileModel file)
		{
			if (file == null)
				WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el sitio web que desea compilar");
			else if (file.FileName.IsEmpty() || !System.IO.File.Exists(file.FullFileName))
				WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("No se encuentra el nombre de archivo");
			else
			{
				Model.WebSites.ProjectModel project = new Application.Bussiness.WebSites.ProjectBussiness().Load(file.FullFileName);

					if (project == null || project.Name.IsEmpty())
						WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("No ha seleccionado ningún proyecto");
					else if (WebCuratorViewModel.Instance.HostController.TasksProcessor.ExistsByType(typeof(Controllers.FullProcessor)))
						WebCuratorViewModel.Instance.ControllerWindow.ShowMessage("Ya se está compilando un proyecto");
					else
						WebCuratorViewModel.Instance.HostController.TasksProcessor.Process(new FullProcessor(WebCuratorViewModel.Instance.ModuleName, project, false));
			}
		}

		/// <summary>
		///		Carga los elementos hijo de un archivo --> No hace nada, simplemente implementa la interface
		/// </summary>
		public OwnerChildModelCollection LoadOwnerChilds(FileModel file, OwnerChildModel parent)
		{
			return new OwnerChildModelCollection();
		}

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		public bool Rename(FileModel file, string newFileName, string title)
		{ 
			// Cambia el nombre al proyecto
			if (!file.IsFolder && System.IO.File.Exists(newFileName))
			{
				if (file.Extension.EqualsIgnoreCase(Model.WebSites.ProjectModel.Extension))
				{
					Model.WebSites.ProjectModel project = new Application.Bussiness.WebSites.ProjectBussiness().Load(newFileName);

					// Cambia el título
					project.Name = title;
					// Y lo graba
					new Application.Bussiness.WebSites.ProjectBussiness().Save(project);
				}
			}
			// Devuelve el valor que indica que ha cambiado el nombre
			return true;
		}

		/// <summary>
		///		Definición de proyectos, archivos y menús
		/// </summary>
		public ProjectDefinitionModel Definition { get; private set; }
	}
}

