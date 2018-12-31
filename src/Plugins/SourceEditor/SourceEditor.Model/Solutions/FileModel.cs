using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Clase con los datos de un archivo
	/// </summary>
	public class FileModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Variables privadas
		private string _title;
		private FileModelCollection _files = null;
		private Definitions.AbstractDefinitionModel _fileDefinition = null;

		public FileModel(FileModel parent, string fileName, string title = null)
		{
			FullFileName = fileName;
			Title = title;
			Parent = parent;
			IsFolder = Directory.Exists(fileName);
		}

		/// <summary>
		///		Limpia la colección de archivos
		/// </summary>
		public void Clear()
		{
			_files = null;
		}

		/// <summary>
		///		Busca el proyecto padre del archivo
		/// </summary>
		public ProjectModel SearchProject()
		{
			if (this is ProjectModel)
				return (this as ProjectModel);
			else if (Parent != null)
				return Parent.SearchProject();
			else
				return null;
		}

		/// <summary>
		///		Obtiene el nombre de archivo relativo al proyecto
		/// </summary>
		public string GetRelativeFileNameToProject()
		{
			ProjectModel project = SearchProject();

				// Si existe el proyecto obtiene el nombre relativo
				if (project == null)
					return FullFileName.Substring(project.PathBase.Length + 1);
				else
					return FullFileName;
		}

		/// <summary>
		///		Objeto padre
		/// </summary>
		public FileModel Parent { get; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public override string Name
		{
			get
			{
				if (IsFolder)
					return FileName;
				else
					return Path.GetFileNameWithoutExtension(FullFileName);
			}
			set { base.Name = value; }
		}

		/// <summary>
		///		Título del archivo
		/// </summary>
		public string Title
		{
			get
			{
				if (_title.IsEmpty())
					return Name;
				else
					return _title;
			}
			set { _title = value; }
		}

		/// <summary>
		///		Archivos hijo
		/// </summary>
		public FileModelCollection Files
		{
			get
			{ // Si no se ha definido la colección de archivos, la carga
				if (_files == null)
				{ 
					// Crea la colección de archivos
					_files = new FileModelCollection();
					// Si es una carpeta, carga los archivos
					if (IsFolder && Directory.Exists(PathBase))
					{
						string[] pathFiles;

							// Añade los directorios
							pathFiles = Directory.GetDirectories(PathBase);
							foreach (string file in pathFiles)
								_files.Add(this, file);
							// Añade los archivos
							pathFiles = Directory.GetFiles(PathBase);
							foreach (string file in pathFiles)
								_files.Add(this, file);
					}
				}
				// Devuelve la colección de archivos
				return _files;
			}
		}

		/// <summary>
		///		Indica si es una carpeta
		/// </summary>
		public virtual bool IsFolder { get; }

		/// <summary>
		///		Directorio base
		/// </summary>
		public virtual string PathBase
		{
			get
			{
				if (IsFolder)
					return FullFileName;
				else
					return Path.GetDirectoryName(FullFileName);
			}
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get { return Path.GetFileName(FullFileName); }
		}

		/// <summary>
		///		Nombre de archivo completo
		/// </summary>
		public string FullFileName { get; private set; }

		/// <summary>
		///		Extensión del archivo (sin punto)
		/// </summary>
		public string Extension
		{
			get
			{
				string extension = "";

					// Si es una carpeta no tiene extensión
					if (!IsFolder)
						extension = Path.GetExtension(FileName);
					// Devuelve la extensión (sin punto)
					if (extension != null && extension.Length > 0)
						extension = extension.Substring(1);
					// Devuelve la extensión
					return extension;
			}
		}

		/// <summary>
		///		Obtiene la definición de archivo
		/// </summary>
		public Definitions.AbstractDefinitionModel FileDefinition
		{
			get
			{ 
				// Obtiene la definición de archivo si no estaba en memoria
				if (_fileDefinition == null)
				{
					ProjectModel project = SearchProject();

						// Obtiene la definición de archivo por la extensión
						if (project != null)
							_fileDefinition = project.Definition.FilesDefinition.SearchByExtension(Extension);
						// Si no se ha encontrado ninguna definición de archivo crea uno nuevo para no volver a entrar
						if (_fileDefinition == null)
							_fileDefinition = new Definitions.FileDefinitionModel(null, null, null, null, false);
				}
				// Devuelve la definición de archivo
				return _fileDefinition;
			}
		}
	}
}
