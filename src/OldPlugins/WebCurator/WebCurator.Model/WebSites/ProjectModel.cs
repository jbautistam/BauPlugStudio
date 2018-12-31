using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.WebCurator.Model.WebSites
{
	/// <summary>
	///		Modelo con los datos de un proyecto de WebCurator
	/// </summary>
	public class ProjectModel : LibDataStructures.Base.BaseExtendedModel
	{   
		// Constantes privadas
		public const string Extension = "wsprj";

		public ProjectModel(string path)
		{
			Path = path;
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName
		{
			get
			{
				if (Name.IsEmpty())
					return null;
				else
					return System.IO.Path.Combine(Path, Name + "." + Extension);
			}
		}

		/// <summary>
		///		Directorio donde se encuentra el proyecto
		/// </summary>
		public string Path { get; }

		/// <summary>
		///		Número de documentos a generar
		/// </summary>
		public int NumberDocuments { get; set; }

		/// <summary>
		///		Ancho máximo de las imágenes
		/// </summary>
		public int MaxImageWidth { get; set; }

		/// <summary>
		///		Ancho de los thumbnails
		/// </summary>
		public int ThumbWidth { get; set; }

		/// <summary>
		///		Horas entre generación de documentos
		/// </summary>
		public int HoursBetweenGenerate { get; set; }

		/// <summary>
		///		Proyectos destino
		/// </summary>
		public ProjectTargetModelCollection ProjectsTarget { get; } = new ProjectTargetModelCollection();

		/// <summary>
		///		Directorios origen de datos de imágenes
		/// </summary>
		public List<string> PathImagesSources { get; set; } = new List<string>();

		/// <summary>
		///		Archivos origen de Rss
		/// </summary>
		public List<string> FilesRssSources { get; set; } = new List<string>();

		/// <summary>
		///		Archivos de XML
		/// </summary>
		public List<string> FilesXMLSentences { get; set; } = new List<string>();
	}
}
