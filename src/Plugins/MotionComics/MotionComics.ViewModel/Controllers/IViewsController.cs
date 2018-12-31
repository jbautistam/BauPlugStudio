using System;

namespace Bau.Libraries.MotionComics.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController 
	{
		/// <summary>
		///		Prepara el directorio de un nuevo proyecto: copia las plantillas y crea los directorios...
		/// </summary>
		void PrepareNewProject(string pathTarget);

		/// <summary>
		///		Abre el formulario XML
		/// </summary>
		void OpenFormXml(string title, string fileName, string projectPath, bool isComic);

		/// <summary>
		///		Abre el formulario 
		/// </summary>
		void OpenFormComic(string pathComic);

		/// <summary>
		///		Obtiene el nombre de las plantillas de un archivo de cómic
		/// </summary>
		string GetComicTemplateFileName();

		/// <summary>
		///		Obtiene el nombre de los recursos de un archivo de cómic
		/// </summary>
		string GetResourceTemplateFileName();

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		string IconFileProject { get; }

		/// <summary>
		///		Icono de un archivo XML
		/// </summary>
		string IconFileXml { get; }

		/// <summary>
		///		Icono de un archivo de cómic
		/// </summary>
		string IconFileComic { get; }

		/// <summary>
		///		Icono del menú de generación
		/// </summary>
		string IconMenuGenerate { get; }
	}
}
