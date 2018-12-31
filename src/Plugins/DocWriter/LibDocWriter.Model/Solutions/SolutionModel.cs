using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Solutions
{
	/// <summary>
	///		Modelo para una solución
	/// </summary>
	public class SolutionModel : Base.BaseDocWriterFileModel
	{
		public SolutionModel()
		{
			Projects = new ProjectsModelCollection(this);
		}

		/// <summary>
		///		Busca un proyeto por nombre de archivo
		/// </summary>
		public ProjectModel SearchProjectForFileName(string fileName)
		{
			ProjectModel project = Projects.SearchForFileName(fileName);

				// Busca el proyecto en las carpetas
				if (project == null)
					project = Folders.SearchProjectForFileName(fileName);
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Busca un proyecto por su nombre
		/// </summary>
		public ProjectModel SearchProjectByName(string name)
		{
			ProjectsModelCollection projects = GetAllProjects();

				// Busca el proyecto por su nombre
				foreach (ProjectModel project in projects)
					if (project.Name.EqualsIgnoreCase(name))
						return project;
				// Si ha llegado hasta aquí es porque no ha encontrado nada
				return null;
		}

		/// <summary>
		///		Obtiene recursivamente una copia de todos los proyectos
		/// </summary>
		public ProjectsModelCollection GetAllProjects()
		{
			ProjectsModelCollection projects = new ProjectsModelCollection(this);

				// Añade los proyectos de la solución
				projects.AddRange(Projects);
				// Añade los proyectos de las carpetas
				projects.AddRange(Folders.GetAllProjects(this));
				// Devuelve la colección de proyectos
				return projects;
		}

		/// <summary>
		///		Borra un proyecto de la solución
		/// </summary>
		public void Delete(ProjectModel project)
		{
			if (!Projects.Delete(project))
				Folders.Delete(project);
		}

		/// <summary>
		///		Borra una carpeta de la solución
		/// </summary>
		public void Delete(SolutionFolderModel folder)
		{
			Folders.Delete(folder);
		}

		/// <summary>
		///		Carpetas de una solución
		/// </summary>
		public SolutionFolderModelCollection Folders { get; } = new SolutionFolderModelCollection();

		/// <summary>
		///		Proyectos
		/// </summary>
		public ProjectsModelCollection Projects { get; }
	}
}
