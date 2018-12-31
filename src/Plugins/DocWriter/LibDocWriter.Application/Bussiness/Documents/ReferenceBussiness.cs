using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Repository.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Documents
{
	/// <summary>
	///		Clase de negocio de <see cref="ReferenceModel"/>
	/// </summary>
	public class ReferenceBussiness
	{
		/// <summary>
		///		Carga una referencia de un archivo
		/// </summary>
		public ReferenceModel Load(FileModel file)
		{
			return new ReferenceRepository().Load(file);
		}

		/// <summary>
		///		Carga una referencia de un archivo
		/// </summary>
		public ReferenceModel Load(ProjectModel project, string fileSource)
		{
			return new ReferenceRepository().Load(new Solutions.FileFactory().GetInstance(project, fileSource));
		}

		/// <summary>
		///		Crea las referencias
		/// </summary>
		public void Create(ProjectModel projectTarget, string pathTarget, ProjectModel projectSource, FilesModelCollection files, bool isRecursive)
		{
			foreach (FileModel file in files)
			{
				string pathFirstTarget = pathTarget;

					// Si lo que se está copiando es un directorio, se añade el directorio al destino
					if (Directory.Exists(file.FullFileName))
						pathFirstTarget = Path.Combine(pathTarget, Path.GetFileName(file.FullFileName));
					// Copia los archivos / directorios
					Copy(projectSource, projectTarget, file.FullFileName,
						 pathFirstTarget, isRecursive || file.IsDocumentFolder);
			}
		}

		/// <summary>
		///		Copia un directorio o crea una referencia
		/// </summary>
		private void Copy(ProjectModel projectSource, ProjectModel projectTarget, string fileSource, string pathTarget, bool isRecursive)
		{
			if (Directory.Exists(fileSource))
			{
				string [] files = Directory.GetFiles(fileSource);

					// Copia los archivos
					foreach (string fileName in files)
						Copy(projectSource, projectTarget, fileName, pathTarget, isRecursive);
					// Copia los directorios
					if (isRecursive)
					{ 
						// Obtiene los directorios
						files = Directory.GetDirectories(fileSource);
						// Copia los archivos
						foreach (string fileName in files)
							Copy(projectSource, projectTarget, fileName, Path.Combine(pathTarget, Path.GetFileName(fileName)), isRecursive);
					}
			}
			else if (File.Exists(fileSource))
			{
				string fileTarget = Path.Combine(pathTarget, Path.GetFileName(fileSource));

					if (!File.Exists(fileTarget))
					{
						ReferenceModel reference;

							// Cambia la extensión al archivo final de referencia
							fileTarget = fileTarget + new Model.Solutions.FileModel(null).GetExtension(FileModel.DocumentType.Reference);
							// Crea el objeto de referencia
							reference = new ReferenceModel(new Solutions.FileFactory().GetInstance(projectTarget, fileTarget));
							reference.ProjectName = projectSource.Name;
							reference.FileNameReference = HelperFiles.GetFileNameRelative(projectSource.File.Path, fileSource);
							// Crea el directorio
							HelperFiles.MakePath(pathTarget);
							// Crea el archivo de referencia
							new ReferenceRepository().Save(reference);
					}
			}
		}

		/// <summary>
		///		Obtiene el nombre del archivo origen
		/// </summary>
		public string GetFileName(SolutionModel solution, ProjectModel project, string fileName)
		{
			return GetFileName(solution, Load(project, fileName));
		}

		/// <summary>
		///		Obtiene el nombre del archivo origen
		/// </summary>
		public string GetFileName(SolutionModel solution, ReferenceModel reference)
		{
			ProjectModel projectSource = solution.SearchProjectByName(reference.ProjectName);

				// Si se ha encontrado el proyecto, transforma la referencia
				if (projectSource != null)
					return Path.Combine(projectSource.File.Path, reference.FileNameReference);
				else
					return reference.FileNameReference;
		}
	}
}
