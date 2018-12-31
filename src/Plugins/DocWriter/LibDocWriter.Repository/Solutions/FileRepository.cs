using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Repository.Solutions
{
	/// <summary>
	///		Repository para los archivos
	/// </summary>
	public class FileRepository
	{
		/// <summary>
		///		Carga los archivos de un proyecto
		/// </summary>
		public FilesModelCollection Load(ProjectModel project)
		{
			return Load(project, project.File.Path);
		}

		/// <summary>
		///		Carga los archivos de una carpeta
		/// </summary>
		public FilesModelCollection Load(FileModel file)
		{
			if (file.IsFolder)
				return Load(file.Project, file.FullFileName);
			else
				return Load(file.Project, file.Path);
		}

		/// <summary>
		///		Carga los archivos de una carpeta
		/// </summary>
		private FilesModelCollection Load(ProjectModel project, string path)
		{
			FilesModelCollection files = new FilesModelCollection(project);

				// Obtiene los archivos
				if (Directory.Exists(path))
				{ 
					// Añade los directorios
					files.AddRange(ConvertFiles(project, Directory.GetDirectories(path)));
					// Añade los archivos
					files.AddRange(ConvertFiles(project, Directory.GetFiles(path)));
				}
				// Devuelve los archivos
				return files;
		}

		/// <summary>
		///		Añade un archivo / directorio a la colección
		/// </summary>
		private FilesModelCollection ConvertFiles(ProjectModel project, string [] arrfileName)
		{
			FilesModelCollection files = new FilesModelCollection(project);

				// Añade los archivos a la colección
				foreach (string fileName in arrfileName)
				{
					FileModel file = ConvertFile(project, fileName);

						if (file != null)
							files.Add(file);
				}
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Convierte un archivo
		/// </summary>
		private FileModel ConvertFile(ProjectModel project, string fileName)
		{
			FileModel file = new FileModel(project);

				// Asigna las propiedades
				file.FullFileName = fileName;
				file.FileType = FileModel.DocumentType.File;
				// Asigna el tipo de documento
				if (Directory.Exists(fileName))
				{ 
					// Indica que es una carpeta
					file.FileType = FileModel.DocumentType.Folder;
					// Comprueba si es un documento o una etiqueta (en ese caso no se añadirían a la colección)
					if (File.Exists(Path.Combine(fileName, file.GetDefaultFileName(FileModel.DocumentType.Document))))
						file.FileType = FileModel.DocumentType.Document;
					else if (File.Exists(Path.Combine(fileName, file.GetDefaultFileName(FileModel.DocumentType.Tag))))
						file.FileType = FileModel.DocumentType.Tag;
				}
				else
				{
					string extension = Path.GetExtension(fileName);

						if (extension.EqualsIgnoreCase(file.GetExtension(FileModel.DocumentType.Document)) ||
								extension.EqualsIgnoreCase(file.GetExtension(FileModel.DocumentType.Tag)))
							file = null;
						else if (extension.EqualsIgnoreCase(file.GetExtension(FileModel.DocumentType.SiteMap)))
							file.FileType = FileModel.DocumentType.SiteMap;
						else if (extension.EqualsIgnoreCase(file.GetExtension(FileModel.DocumentType.Reference)))
							file.FileType = FileModel.DocumentType.Reference;
						else if (extension.EqualsIgnoreCase(file.GetExtension(FileModel.DocumentType.Section)))
							file.FileType = FileModel.DocumentType.Section;
						else if (extension.EqualsIgnoreCase(file.GetExtension(FileModel.DocumentType.Template)))
							file.FileType = FileModel.DocumentType.Template;
						else if (extension.EqualsIgnoreCase(".bmp") || extension.EqualsIgnoreCase(".gif") ||
								 extension.EqualsIgnoreCase(".jpg") || extension.EqualsIgnoreCase(".png") ||
								 extension.EqualsIgnoreCase(".tif") || extension.EqualsIgnoreCase(".tiff"))
							file.FileType = FileModel.DocumentType.Image;
				}
				// Devuelve el objeto
				return file;
		}
	}
}
