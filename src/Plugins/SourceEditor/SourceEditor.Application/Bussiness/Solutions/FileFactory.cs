using System;
using System.IO;

using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.Application.Bussiness.Solutions
{
	/// <summary>
	///		Factory de <see cref="FileModel"/>
	/// </summary>
	public class FileFactory
	{
		/// <summary>
		///		Crea un archivo
		/// </summary>
		public FileModel Create(FileDefinitionModel definition, ProjectModel project, FileModel folderParent, string name)
		{
			FileModel file;

				// Crea el archivo
				if (folderParent != null)
					file = new FileModel(folderParent, GetFileName(definition, folderParent.PathBase, name), name);
				else
					file = new FileModel(project, GetFileName(definition, project.PathBase, name), name);
				// Crea el directorio y un archivo vacío
				LibCommonHelper.Files.HelperFiles.MakePath(file.PathBase);
				LibCommonHelper.Files.HelperFiles.SaveTextFile(file.FullFileName, "");
				// Devuelve el archivo
				return file;
		}

		/// <summary>
		///		Obtiene el nombre de archivo
		/// </summary>
		private string GetFileName(AbstractDefinitionModel definition, string pathBase, string name)
		{
			string fileName;

				// Normaliza el nombre del archivo
				name = LibCommonHelper.Files.HelperFiles.Normalize(name);
				// Si es un paquete, añade el directorio y el nombre de archivo
				if (definition == null)
					fileName = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathBase, name);
				else if (definition is PackageDefinitionModel)
				{
					fileName = LibCommonHelper.Files.HelperFiles.GetConsecutivePath(pathBase, name);
					fileName = Path.Combine(pathBase, (definition as PackageDefinitionModel).FileName);
				}
				else
				{   
					// Añade la extensión
					if (!name.EndsWith("." + (definition as FileDefinitionModel).Extension, StringComparison.CurrentCultureIgnoreCase))
						name += "." + (definition as FileDefinitionModel).Extension;
					// Obtiene el nombre de archivo 
					fileName = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathBase, name);
				}
				// Devuelve el nombre de archivo
				return fileName;
		}

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		public string RenameFile(FileModel file, string newName)
		{
			string newFileName;

				// Cambia el nombre
				if (file.IsFolder)
				{ 
					// Obtiene el nuevo nombre
					newFileName = LibCommonHelper.Files.HelperFiles.GetConsecutivePath(Path.GetDirectoryName(file.FullFileName), newName);
					// Cambia el nombre del directorio
					LibCommonHelper.Files.HelperFiles.Rename(file.FullFileName, newFileName);
				}
				else
				{ 
					// Obtiene el nuevo nombre
					newFileName = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(Path.GetDirectoryName(file.FullFileName),
																					 newName + Path.GetExtension(file.FullFileName));
					// Cambia el nombre de archivo
					LibCommonHelper.Files.HelperFiles.Rename(file.FullFileName, newFileName);
				}
				// Devuelve el nuevo nombre de archivo
				return newFileName;
		}
	}
}
