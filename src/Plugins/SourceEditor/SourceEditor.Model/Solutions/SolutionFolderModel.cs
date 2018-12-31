using System;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Carpeta de una solución
	/// </summary>
	public class SolutionFolderModel : LibDataStructures.Base.BaseExtendedModel
	{
		public SolutionFolderModel(SolutionModel solution)
		{
			Solution = solution;
			Folders = new SolutionFolderModelCollection();
			Projects = new ProjectsModelCollection(solution);
		}

		/// <summary>
		///		Clona una carpeta de solución
		/// </summary>
		public SolutionFolderModel Clone()
		{
			SolutionFolderModel folder = new SolutionFolderModel(Solution);

				// Copia los datos
				folder.Name = Name;
				folder.Description = Description;
				folder.Folders.AddRange(Folders.Clone());
				folder.Projects.AddRange(Projects.Clone());
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Obtiene los proyectos de esta carpeta
		/// </summary>
		internal ProjectsModelCollection GetAllProjects()
		{
			ProjectsModelCollection projects = new ProjectsModelCollection(Solution);

				// Añade los proyectos de la carpeta
				projects.AddRange(Projects);
				// Añade todos los proyectos
				projects.AddRange(Folders.GetAllProjects(Solution));
				// Devuelve la colección de proyectos
				return projects;
		}

		/// <summary>
		///		Solución a la que se asocia la carpeta
		/// </summary>
		public SolutionModel Solution { get; }

		/// <summary>
		///		Carpetas hija de esta carpeta
		/// </summary>
		public SolutionFolderModelCollection Folders { get; }

		/// <summary>
		///		Proyectos hijos de esta carpeta
		/// </summary>
		public ProjectsModelCollection Projects { get; }
	}
}
