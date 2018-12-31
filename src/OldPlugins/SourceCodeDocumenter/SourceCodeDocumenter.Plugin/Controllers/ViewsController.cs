using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.SourceCodeDocumenter.Controllers
{
	/// <summary>
	///		Controlador de ventanas de SourceCodeDocumenter
	/// </summary>
	public class ViewsController : Libraries.LibSourceCodeDocumenter.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Copia las plantillas al directorio destino
		/// </summary>
		public void CopyTemplates(string pathTarget)
		{
			try
			{
				Libraries.LibCommonHelper.Files.HelperFiles.CopyPath(SourceCodeDocumenterPlugin.MainInstance.PathTemplates,
																	 System.IO.Path.Combine(pathTarget, "Templates"));
			}
			catch (Exception exception)
			{
				SourceCodeDocumenterPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("Error al copiar las plantillas: " + exception.Message);
			}
		}

		/// <summary>
		///		Abre el formulario de propiedades de un proyecto
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormProject(DocumenterProjectViewModel viewModel)
		{
			return SourceCodeDocumenterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Documenter.ProjectFileView(viewModel));
		}


		/// <summary>
		///		Abre el formulario de modificación de un archivo de código fuente
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormSourceCode(SourceCodeViewModel viewModel)
		{
			return SourceCodeDocumenterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Documenter.SourceCodeView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de un archivo de SQLServer
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormSqlServer(SqlServerViewModel viewModel)
		{
			return SourceCodeDocumenterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Documenter.SqlServerView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de un archivo de OleDb
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormOleDb(OleDbViewModel viewModel)
		{
			return SourceCodeDocumenterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Documenter.OleDbView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de un de estructuras XML
		/// </summary>
		public void OpenFormXml(string title, string fileName, string projectPath)
		{
			SourceCodeDocumenterPlugin.MainInstance.HostPluginsController.ShowCodeEditor
									  (fileName, GetFullFileNameTemplate("StructDataTemplate.xml"), LayoutEnums.Editor.Xml);
		}

		/// <summary>
		///		Obtiene el nombre completo de un archivo de plantilla
		/// </summary>
		private string GetFullFileNameTemplate(string template)
		{
			return System.IO.Path.Combine(SourceCodeDocumenterPlugin.MainInstance.PathPlugin, "Data", template);
		}

		/// <summary>
		///		Obtiene el nombre de la plantilla de las páginas de documentación
		/// </summary>
		public string GetPageTemplateFileName()
		{
			return GetFullFileNameTemplate("PageTemplate.xml");
		}

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		public string IconFileProject
		{
			get { return "pack://application:,,,/SourceCodeDocumenter.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono de una definición de base de datos
		/// </summary>
		public string IconFileDataBase
		{
			get { return "pack://application:,,,/SourceCodeDocumenter.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono de una structura XML
		/// </summary>
		public string IconFileStructXml
		{
			get { return "pack://application:,,,/SourceCodeDocumenter.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono de un archivo de plantillas
		/// </summary>
		public string IconFileTemplate
		{
			get { return "pack://application:,,,/SourceCodeDocumenter.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono del menú de generación
		/// </summary>
		public string IconMenuGenerate
		{
			get { return "pack://application:,,,/SourceCodeDocumenter.Plugin;component/Themes/Images/Process.png"; }
		}
	}
}
