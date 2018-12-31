using System;

using Bau.Libraries.Plugins.Views.Host;
using Bau.Libraries.DatabaseStudio.ViewModels.Controllers;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Reports;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Queries;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Controllers
{
	/// <summary>
	///		Controlador de vistas
	/// </summary>
	internal class ViewsController : IViewsController
	{
		// Variables privadas
		private string _pathPlugin = string.Empty;

		public ViewsController(IHostPluginsController hostController)
		{
			HostPluginsController = hostController;
		}

		/// <summary>
		///		Abre la vista de conexiones
		/// </summary>
		public bool OpenConnectionView(DatabaseStudio.ViewModels.Projects.Connections.DataBaseConnectionViewModel viewModel)
		{
			return HostPluginsController.HostViewsController.ShowDialog(new Views.Connections.DataBaseConnectionView(viewModel)) == SystemControllerEnums.ResultType.Yes;
		}

		/// <summary>
		///		Abre la ventana de edición de un informe
		/// </summary>
		public void OpenReportView(ReportViewModel viewModel)
		{
			HostPluginsController.LayoutController.ShowDocument("REPORT_" + viewModel.FileName, viewModel.Name, 
																new Views.Reports.ReportView(viewModel, GetHelpFileName(ProjectExplorerViewModel.NodeType.Report)));
		}

		/// <summary>
		///		Abre la ventana de edición de una consulta
		/// </summary>
		public void OpenQueryView(QueryViewModel viewModel)
		{
			HostPluginsController.LayoutController.ShowDocument("Query_" + viewModel.FileName, viewModel.Name, 
																new Views.Queries.QueryView(viewModel));
		}

		/// <summary>
		///		Abre la ventana de edición de un elemento de distribución
		/// </summary>
		public void OpenDeploymentView(DeploymentViewModel viewModel)
		{
			HostPluginsController.LayoutController.ShowDocument("Deployment_" + viewModel.Deployment.GlobalId, viewModel.Name,
																new Views.Deployments.DeploymentView(viewModel));
		}

		/// <summary>
		///		Abre la ventana de selección de un modo de distribución
		/// </summary>
		public SystemControllerEnums.ResultType OpenSelectDeploymentView(DeploymentSelectViewModel viewModel)
		{
			return HostPluginsController.HostViewsController.ShowDialog(new Views.Deployments.DeploymentSelectView(viewModel));
		}

		/// <summary>
		///		Obtiene el archivo de ayuda asociado a un tipo
		/// </summary>
		public string GetHelpFileName(ProjectExplorerViewModel.NodeType node)
		{
			return GetDataFileName(node, false);
		}

		/// <summary>
		///		Obtiene el archivo de plantilla asociado a un tipo
		/// </summary>
		public string GetTemplate(ProjectExplorerViewModel.NodeType node)
		{
			return GetDataFileName(node, true);
		}

		/// <summary>
		///		Obtiene el nombre completo de un archivo de ayuda o plantilla
		/// </summary>
		private string GetDataFileName(ProjectExplorerViewModel.NodeType node, bool isTemplate)
		{
			string path = System.IO.Path.Combine(PathBase, "Data");

				// Añade el directorio
				if (isTemplate)
					path = System.IO.Path.Combine(path, "Template");
				else
					path = System.IO.Path.Combine(path, "Help");
				// Obtiene el nombre de archivo
				path = System.IO.Path.Combine(path, node.ToString() + ".xml");
				// Devuelve el nombre de archivo (si existe)
				if (System.IO.File.Exists(path))
					return path;
				else
					return string.Empty;
		}

		/// <summary>
		///		Obtiene el icono asociado a un tipo de elemento
		/// </summary>
		public string GetIcon(ProjectExplorerViewModel.NodeType node)
		{
			switch (node)
			{
				case ProjectExplorerViewModel.NodeType.Project:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Project.png";
				case ProjectExplorerViewModel.NodeType.ConnectionsRoot:
				case ProjectExplorerViewModel.NodeType.Folder:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Folder.png";
				case ProjectExplorerViewModel.NodeType.Connection:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Connection.png";
				case ProjectExplorerViewModel.NodeType.Deployment:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Deployment.png";
				case ProjectExplorerViewModel.NodeType.TablesRoot:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/TablesRoot.png";
				case ProjectExplorerViewModel.NodeType.Table:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Table.png";
				case ProjectExplorerViewModel.NodeType.ViewsRoot:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/ViewsRoot.png";
				case ProjectExplorerViewModel.NodeType.View:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/View.png";
				case ProjectExplorerViewModel.NodeType.Field:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/File.png";
				case ProjectExplorerViewModel.NodeType.ProceduresRoot:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/StoredProceduresRoot.png";
				case ProjectExplorerViewModel.NodeType.Procedure:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/StoredProcedure.png";
				case ProjectExplorerViewModel.NodeType.Style:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/File.png";
				case ProjectExplorerViewModel.NodeType.ImportScript:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Process.png";
				case ProjectExplorerViewModel.NodeType.Query:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Query.png";
				case ProjectExplorerViewModel.NodeType.Report:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Report.png";
				case ProjectExplorerViewModel.NodeType.Image:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/Image.png";
				default:
					return "/FullDatabaseStudio.Plugin;component/Themes/Images/File.png";
			}
		}

		/// <summary>
		///		Obtiene el icono asociado a un menú
		/// </summary>
		public string GetIcon(ProjectExplorerViewModel.MenuType menu)
		{
			switch (menu)
			{
				case ProjectExplorerViewModel.MenuType.NewProject:
					return "pack://application:,,,/FullDatabaseStudio.Plugin;component/Themes/Images/Project.png";
				case ProjectExplorerViewModel.MenuType.NewConnection:
					return "pack://application:,,,/FullDatabaseStudio.Plugin;component/Themes/Images/Connection.png";
				case ProjectExplorerViewModel.MenuType.NewDeployment:
					return "pack://application:,,,/FullDatabaseStudio.Plugin;component/Themes/Images/Deployment.png";
				case ProjectExplorerViewModel.MenuType.Send:
				case ProjectExplorerViewModel.MenuType.Process:
					return "pack://application:,,,/FullDatabaseStudio.Plugin;component/Themes/Images/Process.png";
				default:
					return string.Empty;
			}
		}

		/// <summary>
		///		Controlador de plugins
		/// </summary>
		public IHostPluginsController HostPluginsController { get; }

		/// <summary>
		///		Controlador de ventanas
		/// </summary>
		public IHostSystemController WindowsController 
		{ 
			get { return HostPluginsController.ControllerWindow; }
		}

		/// <summary>
		///		Directorio base de la aplicación
		/// </summary>
		public string PathBase 
		{	
			get
			{
				// Obtiene el path del plugin si no estaba en memoria
				if (string.IsNullOrEmpty(_pathPlugin))
				{
					UriBuilder uriBuilder;

						// Obtiene el path del ensamblado
						_pathPlugin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
						// Lo trata como una URL porque viene como file://c:/xxxx
						uriBuilder = new UriBuilder(_pathPlugin);
						// Cambia el formato
						_pathPlugin = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uriBuilder.Path));
				}
				// Devuelve el directorio
				return _pathPlugin;
			}
		}
	}
}