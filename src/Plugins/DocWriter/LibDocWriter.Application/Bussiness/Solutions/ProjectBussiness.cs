using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.Repository.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions
{
	/// <summary>
	///		Clase de negocio de <see cref="ProjectModel"/>
	/// </summary>
	public class ProjectBussiness
	{
		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		public ProjectModel Load(SolutionModel solution, string fileName)
		{
			return new ProjectRepository().Load(solution, fileName);
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		public void Save(ProjectModel project)
		{
			new ProjectRepository().Save(project);
		}

		/// <summary>
		///		Obtiene los archivos de determinado tipo
		/// </summary>
		public FilesModelCollection GetFiles(ProjectModel project, FileModel.DocumentType idDocumentType)
		{
			return GetFiles(project, new FileBussiness().Load(project), idDocumentType);
		}

		/// <summary>
		///		Obtiene los archivos de un tipo de una carpeta
		/// </summary>
		private FilesModelCollection GetFiles(ProjectModel project, FilesModelCollection filesProject, FileModel.DocumentType idDocumentType)
		{
			FilesModelCollection files = new FilesModelCollection(project);

				// Añade a la colección de salida los archivos de determinado tipo
				foreach (FileModel fileProject in filesProject)
					if (fileProject.FileType == idDocumentType)
						files.Add(fileProject);
					else
						files.AddRange(fileProject.Files);
				// Devuelve la colección de archivos
				return files;
		}
	}
}
