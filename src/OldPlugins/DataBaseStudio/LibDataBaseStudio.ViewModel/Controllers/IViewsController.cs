using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre el formulario de modificación de conexión
		/// </summary>
		SystemControllerEnums.ResultType OpenFormUpdateConnection(Connections.ConnectionViewModel viewModel);

		/// <summary>
		///		Abre el fomulario de modificación de un informe
		/// </summary>
		void OpenFormUpdateReport(Reports.ReportViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un parámetro de ejecución de un informe
		/// </summary>
		bool OpenFormUpdateReportExecutionParameter(Reports.ReportExecutionViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un archivo
		/// </summary>
		bool OpenFormUpdateReportExecutionFile(Reports.ReportExecutionFileViewModel viewModel);

		/// <summary>
		///		Abre el formulario de generación de un informe
		/// </summary>
		void OpenFormReportGenerate(Reports.ReportGenerateViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de una consulta
		/// </summary>
		void OpenFormUpdateQuery(Queries.QueryViewModel viewModel);

		/// <summary>
		///		Icono del archivo de conexión
		/// </summary>
		string IconFileConnection { get; }

		/// <summary>
		///		Icono de documentación de archivo
		/// </summary>
		string IconFileDocument { get; }

		/// <summary>
		///		Icono del menú generar
		/// </summary>
		string IconMenuGenerate { get; }

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		string IconFileProject { get; }

		/// <summary>
		///		Icono del nodo raíz de tablas
		/// </summary>
		string IconFileTablesRoot { get; }

		/// <summary>
		///		Icono del nodo de tabla
		/// </summary>
		string IconFileTable { get; }

		/// <summary>
		///		Icono del nodo de columna
		/// </summary>
		string IconFileColumn { get; }

		/// <summary>
		///		Icono del nodo raíz de vistas
		/// </summary>
		string IconFileViewsRoot { get; }

		/// <summary>
		///		Icono del nodo de vista
		/// </summary>
		string IconFileView { get; }

		/// <summary>
		///		Icono del nodo de rutinas
		/// </summary>
		string IconFileRoutines { get; }

		/// <summary>
		///		Icono del nodo raíz de procedimientos almacenados
		/// </summary>
		string IconFileStoredProceduresRoot { get; }

		/// <summary>
		///		Icono del nodo de procedimiento almacenado
		/// </summary>
		string IconFileStoredProcedure { get; }

		/// <summary>
		///		Icono del nodo raíz de funciones
		/// </summary>
		string IconFileFunctionsRoot { get; }

		/// <summary>
		///		Icono del nodo de función
		/// </summary>
		string IconFileFunction { get; }

		/// <summary>
		///		Icono del nodo de informes
		/// </summary>
		string IconFileReport { get; }

		/// <summary>
		///		Icono del nodo de consultas
		/// </summary>
		string IconFileQuery { get; }
	}
}
