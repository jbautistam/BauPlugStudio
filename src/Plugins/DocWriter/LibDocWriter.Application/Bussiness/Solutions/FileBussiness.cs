using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Libraries.LibDocWriter.Repository.Solutions;

namespace Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions
{
	/// <summary>
	///		Clase de negocio de <see cref="FileModel"/>
	/// </summary>
	public class FileBussiness
	{
		/// <summary>
		///		Obtiene el tipo de documento a partir de la extensión
		/// </summary>
		public FileModel.DocumentType GetDocumentType(string fileName)
		{
			if (System.IO.Path.GetExtension(fileName).EqualsIgnoreCase(FileModel.DocumentExtension))
				return FileModel.DocumentType.Document;
			else if (System.IO.Path.GetExtension(fileName).EqualsIgnoreCase(FileModel.SiteMapExtension))
				return FileModel.DocumentType.SiteMap;
			else if (System.IO.Path.GetExtension(fileName).EqualsIgnoreCase(FileModel.ReferenceExtension))
				return FileModel.DocumentType.Reference;
			else if (System.IO.Path.GetExtension(fileName).EqualsIgnoreCase(FileModel.SectionExtension))
				return FileModel.DocumentType.Section;
			else if (System.IO.Path.GetExtension(fileName).EqualsIgnoreCase(FileModel.TagExtension))
				return FileModel.DocumentType.Tag;
			else if (System.IO.Path.GetExtension(fileName).EqualsIgnoreCase(FileModel.TemplateExtension))
				return FileModel.DocumentType.Template;
			else
				return FileModel.DocumentType.File;
		}

		/// <summary>
		///		Carga los archivos de un proyecto
		/// </summary>
		public FilesModelCollection Load(ProjectModel project)
		{
			return new FileRepository().Load(project);
		}

		/// <summary>
		///		Carga los archivos de una carpeta
		/// </summary>
		public FilesModelCollection Load(FileModel file)
		{
			return new FileRepository().Load(file);
		}

		/// <summary>
		///		Borra un archivo
		/// </summary>
		public void Delete(FileModel file)
		{
			if (file.IsFolder)
				LibCommonHelper.Files.HelperFiles.KillPath(file.FullFileName);
			else
				LibCommonHelper.Files.HelperFiles.KillFile(file.FullFileName);
		}
	}
}
