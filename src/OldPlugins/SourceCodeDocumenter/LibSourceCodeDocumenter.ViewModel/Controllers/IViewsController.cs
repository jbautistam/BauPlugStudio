using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter;

namespace Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController 
	{
		/// <summary>
		///		Copia las plantillas al directorio
		/// </summary>
		void CopyTemplates(string pathTarget);

		/// <summary>
		///		Abre el formulario de propiedades de un proyecto
		/// </summary>
		SystemControllerEnums.ResultType OpenFormProject(DocumenterProjectViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un documento
		/// </summary>
		SystemControllerEnums.ResultType OpenFormSourceCode(SourceCodeViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un archivo de SQLServer
		/// </summary>
		SystemControllerEnums.ResultType OpenFormSqlServer(SqlServerViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un archivo de OleDb
		/// </summary>
		SystemControllerEnums.ResultType OpenFormOleDb(OleDbViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un archivo XML
		/// </summary>
		void OpenFormXml(string title, string fileName, string projectPath);

		/// <summary>
		///		Obtiene el nombre de las plantillas de las páginas de documentación
		/// </summary>
		string GetPageTemplateFileName();

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		string IconFileProject { get; }

		/// <summary>
		///		Icono de un archivo de base de datos
		/// </summary>
		string IconFileDataBase { get; }

		/// <summary>
		///		Icono de un archivo XML de estructuras
		/// </summary>
		string IconFileStructXml { get; }

		/// <summary>
		///		Icono de un archivo XML de plantillas
		/// </summary>
		string IconFileTemplate { get; }

		/// <summary>
		///		Icono del menú de generación
		/// </summary>
		string IconMenuGenerate { get; }
	}
}