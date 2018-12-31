using System;

using Bau.Libraries.LibNSharpDoc.Projects.Models;

namespace Bau.Libraries.LibNSharpDoc.Projects
{
	/// <summary>
	///		Manager para la documentación de proyectos
	/// </summary>
	public class ProjectDocumentationManager
	{
		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		public ProjectDocumentationModel Load(string fileName)
		{
			return new Repository.ProjectRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		public void Save(ProjectDocumentationModel project, string fileName)
		{
			new Repository.ProjectRepository().Save(fileName, project);
		}
	}
}
