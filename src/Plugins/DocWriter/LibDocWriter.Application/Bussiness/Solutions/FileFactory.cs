using System;
using System.IO;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions
{
	/// <summary>
	///		Factory de <see cref="FileModel"/>
	/// </summary>
	public class FileFactory
	{
		/// <summary>
		///		Crea una carpeta
		/// </summary>
		public FileModel CreateFolder(ProjectModel project, FileModel folderParent, string name)
		{
			FileModel folder = new FileModel(project);

				// Asigna las propiedades
				folder.FullFileName = GetPath(project, folderParent, name);
				folder.Title = name;
				folder.FileType = FileModel.DocumentType.Folder;
				// Crea la carpeta
				LibCommonHelper.Files.HelperFiles.MakePath(folder.FullFileName);
				// Añade la carpeta
				if (folderParent == null)
					project.File.Files.Add(folder);
				else
					project.File.Files.Add(folder);
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Modifica el directorio de una carpeta
		/// </summary>
		public void UpdateFolder(ProjectModel project, FileModel folder, string name)
		{
			string path = Path.Combine(Path.GetDirectoryName(folder.FullFileName), folder.NormalizeFileName(name));

				// Renombra el directorio
				LibCommonHelper.Files.HelperFiles.Rename(folder.FullFileName, path);
				// Asigna las propiedades
				folder.FullFileName = path;
				folder.Title = name;
		}

		/// <summary>
		///		Crea un archivo
		/// </summary>
		public FileModel CreateFile(ProjectModel project, FileModel folderParent, string name, FileModel.DocumentType type)
		{
			FileModel file = new FileModel(project);

				// Asigna las propiedades
				file.Title = name;
				file.FullFileName = GetPath(project, folderParent, name);
				file.FileType = type;
				// Si es un archivo de tipo documento, añade al nombre de archivo, el nombre de archivo predeterminado, si no recoge el
				// título normalizado
				if (!file.IsDocumentFolder)
					file.FullFileName += file.GetExtension(type);
				else
					file.Title = Path.GetFileName(file.FullFileName);
				// Crea el directorio
				LibCommonHelper.Files.HelperFiles.MakePath(file.Path);
				// Graba el archivo
				if (file.FileType == FileModel.DocumentType.File)
					LibCommonHelper.Files.HelperFiles.SaveTextFile(file.FullFileName, "");
				else if (file.FileType != FileModel.DocumentType.Folder)
					new Documents.DocumentBussiness().Save(file);
				// Devuelve el archivo
				return file;
		}

		/// <summary>
		///		Obtiene el directorio
		/// </summary>
		public string GetPath(ProjectModel project, FileModel folderParent, string name)
		{
			if (folderParent == null)
				return Path.Combine(project.File.Path, project.File.NormalizeFileName(name, true));
			else
				return LibCommonHelper.Files.HelperFiles.GetConsecutivePath(folderParent.Path, project.File.NormalizeFileName(name, true));
		}

		/// <summary>
		///		Obtiene un archivo
		/// </summary>
		public FileModel GetInstance(ProjectModel project, string fileTarget)
		{
			FileModel file = new FileModel(project);

				// Asigna el nombre de archivo
				file.FullFileName = fileTarget;
				// Devuelve el archivo
				return file;
		}

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		public void RenameFile(FileModel file, string newName)
		{
			if (file.IsFolder)
			{
				string newPath = LibCommonHelper.Files.HelperFiles.GetConsecutivePath(Path.GetDirectoryName(file.FullFileName), newName);

					// Cambia el nombre del directorio
					LibCommonHelper.Files.HelperFiles.Rename(file.FullFileName, newPath);
			}
			else
			{
				string newFile = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(Path.GetDirectoryName(file.FullFileName),
																					newName + Path.GetExtension(file.FullFileName));

					// Cambia el nombre de archivo
					LibCommonHelper.Files.HelperFiles.Rename(file.FullFileName, newFile);
			}
		}
	}
}
