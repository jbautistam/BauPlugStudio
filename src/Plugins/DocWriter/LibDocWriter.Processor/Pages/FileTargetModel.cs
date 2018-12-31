using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Processor.Pages
{
	/// <summary>
	///		Clase con los datos de un archivo
	/// </summary>
	internal class FileTargetModel
	{   
		// Variables privadas
		private string _fileName, _pathTarget;
		private string _fileNameImage, _fileNameThumbnail;

		internal FileTargetModel(FileModel file, string pathTarget, string fileTarget)
		{
			File = file;
			PathTarget = pathTarget;
			FileNameTarget = fileTarget;
			FileNameSource = Path.Combine(pathTarget, fileTarget);
		}

		/// <summary>
		///		Clona el objeto
		/// </summary>
		internal FileTargetModel Clone()
		{
			FileTargetModel newFile = new FileTargetModel(File, PathTarget, FileNameTarget);

				// Asigna los datos adicionales
				newFile.Title = Title;
				newFile.FileNameSource = FileNameSource;
				newFile.FileNameImage = FileNameImage;
				newFile.FileNameThumbnail = FileNameThumbnail;
				newFile.DateUpdate = DateUpdate;
				newFile.ShowMode = ShowMode;
				newFile.IsRecursive = IsRecursive;
				newFile.ShowAtRss = ShowAtRss;
				// Devuelve el nuevo objeto
				return newFile;
		}

		/// <summary>
		///		Normaliza un nombre de archivo
		/// </summary>
		private string NormalizeFileName(string fileName)
		{
			string result = "";

				// Normaliza el nombre de archivo
				if (!fileName.IsEmpty())
				{
					const string danger = "áéíóúÁÉÍÓÚàèìòùÀÈÌÒÙäëïöüÄËÏÖÜñÑçÇ#";
					const string normalized = "aeiouAEIOUaeiouAEIOUaeiouAEIOUnNcCs";

						// Normaliza la cadena
						foreach (char chrChar in fileName)
						{
							int index = danger.IndexOf(chrChar);

							if (index >= 0)
								result += normalized [index];
							else if (chrChar == ' ')
								result += "-";
							else
								result += chrChar;
						}
				}
				// Devuelve el nombre de archivo normalizado
				return result;
		}

		/// <summary>
		///		Obtiene el directorio completo destino
		/// </summary>
		internal string GetFullPath(Generator Processor)
		{
			return Path.Combine(Processor.PathTarget, PathTarget);
		}

		/// <summary>
		///		Obtiene el nombre de archivo completo (incluyendo el directorio de generación del proyecto)
		/// </summary>
		internal string GetFullFileName(Generator processor)
		{
			return Path.Combine(processor.PathTarget, RelativeFullFileNameTarget);
		}

		/// <summary>
		///		Obtiene el nombre de archivo completo intermedio compilado (incluyendo el directorio de generación del proyecto)
		/// </summary>
		internal string GetFullFileNameCompiledShort(Generator processor)
		{
			return Path.Combine(processor.PathTarget, RelativeFullFileNameTarget + ".srt.tmp");
		}

		/// <summary>
		///		Obtiene el nombre del archivo completo compilado temporal
		/// </summary>
		internal string GetFullFileNameCompiledTemporal(Generator processor)
		{
			return Path.Combine(processor.PathTarget, RelativeFullFileNameTarget + ".tmp");
		}

		/// <summary>
		///		Obtiene la URL absoluta de la página
		/// </summary>
		internal string GetAbsoluteUrlPage(Generator processor)
		{
			return GetAbsoluteUrl(processor, RelativeFullFileNameTarget);
		}

		/// <summary>
		///		Obtiene la URL absoluta de la imagen de la página
		/// </summary>
		internal string GetAbsoluteUrlImage(Generator processor)
		{
			return GetAbsoluteUrlImage(processor, FileNameImage);
		}

		/// <summary>
		///		Obtiene la URL absoluta del thumbnail de la página
		/// </summary>
		internal string GetAbsoluteUrlThumb(Generator processor)
		{
			return GetAbsoluteUrlImage(processor, FileNameThumbnail);
		}

		/// <summary>
		///		Obtiene la URL de una imagen
		/// </summary>
		private string GetAbsoluteUrlImage(Generator processor, string fileNameImage)
		{
			string url = fileNameImage;

				// Si hay algo en la URL
				if (!url.IsEmpty())
				{ 
					// Obtiene la URL de la imagen
					if (Path.GetDirectoryName(url).EqualsIgnoreCase(PathTarget))
						url = Path.Combine(Path.GetDirectoryName(PathTarget) + "\\" + Path.GetFileName(url));
					// Obtiene la URL absoluta
					url = GetAbsoluteUrl(processor, url);
				}
				// Devuelve la URL
				return url;
		}

		/// <summary>
		///		Obtiene la URL absoluta de la página
		/// </summary>
		private string GetAbsoluteUrl(Generator processor, string url)
		{
			return Path.Combine(processor.Project.URLBase, url).Replace("\\", "/");
		}

		/// <summary>
		///		Obtiene un nombre de archivo completo relativo al proyecto eliminando el último directorio si es necesario
		/// </summary>
		private string GetRelativeFullFileName(string fileNameTarget)
		{
			string fileName = Path.GetFileNameWithoutExtension(fileNameTarget);
			string [] paths = PathTarget.Split('\\');
			int intMaxIndex = paths.Length;
			string pathTarget = "";

				// Ajusta el índice para obtener el último directorio
				if (paths.Length > 1 && fileName.EqualsIgnoreCase(paths [paths.Length - 1]))
					intMaxIndex--;
				// Crea el directorio
				for (int index = 0; index < intMaxIndex; index++)
					pathTarget = pathTarget.AddWithSeparator(paths [index], "\\", false);
				// Devuelve el nombre de archivo
				return Path.Combine(pathTarget, fileNameTarget);
		}

		/// <summary>
		///		Archivo origen
		/// </summary>
		internal FileModel File { get; set; }

		/// <summary>
		///		Nombre del archivo original (en el caso de las referencias es el nombre de archivo origen inicial)
		/// </summary>
		internal string FileNameSource { get; set; }

		/// <summary>
		///		Nombre del archivo original
		/// </summary>
		internal string DocumentFileName
		{
			get
			{
				if (FileNameSource.EndsWith(File.GetDefaultFileName(FileModel.DocumentType.Document), StringComparison.CurrentCultureIgnoreCase) ||
						  FileNameSource.EndsWith(File.GetDefaultFileName(FileModel.DocumentType.Tag), StringComparison.CurrentCultureIgnoreCase))
					return Path.GetDirectoryName(FileNameSource);
				else
					return FileNameSource;
			}
		}

		/// <summary>
		///		Nombre del archivo destino (normalizado)
		/// </summary>
		internal string FileNameTarget
		{
			get { return _fileName; }
			set { _fileName = NormalizeFileName(value); }
		}

		/// <summary>
		///		Directorio destino
		/// </summary>
		internal string PathTarget
		{
			get { return _pathTarget; }
			set { _pathTarget = NormalizeFileName(value); }
		}

		/// <summary>
		///		Nombre del archivo de imagen
		/// </summary>
		public string FileNameImage
		{
			get { return _fileNameImage; }
			set { _fileNameImage = NormalizeFileName(value); }
		}

		/// <summary>
		///		Nombre del archivo de thumg
		/// </summary>
		public string FileNameThumbnail
		{
			get { return _fileNameThumbnail; }
			set { _fileNameThumbnail = NormalizeFileName(value); }
		}

		/// <summary>
		///		Nombre de archivo completo relativo al proyecto
		/// </summary>
		internal string RelativeFullFileNameTarget
		{
			get
			{
				return GetRelativeFullFileName(FileNameTarget);
			}
		}

		/// <summary>
		///		Fecha de modificación
		/// </summary>
		internal DateTime DateUpdate { get; set; }

		/// <summary>
		///		Título anterior
		/// </summary>
		internal string PreviousTitle { get; set; }

		/// <summary>
		///		Título del documento
		/// </summary>
		internal string Title { get; set; }

		/// <summary>
		///		Número de página
		/// </summary>
		internal int Page { get; set; }

		/// <summary>
		///		Modo de visualización (si es un documento)
		/// </summary>
		internal DocumentModel.ShowChildsMode ShowMode { get; set; }

		/// <summary>
		///		Indica si es una categoría recursiva (si es un documento)
		/// </summary>
		internal bool IsRecursive { get; set; }

		/// <summary>
		///		Indica si se debe mostrar en las categorías y RSS (si es un documento)
		/// </summary>
		internal bool ShowAtRss { get; set; }
	}
}
