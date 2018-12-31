using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Colección de <see cref="SolutionFolderModel"/>
	/// </summary>
	public class SolutionFolderModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<SolutionFolderModel>
	{
		/// <summary>
		///		Clona una colección de carpetas
		/// </summary>
		internal SolutionFolderModelCollection Clone()
		{
			SolutionFolderModelCollection folders = new SolutionFolderModelCollection();

				// Clona las carpetas
				foreach (SolutionFolderModel folder in this)
					folders.Add(folder.Clone());
				// Devuelve la colección clonada
				return folders;
		}

		/// <summary>
		///		Obtiene recursivamente los proyectos de la solución
		/// </summary>
		internal ProjectsModelCollection GetAllProjects(SolutionModel solution)
		{
			ProjectsModelCollection projects = new ProjectsModelCollection(solution);

				// Añade los proyectos de esta carpeta
				foreach (SolutionFolderModel folder in this)
					projects.AddRange(folder.GetAllProjects());
				// Devuelve los projectos
				return projects;
		}

		/// <summary>
		///		Borra una carpeta de la colección
		/// </summary>
		internal bool Delete(SolutionFolderModel folder)
		{
			bool deleted = false;

				// Borra la carpeta de esta colección
				for (int index = Count - 1; index >= 0 && !deleted; index--)
					if (!deleted && this[index].GlobalId.EqualsIgnoreCase(folder.GlobalId))
					{ 
						// Borra la carpeta 
						RemoveAt(index);
						// Indica que se ha borrado
						deleted = true;
					}
				// Si no se ha borrado, intenta borrar en los hijos
				for (int index = Count - 1; index >= 0 && !deleted; index--)
					if (!deleted)
						deleted = this[index].Folders.Delete(folder);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Borra un proyecto de la colección de carpetas
		/// </summary>
		internal bool Delete(ProjectModel project)
		{
			bool deleted = false;

				// Borra el proyecto de las carpetas
				foreach (SolutionFolderModel folder in this)
				{ 
					// Intenta borra el proyecto de la colección de proyectos de la carpeta
					if (!deleted)
						deleted = folder.Projects.Delete(project);
					// Si no se ha borrado intenta borrarlo de la colección de carpetas
					if (!deleted)
						deleted = folder.Folders.Delete(project);
				}
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Busca un proyecto en las carpetas
		/// </summary>
		internal ProjectModel SearchProjectForFileName(string fileName)
		{
			ProjectModel project = null;

				// Busca el proyecto en las carpetas
				foreach (SolutionFolderModel folder in this)
					if (project == null)
					{ 
						// Busca el proyecto en esta carpeta
						project = folder.Projects.SearchForFileName(fileName);
						// Busca en las carpetas hija
						if (project == null)
							project = folder.Folders.SearchProjectForFileName(fileName);
					}
				// Devuelve el proyecto
				return project;
		}
	}
}
