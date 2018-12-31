using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Colección de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectsModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<ProjectModel>
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
		public ProjectModel Add(Definitions.ProjectDefinitionModel definition, string fileName)
		{
			ProjectModel project = new ProjectModel(Solution, definition, fileName);

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
					if (System.IO.Path.GetDirectoryName(fileName).StartsWith(project.PathBase, StringComparison.CurrentCultureIgnoreCase))
						return project;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Comprueba si existe un proyecto con este nombre de archivo
		/// </summary>
		public bool ExistsByFileName(string fileName)
		{ 
			// Comprueba si existe
			foreach (ProjectModel project in this)
				if (project.FullFileName.EqualsIgnoreCase(fileName))
					return true;
			// Si ha llegado hasta aquí es porque no existe
			return false;
		}

		/// <summary>
		///		Borra un proyecto de la colección
		/// </summary>
		public bool Delete(ProjectModel project)
		{
			bool deleted = false;

				// Recorre la colección para borrar el elemento
				for (int index = Count - 1; index >= 0 && !deleted; index--)
					if (this[index].GlobalId.EqualsIgnoreCase(project.GlobalId))
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
