using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.MotionComics.Controllers
{
	/// <summary>
	///		Controlador de ventanas de MotionComics
	/// </summary>
	public class ViewsController : Libraries.MotionComics.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Prepara el directorio de un nuevo proyecto: copia las plantillas y crea los directorios...
		/// </summary>
		public void PrepareNewProject(string pathTarget)
		{
			string comicTemplate = GetComicTemplateFileName();

				// Copia la plantilla del archivo de cómic
				if (System.IO.File.Exists(comicTemplate))
					try
					{
						Libraries.LibCommonHelper.Files.HelperFiles.CopyFile(comicTemplate,
																	   System.IO.Path.Combine(pathTarget,
																							  System.IO.Path.GetFileName(pathTarget) + ".cml"));
					}
					catch (Exception exception)
					{
						MotionComicsPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage($"Error al copiar el archivo principal: {exception.Message}");
					}
				// Crea los directorios adicionales
				Libraries.LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.Combine(pathTarget, "Images"));
		}

		/// <summary>
		///		Abre el formulario de modificación de estructuras XML
		/// </summary>
		public void OpenFormXml(string title, string fileName, string projectPath, bool isComic)
		{
			string nameTemplate = GetComicTemplateFileName();
			string helpName = GetHelpFileName();

				// Si no es un archivo de cómic, se abre un archivo de recursos
				if (!isComic)
					nameTemplate = GetResourceTemplateFileName();
				// Muestra el editor de código
				MotionComicsPlugin.MainInstance.HostPluginsController.ShowCodeEditor
															(fileName, nameTemplate,
															 LayoutEnums.Editor.Xml, helpName);
		}

		/// <summary>
		///		Abre el formulario de cómics
		/// </summary>
		public void OpenFormComic(string pathComic)
		{
			MotionComicsPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument(pathComic, System.IO.Path.GetFileName(pathComic),
																								new Views.Comic.ComicView(pathComic));
		}

		/// <summary>
		///		Obtiene el nombre de un archivo de ayuda
		/// </summary>
		private string GetHelpFileName()
		{
			return System.IO.Path.Combine(MotionComicsPlugin.MainInstance.PathPlugin, "Data\\Help\\Help.xml");
		}

		/// <summary>
		///		Obtiene el nombre completo de un archivo de plantilla
		/// </summary>
		private string GetFullFileNameTemplate(string template)
		{
			return System.IO.Path.Combine(MotionComicsPlugin.MainInstance.PathPlugin, "Data\\Templates", template);
		}

		/// <summary>
		///		Obtiene el nombre de la plantilla de un archivo de cómic
		/// </summary>
		public string GetComicTemplateFileName()
		{
			return GetFullFileNameTemplate("ComicTemplate.xml");
		}

		/// <summary>
		///		Obtiene el nombre de los recursos de un archivo de cómic
		/// </summary>
		public string GetResourceTemplateFileName()
		{
			return GetFullFileNameTemplate("ResourceTemplate.xml");
		}

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		public string IconFileProject
		{
			get { return "pack://application:,,,/MotionComics.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono de un archivo XML
		/// </summary>
		public string IconFileXml
		{
			get { return "pack://application:,,,/MotionComics.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono de un archivo de cómic
		/// </summary>
		public string IconFileComic
		{
			get { return "pack://application:,,,/MotionComics.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono del menú de generación
		/// </summary>
		public string IconMenuGenerate
		{
			get { return "pack://application:,,,/MotionComics.Plugin;component/Themes/Images/Process.png"; }
		}
	}
}