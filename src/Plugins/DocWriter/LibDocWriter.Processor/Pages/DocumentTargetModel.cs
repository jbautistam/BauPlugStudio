using System;
using System.IO;

using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Processor.Pages
{
	/// <summary>
	///		Clase con los datos de un documento compilado
	/// </summary>
	internal class DocumentTargetModel
	{
		internal DocumentTargetModel()
		{
		}

		/// <summary>
		///		Obtiene la URL del archivo origen
		/// </summary>
		internal string GetUrlSource()
		{
			if (FileTarget.FileNameSource.EndsWith(FileModel.DocumentExtension, StringComparison.CurrentCultureIgnoreCase) ||
					  FileTarget.FileNameSource.EndsWith(FileModel.TagExtension, StringComparison.CurrentCultureIgnoreCase))
				return Path.GetDirectoryName(FileTarget.FileNameSource);
			else
				return FileTarget.FileNameSource;
		}

		/// <summary>
		///		Obtiene la URL en internet del archivo destino
		/// </summary>
		internal string GetInternetUrl(ProjectModel project)
		{
			return Path.Combine(project.URLBase, FileTarget.PathTarget, FileTarget.FileNameTarget).Replace("\\", "/");
		}

		/// <summary>
		///		Obtiene la Url del archivo imagen en internet
		/// </summary>
		internal string GetFullUrlImage(ProjectModel project)
		{ 
			// TODO --> Falta la conversión
			return Document.URLImageSummary;
		}

		/// <summary>
		///		Obtiene la URL en internet del thumbnail
		/// </summary>
		internal string GetFullUrlThumb(ProjectModel project)
		{ 
			// TODO --> Falta la conversión
			return Document.URLImageSummary;
		}

		/// <summary>
		///		Archivo destino
		/// </summary>
		internal FileTargetModel FileTarget { get; set; }

		/// <summary>
		///		Documento origen / compilado
		/// </summary>
		internal DocumentModel Document { get; set; }
	}
}
