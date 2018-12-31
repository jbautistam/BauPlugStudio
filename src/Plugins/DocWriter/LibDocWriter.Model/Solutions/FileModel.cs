using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Solutions
{
	/// <summary>
	///		Modelo con los datos de un archivo
	/// </summary>
	public class FileModel : Base.BaseDocWriterFileModel
	{ 
		// Constantes públicas
		public const string DocumentExtension = ".dcx";
		public const string SectionExtension = ".sct";
		public const string SiteMapExtension = ".tpg";
		public const string TagExtension = ".tgx";
		public const string TemplateExtension = ".tpt";
		public const string ReferenceExtension = ".rfx";

		/// <summary>
		///		Tipo de archivo
		/// </summary>
		public enum DocumentType
		{
			Unknown,
			Folder,
			Document,
			Image,
			Section,
			SiteMap,
			Tag,
			Template,
			Reference,
			File
		}
		// Variables privadas
		private string _title;
		private DocumentType _fileType;

		public FileModel(ProjectModel project, string fileName = null)
		{
			Project = project;
			FullFileName = fileName;
			Files = new FilesModelCollection(project);
			FileType = DocumentType.Unknown;
		}

		/// <summary>
		///		Obtiene el nombre de archivo predeterminado para un tipo
		/// </summary>
		public string GetDefaultFileName(DocumentType type)
		{
			switch (type)
			{
				case DocumentType.Document:
					return "Document" + DocumentExtension;
				case DocumentType.Tag:
					return "Tag" + TagExtension;
				default:
					return null;
			}
		}

		/// <summary>
		///		Obtiene la extensión de un archivo
		/// </summary>
		public string GetExtension(DocumentType type)
		{
			switch (type)
			{
				case DocumentType.Document:
					return DocumentExtension;
				case DocumentType.Reference:
					return ReferenceExtension;
				case DocumentType.Section:
					return SectionExtension;
				case DocumentType.SiteMap:
					return SiteMapExtension;
				case DocumentType.Tag:
					return TagExtension;
				case DocumentType.Template:
					return TemplateExtension;
				default:
					return "";
			}
		}

		/// <summary>
		///		Obtiene el tipo de archivo por la extensión de un archivo
		/// </summary>
		public DocumentType GetFileTypeByExtension()
		{
			string extension = System.IO.Path.GetExtension(DocumentFileName);

				if (extension.EqualsIgnoreCase(DocumentExtension))
					return DocumentType.Document;
				else if (extension.EqualsIgnoreCase(ReferenceExtension))
					return DocumentType.Reference;
				else if (extension.EqualsIgnoreCase(SectionExtension))
					return DocumentType.Section;
				else if (extension.EqualsIgnoreCase(SiteMapExtension))
					return DocumentType.SiteMap;
				else if (extension.EqualsIgnoreCase(TagExtension))
					return DocumentType.Tag;
				else if (extension.EqualsIgnoreCase(TemplateExtension))
					return DocumentType.Template;
				else
					return DocumentType.File;
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Título
		/// </summary>
		public string Title
		{
			get
			{
				if (_title.IsEmpty())
					return FileName;
				else
					return _title;
			}
			set { _title = value; }
		}

		/// <summary>
		///		Archivos
		/// </summary>
		public FilesModelCollection Files { get; }

		/// <summary>
		///		Comprueba si el archivo está asociado a un directorio
		/// </summary>
		public bool IsFolder
		{
			get { return System.IO.Directory.Exists(base.FullFileName); }
		}

		/// <summary>
		///		Comprueba si el tipo de documento implica un directorio
		/// </summary>
		public bool IsDocumentFolder
		{
			get { return FileType == DocumentType.Document || FileType == DocumentType.Tag; }
		}

		/// <summary>
		///		Directorio
		/// </summary>
		public new string Path
		{
			get
			{
				if (IsFolder)
					return FullFileName;
				else
					return base.Path;
			}
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string DocumentFileName
		{
			get
			{
				switch (_fileType) // ... obtiene el valor de la variable y no de la propiedad -> evita llamadas recursivas
				{
					case DocumentType.Document:
					case DocumentType.Tag:
						if (!FullFileName.EndsWith(GetDefaultFileName(_fileType), StringComparison.CurrentCultureIgnoreCase))
							return System.IO.Path.Combine(FullFileName, GetDefaultFileName(_fileType));
						else
							return FullFileName;
					default:
						return FullFileName;
				}
			}
		}

		/// <summary>
		///		Tipo de archivo
		/// </summary>
		public DocumentType FileType
		{
			get
			{ 
				// Obtiene el tipo de archivo por la extensión
				if (_fileType == DocumentType.Unknown)
					_fileType = GetFileTypeByExtension();
				// Devuelve el tipo de archivo
				return _fileType;
			}
			set { _fileType = value; }
		}

		/// <summary>
		///		Obtiene el nombre
		/// </summary>
		public string IDFileName
		{
			get
			{
				if (Project.File.Path.Length >= FullFileName.Length)
					return Project.File.Path;
				else
					return FullFileName.Substring(Project.File.Path.Length + 1);
			}
		}

		/// <summary>
		///		Comprueba si el archivo es una imagen
		/// </summary>
		public bool IsImage
		{
			get
			{
				return FullFileName.EndsWith(".bmp", StringComparison.CurrentCultureIgnoreCase) ||
					   FullFileName.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) ||
					   FullFileName.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
					   FullFileName.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
					   FullFileName.EndsWith(".tif", StringComparison.CurrentCultureIgnoreCase);
			}
		}
	}
}
