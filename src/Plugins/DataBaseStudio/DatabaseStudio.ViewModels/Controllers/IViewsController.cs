using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Connections;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Queries;
using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Reports;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Controllers
{
	/// <summary>
	///		Interface para el controlador de host
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre el cuadro de diálogo para modificar los datos de una conexión
		/// </summary>
		bool OpenConnectionView(DataBaseConnectionViewModel viewModel);

		/// <summary>
		///		Abre el formulario para modificar los datos de una distribución
		/// </summary>
		void OpenDeploymentView(DeploymentViewModel viewModel);

		/// <summary>
		///		Abre la ventana de edición de un informe
		/// </summary>
		void OpenReportView(ReportViewModel viewModel);

		/// <summary>
		///		Abre la ventana de edición de una consulta
		/// </summary>
		void OpenQueryView(QueryViewModel viewModel);

		/// <summary>
		///		Abre la ventana de selección de un modo de distribución
		/// </summary>
		SystemControllerEnums.ResultType OpenSelectDeploymentView(DeploymentSelectViewModel viewModel);

		/// <summary>
		///		Obtiene el icono asociado a un tipo de elemento
		/// </summary>
		string GetIcon(ProjectExplorerViewModel.NodeType node);

		/// <summary>
		///		Obtiene el icono asociado a un menú
		/// </summary>
		string GetIcon(ProjectExplorerViewModel.MenuType menu);

		/// <summary>
		///		Obtiene el archivo de ayuda asociado a un tipo
		/// </summary>
		string GetHelpFileName(ProjectExplorerViewModel.NodeType node);

		/// <summary>
		///		Obtiene el archivo de plantilla asociado a un tipo
		/// </summary>
		string GetTemplate(ProjectExplorerViewModel.NodeType node);
	}
}
