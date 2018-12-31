using System;

using Bau.Libraries.LibDataBaseStudio.ViewModel.Connections;
using Bau.Libraries.LibDataBaseStudio.ViewModel.Reports;
using Bau.Libraries.LibDataBaseStudio.ViewModel.Queries;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Plugins.DataBaseStudio.Controllers
{
	/// <summary>
	///		Controlador de ventanas de DataBaseStudio
	/// </summary>
	public class ViewsController : Libraries.LibDataBaseStudio.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre el formulario de modificación de una conexión
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormUpdateConnection(ConnectionViewModel viewModel)
		{
			return DataBaseStudioPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Connections.ConnectionView(viewModel));
		}

		/// <summary>
		///		Abre el fomulario de modificación de un informe
		/// </summary>
		public void OpenFormUpdateReport(ReportViewModel viewModel)
		{
			DataBaseStudioPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("REPORT_" + viewModel.FileName,
																								  viewModel.Name, new Views.Reports.ReportView(viewModel));
		}

		/// <summary>
		///		Abre el cuadro de diálogo que modifica los parámetros
		/// </summary>
		public bool OpenFormUpdateReportExecutionParameter(ReportExecutionViewModel viewModel)
		{
			return DataBaseStudioPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog
											  (new Views.Reports.ReportExecutionView(viewModel)) == SystemControllerEnums.ResultType.Yes;
		}

		/// <summary>
		///		Abre el cuadro de diálogo que modifica un archivo
		/// </summary>
		public bool OpenFormUpdateReportExecutionFile(ReportExecutionFileViewModel viewModel)
		{
			return DataBaseStudioPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog
												  (new Views.Reports.ReportExecutionFileView(viewModel)) == SystemControllerEnums.ResultType.Yes;
		}

		/// <summary>
		///		Abre el formulario de generación de un informe
		/// </summary>
		public void OpenFormReportGenerate(ReportGenerateViewModel viewModel)
		{
			DataBaseStudioPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog
					  (new Views.Reports.ReportGenerateView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de una tabla pivot
		/// </summary>
		public void OpenFormUpdateQuery(QueryViewModel viewModel)
		{
			DataBaseStudioPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("QUERY_" + viewModel.FileName,
																								  viewModel.Name, new Views.Queries.QueryView(viewModel));
		}

		/// <summary>
		///		Icono del archivo de conexión
		/// </summary>
		public string IconFileConnection
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Connection.png"; }
		}

		/// <summary>
		///		Icono del archivo de documentos
		/// </summary>
		public string IconFileDocument
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Document.png"; }
		}

		/// <summary>
		///		Icono del menú de generación
		/// </summary>
		public string IconMenuGenerate
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Process.png"; }
		}

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		public string IconFileProject
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono del nodo raíz de tablas
		/// </summary>
		public string IconFileTablesRoot
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/TablesRoot.png"; }
		}

		/// <summary>
		///		Icono del nodo de tabla
		/// </summary>
		public string IconFileTable
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Table.png"; }
		}

		/// <summary>
		///		Icono del nodo de columna
		/// </summary>
		public string IconFileColumn
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Column.png"; }
		}

		/// <summary>
		///		Icono del nodo raíz de vistas
		/// </summary>
		public string IconFileViewsRoot
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/ViewsRoot.png"; }
		}

		/// <summary>
		///		Icono del nodo de vista
		/// </summary>
		public string IconFileView
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/View.png"; }
		}

		/// <summary>
		///		Icono del nodo de rutinas
		/// </summary>
		public string IconFileRoutines
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Routines.png"; }
		}

		/// <summary>
		///		Icono del nodo raíz de procedimientos almacenados
		/// </summary>
		public string IconFileStoredProceduresRoot
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/StoredProceduresRoot.png"; }
		}

		/// <summary>
		///		Icono del nodo de procedimiento almacenado
		/// </summary>
		public string IconFileStoredProcedure
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/StoredProcedure.png"; }
		}

		/// <summary>
		///		Icono del nodo raíz de funciones
		/// </summary>
		public string IconFileFunctionsRoot
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/FunctionsRoot.png"; }
		}

		/// <summary>
		///		Icono del nodo de función
		/// </summary>
		public string IconFileFunction
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Function.png"; }
		}

		/// <summary>
		///		Icono del nodo de informes
		/// </summary>
		public string IconFileReport
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Report.png"; }
		}

		/// <summary>
		///		Icono del nodo de consultas
		/// </summary>
		public string IconFileQuery
		{
			get { return "pack://application:,,,/DataBaseStudio.Plugin;component/Themes/Images/Query.png"; }
		}
	}
}
