using System;
using System.IO;

using Bau.Libraries.WebCurator.Model.WebSites;

namespace Bau.Libraries.WebCurator.Application.Bussiness.WebSites
{
	/// <summary>
	///		Clase de negocio de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectBussiness
	{
		/// <summary>
		///		Carga los proyectos de un directorio
		/// </summary>
		public ProjectModelCollection LoadAll(string path)
		{
			ProjectModelCollection projects = new ProjectModelCollection();

				// Carga los proyectos
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path, "*" + ProjectModel.Extension);

						// Añade los proyectos
						foreach (string file in files)
						{
							ProjectModel project = new ProjectModel(path);

								// Asigna las propiedades
								project.Name = Path.GetFileNameWithoutExtension(file);
								// Añade el proyecto a la colección
								projects.Add(project);
						}
				}
				// Devuelve la colección de proyectos
				return projects;
		}

		/// <summary>
		///		Carga los proyectos de un directorio y lee los XML
		/// </summary>
		public ProjectModelCollection LoadAllFull(string pathLibrary)
		{
			ProjectModelCollection projects = LoadAll(pathLibrary);
			ProjectModelCollection fullProjects = new ProjectModelCollection();

				// Carga el XML de los proyectos
				foreach (ProjectModel project in projects)
					fullProjects.Add(Load(project.FileName));
				// Devuelve la colección de proyectos cargados
				return fullProjects;
		}

		/// <summary>
		///		Carga el archivo de un proyecto
		/// </summary>
		public ProjectModel Load(string fileName)
		{
			return new Repository.WebSites.ProjectRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		public void Save(ProjectModel project)
		{
			new Repository.WebSites.ProjectRepository().Save(project);
		}

		/// <summary>
		///		Elimina un proyecto
		/// </summary>
		public void Delete(ProjectModel project)
		{
			LibCommonHelper.Files.HelperFiles.KillFile(project.FileName);
		}
	}
}
