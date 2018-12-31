using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions
{
	/// <summary>
	///		Factory para los proyectos
	/// </summary>
	public class ProjectFactory
	{
		/// <summary>
		///		Crea un proyecto
		/// </summary>
		public ProjectModel Create(SolutionModel solution, SolutionFolderModel folder, string path)
		{
			ProjectModel project = new ProjectModel(solution);

				// Asigna el directorio y nombre de archivo
				if (!path.EndsWith(ProjectModel.FileName))
				{ 
					// Crea el directorio
					LibCommonHelper.Files.HelperFiles.MakePath(path);
					// Asigna el nobmre de archivo
					project.File.FullFileName = System.IO.Path.Combine(path, "WebDefinition.wdx");
				}
				else
					project.File.FullFileName = path;
				// Añade el proyecto a la solución
				if (folder != null)
					folder.Projects.Add(project);
				else
					solution.Projects.Add(project);
				// Graba la solución
				new SolutionBussiness().Save(solution);
				// Devuelve el proyecto
				return project;
		}
	}
}
