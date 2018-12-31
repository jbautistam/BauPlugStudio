using System;
using System.Collections.Generic;

using Bau.Libraries.LibMotionComic.Model.Components;

namespace Bau.Libraries.LibMotionComic.Model
{
	/// <summary>
	///		Clase con los datos de un cómic
	/// </summary>
	public class ComicModel
	{
		public ComicModel(string path)
		{
			Path = path;
		}

		/// <summary>
		///		Directorio donde se encuentran los archivos del cómic
		/// </summary>
		public string Path { get; }

		/// <summary>
		///		Título del cómic
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Resumen del cómic
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		///		Nombre del archivo de thumb
		/// </summary>
		public string ThumbFileName { get; set; }

		/// <summary>
		///		Ancho del cómic
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		///		Alto del cómic
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		///		Recursos del cómic
		/// </summary>
		public ResourcesDictionary Resources { get; } = new ResourcesDictionary();

		/// <summary>
		///		Lenguages de los textos del cómic
		/// </summary>
		public ResourcesDictionary Languages { get; } = new ResourcesDictionary();

		/// <summary>
		///		Páginas del cómic
		/// </summary>
		public List<PageModel> Pages { get; } = new List<PageModel>();
	}
}
