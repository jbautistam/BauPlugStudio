using System;

using Bau.Libraries.DatabaseStudio.Models;

namespace Bau.Libraries.DatabaseStudio.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="ProjectModel"/>
	/// </summary>
    public class ProjectBussiness
    {
		// Constantes públicas
		public const string ProjectFileName = "DBStudio.dperj";

		/// <summary>
		///		Carga los datos de un proyecto de un directorio
		/// </summary>
		public ProjectModel Load(string path)
		{
			return new Repository.ProjectRepository().Load(GetProjectFileName(path));
		}

		/// <summary>
		///		Graba un proyecto
		/// </summary>
		public void Save(ProjectModel project, string path)
		{
			new Repository.ProjectRepository().Save(project, GetProjectFileName(path));
		}

		/// <summary>
		///		Obtiene el nombre de un archivo de proyecto
		/// </summary>
		public string GetProjectFileName(string path)
		{
			return System.IO.Path.Combine(path, ProjectFileName);
		}
    }
}
