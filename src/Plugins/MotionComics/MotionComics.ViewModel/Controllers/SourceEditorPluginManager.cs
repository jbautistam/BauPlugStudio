using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.SourceEditor.Model.Plugins;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.MotionComics.ViewModel.Controllers
{
	/// <summary>
	///		Manager para comunicar este plugin con SourceEditor
	/// </summary>
	internal class SourceEditorPluginManager : IPluginSourceEditor
	{
		// Constantes privadas
		private const string ProjectType = "MotionComicsProject";
		// Constantes con las acciones
		private const string ActionOpenComic = "ShowComic";
		// Constantes con las extensiones
		private const string ExtensionProject = "cmprj";
		private const string ExtensionComic = "cml";
		private const string ExtensionXml = "xml";

		/// <summary>
		///		Inicializa el plugin sobre SourceEditor
		/// </summary>
		internal void InitPlugin(MessageRequestPlugin message)
		{ 
			// Inicializa las definiciones de proyecto
			InitProjectDefinitions();
			// Añade el plugin al mensaje
			message?.CreatePlugin(ExtensionProject, this);
		}

		/// <summary>
		///		Inicializa las definiciones del proyecto
		/// </summary>
		private void InitProjectDefinitions()
		{
			ProjectDefinitionModel definition = new ProjectDefinitionModel("MotionComics",
																		   MotionComicsViewModel.Instance.ViewsController.IconFileProject,
																		   MotionComicsViewModel.Instance.ModuleName,
																		   ProjectType, ExtensionProject);

				// Añade los diferentes tipos de archivos de documentación
				definition.FilesDefinition.Add(definition, "Archivo de cómic",
												  MotionComicsViewModel.Instance.ViewsController.IconFileComic,
												  ExtensionComic, true,
												  MotionComicsViewModel.Instance.ViewsController.GetComicTemplateFileName());
				definition.FilesDefinition.Add(definition, "Archivo XML de recursos",
												  MotionComicsViewModel.Instance.ViewsController.IconFileXml,
												  ExtensionXml, true,
												  MotionComicsViewModel.Instance.ViewsController.GetComicTemplateFileName());
				// Añade las opciones de menú al archivo de proyecto
				definition.Menus.Add(MenuModel.MenuType.Command, ActionOpenComic, "A_brir cómic",
										MotionComicsViewModel.Instance.ViewsController.IconMenuGenerate);
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
					// Prepara el directorio para un nuevo proyecto
					if (isNew)
						MotionComicsViewModel.Instance.ViewsController.PrepareNewProject(file.SearchProject().PathBase);
				}
				else if (file.Extension.EqualsIgnoreCase(ExtensionXml) ||
								 file.Extension.EqualsIgnoreCase(ExtensionComic))
					MotionComicsViewModel.Instance.ViewsController.OpenFormXml(file.Title, file.FullFileName, file.SearchProject().PathBase,
																			   file.Extension.EqualsIgnoreCase(ExtensionComic));
				else
					isOpen = false;
				// Devuelve el valor que indica si se ha abierto
				return isOpen;
		}

		/// <summary>
		///		Ejecuta una acción sobre un archivo --> En este caso simplemente implementa la interface
		/// </summary>
		public bool ExecuteAction(FileModel file, MenuModel menu)
		{
			// Ejecuta los comandos
			if (menu.Key.EqualsIgnoreCase(ActionOpenComic))
				MotionComicsViewModel.Instance.ViewsController.OpenFormComic(file.SearchProject().PathBase);
			// Indica que se ha ejecutado correctamente
			return true;
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
