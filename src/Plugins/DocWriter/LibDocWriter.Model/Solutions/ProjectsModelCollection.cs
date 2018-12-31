using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Solutions
{
	/// <summary>
	///		Colección de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectsModelCollection : Base.BaseDocWriterModelCollection<ProjectModel>
	{
		public ProjectsModelCollection(SolutionModel solution)
		{
			Solution = solution;
		}

		/// <summary>
		///		Clona una colección de proyectos
		/// </summary>
		internal ProjectsModelCollection Clone()
		{
			ProjectsModelCollection projects = new ProjectsModelCollection(Solution);

				// Clona los proyectos
				foreach (ProjectModel project in this)
					projects.Add(project);
				// Devuelve la colección de proyectos
				return projects;
		}

		/// <summary>
		///		Añade un proyecto a la solución
		/// </summary>
		public ProjectModel Add(string fileName)
		{
			ProjectModel project = new ProjectModel(Solution);

				// Asigna las propiedades al proyecto
				project.File.FullFileName = fileName;
				// Añade el proyecto
				Add(project);
				// Devuelve el proyecto añadido
				return project;
		}

		/// <summary>
		///		Busca el proyecto al que pertenece un archivo
		/// </summary>
		public ProjectModel SearchForFileName(string fileName)
		{ 
			// Recorre la colección de proyectos
			if (!fileName.IsEmpty())
				foreach (ProjectModel project in this)
					if (System.IO.Path.GetDirectoryName(fileName).StartsWith(project.File.Path, StringComparison.CurrentCultureIgnoreCase))
						return project;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Borra un proyecto de la colección
		/// </summary>
		public bool Delete(ProjectModel project)
		{
			bool deleted = false;

				// Recorre la colección para borrar el elemento
				for (int index = Count - 1; index >= 0 && !deleted; index--)
					if (this [index].GlobalId.EqualsIgnoreCase(project.GlobalId))
					{ 
						// Elimina el elemento
						RemoveAt(index);
						// Indica que se ha borrado
						deleted = true;
					}
				// Devuelve el valor que indica que se ha borrado
				return deleted;
		}

		/// <summary>
		///		Solución
		/// </summary>
		public SolutionModel Solution { get; }
	}
}
